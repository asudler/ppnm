using System;
using System.Collections.Generic;
using static System.Math;
using static rk;
using static spline;
public static class ode {
    /* norm:
     * calculate the vector norm of a list of doubles */
    public static double norm(double[] arr) {
        double meanabs = 0;
        for(int i = 0; i < arr.Length; i++) meanabs += Abs(arr[i]);
        if(meanabs == 0) meanabs = 1;
        meanabs /= arr.Length;
        double sum = 0;
        for(int i = 0; i < arr.Length; i++) {
            sum += (arr[i]/meanabs)*(arr[i]/meanabs);
        }
        return meanabs*Sqrt(sum);
    } // norm (should've just used Dmitri's vector class...)

    /* driver:
     * solve ode (using rksolve45) with an adaptive step size */
    public static (List<double>, List<double[]>) driver(
        Func<double, double[], double[]> F, // f from dy/dx = f(x,y)
        (double, double) interval, // x start point to x end point
        double[] yi, // y value at start point
        double h=0.125, // initial dx
        double acc=1E-1, // absolute accuracy goal
        double eps=1E-1 // relative accuracy goal
    ) {
        var (a, b) = interval; double x = a; double[] y = yi;
        var xlist = new List<double>(); xlist.Add(x);
        var ylist = new List<double[]>(); ylist.Add(y);
        do {
            if(x >= b) return (xlist, ylist); // job done, breaks loop
            if(x + h > b) h = b - x; // last step should end at b
            var (yh, yerr) = rkstep45(F, x, y, h);
            double tol = (acc + eps*norm(yh))*Sqrt(h/(b - a));
            double err = norm(yerr);
            if(err <= tol) { // accept step
                x += h; y = yh;
                xlist.Add(x);
                ylist.Add(y);
            }
            h *= Min(Pow(tol/err, 0.25)*0.95, 2); // re-adjust stepsize
        } while(true);
    } // driver

    /* interpolant:
     * interpolate the solution of a numerically solved ODE
     * using cubic splines
     * uncommented arguments the same as driver (see above)
    public static Func<double, double[]> interpolant(
        Func<double, double[], double[]> f,
        (double, double) interval,
        double[] yi,
        double h=0.125,
        double acc=1E-1,
        double eps=1E-1,
        double nmax=999,
        double[] xs
    ) {
        (var xlist, var ylist) = driver(f, interval, yi, h, acc, eps);
        var interpolations = new List<double[]>();
        foreach(y in ylist) {
            spline = cubic_spline(xlist, y);
            for(i = 0; i < xs.Length; i++) {
                spline.evaluate(xs[i]);
            }
        }
        return 
    } */
} // ode
