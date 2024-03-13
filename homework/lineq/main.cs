using System;
using static System.Console;
using static qrgs;
using static matrix;

public class main {
    public static int check_decomp() {
        Write("CHECKING DECOMP:\n");
        
        int n = 20; // n > m
        int m = 10; // tall matrix!

        Write($"initializing a(n) {n}x{m} matrix A with "); 
        Write("random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n,m);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < m; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Write("done\n");

        Write("factorizing A into QR...");
        var help = decomp(A);
        matrix Q = help.Item1;
        matrix R = help.Item2;
        Write("done\n");

        Write("checking that R is upper-triangular...");
        bool test = true;
        if(R.size1 != R.size2) test = false;
        for(int i = 1; i < R.size1; i++) {
            for(int j = 0; j < i; j++) {
                if(R[i,j] != 0) test = false;
            }
        }
        if(test) Write("passed\n");
        else Write("womp womp\n");

        Write("checking that Q.transpose()*Q = 1...");
        matrix identity = Q.transpose()*Q;
        test = true;
        for(int i = 0; i < identity.size1; i++) {
            if(!approx(identity[i,i], 1)) test = false;
        }
        if(test) Write("passed\n");
        else Write("womp womp\n");

        Write("checking that Q*R = A...");
        test = true;
        if(!A.approx(Q*R)) test = false;
        if(test) Write("passed\n");
        else Write("womp womp\n");

        WriteLine();
        return 0;
    } // check_decomp

    public static int check_solve() {
        Write("CHECKING SOLVE:\n");
        
        int n = 20; // n = m; square matrix!

        Write($"initializing a(n) {n}x{n} matrix A with ");
        Write("random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n,n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < n; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Write("done\n");

        Write("initializing a vector b of the same length with ");
        Write("random entries between 0 and 1...");
        vector b = new vector(A.size1);
        for(int i = 0; i < A.size1; i++) b[i] = rnd.NextDouble();
        Write("done\n");

        Write("checking solve() method (which ");
        Write("includes decomposing matrix A)...");
        vector sol = solve(A, b);
        vector check = decomp(A).Item1*decomp(A).Item2*sol;
        bool test = true;
        if(!b.approx(check)) test = false;
        if(test) Write("passed\n");
        else Write("womp womp\n");

        Write("checking that A*x = b...");
        test = true;
        if(!b.approx(A*sol)) test = false;
        if(test) Write("passed\n");
        else Write("womp womp\n");

        WriteLine();
        return 0;
    } // check_solve

    public static int check_inverse() {
        Write("CHECKING INVERSE:\n");

        int n = 20; // again, square matrix!

        Write($"initializing a(n) {n}x{n} matrix A with ");
        Write("random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n,n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < n; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Write("done\n");

        Write("calculating and checking inverse...");
        matrix B = inverse(A);
        matrix I = new matrix(A.size2, B.size1);
        I.set_identity();
        bool test = true;
        if(!I.approx(A*B)) test = false;
        if(test) Write("passed\n");
        else Write("womp womp\n");

        WriteLine();
        return 0;
    }

    public static int Main() {
        check_decomp();
        check_solve();
        check_inverse();

        return 0;
    } // Main
} // main
