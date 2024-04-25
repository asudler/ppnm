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
        double sol = quad(f1, 0, 1);
        if(Math.Abs(sol - 2.0/3.0) < 0.001) {
            Error.Write($"∫[0,1] dx √(x) = {sol}\n");
        }
        sol = quad(f2, 0, 1);
        if(Math.Abs(sol - 2.0) < 0.001) {
            Error.Write($"∫[0,1] dx 1/√(x) = {sol}\n");
        }
        sol = quad(f3, 0, 1);
        if(Math.Abs(sol - Math.PI) < 0.001) {
            Error.Write($"∫[0,1] dx 4√(1-x²) = {sol}\n");
        }
        sol = quad(f4, 0, 1);
        if(Math.Abs(sol + 4) < 0.001) {
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}\n");
        }

        // making the error function data
        // which will be written to stdout
        double abserr = 1E-6;
        double relerr = 1E-6;
        Func<double, double> erf = z => {
            if(z >= 0 && z <= 1) {
                Func<double, double> integrand = x => Math.Exp(-x*x);
                return 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, z, delta: abserr, eps: relerr);
            }
            if(z > 1) {
                Func<double, double> integrand = t
                    => Math.Exp(-Math.Pow((z + (1-t)/t),2))/t/t;
                return 1 
                    - 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, 1, delta: abserr, eps: relerr);
            }
            if(z >= -1 && z < 0) {
                Func<double, double> integrand = x => Math.Exp(-x*x);
                return -2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, -z, delta: abserr, eps: relerr);
            }
            if(z < -1) {
                Func<double, double> integrand = t
                    => Math.Exp(-Math.Pow((-z + (1-t)/t),2))/t/t;
                return -1*(1
                    - 2/Math.Sqrt(Math.PI)
                    * quad(integrand, 0, 1, delta: abserr, eps: relerr));
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
        double abserr = 1E-4, relerr = 1E-4;
        double sol = quad(f1, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol - 2) < abserr) {
            Error.Write($"quad ({ncalls} calls): ∫[0,1] dx 1/√(x) = {sol}\n");
        }
        ncalls = 0;
        sol = clenshaw_curtis(f1, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol - 2) < abserr) {
            Error.Write($"clenshaw_curtis ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx 1/√(x) = {sol}\n");
        }
        ncalls = 0;
        sol = quad(f2, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol + 4) < abserr) {
            Error.Write($"quad ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}\n");
        }
        ncalls = 0;
        sol = clenshaw_curtis(f2, 0, 1, delta: abserr, eps: relerr);
        if(Math.Abs(sol + 4) < abserr) {
            Error.Write($"clenshaw_curtis ({ncalls} calls): ");
            Error.Write($"∫[0,1] dx ln(x)/√(x) = {sol}\n");
        }
    } // taskb

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska(args);
            if(arg == "-taskb") taskb(args);
        }
        return 0;
    } // Main
} // main
