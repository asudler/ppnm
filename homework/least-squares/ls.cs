using System;
using static qrgs;
public static class ls {
    public static (vector, matrix) lsfit
    (Func<double,double>[] fs, vector x, vector y, vector dy) {
        int n = x.size, m = fs.Length;
        var A = new matrix(n, m);
        var b = new vector(n);
        for(int i = 0; i < n; i++) {
            b[i] = y[i]/dy[i];
            for(int j = 0; j < m; j++) {
                A[i,j] = fs[j](x[i])/dy[i];
            }
        }
        matrix S = pinverse(A.transpose()*A);
        return (qrgs.solve(A, b), S);
    }
}

