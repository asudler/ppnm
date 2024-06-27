using System;
using static System.Console;
using static lu;

public class main {
    public static void debug(string[] args) {
        int n = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-n") n = int.Parse(fields[1]);
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
        Error.Write("done\n");

        Error.Write("factorizing A into LU...");
        (matrix L, matrix U) = decomp(A);
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

        Error.Write("checking that L*U = A...");
        test = true;
        if(!A.approx(L*U)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking solve below\n");
        Error.Write("initializing a vector b of the same length as A with ");
        Error.Write("random entries between 0 and 1...");
        vector b = new vector(n);
        for(int i = 0; i < n; i++) b[i] = rnd.NextDouble();
        Error.Write("done\n");

        Error.Write("checking solve method (which ");
        Error.Write("includes decomposing matrix A)...");
        vector sol = solve(A, b);
        vector check = decomp(A).Item1*decomp(A).Item2*sol;
        test = true;
        if(!b.approx(check)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that A*x = b...");
        test = true;
        if(!b.approx(A*sol)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking inverse below\n");
        Error.Write("calculating and checking inverse of A...");
        matrix B = inverse(A);
        matrix I = new matrix(n, n);
        I.set_identity();
        test = true;
        if(!I.approx(A*B)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking determinant properties below\n");
        Error.Write("det(A)*det(A^{-1}) = 1...");
        test = true;
        if(1.0 != Math.Round(determinant(A)*determinant(B),4)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(c*A) = c^n*det(A)...");
        test = true;
        double c = rnd.NextDouble();
        if(Math.Round(determinant(c*A),4) 
            != Math.Round(Math.Pow(c,n)*determinant(A),4)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(A) = 1/det(A^{-1})...");
        test = true;
        if(Math.Round(determinant(A),1)
            != Math.Round(1/determinant(B),1)) test = false;
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
