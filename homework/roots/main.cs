using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;
using static roots;
using static ode;

public static class main {
    public static Func<vector, vector> f_cubic = x 
        => new vector(1 + x[0]*x[0]*x[0]); // f(x) = 1 + x^3
    public static Func<vector, vector> f_2d = x
        => new vector(x[0] + x[1]*x[1] - 5, x[0] + x[1] - 3); // 2d function
    public static Func<vector, vector> df_rosenbrock = x
        => new vector(-2*(1 - x[0]) - 400*x[0]*(-x[0]*x[0] + x[1]),
                200*(-x[0]*x[0] + x[1])); // gradient of Rosenbrock function
    public static Func<vector, vector> df_himmelblau = x
        => new vector(4*x[0]*(-11 + x[0]*x[0] + x[1]) 
                + 2*(-7 + x[0] + x[1]*x[1]), 2*(-11 + x[0]*x[0] + x[1])
                + 4*x[1]*(-7 + x[0] + x[1]*x[1])); // grad of Himmelblau fcn

    public static void taska() {
        vector start1d = new vector(1.0);
        Error.Write("taska: debugging newton's method\n");
        Error.Write("f(x) = 1 + x^3: ");
        vector roots = newton(f_cubic, start1d);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at x = -1)\n");
        vector start2d = new vector(-1.0, -1.0);
        start2d[0] = 1; start2d[1] = 1;
        Error.Write("f(x) = (x^2 + y^2, x + 3): ");
        roots = newton(f_2d, start2d);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at (x,y) = (1,2))\n");
        Error.Write("finding the minimum of the 2D Rosenbrock function: ");
        roots = newton(df_rosenbrock, start2d); // find root of derivative
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at (x,y) = (1,1))\n");
        Error.Write("finding the minima of the Himmelblau fcn, ");
        Error.Write("of which there are four; therefore, we ought to ");
        Error.Write("make four separate trials beginning in the vicinity ");
        Error.Write("of a known solution. ");
        Error.Write("the known solutions are (3,2), (-2.805,3.131), ");
        Error.Write("(-3.780,-3.283), and (3.584,-1.848). ");
        Error.Write("(sol1): ");
        vector hstart = new vector(4,3);
        roots = newton(df_himmelblau, hstart);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol2): ");
        hstart[0] = -2.8; hstart[1] = 3.1;
        roots = newton(df_himmelblau, hstart);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol3): ");
        hstart[0] = -4; hstart[1] = -3;
        roots = newton(df_himmelblau, hstart);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol4): ");
        hstart[0] = 3.5; hstart[1] = -2;
        roots = newton(df_himmelblau, hstart);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("\n");
    } // taska

    public static double[] derivs(double r, double[] f, double energy) {
        return new double[] {f[1], -2*(energy + 1/r)*f[0]};
    } // derivs

    public static double aux
    (double rmin, double rmax, double energy, 
     double acc=1E-3, double eps=1E-3) {
        Func<double, double[], double[]> y = (r, f)
            => derivs(r, f, energy);
        double[] ic = new double[] {rmin - rmin*rmin, 1 - 2*rmin};
        (List<double> rs, List<double[]> sols)
            = ode.driver(y, (rmin, rmax), ic, acc:acc, eps:eps);
        return sols[rs.Count - 1][0];
    } // aux

    public static void taskb(string[] args) {
        double rmin = 0, rmax = 0, guess = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-rmin") rmin = double.Parse(fields[1]);
            if(fields[0] == "-rmax") rmax = double.Parse(fields[1]);
            if(fields[0] == "-guess") guess = double.Parse(fields[1]);
        }
        if(rmin == 0 || rmax == 0 || guess == 0) {
            throw new Exception("bad input args");
        }
        Func<vector,vector> help = x
            => new vector(aux(rmin, rmax, x[0]));
        vector start = new vector(guess);
        vector roots = newton(help, start);
        Error.Write($"root found at E0 = {roots[0]}\n");
        Func<double, double[], double[]> y = (r, f)
            => derivs(r, f, roots[0]);
        double[] ic = new double[] {rmin - rmin*rmin, 1 - 2*rmin};
        (List<double> rs, List<double[]> sols)
            = ode.driver(y, (rmin, rmax), ic);
        Write("r,E,dE\n");
        for(int i = 0; i < rs.Count; i++) {
            Write($"{rs[i]},");
            for(int j = 0; j < sols[0].Length; j++) Write($"{sols[i][j]},");
            Write("\n");
        }

        // checking convergence
        string fname1 = "out.taskb.convradii.dat",
               fname2 = "out.taskb.convacc.dat";
        if(!File.Exists(fname1)) {
            File.AppendAllText(fname1, "rmin,rmax,E0\n");
            double rminmin = 0.0025, rminmax = 0.25,
                rmaxmin = 3, rmaxmax = 15;
            int gridsize = 40;
            for(int i = 0; i <= gridsize; i++) {
                double rmin2 = rminmin*Math.Pow(rminmax/rminmin, 
                    (double)i/gridsize);
                for(int j = 0; j <= gridsize; j++) {
                    double rmax2 = rmaxmin*Math.Pow(rmaxmax/rmaxmin, 
                        (double)j/gridsize);
                    Func<vector,vector> help2 = x
                        => new vector(aux(rmin2, rmax2, x[0]));
                    vector roots2 = newton(help2, start);
                    File.AppendAllText(
                        fname1, $"{rmin2},{rmax2},{roots2[0]}\n");
                }
                File.AppendAllText(fname1, "\n");
            }
        }
        if(!File.Exists(fname2)) {
            File.AppendAllText(fname2, "acc,eps,E0\n");
            int grid = 40;
            for(int i = grid; i >= 0; i--) {
                double acc = Math.Pow(10, -i/10.0);
                for(int j = grid; j >= 0; j--) {
                    double eps = Math.Pow(10, -j/10.0);
                    Func<vector,vector> help3 = x
                        => new vector(aux(rmin, rmax, x[0], acc, eps));
                    vector roots3 = newton(help3, start);
                    File.AppendAllText(
                        fname2, $"{acc},{eps},{roots3[0]}\n");
                }
                File.AppendAllText(fname2, "\n");
            }
        }
    } // taskb

    public static void taskc() {
        vector start1d = new vector(1.0);
        Error.Write("taskc: debugging newton's method, ");
        Error.Write("now with new epic quadratic minimization\n");
        Error.Write("f(x) = 1 + x^3: ");
        vector roots = newton(f_cubic, start1d, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at x = -1)\n");
        vector start2d = new vector(-1.0, -1.0);
        start2d[0] = 1; start2d[1] = 1;
        Error.Write("f(x) = (x^2 + y^2, x + 3): ");
        roots = newton(f_2d, start2d, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at (x,y) = (1,2))\n");
        Error.Write("finding the minimum of the 2D Rosenbrock function: ");
        roots = newton(df_rosenbrock, start2d, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(known sol at (x,y) = (1,1))\n");
        Error.Write("finding the minima of the Himmelblau fcn, ");
        Error.Write("of which there are four; therefore, we ought to ");
        Error.Write("make four separate trials beginning in the vicinity ");
        Error.Write("of a known solution. ");
        Error.Write("the known solutions are (3,2), (-2.805,3.131), ");
        Error.Write("(-3.780,-3.283), and (3.584,-1.848). ");
        Error.Write("(sol1): ");
        vector hstart = new vector(4,3);
        roots = newton(df_himmelblau, hstart, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol2): ");
        hstart[0] = -2.8; hstart[1] = 3.1;
        roots = newton(df_himmelblau, hstart, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol3): ");
        hstart[0] = -4; hstart[1] = -3;
        roots = newton(df_himmelblau, hstart, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("(sol4): ");
        hstart[0] = 3.5; hstart[1] = -2;
        roots = newton(df_himmelblau, hstart, quadmin:true);
        for(int i = 0; i < roots.size; i++) {
            Error.Write($"roots[{i}] = {roots[i]} ");
        }
        Error.Write("\n");
    } // taskc

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska();
            if(arg == "-taskb") taskb(args);
            if(arg == "-taskc") taskc();
        }
        return 0;
    } // Main
} // main

