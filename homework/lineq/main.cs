using System;
using static System.Console;
using static qrgs;

public class main {
    public static void debug(string[] args) {
        int n = 0, m = 0;
        foreach(string arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-m") m = int.Parse(fields[1]);
            if(fields[0] == "-n") n = int.Parse(fields[1]);
        }
        if(n == 0) throw new Exception("specify matrix dims. in makefile\n");
        if(n < m) throw new Exception("matrix not tall (n < m)\n");

        Error.Write($"debug: checking lu decomposition routine below\n");
        Error.Write($"initializing a real {n}x{m} matrix A with ");
        Error.Write($"random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n,m);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < m; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Error.Write("done\n");

        Error.Write("factorizing A into QR...");
        (matrix Q, matrix R) = decomp(A);
        Error.Write("done\n");

        Error.Write("checking that R is upper-triangular...");
        bool test = true;
        if(R.size1 != R.size2) test = false;
        for(int i = 1; i < R.size1; i++) {
            for(int j = 0; j < i; j++) {
                if(R[i,j] != 0) test = false;
            }
        }
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that Q.transpose()*Q = 1...");
        matrix identity = Q.transpose()*Q;
        test = true;
        for(int i = 0; i < identity.size1; i++) {
            if(!matrix.approx(identity[i,i], 1)) test = false;
        }
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that Q*R = A...");
        test = true;
        if(!A.approx(Q*R)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking solve below\n");
        Error.Write("generating a square matrix AA...");
        matrix AA = new matrix(n,n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j < n; j++) {
                AA[i,j] = rnd.NextDouble();
            }
        }
        Error.Write("initializing a vector b of the same length as AA with ");
        Error.Write("random entries between 0 and 1...");
        vector b = new vector(n);
        for(int i = 0; i < n; i++) b[i] = rnd.NextDouble();
        Error.Write("done\n");

        Error.Write("checking solve method (which ");
        Error.Write("includes decomposing matrix AA)...");
        vector sol = solve(AA, b);
        vector check = decomp(AA).Item1*decomp(AA).Item2*sol;
        test = true;
        if(!b.approx(check)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("checking that AA*x = b...");
        test = true;
        if(!b.approx(AA*sol)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking inverse below\n");
        Error.Write("calculating and checking inverse of AA...");
        matrix B = inverse(AA);
        matrix I = new matrix(n, n);
        I.set_identity();
        test = true;
        if(!I.approx(AA*B)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
        Error.WriteLine();

        Error.Write("debug: checking determinant properties below\n");
        Error.Write("det(AA)*det(AA^{-1}) = 1...");
        test = true;
        if(1.0 != Math.Round(determinant(AA)*determinant(B),4)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(c*AA) = c^n*det(AA)...");
        test = true;
        double c = rnd.NextDouble();
        if(Math.Round(determinant(c*AA),4) 
            != Math.Round(Math.Pow(c,n)*determinant(AA),4)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");

        Error.Write("det(AA) = 1/det(AA^{-1})...");
        test = true;
        if(Math.Round(determinant(AA),1)
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
