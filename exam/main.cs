using System;
using static System.Console;
using static lu;
using static matrix;

public class main {
    public static void debug(string[] args) {
        int n = 0; bool row_pivots = false;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-n") n = int.Parse(fields[1]);
            if(fields[0] == "-row_pivots") row_pivots = bool.Parse(fields[1]);
        }
        if(n == 0) throw new Exception("specify matrix dims. in makefile\n");

        Error.Write($"debug: checking lu decomposition routine below\n");
        Error.Write($"initializing a real {n}x{n} matrix A with ");
        Error.Write($"random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n,n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < n; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        if(row_pivots) A[0,0] = 0;
        Error.Write("done\n");

        Error.Write("determining the nature of the permutation matrix P...");
        if(approx(A[0,0], 0.0)) {
            Error.Write("permutation matrix P needed, i.e. ");
            Error.Write("P != I\n");
        }
        else {
            Error.Write("permutation matrix P unnecessary, i.e. ");
            Error.Write("P = I\n");
        }

        Error.Write("factorizing P*A into LU...");
        (matrix L, matrix U, matrix P) = decomp(A);
        Error.Write("done\n");

        Error.Write("checking that L is lower-triangular...");
        bool test = true;
        if(L.size1 != L.size2) test = false;
        for(int i = 0; i < n; i++) {
            for(int j = i + 1; j < n; j++) {
                if(L[i,j] != 0) test = false;
            }
        }
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that U is upper-triangular...");
        test = true;
        if(U.size1 != U.size2) test = false;
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < i - 1; j++) {
                if(U[i,j] != 0) test = false;
            }
        }
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that L*U = P*A...");
        test = true;
        if(!(P*A).approx(L*U)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking solve below\n");
        Error.Write("initializing a vector b of the same length as P*A with ");
        Error.Write("random entries between 0 and 1...");
        vector b = new vector(n);
        for(int i = 0; i < n; i++) b[i] = rnd.NextDouble();
        Error.Write("done\n");

        Error.Write("checking solve method (which ");
        Error.Write("includes decomposing matrix P*A)...");
        vector sol = solve(A, b).Item1;
        vector check = decomp(A).Item1*decomp(A).Item2*sol;
        test = true;
        if(!b.approx(check)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that P*A*x = b...");
        test = true;
        if(!b.approx(P*A*sol)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking inverse below\n");
        Error.Write("calculating and checking inverse of P*A...");
        matrix B = inverse(A).Item1;
        matrix I = matrix.id(n);
        test = true;
        if(!I.approx(P*A*B)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking determinant properties below\n");
        Error.Write("det(A)*det(A^{-1}) = 1...");
        test = true;
        if(!approx(determinant(A)*determinant(B), 1.0)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(c*A) = c^n*det(A)...");
        test = true;
        double c = rnd.NextDouble();
        if(!approx(determinant(c*A), Math.Pow(c,n)*determinant(A))) 
            test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(A) = 1/det(A^{-1})...");
        test = true;
        if(!approx(determinant(A), 1/determinant(B))) 
            test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();
    } // debug
    
    public static void time(string[] args) {
        int n = 0;
        foreach(string arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-n") n = int.Parse(words[1]); 
        }
        Error.Write($"size={n}...");

        Random rnd = new Random();
        matrix A = new matrix(n,n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < n; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Error.Write("decomposing...");
        decomp(A);
        Error.Write("done\n");
    } // time

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-debug") debug(args);
            if(arg == "-time") time(args);
        }
        return 0;
    } // Main
} // main
