using System;
using static System.Math;
using static vector;
using static matrix;
using static qrgs;

public static class roots { 
    /* newton:
     * Newton's method with numerical Jacobian
     * and back-tracking line search */
    public static vector newton(
        Func<vector, vector> f,
        vector start,
        vector dx=null,
        double acc=1E-3,
        double lambda_min=1.0/1024,
        bool quadmin=false
    ) {
        vector x = start.copy(), fx = f(x), z, fz;
        do { // Newton's iterations
            if(fx.norm() < acc) break;
            matrix J = jacobian(f, x, fx, dx);
            vector qr = solve(J, -fx);
            double lambda = 1;
            if(!quadmin) do { // linear search
                z = x + lambda*qr;
                fz = f(z);
                if(fz.norm() < (1 - lambda/2)*fx.norm()) break;
                if(lambda < lambda_min) break;
                lambda /= 2;
            } while(true);
            else do { // sexy quadratic search
                z = x + lambda*qr;
                fz = f(z);
                if(fz.norm() < (1 - lambda/2)*fx.norm()) break;
                if(lambda < lambda_min) break;
                double c = (0.5*fz.norm()*fz.norm() 
                    - 0.5*f(x).norm()*f(x).norm() 
                    + lambda*f(x).norm()*f(x).norm())/lambda/lambda;
                lambda = f(x).norm()*f(x).norm()/2/c;
            } while(true);
            x = z; fx = fz;
        } while(true);
        return x;
    } // newton

    /* jacobian:
     * numerical estimation of jacobian */
    public static matrix jacobian(
        Func<vector, vector> f,
        vector x,
        vector fx=null,
        vector dx=null        
    ) {
        if(dx == null) dx = x.map(xi => Abs(xi)*Pow(2, -26));
        if(fx == null) fx = f(x);
        matrix J = new matrix(x.size);
        for(int i = 0; i < x.size; i++) {
            if(dx[i] == 0) throw new Exception("jacobian: bad slope");
            x[i] += dx[i];
            vector df = f(x) - fx;
            for(int j = 0; j < x.size; j++) J[j,i] = df[j]/dx[i]; 
            x[i] -= dx[i];
        }
        return J;
    } // jacobian
} // roots

