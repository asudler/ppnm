using System;

public static class montecarlo {
    /* plainmc:
     * plain Monte Carlo integration algorithm
     * see Fedorov p. 75 */
    public static (double, double) plainmc
    (Func<double[],double> f, double[] a, double[] b, int N,
     bool write=false) {
        int dim = a.Length; double V = 1;
        for(int i = 0; i < dim; i++) V *= b[i] - a[i];
        double sum1 = 0, sum2 = 0;
        double[] x = new double[dim];
        var rnd = new Random();
        for(int i = 0; i < N; i++) {
            for(int k = 0; k < dim; k++) {
                x[k] = a[k] + rnd.NextDouble()*(b[k] - a[k]);
                if(write) Console.Write($"{x[k]}\t");
            }
            if(write) Console.Write($"\n");
            double fx = f(x);
            sum1 += fx; sum2 += fx*fx;
        }
        double mean = sum1/N, sigma = Math.Sqrt(sum2/N - mean*mean);
        return (mean*V, sigma*V/Math.Sqrt(N));
    } // plainmc

    /* quasimc:
     * quasi-random sampling using (1) Halton sequence below
     * and (2) lattice sequence; error is the difference
     * between the two methods  */
    public static (double, double, double) quasimc
    (Func<double[],double> f, double[] a, double[] b, int N) {
        int dim = a.Length; double V = 1;
        for(int i = 0; i < dim; i++) V *= b[i] - a[i];
        double sumh = 0, suml = 0;
        double[] xh = new double[dim], xl = new double[dim];
        for(int i = 0; i < N; i++) {
            double[] rndh = halton(i, dim), rndl = lattice(i, dim);
            for(int k = 0; k < dim; k++) {
                xh[k] = a[k] + rndh[k]*(b[k] - a[k]);
                xl[k] = a[k] + rndl[k]*(b[k] - a[k]);
            }
            double fxh = f(xh), fxl = f(xl);
            sumh += fxh; suml += fxl;
        }
        double meanh = sumh/N, meanl = suml/N, sigma = Math.Abs(meanh - meanl);
        return (meanh*V, meanl*V, sigma*V);
    } // quasimc

