using System;
public static class rk {
    public static (double[], double[]) rkstep12(
        Func<double, double[], double[]> f,
        double x,
        double[] y,
        double h
    ) {
        double[] k1 = f(x,y);
        double[] yt = new double[y.Length];
        for(int i = 0; i < y.Length; i++) yt[i] = y[i] + h/2*k1[i];
        double[] k2 = f(x + h/2, yt);
        double[] yh = new double[y.Length];
        for(int i = 0; i < y.Length; i++) yh[i] = y[i] + h*k2[i];
        double[] err = new double[y.Length];
        for(int i = 0; i < y.Length; i++) err[i] = h*(k2[i] - k1[i]);
        return (yh, err);
    } // rkstep12
    
    public static (double[], double[]) rkstep45(
        Func<double, double[], double[]> f,
        double x,
        double[] y,
        double h
    ) {
        double[] a1 = new double[] {1.0/4};
        double[] a2 = new double[] {3.0/32, 9.0/32};
        double[] a3 = new double[] {1932.0/2197, -7200.0/2197, 7296.0/2197};
        double[] a4 = new double[] {439.0/216, -8.0, 3680.0/513, -845.0/4104};
        double[] a5 = new double[] 
            {-8.0/27, 2.0, -3544.0/2565, 1859.0/4104, -11.0/40};
        double[] b0 = new double[] 
            {16.0/135, 0, 6656.0/12825, 28561.0/56430, -9.0/50, 2.0/55};
        double[] b1 = new double[] 
            {25.0/216, 0, 1408.0/2565, 2197.0/4104, -1.0/5, 0};
        double[] c = new double[] {0, 1.0/4, 3.0/8, 12.0/13, 1.0, 1.0/2};
        int n = y.Length;
        double[] yt = new double[n], yh = new double[n], 
            yhs = new double[n], yerr = new double[n];
    
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
            yt[i] = y[i]
                + (a5[0]*k1[i] + a5[1]*k2[i] + a5[2]*k3[i] 
                + a5[3]*k4[i] + a5[4]*k5[i])*h;
        }

        // k6 step and error
        double[] k6 = f(x + c[5]*h, yt);
        for(int i = 0; i < n; i++) {
            yh[i] = y[i]
                + (b0[0]*k1[i] + b0[2]*k3[i] + b0[3]*k4[i] 
                + b0[4]*k5[i] + b0[5]*k6[i])*h;
            yhs[i] = y[i]
                + (b1[0]*k1[i] + b1[2]*k3[i] + b1[3]*k4[i]
                + b1[4]*k5[i] + b1[5]*k6[i])*h;
            yerr[i] = yh[i] - yhs[i];
        }
        return (yh, yerr);
    } // rkstep45
} // rk

