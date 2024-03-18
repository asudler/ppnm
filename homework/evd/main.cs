using System;
using static System.Console;
using static System.Math;
using static jacobi;
using static matrix;

public class main {
    /* check_cyclic:
     * believe it or not,
     * checks the functionality of the cyclic
     * function from the jacobi library */
    public static void check_cyclic() {
        Error.Write("##### HOMEWORK EVD PART (A): checking cyclic... #####\n");
        
        int n = 300; // size of matrix
        Error.Write($"generating a random symmetric {n}x{n} matrix A ");
        Error.Write("with random entries between 0 and 1...");
        Random rnd = new Random();
        matrix A = new matrix(n, n);
        for(int i = 0; i < n; i++) {
            for(int j = 0; j <= i; j++) {
                A[i,j] = rnd.NextDouble();
                A[j,i] = A[i,j];
            }
        }
        Error.Write("done\n");

        Error.Write("performing jacobi diagonalization on matrix A ");
        Error.Write("using cyclic sweeps...");
        var help = cyclic(A);
        vector w = help.Item1;
        matrix V = help.Item2;
        Error.Write("done\n");

        Error.Write("checking that V*(V^T) == 1...");
        matrix mcheck = new matrix(n, n);
        mcheck = V*V.transpose();
        matrix I = new matrix(n, n);
        I.set_identity();
        bool test = true;
        if(!mcheck.approx(I)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
    
        Error.Write("checking that (V^T)*V == 1...");
        mcheck = V.transpose()*V;
        test = true;
        if(!mcheck.approx(I)) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n"); 

        Error.Write("checking that (V^T)*A*V == D where ");
        Error.Write("the columns of D are zero except ");
        Error.Write("along the diagonal where instead ");
        Error.Write("they should equal the eigenvalue...");
        test = true;
        mcheck = V.transpose()*A*V;
        for(int i = 0; i < mcheck.size1; i++) {
            if(!approx(mcheck[i,i], w[i])) test = false;
        }
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n"); 

        Error.Write("checking that V*D*(V^T) == A...");
        test = true;
        if(!A.approx(V*mcheck*V.transpose())) test = false;
        if(test) Error.Write("passed\n");
        else Error.Write("womp womp\n");
    } // check_cyclic

    /* hydrogen_grid:
     * uhh see part (b) of the assignment */
    public static void hydrogen_grid(string[] args) {
        Error.Write("##### HOMEWORK EVD PART (b): hydrogen #####\n");

        // (1) get parameters
        double rmax = 0, dr = 0;
        bool convergence = false, wavefunctions = false;
        foreach(string arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-rmax") rmax = double.Parse(words[1]);
            if(words[0] == "-dr") dr = double.Parse(words[1]);
            if(words[0] == "-convergence") convergence = true;
            if(words[0] == "-wavefunctions") wavefunctions = true;
        }
        if(rmax == 0) {
            rmax = 10.0;
            Error.Write("no user value provided for rmax, ");
            Error.Write("so a genie has provided its value: ");
            Error.Write("rmax = 10.0 :)\n");
        }
        if(dr == 0) {
            dr = 0.3;
            Error.Write("no user value provided for dr, ");
            Error.Write("so a genie has provided its value: ");
            Error.Write("rmax = 0.3 :)\n");
        }

        // (2) set up Hamiltonian matrix
        int npoints = (int) (rmax/dr) - 1;
        vector r = new vector(npoints);
        for(int i = 0; i < npoints; i++) r[i] = dr*(i+1);
        matrix H = new matrix(npoints, npoints);
        for(int i = 0; i < npoints - 1; i++) {
            H[i,i]   = -2*(-0.5/dr/dr);
            H[i,i+1] = 1*(-0.5/dr/dr);
            H[i+1,i] = 1*(-0.5/dr/dr);
        }
        H[npoints-1,npoints-1] = -2*(-0.5/dr/dr);
        for(int i = 0; i < npoints; i++) H[i,i] += -1/r[i];

        // (3) diagonalize Hamiltonian matrix
        var sol = cyclic(H);
        vector eigenvalues = sol.Item1;
        matrix eigenvectors = sol.Item2;

        // (4) (optional) checking convergence
        if(convergence) {
            Write($"{rmax}\t{dr}\t{eigenvalues[0]}\n");
        }

        // (5) check normalization condition on wavefunctions
        double c = 1/Sqrt(dr);
        bool check = true;
        for(int i = 0; i < eigenvectors.size2; i++) {
            double sum = 0;
            for(int j = 0; j < eigenvectors.size1; j++) {
                sum += Pow(c*eigenvectors[j,i], 2)*dr;
            }
            if(!approx(sum,1)) {
                check = false;
                Error.WriteLine($"{sum}");
            }
        }
        if(wavefunctions && check) {
            Write("each column gives the k-th eigenvector ");
            Write("of the Hamiltonian matrix...\n");
            for(int i = 0; i < eigenvectors.size1; i++) {
                for(int j = 0; j < eigenvectors.size2; j++) {
                    Write($"{c*eigenvectors[i,j]}\t");
                }
                WriteLine();
            }
        }
    } // hydrogen_grid

    public static void check_nops(string[] args) {
        Error.Write("##### HOMEWORK EVD PART (c): ");
        Error.Write("check operation scaling #####\n");
        
        int size = 0;
        foreach(string arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-size") size = (int)double.Parse(words[1]);
        }
        
        Random rnd = new Random();
        matrix A = new matrix(size, size);
        for(int i = 0; i < size; i++) {
            for(int j = 0; j <= i; j++) {
                A[i,j] = rnd.NextDouble();
                A[j,i] = A[i,j];
            }
        }
        cyclic(A);
    }

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-parta") check_cyclic();
            if(arg == "-partb") hydrogen_grid(args);
            if(arg == "-partc") check_nops(args);
        }
        return 0;
    } // Main
} // main
