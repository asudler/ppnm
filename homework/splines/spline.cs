using System;
using static System.Math;

public static class spline {
    /* binsearch:
     * binary search algorithm; i.e.,
     * finding target interval of a sorted array
     * by bisection */
    public static int binsearch(double[] x, double z) {
        if(z < x[0] || z > x[x.Length-1]) {
            throw new Exception("binsearch: bad z");
        }
        int i = 0; int j = x.Length - 1;
        while(j - i > 1) {
            int mid = (i+j)/2;
            if(z > x[mid]) i = mid; else j = mid;
        }
        return i; // lower or "left" index
    }

    /* linear_spline:
     * connecting the dots */
    public static double linear_spline(double[] x, double[] y, double z) {
        int i = binsearch(x, z);
        double dx = x[i+1] - x[i];
        if(!(dx > 0)) {
            throw new Exception("linear_spline: bad x (is x sorted?)");
        }
        double dy = y[i+1] - y[i];
        return y[i] + dy/dx*(z - x[i]);
    }

    /* linear_integrate:
     * integrate a LINEAR function y[] from x[0] to z
     * (only to be used on LINEAR datasets) */
    public static double linear_integrate(double[] x, double[] y, double z) {
        int i = binsearch(x, z); 
        double integral = 0;
        double dx, dy;
        for(int j = 0; j < i; j++) {
            dx = x[j+1] - x[j];
            dy = y[j+1] - y[j];
            if(!(dx > 0)) {
                throw new Exception("linear_integrate: bad x (is x sorted?)");
            }
            integral += dx*y[j] + dx*dy/2;
        }
        
        // extrapolate to z by analytical integration
        dx = x[i+1] - x[i];
        dy = y[i+1] - y[i];
        integral += y[i]*(z - x[i]) + dy/dx*Pow(z - x[i], 2)/2;
        return integral;
    } // linear_integrate

    /* quadratic_spline:
     * connecting the dots...
     * ...but now the ruler can bend! */
    public class quadratic_spline {
        double[] x, y, b, c;
        public quadratic_spline(double[] xs, double[] ys) {
            if(xs.Length != ys.Length) {
                throw new Exception($"quadratic_spline: invalid dimensions");
            }
            x = xs; y = ys;
            int n = x.Length;
            double[] p = new double[n-1], dx = new double[n-1];
            b = new double[n-1]; c = new double[n-1];
            for(int i = 0; i < n - 1; i++) {
                dx[i] = x[i+1] - x[i];
                p[i] = (y[i+1] - y[i])/dx[i];
            }

            // run forward conversion then backward conversion
            for(int i = 0; i < n - 2; i++) {
                c[i+1] = (p[i+1] - p[i] - c[i]*dx[i])/dx[i+1];
            }
            c[n-2] /= 2;
            for(int i = n - 3; i >= 0; i--) {
                c[i] = (p[i+1] - p[i] - c[i+1]*dx[i+1])/dx[i];
                b[i] = p[i] - c[i]*dx[i];
            }
            b[n-2] = p[n-2] - c[n-2]*dx[n-2];
        } // constructor

        public double evaluate(double z) {
            int i = binsearch(x, z);
            return y[i] + b[i]*(z - x[i]) + c[i]*Pow(z-x[i],2);
        } // evaluate

        public double differentiate(double z) {
            int i = binsearch(x, z);
            return b[i] + 2*c[i]*(z - x[i]);
        } // differentiate

        public double integrate(double z) {
            int i = binsearch(x, z);
            double integral = 0;
            double dx;
            for(int j = 0; j < i; j++) {
                dx = x[j+1] - x[j];
                if(!(dx > 0)) {
                    throw new 
                    Exception("linear_integrate: bad x (is x sorted?)");
                }
                integral += y[j]*dx + b[j]*Pow(dx, 2)/2
                    + c[j]*Pow(dx, 3)/3;
            }
            integral += y[i]*(z-x[i]) + 1.0*b[i]*Pow(z - x[i], 2)/2
                + c[i]*Pow(z - x[i], 3)/3;
            return integral;
        } // integrate
    } // quadratic_spline
} // spline

