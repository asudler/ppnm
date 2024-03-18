using System;
using System.Collections.Generic;
using static System.Console;
using static ls;

public static class main {
    public static void exponential_decay_fit() {
        var fs = new Func<double,double>[] { z => 1.0, z => -z }; // ln
    }

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
        // (note: this seems overly complicated...)
        double[] times = new double[9];
        double[] activities = new double[9];
        double[] uncertainties = new double[9];
        var instream = new System.IO.StreamReader(infile); 
        string header = instream.ReadLine(); // ignore header
        char[] split_delimiters = {' ','\t','\n'};
        var split_options = StringSplitOptions.RemoveEmptyEntries;
        int counter = 0;
        for(string line = instream.ReadLine();
        line != null; line = instream.ReadLine()) {
            WriteLine("yo");
            var data = line.Split(split_delimiters, split_options);
            Write($"{data[2]}\n");
            times[counter] = double.Parse(data[0]);
            times[counter] = double.Parse(data[1]);
            times[counter] = double.Parse(data[2]);
        }
        instream.Close();

        exponential_decay_fit();        
        return 0;
    }
}
