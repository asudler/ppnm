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
    public vector p; // network parameters
    
    public ann(int n) {
        this.n = n;
        p = new vector(3*n); // ais, pis, and wis
    } // constructor #1
    
    public ann(int n, Func<double, double> f, Func<double,double> df) {
        this.n = n;
        this.f = f;
        this.df = df;
        p = new vector(3*n); // ais, pis, and wis
    } // constructor #2

    public double response(double x, vector q) {
        double sum = 0;
        for(int i = 0; i < n; i++) {
            sum += f((x - q[3*i])/q[3*i+1])*q[3*i+2];
        }
        return sum;
    } // response

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

        // need to get ourselves a starting p vector, right?
        // ais: use xrange
        // bis: cannot be 0 (division), otherwise random
        // wis: random
        var rnd = new Random();
        for(int i = 0; i < n; i++) {
            p[3*i] = x[0] + i*(x[size-1] - x[0])/(n - 1);
            p[3*i+1] = (rnd.NextDouble() + 1E-4)*(rnd.Next(0,2)*2 - 1);
            p[3*i+2] = rnd.NextDouble()*(rnd.Next(0,2)*2 - 1);
        }
        
        // let's minimize that mofo
        (p, nfev) = downhill_simplex(f_cost, p.size, 1E-5, 10000000);
    } // train
} // ann