    /* stratmc:
     * recursive stratified monte carlo sampling algorithm, 
     * it probably could be done more eloquently... */
    public static (double, double) stratmc
    (Func<double[],double> f, double[] a, double[] b, int N,
     bool write=false, int nmin=100) {
        if(a.Length != b.Length) throw new Exception("bad limit dimensions");
        if(N < nmin) return plainmc(f, a, b, N, write);
        
        // (1) sample nmin points and estimate the integral and variance
        // (2) figure out which dimension has largest variance
        int dim = a.Length; double V = 1;
        for(int i = 0; i < dim; i++) V *= b[i] - a[i];
        double[] sum1 = new double[dim], sum1v = new double[dim],
            sum2 = new double[dim], sum2v = new double[dim];
        int[] count1 = new int[dim], count2 = new int[dim];
        var rnd = new Random();
        for(int i = 0; i < nmin; i++) {
            double[] x = new double[dim];
            for(int j = 0; j < dim; j++) {
                x[j] = a[j] + rnd.NextDouble()*(b[j] - a[j]);
                if(write) Console.Write($"{x[j]}\t");
            }
            if(write) Console.Write("\n");
            double fx = f(x);
            for(int j = 0; j < dim; j++) {
                if(x[j] < a[j]/2 + b[j]/2) { 
                    sum1[j] += fx; 
                    sum1v[j] += fx*fx;
                    count1[j] += 1;
                }
                else { sum2[j] += fx; sum2v[j] += fx*fx; count2[j] += 1; }
            }
        }
        double help1 = 0, help2 = 0; int vdim = 0, side = 0;
        for(int i = 0; i < dim; i++) {
            double[] sigma = new double[dim*2];
            sigma[2*i] = Math.Sqrt(sum1v[i]/count1[i] 
                    - sum1[i]*sum1[i]/count1[i]/count1[i]);
            sigma[2*i + 1] = Math.Sqrt(sum2v[i]/count2[i]  
                - sum2[i]*sum2[i]/count2[i]/count2[i]);
            if(count1[i] == 0) sigma[2*i] = 0;  
            if(count2[i] == 0) sigma[2*i + 1] = 0;
            if(sigma[2*i] > help1) {
                help1 = sigma[2*i];
                help2 = sigma[2*i + 1];
                vdim = i;
                side = 0;
            }
            if(sigma[2*i + 1] > help1) {
                help1 = sigma[2*i + 1];
                help2 = sigma[2*i];
                vdim = i;
                side = 1;
            }
        }
        
        // (3) subdivide the volume along this dimension (vdim)
        double[] aa = new double[dim], bb = new double[dim];
        Array.Copy(a, aa, dim); Array.Copy(b, bb, dim);
        aa[vdim] = a[vdim]/2 + b[vdim]/2;
        bb[vdim] = a[vdim]/2 + b[vdim]/2;

        // (4) divide the remaining points between the two volumes
        // proportional to the sub-variances. if the subvariances
        // are equally 0, then the points should be divided evenly
        double factor;
        if(help1 == 0 && help2 == 0) factor = 0.5;
        else factor = help1/(help1 + help2);
        int n1 = (int)(factor*(N - nmin)), n2 = N - nmin - n1;

        // (5) dispatch recursive calls to the subvolumes
        // and get that result babeyyyyy
        double sol1 = 0, sol2 = 0, err1 = 0, err2 = 0;
        if(side == 0) {
            if(n1 != 0 && n2 != 0) {
                (sol1, err1) = stratmc(f, a, bb, n1, write);
                (sol2, err2) = stratmc(f, aa, b, n2, write);
            }
            if(n1 != 0 && n2 == 0) {
                (sol1, err1) = stratmc(f, a, bb, n1, write);
                (sol2, err2) = (V/2*sum2[vdim]/count2[vdim], 
                    V/2*Math.Sqrt(sum2v[vdim]/count2[vdim]
                    - sum2[vdim]*sum2[vdim]/count2[vdim]/count2[vdim]));
            }
        }
        else {
            if(n1 != 0 && n2 != 0) {
                (sol1, err1) = stratmc(f, aa, b, n1, write);
                (sol2, err2) = stratmc(f, a, bb, n2, write);
            }
            if(n1 != 0 && n2 == 0) {
                (sol1, err1) = stratmc(f, aa, b, n1, write);
                (sol2, err2) = (V/2*sum1[vdim]/count1[vdim],
                    V/2*Math.Sqrt(sum1v[vdim]/count1[vdim]
                    - sum1[vdim]*sum1[vdim]/count1[vdim]/count1[vdim]));
            }
        }
        return (sol1 + sol2, Math.Sqrt(err1*err1 + err2*err2));
    } // stratmc

    /* corput:
     * van der Corput sequence over the unit interval 
     * in base b */
    public static double corput(int n, int b) {
        double q = 0, bk = 1.0/b;
        while(n > 0) {
            q += (n % b)*bk;
            n /= b;
            bk /= b;
        }
        return q;
    } // corput

    /* halton:
     * the Halton sequence for low-discrepancy sampling */
    public static double[] halton(int n, int d) {
        double[] x = new double[d];
        int[] bases = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
        if(d > bases.Length)
            throw new Exception("halton: too many dimensions");
        for(int i = 0; i < d; i++) x[i] = corput(n, bases[i]);
        return x;
    } // halton

    /* lattice:
     * lattice rule (additive recurrence) for primes */
    public static double[] lattice(int n, int d) {
        double[] x = new double[d];
        int[] primes = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
        if(d > primes.Length)
            throw new Exception("lattice: too many dimensions");
        for(int i = 0; i < d; i++)
            x[i] = Math.Sqrt(primes[i])*n % 1; // just the decimals
        return x;
    } // lattice
} // montecarlo
