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
        Write("column1: t, column2: y, column3: dy\n");
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
        Write("column1: t, column2: x, column3: y\n");
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
        Write("column1: t, column2: y0, column3: y1\n");
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

    public static (double, double, double) accel(
        double[] r1, double[] r2, double[] r3,
        double m2=1, double m3=1, double G=1
    ) {
        if(r1.Length != 3 || r2.Length != 3 || r3.Length != 3)
            throw new Exception("bad dimensions");
        double factor2 = G*m2/Pow(Pow(r2[0] - r1[0], 2) +
            Pow(r2[1] - r1[1], 2) + Pow(r2[2] - r1[2], 2), 3.0/2);
        double factor3 = G*m2/Pow(Pow(r3[0] - r1[0], 2) +
            Pow(r3[1] - r1[1], 2) + Pow(r3[2] - r1[2], 2), 3.0/2);
        return (
            factor2*(r2[0] - r1[0]) + factor3*(r3[0] - r1[0]),
            factor2*(r2[1] - r1[1]) + factor3*(r3[1] - r1[1]),
            factor2*(r2[2] - r1[2]) + factor3*(r3[2] - r1[2])
        );
    } // 3-body 3-dimensional newtonian gravitational acceleration
    
    public static double[] derivs(
        double[] z, double m1=1, double m2=1, double m3=1, double G=1
    ) {
        var help1 = accel(
                new double[] {z[9], z[10], z[11]},
                new double[] {z[12], z[13], z[14]},
                new double[] {z[15], z[16], z[17]},
                m2, m3, G
            );
        var help2 = accel(
                new double[] {z[12], z[13], z[14]},
                new double[] {z[9], z[10], z[11]},
                new double[] {z[15], z[16], z[17]},
                m1, m3, G
            );
        var help3 = accel(
                new double[] {z[15], z[16], z[17]},
                new double[] {z[9], z[10], z[11]},
                new double[] {z[12], z[13], z[14]},
                m1, m2, G
            );
        return new double[] {
            help1.Item1, help1.Item2, help1.Item3,
            help2.Item1, help2.Item2, help2.Item3,
            help3.Item1, help3.Item2, help3.Item3,
            z[0], z[1], z[2], z[3], z[4], z[5], z[6], z[7], z[8]
        };
    } // 3-body accel-velocity state

    public static void threebody(string[] args) {
        double xi1 = 0, yi1 = 0, zi1 = 0, xi2 = 0, yi2 = 0, zi2 = 0,
            xi3 = 0, yi3 = 0, zi3 = 0, dxi1 = 0, dyi1 = 0, dzi1 = 0,
            dxi2 = 0, dyi2 = 0, dzi2 = 0, dxi3 = 0, dyi3 = 0, dzi3 = 0,
            ti = 0, tf = 0, m1 = 1, m2 = 1, m3 = 1, G = 1, dt = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-xi1") xi1 = double.Parse(fields[1]);
            if(fields[0] == "-yi1") yi1 = double.Parse(fields[1]);
            if(fields[0] == "-zi1") zi1 = double.Parse(fields[1]);
            if(fields[0] == "-xi2") xi2 = double.Parse(fields[1]);
            if(fields[0] == "-yi2") yi2 = double.Parse(fields[1]);
            if(fields[0] == "-zi2") zi2 = double.Parse(fields[1]);
            if(fields[0] == "-xi3") xi3 = double.Parse(fields[1]);
            if(fields[0] == "-yi3") yi3 = double.Parse(fields[1]);
            if(fields[0] == "-zi3") zi3 = double.Parse(fields[1]);
            if(fields[0] == "-dxi1") dxi1 = double.Parse(fields[1]);
            if(fields[0] == "-dyi1") dyi1 = double.Parse(fields[1]);
            if(fields[0] == "-dzi1") dzi1 = double.Parse(fields[1]);
            if(fields[0] == "-dxi2") dxi2 = double.Parse(fields[1]);
            if(fields[0] == "-dyi2") dyi2 = double.Parse(fields[1]);
            if(fields[0] == "-dzi2") dzi2 = double.Parse(fields[1]);
            if(fields[0] == "-dxi3") dxi3 = double.Parse(fields[1]);
            if(fields[0] == "-dyi3") dyi3 = double.Parse(fields[1]);
            if(fields[0] == "-dzi3") dzi3 = double.Parse(fields[1]);
            if(fields[0] == "-ti") ti = double.Parse(fields[1]);
            if(fields[0] == "-tf") tf = double.Parse(fields[1]);
            if(fields[0] == "-m1") m1 = double.Parse(fields[1]);
            if(fields[0] == "-m2") m2 = double.Parse(fields[1]);
            if(fields[0] == "-m3") m3 = double.Parse(fields[1]);
            if(fields[0] == "-G") G = double.Parse(fields[1]);
            if(fields[0] == "-dt") dt = double.Parse(fields[1]);
        }
        Func<double, double[], double[]> f = (t, z) 
            => derivs(z, m1, m2, m3, G);
        double[] y0_arr = new double[] {
            dxi1, dyi1, dzi1, dxi2, dyi2, dzi2, dxi3, dyi3, dzi3,
            xi1, yi1, zi1, xi2, yi2, zi2, xi3, yi3, zi3
        };
        if(dt == 0) {
            (List<double> ts, List<double[]> ys) 
                = ode.driver(f, (ti, tf), y0_arr);
            Write("column1: t, column2-10: dr1-dr3, column11-19: r1-r3\n");
            for(int i = 0; i < ts.Count; i++) {
                Write($"{ts[i]}\t");
                for(int j = 0; j < ys[0].Length; j++) Write($"{ys[i][j]}\t");
                Write("\n");
            }
        }
        else {
            List<cubic_spline> ys = ode.interpolant(f, (ti, tf), y0_arr);
            while(ti < tf) {
                Write($"{ti}\t");
                for(int i = 0; i < ys.Count; i++) 
                    Write($"{ys[i].evaluate(ti)}\t");
                Write("\n");
                ti += dt;
            }
            Write($"{tf}\t");
            for(int i = 0; i < ys.Count; i++) Write($"{ys[i].evaluate(tf)}\t");
            Write("\n");
        }
    } // threebody
    
    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-test") polynomial(args);
            if(arg == "-oscillator") oscillator(args);
            if(arg == "-lotkavolterra") lotkavolterra(args);
            if(arg == "-planet") planet(args);
            if(arg == "-threebody") threebody(args);
        }
        return 0;
    } // Main
} // main

