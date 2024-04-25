using System;

public static class integrate {
    /* quad:
     * estimates the integral of a given function f(x)
     * on a given interval [a,b] with the required
     * absolute (delta) or relative (eps) accuracy goals 
     * returns (integral, error) */
    public static (double, double) quad
    (Func<double, double> f, double a, double b,
    double delta=0.001, double eps=0.001,
    double f2=double.NaN, double f3=double.NaN) {
        // flip limits if b < a
        if(b < a) {
            Func<double,double> g = x => -1*f(x);
            return quad(g, b, a, delta, eps, f2, f3);
        }

        // transform integral if it's improper
        if(double.IsNegativeInfinity(a) && double.IsPositiveInfinity(b)) {
            Func<double, double> g = t
                => f(t/(1 - t*t))*(1 + t*t)/Math.Pow(1 - t*t, 2);
            return quad(g, -1, 1, delta, eps, f2, f3);
        }
        if(double.IsPositiveInfinity(b)) {
            Func<double, double> g = t
                => f(a + t/(1 - t))/Math.Pow(1 - t, 2);
            return quad(g, 0, 1, delta, eps, f2, f3);
        }
        if(double.IsNegativeInfinity(a)) {
            Func<double, double> g = t
                => f(b + t/(1 + t))/Math.Pow(1 + t, 2);
            return quad(g, -1, 0, delta, eps, f2, f3);
        }

        // calculates the integral with error
        double h = b - a;
        if(double.IsNaN(f2)) {
            f2 = f(a + 2*h/6);
            f3 = f(a + 4*h/6);
        }
        double f1 = f(a + h/6), f4 = f(a + 5*h/6);
        double Q = (2*f1 + f2 + f3 + 2*f4)/6*(b - a); // higher order rule
        double q = (f1 + f2 + f3 + f4)/4*(b - a); // lower order rule
        double err = Math.Abs(Q - q);
        if(err <= delta + eps*Math.Abs(Q)) return (Q, err);
        else {
            (double quad1, double err1) 
                = quad(f,a,(a+b)/2,delta/Math.Sqrt(2),eps,f1,f2);
            (double quad2, double err2) 
                = quad(f,(a+b)/2,b,delta/Math.Sqrt(2),eps,f3,f4);
            return (quad1 + quad2, Math.Sqrt(err1*err1 + err2*err2));
        };
    } // quad

    /* clenshaw_curtis:
     * adaptive integration using the Clenshaw-Curtis variable
     * transformation, see Fedorov p. 72-73 */
    public static (double, double) clenshaw_curtis
    (Func<double, double> f, double a, double b,
    double delta=0.001, double eps=0.001,
    double f2=Double.NaN, double f3=Double.NaN) {
        Func<double, double> g = theta
            => f((a + b)/2 + (b - a)/2*Math.Cos(theta))
            * Math.Sin(theta) * (b - a)/2;
        return quad(g, 0, Math.PI, delta, eps, f2, f3);
    } // clenshaw_curtis
} // integrate

