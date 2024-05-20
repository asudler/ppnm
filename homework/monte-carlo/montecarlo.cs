using System;

public static class montecarlo {
    /* plainmc:
     * plain Monte Carlo integration algorithm
     * see Fedorov p. 75 */
    public static (double, double) plainmc
    (Func<double[],double> f, double[] a, double[] b, int N) {
        int dim = a.Length; double V = 1;
        for(int i = 0; i < dim; i++) V *= b[i] - a[i];
        double sum1 = 0, sum2 = 0;
        double[] x = new double[dim];
        var rnd = new Random();
        for(int i = 0; i < N; i++) {
            for(int k = 0; k < dim; k++) {
                x[k] = a[k] + rnd.NextDouble()*(b[k] - a[k]);
            }
            double fx = f(x);
            sum1 += fx;
            sum2 += fx*fx;
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
