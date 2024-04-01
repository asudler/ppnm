using System.IO;
using System;
using static System.Console;
using static System.Math;
using static spline;

public static class main {
    public static void taska(string[] args) {
        // get input parameters
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

        // run linear spline and linear integration
        for(int i = 0; i <= nintervals; i++) {
            double z = (x[x.Length-1]/nintervals)*i;
            Write($"{z}\t {linear_spline(x, y, z)}\t");
            Write($"{linterpInteg(x, y, z)}\n");
        }
    }

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
    }

    public static int Main(string[] args) {
        foreach(string arg in args) {
            if(arg == "-makedata") makedata(args);
            if(arg == "-parta") taska(args);
        }
        return 0;
    }
}
