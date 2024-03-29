using System;
using System.Collections.Generic;
using static System.Math;
using static System.Console;
using static ls;

public static class main {
    public static int Main(string[] args) {
        // get input file
        string infile = null;
        foreach(var arg in args) {
            var words = arg.Split(':');
            if(words[0] == "-input") infile = words[1];
        }
        if(infile == null) {
            Error.WriteLine("wrong filename argument");
            return 1;
        }

        // read input file
        // (note: this seems overly complicated and specific...)
        double[] times = new double[9];
        double[] activities = new double[9];
        double[] uncertainties = new double[9];
        var instream = new System.IO.StreamReader(infile); 
        instream.ReadLine(); // ignore header
        char[] split_delimiters = {' ','\t','\n'};
        var split_options = StringSplitOptions.RemoveEmptyEntries;
        int counter = 0;
        for(string line = instream.ReadLine();
        line != null; line = instream.ReadLine()) {
            var data = line.Split(split_delimiters, split_options);
            times[counter] = double.Parse(data[0]);
            activities[counter] = double.Parse(data[1]);
            uncertainties[counter]
                = Abs(double.Parse(data[2])/activities[counter]); // \del y/y
            activities[counter] = Log(activities[counter]); // ln(y)
            counter += 1;
        }
        instream.Close();
        
        var fs = new Func<double,double>[] { z => 1.0, z => -z }; // ln fit
        (vector c, matrix S) = lsfit(fs, times, activities, uncertainties);
//        Write($"output from fit f(t) = ln(a) - lambda*t:\n");
        Write($"a={c[0]}\n");
        Write($"lambda={c[1]}\n");
        Write($"uncert_a={Sqrt(S[0,0])}\n");
        Write($"uncert_lambda={Sqrt(S[1,1])}\n");
        Error.Write("S matrix:\n");
        for(int i = 0; i < S.size1; i++) {
            for(int j = 0; j < S.size2; j++) {
                Error.Write($"{S[i,j]}\t");
            }
            Error.Write("\n");
        }
//        Write($"from the fit, the half-life time t_h = ln(2)/lambda ");
//        Write($"= {Log(2.0)/c[1]}. The modern accepted value for ");
//        Write($"the half-life of Radium-224 is 3.6 days ");
//        Write($"(https://www.britannica.com/science/radium-224).\n");
        
        return 0;
    }
}
