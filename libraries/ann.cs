using System;
using static System.Console;
using static System.Math;
using static vector;
using static minimization;

public class ann {
    public int n; // number of hidden neurons
    public int nfev; // # of loops to train network
    public Func<double,double> f = x => x*Exp(-x*x); // activation function
    public Func<double,double> df = x 
        => Exp(-x*x) - 2*x*x*Exp(-x*x); // derivative of activation function
    public Func<double,double> d2f = x
        => -4*x*Exp(-x*x) + x*(4*x*x*Exp(-x*x) - 2*Exp(-x*x)); // hmm...
    public Func<double,double> F = x
        => 0.5*(1/Math.E - Exp(-x*x)); // integral of act. fcn. on [-1,x]
    public vector p; // network parameters
    
    public ann(int n) {
        this.n = n;
        p = new vector(3*n); // ais, pis, and wis
    } // constructor #1
    
    public ann(
        int n, 
        Func<double,double> f, 
        Func<double,double> df,
        Func<double,double> d2f,
        Func<double,double> F
    ) {
        this.n = n;
        this.f = f;
        this.df = df;
        this.d2f = d2f;
        this.F = F;
        p = new vector(3*n); // ais, pis, and wis
    } // constructor #2

    public double response(double x, vector q) {
        double sum = 0;
        for(int i = 0; i < n; i++) {
            sum += f((x - q[3*i])/q[3*i+1])*q[3*i+2];
        }
        return sum;
    } // response

    public double d_response(double x, vector q) {
        double sum = 0;
        for(int i = 0; i < n; i++) {
            sum += df((x -  q[3*i])/q[3*i+1])*q[3*i+2]/q[3*i+1];
        }
        return sum;
    } // d_response

    public double d2_response(double x, vector q) {
        double sum = 0;
        for(int i = 0; i < n; i++) {
            sum += d2f((x -  q[3*i])/q[3*i+1])*q[3*i+2]/q[3*i+1]/q[3*i+1];
        }
        return sum;
    } // d2_response

    public double integrate_response(double x, double x0, vector q) {
        double sum = 0;
        for(int i = 0; i < n; i++) {
            sum += F((x - q[3*i])/q[3*i+1])*q[3*i+2]*q[3*i+1]; 
            sum -= F((x0 - q[3*i])/q[3*i+1])*q[3*i+2]*q[3*i+1];
        }
        return sum;
    } // integrate_response

    public void train(vector x, vector y) {
        if(x.size != y.size) throw new Exception("try again loser");
        int size = x.size;
        
        // cost function
        Func<vector, double> f_cost = q => {
            double sum = 0;
            for(int i = 0; i < size; i++) {
                sum += Pow(response(x[i], q) - y[i], 2);
            }
            return sum;
        };
        
        // let's minimize that mofo
        (p, nfev) = downhill_simplex(f_cost, p.size, 1E-5);
    } // train
} // ann

