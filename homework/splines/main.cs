using System.IO;
using System;
using static System.Console;
using static System.Math;
using static spline;

public static class main {
    public static (int, double[], double[]) getinput(string[] args) {
        string infile = null;
        int nintervals = 0;
        foreach(var arg in args) {
            var words = arg.Split(':');
            if(words[0] == "-input") infile = words[1];
            if(words[0] == "-nintervals") nintervals = int.Parse(words[1]);
        }
        if(infile == null) {
            throw new Exception("bad filename argument");
        }
        if(nintervals == 0) {
            throw new Exception("bad nintervals argument");
        }

        // read input file
        var lines = File.ReadAllLines(infile);
        double[] x = new double[lines.Length], y = new double[lines.Length];
        for(int i = 0; i < lines.Length; i++) {
            var fields = lines[i].Split("\t");
            x[i] = double.Parse(fields[0]);
            y[i] = double.Parse(fields[1]);
        }
        return (nintervals, x, y);
    } // getinput
    
    public static void taska(string[] args) {
        (int nintervals, double[] x, double[] y) = getinput(args);

        // run linear spline and linear integration over dataset
        for(int i = 0; i <= nintervals; i++) {
            double z = (x[x.Length-1]/nintervals)*i;
            Write($"{z}\t {linear_spline(x, y, z)}\t");
            Write($"{linear_integrate(x, y, z)}\n");
        }
    } // taska

    public static void taskb(string[] args) {
        (int nintervals, double[] x, double[] y) = getinput(args);

        // run quadratic spline over dataset
        quadratic_spline spline = new quadratic_spline(x, y);
        for(int i = 0; i <= nintervals; i++) {
            double z = (x[x.Length-1]/nintervals)*i;
            Write($"{z}\t {spline.evaluate(z)}\t");
            Write($"{spline.differentiate(z)}\t {spline.integrate(z)}\n");
        }
    } // taskb

    public static void taskc(string[] args) {
        (int nintervals, double[] x, double[] y) = getinput(args);

        // run cubic spline over dataset
        cubic_spline spline = new cubic_spline(x, y);
        for(int i = 0; i <= nintervals; i++) {
            double z = (x[x.Length-1]/nintervals)*i;
            Write($"{z}\t {spline.evaluate(z)}\t");
            Write($"{spline.differentiate(z)}\t {spline.integrate(z)}\n");
        }
    } // taskc

    public static void makedata(string[] args) {
        int nminusone = 0; double xmax = 0;
        foreach(var arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-nminusone") nminusone = int.Parse(words[1]);
            if(words[0] == "-xmax") xmax = int.Parse(words[1]);
        }
        if(nminusone == 0 || xmax == 0) {
            throw new Exception("nminusone or xmax not given a good value");
        }
        for(int i = 0; i <= nminusone; i++) {
            double x = (i/xmax)*nminusone;
            double y = Exp(-0.1*x)*Sin(x);
            Write($"{x}\t {y}\n");
        }
    } // makedata

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-makedata") makedata(args);
            if(arg == "-taska") taska(args);
            if(arg == "-taskb") taskb(args);
            if(arg == "-taskc") taskc(args);
        }
        return 0;
    } // Main
} // main
