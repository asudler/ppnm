using System;
using System.Collections.Generic;
using static System.Console;
using static System.Math;

public static class main {
    public static Func<double, double> g = x
        => Cos(5*x - 1) * Exp(-x*x);

    public static void makedata(Func<double, double> f, string[] args) {
        double xi = 0, xf = 0; int n = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-xi") xi = double.Parse(fields[1]);
            if(fields[0] == "-xf") xf = double.Parse(fields[1]);
            if(fields[0] == "-n") n = int.Parse(fields[1]);
        }
        double dx = (xf - xi)/(n - 1);
        for(int i = 0; i < n; i++) {
            Write($"{xi}\t{f(xi)}\n");
            xi += dx;
        }
    } // makedata

    public static void taska(string[] args) {
        // get data
        List<double> x = new List<double>();
        List<double> y = new List<double>();
        var separators = new string[] {" ", "\t", ",", ":"};
        var options = StringSplitOptions.RemoveEmptyEntries;
        do {
            string line = Console.In.ReadLine();
            if(line == null) break;
            string[] fields = line.Split(separators, options);
            x.Add(double.Parse(fields[0]));
            y.Add(double.Parse(fields[1]));
        } while(true);

        // get input parameters from makefile
        int n_neurons = 0, n_out = 0;
        double xi = 0, xf = 0;
        foreach(string arg in args) {
            string[] fields = arg.Split(separators, options);
            if(fields[0] == "-n_neurons") n_neurons = int.Parse(fields[1]);
            if(fields[0] == "-n_out") n_out = int.Parse(fields[1]);
            if(fields[0] == "-xi") xi = double.Parse(fields[1]);
            if(fields[0] == "-xf") xf = double.Parse(fields[1]);
        }

        // train neural network
        ann swag = new ann(n_neurons);
        vector xv = new vector(x.ToArray()); 
        vector yv = new vector(y.ToArray());
        Error.Write($"{n_neurons} {n_out} {xi} {xf}\n");
        swag.train(xv, yv);
        double dx = (xf - xi)/(n_out - 1);
        Write($"nfev={swag.nfev}\n");
        for(int i = 0; i < n_out; i++) {
            Write($"{xi}\t{swag.response(xi, swag.p)}\n");
            xi += dx;
        }
    } // taska

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-makedata") makedata(g, args);
            if(arg == "-taska") taska(args);
//            if(arg == "-taskb") taskb(args);
//            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main
