using System;
using static System.Console;
using static integrate;

public static class main {
    public static void taska(string[] args) {
        Func<double, double> f1 = x => Math.Sqrt(x);
        Func<double, double> f2 = x => 1/Math.Sqrt(x);
        Func<double, double> f3 = x => 4*Math.Sqrt(1 - x*x);
        Func<double, double> f4 = x => Math.Log(x)/Math.Sqrt(x);

        // the default accuracy goal is 0.001,
        // so we will check that our numerical results
        // are within 0.001 of the analytical value
        // before writing to stderr
        Error.Write("abserr = relerr = 1E-3 (default)\n");
        (double sol, double err) = quad(f1, 0, 1);
        if(Math.Abs(sol - 2.0/3.0) < 0.001) {
            Error.Write($"∫[0,1] dx √(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        (sol, err) = quad(f2, 0, 1);
        if(Math.Abs(sol - 2.0) < 0.001) {
            Error.Write($"∫[0,1] dx 1/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        (sol, err) = quad(f3, 0, 1);
        if(Math.Abs(sol - Math.PI) < 0.001) {
            Error.Write($"∫[0,1] dx 4√(1-x²) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        (sol, err) = quad(f4, 0, 1);
        if(Math.Abs(sol + 4) < 0.001) {
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }

        // making the error function data
        // which will be written to stdout
        // reducing error for maximal sexiness
        double abserr = 1E-6;
        double relerr = 1E-6;
        Func<double, double> erf = z => {
            if(z >= 0 && z <= 1) {
                Func<double, double> integrand = x => Math.Exp(-x*x);
                return 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, z, delta:abserr, eps:relerr).Item1;
            }
            if(z > 1) {
                Func<double, double> integrand = t
                    => Math.Exp(-Math.Pow((z + (1-t)/t),2))/t/t;
                return 1 
                    - 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, 1, delta:abserr, eps:relerr).Item1;
            }
            if(z >= -1 && z < 0) {
                Func<double, double> integrand = x => Math.Exp(-x*x);
                return -2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, -z, delta:abserr, eps:relerr).Item1;
            }
            if(z < -1) {
                Func<double, double> integrand = t
                    => Math.Exp(-Math.Pow((-z + (1-t)/t),2))/t/t;
                return -1*(1
                    - 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, 1, delta:abserr, eps:relerr).Item1);
            }
            else {
                return 0;
            }
        };

        // determine how much data we want to generate
        // from makefile input
        int n = 0;
        double start = 0, end = 0;
        foreach(var arg in args) {
            var words = arg.Split(':');
            if(words[0] == "-n") n = int.Parse(words[1]);
            if(words[0] == "-start") start = double.Parse(words[1]);
            if(words[0] == "-end") end = double.Parse(words[1]);
        }
        if(n == 0 || start == 0 || end == 0) 
            throw new Exception("bad input args");
        double dz = Math.Abs(start - end)/(n - 1);
        for(int i = 0; i < n; i++) {
            Write($"{start}\t{erf(start)}\n");
            start += dz;
        }
    } // taska

    public static void taskb(string[] args) {
        int ncalls = 0;
        Func<double, double> f1 = x => {
            ncalls++; 
            return 1/Math.Sqrt(x);
        };
        Func<double, double> f2 = x => {
            ncalls++; 
            return Math.Log(x)/Math.Sqrt(x);
        };

        // similar tests as in taska,
        // i.e. check known integrals and errors
        // but we will use lower errors than taska
        Error.Write("### C# Implementation ###\n");
        double abserr = 1E-4, relerr = 1E-4;
        Error.Write($"abserr = {abserr}, relerr = {relerr}\n");
        (double sol, double err) = quad(f1, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol - 2) < err) {
            Error.Write($"quad ({ncalls} calls): ∫[0,1] dx 1/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        ncalls = 0;
        (sol, err) = clenshaw_curtis(f1, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol - 2) < err) {
            Error.Write($"clenshaw_curtis ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx 1/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        ncalls = 0;
        (sol, err) = quad(f2, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol + 4) < err) {
            Error.Write($"quad ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        ncalls = 0;
        (sol, err) = clenshaw_curtis(f2, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol + 4) < err) {
            Error.Write($"clenshaw_curtis ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}, ");
            Error.Write($"error estimate = {err}\n\n");
        }
    } // taskb

    public static void taskc(string[] args) {
        int ncalls = 0;
        Func<double, double> f1 = x => {
            ncalls++;
            return 1/x/x;
        };
        Func<double, double> f2 = x => {
            ncalls++;
            return 2/(1 + x*x);
        };
        Func<double, double> f3 = x => {
            ncalls++;
            return Math.Exp(-x*x);
        };

        // checking known integrals
        // a la taskb...
        Error.Write("### C# Implementation ###\n");
        double abserr = 1E-4, relerr = 1E-4;
        Error.Write($"abserr = {abserr}, relerr = {relerr}\n");
        (double sol, double err) 
            = quad(f1,1,double.PositiveInfinity,delta:abserr,eps:relerr);
        if(Math.Abs(sol - 1) < err) {
            Error.Write($"quad ({ncalls} calls): ∫[1,inf] dx 1/x/x = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        (sol, err)
            = quad(f2,double.NegativeInfinity,0,delta:abserr,eps:relerr);
        if(Math.Abs(sol - Math.PI) < err) {
            Error.Write($"quad ({ncalls} calls): ");
            Error.Write($"∫[-inf,0] dx 2/(1+x*x) = {sol}, ");
            Error.Write($"error estimate = {err}\n");
        }
        (sol, err)
            = quad(f3,double.NegativeInfinity,double.PositiveInfinity,
              delta:abserr,eps:relerr);
        if(true) { // our error estimate doesn't work so well here...
            Error.Write($"quad ({ncalls} calls): ");
            Error.Write($"∫[-inf,inf] dx exp(-x*x) = {sol}, ");
            Error.Write($"error estimate = {err}\n\n");
        }
    } // taskc

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska(args);
            if(arg == "-taskb") taskb(args);
            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main
