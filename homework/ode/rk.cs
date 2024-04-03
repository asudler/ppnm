using System;
public static class rk {
    public static (double[], double[]) rkstep45(
        Func<double, double[], double[]> f,
        double x,
        double[] y,
        double h
    ) {
    double[] a1 = new double[] {1/4};
    double[] a2 = new double[] {3/32, 9/32};
    double[] a3 = new double[] {1932/2197, -7200/2197, 7296/2197};
    double[] a4 = new double[] {439/216, -8, 3680/513, -845/4104};
    double[] a5 = new double[] {-8/27, 2, -3544/2565, 1859/4104, -11/40};
    double[] b0 = new double[] 
        {16/135, 0, 6656/12825, 28561/56430, -9/50, 2/55};
    double[] b1 = new double[] {25/216, 0, 1408/2565, 2197/4104, -1/5, 0};
    
    double[] c = new double[] {0, 1/4, 3/8, 12/13, 1, 1/2};
    int n = y.Length;
    double[] yt = new double[n], yh = new double[n], yerr = new double[n];
    
    // k1 step
    double[] k1 = f(x, y);
    for(int i = 0; i < n; i++) {
        yt[i] = y[i] + a1[0]*k1[i]*h;
    }

    // k2 step
    double[] k2 = f(x + c[1]*h, yt);
    for(int i = 0; i < n; i++) {
        yt[i] = y[i] + (a2[0]*k1[i] + a2[1]*k2[i])*h;
    }

    // k3 step
    double[] k3 = f(x + c[2]*h, yt);
    for(int i = 0; i < n; i++) {
        yt[i] = y[i] + (a3[0]*k1[i] + a3[1]*k2[i] + a3[2]*k3[i])*h;
    }

    // k4 step
    double[] k4 = f(x + c[3]*h, yt);
    for(int i = 0; i < n; i++) {
        yt[i] = y[i] 
            + (a4[0]*k1[i] + a4[1]*k2[i] + a4[2]*k3[i] + a4[3]*k4[i])*h;
    }

    // k5 step
    double[] k5 = f(x + c[4]*h, yt);
    for(int i = 0; i < n; i++) {
        yh[i] = y[i]
            + (a5[0]*k1[i] + a5[1]*k2[i] + a5[2]*k3[i] 
            + a5[3]*k4[i] + a5[4]*k5[i])*h;
    }

    // k6 step and error
    double[] k6 = f(x + c[5]*h, yh);
    for(int i = 0; i < n; i++) {
        yt[i] = y[i]
            + (b0[0]*k1[i] + b0[2]*k3[i] + b0[3]*k4[i] 
            + b0[4]*k5[i] + b0[5]*k6[i])*h;
        yerr[i] = yh[i] - yt[i];
    }

    return (yh, yerr);
    } // rkstep45
} // rk

