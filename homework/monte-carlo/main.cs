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

        Func<double[], double> f2 = x => {
            if(Math.Sqrt(x[0]*x[0] + x[1]*x[1]) <= 1) 
                return Math.Exp(-(x[0]*x[0] + x[1]*x[1])/2);
            else return 0;
        }; // 2D Gaussian function

        Func<double[], double> f3 = x => {
            return 1;
        }; // cube function (interesting enough!)

        // testing our monte carlo integration routine
        Error.Write($"montecarlo: plainmc tests (n = {n})\n");
        Error.Write($"unit circle area: ");
        double[] ll = {-1,-1}, ur = {1,1};
        (double sol, double err) = plainmc(f1, ll, ur, n);
        Error.Write($"sol = {sol}, err = {err} ");
        Error.Write($"(real err = {Math.Abs(Math.PI - sol)})\n");
        Error.Write($"2D Gaussian in unit circle: ");
        (sol, err) = plainmc(f2, ll, ur, n);
        Error.Write($"sol = {sol}, err = {err} ");
        double help = 2*(1 - 1/Math.Sqrt(Math.E))*Math.PI;
        Error.Write($"(real err = {Math.Abs(sol - help)})\n");
        Error.Write($"2x2 cube (like 4 minecraft blocks): ");
        (sol, err) = plainmc(f3, ll, ur, n);
        Error.Write($"sol = {sol}, err = {err} ");
        Error.Write($"(real err = {Math.Abs(sol - 4)})\n");
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

