using System;
using System.Collections.Generic;
using static System.Console;
using static System.Math;
using static ode;
public static class main {
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
    
    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-oscillator") oscillator(args);
            if(arg == "-lotkavolterra") lotkavolterra(args);
        }
        return 0;
    } // Main
} // main

