using System;

public static class spline {
    /* binsearch:
     * binary search algorithm; i.e.,
     * finding target value of a sorted array
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
        return i;
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
        for(int j = 0; j < i - 1; j++) {
            double dx = x[j+1] - x[j];
            if(!(dx > 0)) {
                throw new Exception("linear_integrate: bad x (is x sorted?)");
            }
            double dy = y[j+1] - y[j];
            integral += dx*y[j] + dx*dy/2; // trapezoidal rule
        }
        
        // extrapolate to z by analytical integration
        integral += (z-x[i])+(linear_spline(x, y, z) + y[i])/2;
        return integral;
    } // linear_integrate
} // spline

