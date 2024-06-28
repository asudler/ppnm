using System;
using static matrix;

public static class lu {
    /* decomp:
     * performs the LU factorization
     * of a real square matrix.
     * returns L, U, and the permutation matrix P
     * (P is identity matrix if it's not needed)
     * (see Fedorov 2024, p. 23-24 */
    public static (matrix, matrix, matrix) decomp(matrix A) {
        if(A.size1 != A.size2) throw new Exception("input matrix not square");
        matrix L = new matrix(A.size1, A.size1),
               U = new matrix(A.size1, A.size1),
               P = id(A.size1);

        // row pivoting if necessary
        if(approx(A[0,0], 0.0)) {
            int rpiv = Array.IndexOf((double[])A[0], A[0].max());
            if(approx(A[0][rpiv], 0.0))
                throw new Exception("input matrix cannot be LU decomposed");
            P[0,0] = 0;
            P[0,rpiv] = 1;
            P[rpiv,0] = 1;
            P[rpiv,rpiv] = 0;
            A = P*A;
        }

        // Doolittle's algorithm
        // consider adding pivot mechanism in future versions
        for(int i = 0; i < A.size1; i++) {
            L[i,i] = 1;
            for(int j = i; j < A.size1; j++) {
                double sum = 0;
                for(int k = 0; k < i; k++) sum += L[i,k]*U[k,j];
                U[i,j] = A[i,j] - sum;
            }
            for(int j = i + 1; j < A.size1; j++) {
                double sum = 0;
                for(int k = 0; k < i; k++) sum += L[j,k]*U[k,i];
                L[j,i] = 1/U[i,i]*(A[j,i] - sum);
            }
        }
        return (L, U, P);
    } // decomp

    /* solve:
     * Ax = b ==> LUx = b can be solved in two stages,
     * (1) Ly = b for y with forward sub, then 
     * (2) Ux = y for x with backward sub */
    public static (vector, matrix) solve(matrix A, vector b) {
        if(b.size != A.size2) throw new Exception("invalid b dims");
        (matrix L, matrix U, matrix P) = decomp(A);
        vector x = new vector(A.size1), y = new vector(A.size1);
        for(int i = 0; i < b.size; i++) {
            double sum = 0;
            for(int j = 0; j < i; j++) sum += L[i,j]*y[j];
            y[i] = b[i] - sum;
        } // forward sub
        for(int i = b.size - 1; i >= 0; i--) {
            double sum = 0;
            for(int j = i + 1; j < b.size; j++) sum += U[i,j]*x[j];
            x[i] = (y[i] - sum)/U[i,i];
        } // back sub
        return (x, P);
    } // solve

    /* determinant:
     * takes determinant through LU decomposition routine
     * i.e. det(A) = det(U) = \Gamma U_{ii} */
    public static double determinant(matrix A) {
        matrix U = decomp(A).Item2;
        double product = 1;
        for(int i = 0; i < U.size1; i++) {
            product *= U[i,i];
        }
        return product;
    } // determinant

    /* inverse:
     * returns the inverse matrix of the non-singular square matrix A */
    public static (matrix, matrix) inverse(matrix A) {
        matrix inverse = new matrix(A.size1, A.size2);
        matrix P = decomp(A).Item3;
        for(int i = 0; i < A.size1; i++) {
            vector e = new vector(A.size1);
            e.set_zero();
            e[i] = 1;
            vector sol = solve(A, e).Item1;
            inverse[i] = sol;
        }
        return (inverse, P);
    } // inverse
} // lu
