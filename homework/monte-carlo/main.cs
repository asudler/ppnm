using System;
using static System.Console;
using static montecarlo;

public static class main {
    public static void taska(string[] args) {
        int n = 0, nmax = 0, nmin = 0, skip = 0;
        foreach(var arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-n") n = int.Parse(words[1]);
            if(words[0] == "-nmax") nmax = int.Parse(words[1]);
            if(words[0] == "-nmin") nmin = int.Parse(words[1]);
            if(words[0] == "-skip") skip = int.Parse(words[1]);
        }
        if(n == 0 || nmax == 0) throw new Exception("bad input args");
        
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

        // testing our plain monte carlo integration routine
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

        // calculating some wacky integral
        Func<double[], double> gamma = x => {
           return 1/(1 - Math.Cos(x[0])*Math.Cos(x[1])
               *Math.Cos(x[2]))/Math.Pow(Math.PI, 3); 
        }; // gamma-ish function

        double[] zero = {0,0,0}, 
            pis = {Math.PI, Math.PI, Math.PI};
        Error.Write($"some wacky gamma-ish function: ");
        (sol, err) = plainmc(gamma, zero, pis, n);
        Error.Write($"sol = {sol}, err = {err} ");
        double realans = 1.393203929685676;
        Error.Write($"(real err = {Math.Abs(sol - realans)})\n");

        // we should see how error scales with n
        // perhaps use the 2D Gaussian function...?
        Console.Write("columns are: ");
        Console.Write("(1) n, ");
        Console.Write("(2) plainmc sol, ");
        Console.Write("(3) plainmc err, ");
        Console.Write("(4) real err\n");
        for(int i = nmin; i <= nmax; i += skip) {
            (sol, err) = plainmc(f2, ll, ur, i);
            Console.Write($"{i}\t{sol}\t{err}\t{Math.Abs(sol - help)})\n");
        }
    } // taska

    public static void taskb(string[] args) {
        int n = 0, nmax = 0, nmin = 0, skip = 0;
        foreach(var arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-n") n = int.Parse(words[1]);
            if(words[0] == "-nmax") nmax = int.Parse(words[1]);
            if(words[0] == "-nmin") nmin = int.Parse(words[1]);
            if(words[0] == "-skip") skip = int.Parse(words[1]);
        }
        if(n == 0 || nmax == 0) throw new Exception("bad input args");
        
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

        // testing our quasi monte carlo integration routine
        Error.Write($"montecarlo: quasimc tests (n = {n})\n");
        Error.Write($"unit circle area: ");
        double[] ll = {-1,-1}, ur = {1,1};
        (double solh, double soll, double err) = quasimc(f1, ll, ur, n);
        Error.Write($"solh = {solh}, soll = {soll}, err = {err} ");
        Error.Write($"(real errh = {Math.Abs(Math.PI - solh)}, ");
        Error.Write($"real errl = {Math.Abs(Math.PI - soll)})\n"); 
        Error.Write($"2D Gaussian in unit circle: ");
        (solh, soll, err) = quasimc(f2, ll, ur, n);
        Error.Write($"solh = {solh}, soll = {soll}, err = {err} ");
        double help = 2*(1 - 1/Math.Sqrt(Math.E))*Math.PI;
        Error.Write($"(real errh = {Math.Abs(solh - help)}, ");
        Error.Write($"real errl = {Math.Abs(soll - help)})\n");
        Error.Write($"2x2 cube (like 4 minecraft blocks): ");
        (solh, soll, err) = quasimc(f3, ll, ur, n);
        Error.Write($"solh = {solh}, soll = {soll}, err = {err} ");
        Error.Write($"(real errh = {Math.Abs(solh - 4)}, ");
        Error.Write($"real errl = {Math.Abs(soll - 4)})\n");

        // we should see how error scales with n
        // perhaps use the 2D Gaussian function...?
        Console.Write("columns are: ");
        Console.Write("(1) n, ");
        Console.Write("(2) haltonmc sol, ");
        Console.Write("(3) latticemc sol, ");
        Console.Write("(4) quasimc method difference (err), ");
        Console.Write("(5) real haltonmc err, ");
        Console.Write("(5) real latticemc err\n");
        for(int i = nmin; i <= nmax; i += skip) {
            (solh, soll, err) = quasimc(f2, ll, ur, i);
            Console.Write($"{i}\t{solh}\t{soll}\t");
            Console.Write($"{Math.Abs(solh - soll)}\t");
            Console.Write($"{Math.Abs(solh - help)}\t");
            Console.Write($"{Math.Abs(soll - help)}\n");
        }
    } // taskb

    public static void taskc(string[] args) {
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

        // testing our stratified monte carlo integration routine
        Error.Write($"montecarlo: stratmc tests (n = {n})\n");
        Error.Write($"unit circle area: ");
        double[] ll = {-1.2,-1.2}, ur = {1.2,1.2};
        (double sol, double err) = stratmc(f1, ll, ur, n, true);
        Error.Write($"sol = {sol}, err = {err} ");
        Error.Write($"(real err = {Math.Abs(Math.PI - sol)})\n");
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

