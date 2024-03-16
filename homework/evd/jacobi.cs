using static System.Math;

public static class jacobi {
    /* timesJ:
     * performs an in-place multiplication of
     * the given matrix A with the Jacobi matrix
     * J(p, q, theta) from the right */
    public static void timesJ(matrix A, int p, int q, double theta) {
        double c = Cos(theta), s = Sin(theta);
        for(int i = 0; i < A.size1; i++) {
            double aip = A[i,p], aiq = A[i,q];
            A[i,p] = c*aip - s*aiq;
            A[i,q] = s*aip + c*aiq;
        }
    } // timesJ

    /* Jtimes:
     * performs an in-place multiplication of
     * the given matrix A with the Jacobi matrix
     * J(p, q, theta) from the left */
    public static void Jtimes(matrix A, int p, int q, double theta) {
        double c = Cos(theta), s = Sin(theta);
        for(int j = 0; j < A.size1; j++) {
            double apj = A[p,j], aqj = A[q,j];
            A[p,j] = c*apj + s*aqj;
            A[q,j] = -s*apj + c*aqj;
        }
    } // Jtimes

    /* cyclic:
     * Jacobi eigenvalue algorithm for real
     * symmetric matrices using cyclic sweeps */
    public static (vector, matrix) cyclic(matrix M) {
        matrix A = M.copy();
        matrix V = matrix.id(M.size1);
        vector w = new vector(M.size1);
        bool changed;
        do {
            changed = false;
            for(int p = 0; p < M.size1 - 1; p++) {
                for(int q = p + 1; q < M.size1; q++) {
                    double apq = A[p,q], app = A[p,p], aqq = A[q,q];
                    double theta = 0.5*Atan2(2*apq, aqq - app);
                    double c = Cos(theta), s = Sin(theta);
                    double new_app = c*c*app - 2*s*c*apq + s*s*aqq;
                    double new_aqq = s*s*app + 2*s*c*apq + c*c*aqq;
                    if(new_app != app || new_aqq != aqq) {
                        changed = true;
                        timesJ(A, p, q, theta); // A <-- A*J
                        Jtimes(A, p, q, -theta); // A <-- (J^T)*A
                        timesJ(V, p, q, theta); // V <-- V*J
                    }
                }
            }
        } while (changed);
        for(int i = 0; i < M.size1; i++) {
            w[i] = A[i,i];
        }
        return (w, V);
    } // cyclic
} // jacobi
