using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;
using static System.Math;
using static minimization;

public static class main {
    public static Func<vector, double> f_rosenbrock = x
        => (1 - x[0])*(1 - x[0])
            + 100*(x[1] - x[0]*x[0])*(x[1] - x[0]*x[0]);
    public static Func<vector, double> f_himmelblau = x
        => Pow(x[0]*x[0] + x[1] - 11, 2) + Pow(x[0] + x[1]*x[1] - 7, 2);
    
    public static void taska() {
        // Rosenbrock
        vector start = new vector(-1.0,-2.0);
        Error.Write("finding the minimum of the 2D Rosenbrock function ");
        (vector mins, int nfev) = newton(f_rosenbrock, start);
        Error.Write($"(nfev={nfev}): ");
        for(int i = 0; i < mins.size; i++) {
            Error.Write($"mins[{i}]={mins[i]} ");
        }
        Error.Write($"f(min)={f_rosenbrock(mins)} ");
        Error.Write("(known sol at (x,y)=(1,1))\n");

        // Himmelblau
        // note: here, the algorithm does not correctly identify minima
        // for certain starting positions---perhaps it finds
        // saddle points?
        start[0] = -5.0; start[1] = -3.0;
        Error.Write("finding a minimum of the Himmelblau function ");
        (mins, nfev) = newton(f_himmelblau, start);
        Error.Write($"(nfev={nfev}): ");
        for(int i = 0; i < mins.size; i++) {
            Error.Write($"mins[{i}]={mins[i]} ");
        }
        Error.Write($"f(min)={f_himmelblau(mins)} ");
        Error.Write("(known sol at (x,y)=(-3.780,-3.283))\n");
    } // taska

    public static List<double[]> getdata(
        string infile, // file to read
        int skip_header=0, // skip lines at beginning of file
        int skip_footer=0, // skip lines at end of file
        char[] delimiter=(char[])null // null ==> whitespace
    ) {
        List<double[]> data = new List<double[]>();
        var lines = File.ReadAllLines(infile);
        for(int i = skip_header; i < lines.Length - skip_footer; i++) {
            var fields = lines[i].Split(delimiter,
                StringSplitOptions.RemoveEmptyEntries); // >1 delimiter
            double[] scan = new double[fields.Length];
            for(int j = 0; j < fields.Length; j++) {
                scan[j] = double.Parse(fields[j]);
            }
            data.Add(scan);
        }
        return data;
    } // getdata (read input file)

    public static void taskb(string[] args) {
        // read from makefile
        string infile = null; int skip_header = 0;
        double m0 = 0, Gamma0 = 0, A0 = 0;
        foreach(var arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-infile") infile = fields[1];
            if(fields[0] == "-skip_header") skip_header = int.Parse(fields[1]);
            if(fields[0] == "-m0") m0 = double.Parse(fields[1]);
            if(fields[0] == "-Gamma0") Gamma0 = double.Parse(fields[1]);
            if(fields[0] == "-A0") A0 = double.Parse(fields[1]);
        }

        // get data from input file
        List<double[]> data = getdata(infile, skip_header);
        double[] energies = new double[data.Count],
            signals = new double[data.Count], 
            uncertainties = new double[data.Count];
        for(int i = 0; i < data.Count; i++) {
            energies[i] = data[i][0];
            signals[i] = data[i][1];
            uncertainties[i] = data[i][2];
        }

        // define functions
        Func<double, vector, double> breit_wigner = (energy, pars)
            => pars[2]/(Pow(energy - pars[0], 2) + pars[1]*pars[1]/4);
        Func<vector, double> deviation = pars => { 
            double sum = 0.0;
            for(int i = 0; i < energies.Length; i++) {
                sum += Pow((breit_wigner(energies[i], pars) - signals[i])
                    /uncertainties[i], 2);
            }
            return sum;
        };

        // find minimum
        vector start = new vector(m0, Gamma0, A0);
        Write("# finding the fit parameters of Higgs data ");
        (vector mins, int nfev) = newton(deviation, start);
        Write($"(nfev={nfev}):\n");
        Write($"m={mins[0]}\n");
        Write($"Gamma={mins[1]}\n");
        Write($"A={mins[2]}\n");
    } // taskb

    public static void taskc(string[] args) {
        // copy-paste from taska
        // Rosenbrock
        vector start = new vector(-1.0,-2.0);
        Error.Write("finding the minimum of the 2D Rosenbrock function ");
        (vector mins, int nfev) = newton(f_rosenbrock, start, central:true);
        Error.Write($"(nfev={nfev}): ");
        for(int i = 0; i < mins.size; i++) {
            Error.Write($"mins[{i}]={mins[i]} ");
        }
        Error.Write($"f(min)={f_rosenbrock(mins)} ");
        Error.Write("(known sol at (x,y)=(1,1))\n");

        // Himmelblau
        // note: here, the algorithm does not correctly identify minima
        // for certain starting positions---perhaps it finds
        // saddle points?
        start[0] = -5.0; start[1] = -3.0;
        Error.Write("finding a minimum of the Himmelblau function ");
        (mins, nfev) = newton(f_himmelblau, start, central:true);
        Error.Write($"(nfev={nfev}): ");
        for(int i = 0; i < mins.size; i++) {
            Error.Write($"mins[{i}]={mins[i]} ");
        }
        Error.Write($"f(min)={f_himmelblau(mins)} ");
        Error.Write("(known sol at (x,y)=(-3.780,-3.283))\n");

        // copy-paste from taskb
        // read from makefile
        string infile = null; int skip_header = 0;
        double m0 = 0, Gamma0 = 0, A0 = 0;
        foreach(var arg in args) {
            var fields = arg.Split(":");
            if(fields[0] == "-infile") infile = fields[1];
            if(fields[0] == "-skip_header") skip_header = int.Parse(fields[1]);
            if(fields[0] == "-m0") m0 = double.Parse(fields[1]);
            if(fields[0] == "-Gamma0") Gamma0 = double.Parse(fields[1]);
            if(fields[0] == "-A0") A0 = double.Parse(fields[1]);
        }

        // get data from input file
        List<double[]> data = getdata(infile, skip_header);
        double[] energies = new double[data.Count],
            signals = new double[data.Count], 
            uncertainties = new double[data.Count];
        for(int i = 0; i < data.Count; i++) {
            energies[i] = data[i][0];
            signals[i] = data[i][1];
            uncertainties[i] = data[i][2];
        }

        // define functions
        Func<double, vector, double> breit_wigner = (energy, pars)
            => pars[2]/(Pow(energy - pars[0], 2) + pars[1]*pars[1]/4);
        Func<vector, double> deviation = pars => { 
            double sum = 0.0;
            for(int i = 0; i < energies.Length; i++) {
                sum += Pow((breit_wigner(energies[i], pars) - signals[i])
                    /uncertainties[i], 2);
            }
            return sum;
        };

        // find minimum
        vector start2 = new vector(m0, Gamma0, A0);
        Write("# finding the fit parameters of Higgs data ");
        (mins, nfev) = newton(deviation, start2, central:true);
        Write($"(nfev={nfev}):\n");
        Write($"m={mins[0]}\n");
        Write($"Gamma={mins[1]}\n");
        Write($"A={mins[2]}\n");
    } // taskc

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-taska") taska();
            if(arg == "-taskb") taskb(args);
            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main
