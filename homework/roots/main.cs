using System;
using static System.Console;
using static roots;

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

    public static void taska(string[] args) {
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

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska(args);
//            if(arg == "-taskb") taskb(args);
//            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main

