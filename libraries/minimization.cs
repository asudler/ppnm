using System;
using System.Linq;
using System.Collections.Generic;
using static System.Math;
using static vector;
using static matrix;
using static qrgs;

public static class minimization {
    /* newton:
     * Newton's minimization method with numerical gradient,
     * numerical Hessian matrix, and backtracking line search */
    public static (vector, int) newton(
        Func<vector,double> phi, // objective function
        vector x, // starting point
        Func<vector,vector> g_phi=null, // known analytic gradient of phi
        double lambda_min=1.0/1024, // minimum lambda to try
        double acc=1E-3, // accuracy goal
        bool central=false, // central finite difference approximation
        int maxfev=100000 // maximum number of iterations (func. evals.)
    ) {
        int counter = 0;
        if(g_phi != null) do { // newton's iterations
            vector del_phi = g_phi(x);
            if(del_phi.norm() < acc) break;
            matrix H = hessian(phi, x);
            vector qr = solve(H, -del_phi);
            double lambda = 1, phix = phi(x);
            do { // linesearch
                if(phi(x + lambda*qr) < phix) break; // accept good step
                if(lambda < lambda_min) break; // accept anyway
                lambda /= 2;
            } while(true);
            x += lambda*qr;
            counter += 1;
        } while(counter < maxfev);
        else if(!central) do { // newton's iterations
            vector del_phi = gradient(phi, x);
            if(del_phi.norm() < acc) break;
            matrix H = hessian(phi, x);
            vector qr = solve(H, -del_phi);
            double lambda = 1, phix = phi(x);
            do { // linesearch
                if(phi(x + lambda*qr) < phix) break; // accept good step
                if(lambda < lambda_min) break; // accept anyway
                lambda /= 2;
            } while(true);
            x += lambda*qr;
            counter += 1;
        } while(counter < maxfev);
        else do {
            vector del_phi = gradient(phi, x, central);
            if(del_phi.norm() < acc) break;
            matrix H = hessian(phi, x, central);
            vector qr = solve(H, -del_phi);
            double lambda = 1, phix = phi(x);
            do { 
                if(phi(x + lambda*qr) < phix) break; 
                if(lambda < lambda_min) break;
                lambda /= 2;
            } while(true);
            x += lambda*qr;
            counter += 1;
        } while(counter < maxfev);
        return (x, counter);
    } // newton

    /* gradient:
     * calculates the gradient of a vector-valued function */
    public static vector gradient(
        Func<vector,double> phi, 
        vector x,
        bool central=false
    ) {
        vector del_phi = new vector(x.size);
        if(!central) {
            double phix = phi(x); // do not recalculate at each step
            for(int i = 0; i < x.size; i++) {
                double dx = Max(Abs(x[i]), 1)*Pow(2, -26);
    		    x[i] += dx;
	    	    del_phi[i] = (phi(x) - phix)/dx;
		        x[i] -= dx;
            }
        } else {
            for(int i = 0; i < x.size; i++) {
                double dx = Max(Abs(x[i]), 1)*Pow(2, -26);
                x[i] += dx;
                del_phi[i] = phi(x)/2/dx;
                x[i] -= 2*dx;
                del_phi[i] -= phi(x)/2/dx;
                x[i] += dx;
            }
        }
        return del_phi;
    } // gradient

