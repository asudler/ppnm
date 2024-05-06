using System;
using System.Collections.Generic;
using static System.Console;
using static System.Math;
using static ode;
using static spline;
public static class main {
    public static void polynomial(string[] args) {
        Func<double, double[], double[]> f0 = (x, y)
            => new double[] {0};
        Func<double, double[], double[]> f1 = (x, y)
            => new double[] {1};
        Func<double, double[], double[]> f2 = (x, y)
            => new double[] {2*x};
        Func<double, double[], double[]> f3 = (x, y)
            => new double[] {3*x*x};
        Func<double, double[], double[]> f4 = (x, y)
            => new double[] {4*x*x*x};
        double[] y0_arr = new double[] {0};
        (List<double> xs, List<double[]> ys) = ode.driver(f4, (0, 5), y0_arr);
        for(int i = 0; i < xs.Count; i++) {
            Write($"{xs[i]}\t{ys[i][0]}\t\n");
        }
    } // polynomial, used for testing

    public static void oscillator(string[] args) {
        double b = 0, c = 0, yi = 0, dyi = 0, ti = 0, tf = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-b") b = double.Parse(fields[1]);
            if(fields[0] == "-c") c = double.Parse(fields[1]);
            if(fields[0] == "-yi") yi = double.Parse(fields[1]);
            if(fields[0] == "-dyi") dyi = double.Parse(fields[1]);
            if(fields[0] == "-ti") ti = double.Parse(fields[1]);
            if(fields[0] == "-tf") tf = double.Parse(fields[1]);
        }
        Func<double, double[], double[]> f = (x, y)
            => new double[] {y[1], -b*y[1] - c*Sin(y[0])};
        double[] y0_arr = new double[] {yi, dyi};
        (List<double> xs, List<double[]> ys) = ode.driver(f, (ti, tf), y0_arr);
        for(int i = 0; i < xs.Count; i++) {
            Write($"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n");
        }
    } // oscillator

    public static void lotkavolterra(string[] args) {
        double a = 0, b = 0, c = 0, d = 0, xi = 0, yi = 0, ti = 0, tf = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-a") a = double.Parse(fields[1]);
            if(fields[0] == "-b") b = double.Parse(fields[1]);
            if(fields[0] == "-c") c = double.Parse(fields[1]);
            if(fields[0] == "-d") d = double.Parse(fields[1]);
            if(fields[0] == "-xi") xi = double.Parse(fields[1]);
            if(fields[0] == "-yi") yi = double.Parse(fields[1]);
            if(fields[0] == "-ti") ti = double.Parse(fields[1]);
            if(fields[0] == "-tf") tf = double.Parse(fields[1]);
        }
        Func<double, double[], double[]> f = (x, y)
            => new double[] {a*y[0] - b*y[0]*y[1], -c*y[1] + d*y[0]*y[1]};
        double[] y0_arr = new double[] {xi, yi};
        (List<double> xs, List<double[]> ys) = ode.driver(f, (ti, tf), y0_arr);
        for(int i = 0; i < xs.Count; i++) {
            Write($"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n");
        }
    } // lotkavolterra

    public static void planet(string[] args) {
        double greps = 0, yi = 0, dyi = 0, ti = 0, tf = 0, dt = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-dt") dt = double.Parse(fields[1]);
            if(fields[0] == "-ti") ti = double.Parse(fields[1]);
            if(fields[0] == "-tf") tf = double.Parse(fields[1]);
            if(fields[0] == "-yi") yi = double.Parse(fields[1]);
            if(fields[0] == "-dyi") dyi = double.Parse(fields[1]);
            if(fields[0] == "-greps") greps = double.Parse(fields[1]);
        }
        Func<double, double[], double[]> f = (x, y) => new double[] {
            y[1], 1 - y[0] + greps*y[0]*y[0]
        };
        double[] y0_arr = new double[] {yi, dyi};
        List<cubic_spline> ys = ode.interpolant(f, (ti, tf), y0_arr);
        double t = 0;
        while(t < tf) {
            Write($"{t}\t");
            for(int i = 0; i < ys.Count; i++) Write($"{ys[i].evaluate(t)}\t");
            Write("\n");
            t += dt;
        }
        Write($"{tf}\t");
        for(int i = 0; i < ys.Count; i++) Write($"{ys[i].evaluate(tf)}\t");
        Write("\n");
    } // planet
    
    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-test") polynomial(args);
            if(arg == "-oscillator") oscillator(args);
            if(arg == "-lotkavolterra") lotkavolterra(args);
            if(arg == "-planet") planet(args);
        }
        return 0;
    } // Main
} // main

