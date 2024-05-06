using System;
using System.Collections.Generic;
using static System.Console;
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
     * solve ode with an adaptive step size */
    public static (List<double>, List<double[]>) driver(
        Func<double, double[], double[]> F, // f from dy/dx = f(x,y)
        (double, double) interval, // x start point to x end point
        double[] yi, // y value at start point
        double h=0.125, // initial dx
        double acc=1E-3, // absolute accuracy goal
        double eps=1E-3, // relative accuracy goal
        string method="rk45" // integration algorithm
    ) {
        var (a, b) = interval; double x = a; double[] y = yi;
        var xlist = new List<double>(); xlist.Add(x);
        var ylist = new List<double[]>(); ylist.Add(y);
        if(method == "rk45") do {
            if(x >= b) return (xlist, ylist); // job done, breaks loop
            if(x + h > b) h = b - x; // last step should end at b
            var (yh, yerr) = rkstep45(F, x, y, h);
//            double tol = (acc + eps*norm(yh))*Sqrt(h/(b - a));
            // (scipy tol, less rigid, see below)
            double tol = acc + eps*norm(yh);
            double err = norm(yerr);
            Error.Write($"ode: err={err} tol={tol} h={h}\n");
            if(err <= tol) { // accept step
                x += h; y = yh;
                xlist.Add(x);
                ylist.Add(y);
            }
            h *= Min(Pow(tol/err, 0.25)*0.95, 2); // re-adjust stepsize
        } while(true);
        if(method == "rk12") do {
            if(x >= b) return (xlist, ylist); // job done, breaks loop
            if(x + h > b) h = b - x; // last step should end at b
            var (yh, yerr) = rkstep12(F, x, y, h);
            double tol = (acc + eps*norm(yh))*Sqrt(h/(b - a));
            // (scipy tol, less rigid, see below)
            // double tol = acc + eps*norm(yh);
            double err = norm(yerr);
            Error.Write($"ode: err={err} tol={tol} h={h}\n");
            if(err <= tol) { // accept step
                x += h; y = yh;
                xlist.Add(x);
                ylist.Add(y);
            }
            h *= Min(Pow(tol/err, 0.25)*0.95, 2); // re-adjust stepsize
        } while(true);
        else throw new Exception("invalid method specification");
    } // driver

    /* interpolant:
     * returns cubic spline interpolant of ode driver data */
    public static List<cubic_spline> interpolant(
        Func<double, double[], double[]> f,
        (double, double) interval,
        double[] yi,
        double h=0.125,
        double acc=1E-3,
        double eps=1E-3,
        string method="rk45"   
    ) {
        (var xlist, var ylist) = driver(f, interval, yi, h, acc, eps, method);
        double[] xs = xlist.ToArray();
        var help = new List<cubic_spline>();
        for(int i = 0; i < ylist[0].Length; i++) {
            double[] ys = new double[xs.Length];
            for(int j = 0; j < ylist.Count; j++) {
                ys[j] = ylist[j][i];
            }
            cubic_spline cspline = new cubic_spline(xs, ys);
            help.Add(cspline);
        }
        return help;
    } // interpolant
} // ode