    public static matrix hessian(
        Func<vector,double> phi, 
        vector x,
        bool central=false
    ) {
	    matrix H = new matrix(x.size);
        if(!central) {
	        vector del_phix = gradient(phi, x, central);
	        for(int j = 0; j < x.size; j++) {
    		    double dx = Max(Abs(x[j]), 1)*Pow(2,-13); // for num. gradient
		        x[j] += dx;
		        vector d_del_phi = gradient(phi, x, central) - del_phix;
		        for(int i = 0; i < x.size; i++) H[i,j] = d_del_phi[i]/dx;
		        x[j] -= dx;
	        }
        } else { // lecture notes 2024 ed. eq. (10.48)
            for(int j = 0; j < x.size; j++) {
                double dxj = Max(Abs(x[j]), 1)*Pow(2,-13);
                x[j] += dxj;
                for(int i = 0; i < x.size; i++) {
                    double dxi = Max(Abs(x[i]), 1)*Pow(2,-13);
                    x[i] += dxi;
                    H[i,j] = phi(x)/4/dxj/dxi;
                    x[i] -= 2*dxi;
                    H[i,j] -= phi(x)/4/dxj/dxi;
                    x[i] += dxi;
                }
                x[j] -= 2*dxj;
                for(int i = 0; i < x.size; i++) {
                    double dxi = Max(Abs(x[i]), 1)*Pow(2,-13);
                    x[i] += dxi;
                    H[i,j] -= phi(x)/4/dxj/dxi;
                    x[i] -= 2*dxi;
                    H[i,j] += phi(x)/4/dxj/dxi;
                    x[i] += dxi;
                }
                x[j] += dxj;
            }
        }
        return (H+H.T)/2; // you think? I hope so!
    } // hessian

    // trying to implement the downhill simplex method
    // which should help with the neural network homework
    // since I have no freaking clue how to take the analytic
    // gradient and hessian of the neuron function thing
    public static (vector, int) downhill_simplex(
        Func<vector,double> phi, // function to minimize
        int size, // pass in size of vector
        double acc=1E-3, // target system size
        int maxfev=100000 // maximum allowed iterations
    ) {
        // initialize vertices
        List<vector> vertices = new List<vector>();
        List<double> phis = new List<double>(); 
        var rnd = new Random();
        for(int i = 0; i < size + 1; i++) {
            vector pi = new vector(size);
            for(int j = 0; j < size; j++) pi[j] = rnd.NextDouble() + 1E-4;
            vertices.Add(pi);
            phis.Add(phi(pi)); // lmao they are all almost the same word
            if(phis[i] == double.NaN) throw new Exception("!");
        }

        // do the thing
        vector min = new vector(size);
        int counter = 0; double vsize = 100;
        do {
            // find highest, lowest, and centroid points of the simplex
            double minval = phis.Min();
            int minindex = phis.IndexOf(minval);
            double maxval = phis.Max();
            int maxindex = phis.IndexOf(maxval);
            min = vertices[minindex];

            vector centroid = new vector(size);
            for(int i = 0; i < size + 1; i++) {
                if(i != maxindex) centroid += vertices[i]/size;
            }

            // try reflection
            vector pre = centroid + (centroid - vertices[maxindex]);
            if(phi(pre) < minval) {
                // try expansion
                vector pex = centroid + 2*(centroid - vertices[maxindex]);
                if(phi(pex) < phi(pre)) {
                    vertices[maxindex] = pex;
                    phis[maxindex] = phi(pex);
                }
                else {
                    vertices[maxindex] = pre;
                    phis[maxindex] = phi(pre);
                }
            } 
            else {
                if(phi(pre) < maxval) {
                    vertices[maxindex] = pre;
                    phis[maxindex] = phi(pre);
                }
                else {
                    // try contraction
                    vector pco 
                        = centroid - 0.5*(centroid - vertices[maxindex]);
                    if(phi(pco) < maxval) {
                        vertices[maxindex] = pco;
                        phis[maxindex] = phi(pco);
                    }
                    else {
                        // do reduction
                        for(int i = 0; i < size + 1; i++) {
                            if(i != minindex) {
                                vertices[i] = 0.5*(vertices[i]
                                    + vertices[minindex]);
                                phis[i] = phi(vertices[i]);
                            }
                        }
                    }
                }
            }

            // calculate size of simplex
            // size => distance between two furthest vertices
            vector help = new vector(size);
            for(int i = 0; i < size + 1; i++) {
                for(int j = 0; j < size + 1; j++) {
                    if(i != j) {
                        for(int k = 0; k < size; k++) { 
                            help[k] = vertices[j][k] - vertices[i][k];
                        }
                        if(help.norm() < vsize) vsize = help.norm();
                    }
                }
            }
            counter++;
        } while(vsize > acc && counter < maxfev);
        return (min, counter);
    } // downhill_simplex
} // minimization
