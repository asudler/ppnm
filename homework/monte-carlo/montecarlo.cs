using System;

public static class montecarlo {
    /* plainmc:
     * plain Monte Carlo integration algorithm
     * see Fedorov p. 75 */
    public static (double, double) plainmc
    (Func<double[],double> f, double[] a, double[] b, int N) {
        int dim = a.Length; double V = 1;
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];
        double sum1 = 0, sum2 = 0;
        double[] x = new double[dim];
        var rnd = new Random();
        for(int i = 0; i < N; i++) {
            for (int k = 0; k < dim; k++) {
                x[k] = a[k] + rnd.NextDouble()*(b[k] - a[k]);
            }
            double fx = f(x);
            sum1 += fx;
            sum2 += fx*fx;
        }
        double mean = sum1/N, sigma = Math.Sqrt(sum2/N - mean*mean);
        return (mean*V, sigma*V/Math.Sqrt(N));
    } // plainmc
} // montecarlo
