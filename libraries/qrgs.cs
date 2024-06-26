using static System.Console;
using static System.Math;

public partial class qrgs {
    /* decomp:
     * performs stabilized Gram-Schmidt orthogonalization
     * of an n x m (n >= m) matrix 
     * (see Fedorov text, p. 17-18) */
    public static (matrix, matrix) decomp(matrix A) {
        matrix Q = A.copy();
        matrix R = new matrix(A.size2, A.size2);
        for(int i = 0; i < R.size2; i++) {
            R[i,i] = Q[i].norm(); // Q[i] --> i-th column
            Q[i] /= R[i,i];
            for(int j = i + 1; j < A.size2; j++) {
                R[i,j] = Q[i].dot(Q[j]);
                Q[j] -= Q[i]*R[i,j];
            }
        }
        return (Q, R);
    } // decomp

    /* solve:
     * uses the matrices Q and R from "decomp"
     * and solves Q.R.x = b for the given
     * RHS "b" */
    public static vector solve(matrix Q, matrix R, vector b) {
        vector x = new vector(Q.size1);
        vector rhs = Q.transpose()*b;
        for(int i = rhs.size-1; i >= 0; i--) {
            double sum = 0;
            for(int j = i+1; j < rhs.size; j++) {
                sum += R[i,j]*x[j];
            }
            x[i] = (rhs[i] - sum)/R[i,i];
        }
        return x;
    } // solve 

    public static vector solve(matrix A, vector b) {
        (matrix Q, matrix R) = decomp(A);
        return solve(Q, R, b);
    } // solve

    /* determinant:
     * calculates the magnitude of the 
     * determinant of a matrix A
     * via QR decomposition
     * (see Fedorov text p. 25) */
    public static double determinant(matrix A) {
        matrix R = decomp(A).Item2;
        double product = 1;
        for(int i = 0; i < R.size1; i++) {
            product *= R[i,i];
        }
        return Abs(product);
    } // determinant

    /* inverse:
     * calculates the inverse of a square matrix A
     * using QRGS decomposition */
    public static matrix inverse(matrix A) {
        matrix inverse = new matrix(A.size2, A.size1);
        for(int i = 0; i < A.size1; i++) {
            vector e = new vector(A.size1);
            e.set_zero();
            e[i] = 1;
            vector sol = solve(A, e);
            inverse[i] = sol;
        }
        return inverse;
    } // inverse

    /* pinverse:
     * calculates the pseudo-inverse (Moore-Penrose inverse)
     * of a tall matrix A using
     * QRGS decomposition */
    public static matrix pinverse(matrix A) {
        (matrix Q, matrix R) = decomp(A);
        matrix I = new matrix(Q.size1);
        I.set_identity();
        for(int i = 0; i < Q.size1; i++) {
            I[i] = solve(Q, R, I[i]);
        }
        return I;
    } // pinverse
} // qrgs

