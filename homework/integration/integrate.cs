using System;

public static class integrate {
    /* quad:
     * estimates the integral of a given function f(x)
     * on a given interval [a,b] with the required
     * absolute (delta) or relative (eps) accuracy goals */
    public static double quad
    (Func<double, double> f, double a, double b,
    double delta=0.001, double eps=0.001,
    double f2=Double.NaN, double f3=Double.NaN) {
        double h = b - a;
        if(Double.IsNaN(f2)) {
            f2 = f(a + 2*h/6);
            f3 = f(a + 4*h/6);
        }
        double f1 = f(a + h/6), f4 = f(a + 5*h/6);
        double Q = (2*f1 + f2 + f3 + 2*f4)/6*(b - a); // higher order rule
        double q = (f1 + f2 + f3 + f4)/4*(b - a); // lower order rule
        double err = Math.Abs(Q - q);
        if(err <= delta + eps*Math.Abs(Q)) return Q;
        else return quad(f,a,(a+b)/2,delta/Math.Sqrt(2),eps,f1,f2)
            + quad(f,(a+b)/2,b,delta/Math.Sqrt(2),eps,f3,f4);
    } // quad
} // integrate

