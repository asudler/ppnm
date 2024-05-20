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

        // now we will see if the error scales with the
        // square root of the number of points by
        // calculating some wacky integral
        Func<double[], double> gamma = x => {
           return 1/(1 - Math.Cos(x[0])*Math.Cos(x[1])
               *Math.Cos(x[2]))/Math.Pow(Math.PI, 3); 
        }; // gamma-ish function

        double realans = 1.393203929685676;
        double[] zero = {0,0,0}, 
            pis = {Math.PI, Math.PI, Math.PI};
        for(int i = nmin; i <= nmax; i += skip) {
            (sol, err) = plainmc(gamma, zero, pis, i);
            Write($"{i}\t{sol}\t{err}\t{Math.Abs(sol - realans)}\n");
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

        // calculating some wacky integral
        Func<double[], double> gamma = x => {
           return 1/(1 - Math.Cos(x[0])*Math.Cos(x[1])
               *Math.Cos(x[2]))/Math.Pow(Math.PI, 3); 
        }; // gamma-ish function

        double[] corner1 = {1,1,1}, corner2 = {3, 3, 3};
        Write("columns are: ");
        Write("(1) n, ");
        Write("(2) plain mc sol, ");
        Write("(3) halton mc sol, ");
        Write("(4) lattice mc sol, ");
        Write("(5) plain mc err, ");
        Write("(6) quasi mc method difference (err)\n");
        for(int i = nmin; i <= nmax; i += skip) {
            (double sol, double perr) = plainmc(gamma, corner1, corner2, i);
            (solh, soll, err) = quasimc(gamma, corner1, corner2, i);
            Write($"{i}\t{sol}\t{solh}\t{soll}\t");
            Write($"{err}\t{Math.Abs(solh - soll)}\n");
        }
    } // taskb
    
    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska(args);
            if(arg == "-taskb") taskb(args);
//            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main

