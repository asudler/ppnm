using System;
using static System.Console;
using static montecarlo;

public static class main {
    public static void taska(string[] args) {
        int n = 0;
        foreach(var arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-n") n = int.Parse(words[1]);
        }
        if(n == 0) throw new Exception("bad input args");
        
        Func<double[], double> f1 = x => {
            if(Math.Sqrt(x[0]*x[0] + x[1]*x[1]) <= 1) return 1;
            else return 0;
        }; // unit circle function


        // testing our monte carlo integration routine
        Error.Write($"montecarlo: plainmc tests (n = {n})\n");
        Error.Write($"unit circle area: ");
        double[] ll = {-1,-1}, ur = {1,1};
        (double sol, double err) = plainmc(f1, ll, ur, n);
        Error.Write($"sol = {sol}, err = {err} ");
        Error.Write($"(real err = {Math.Abs(Math.PI - sol)})\n");
    } // taska
    
    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska(args);
//            if(arg == "-taskb") taskb(args);
//            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main

