using System;
using static System.Console;
using static qrgs;

public class main {
    public static int Main(string[] args) {
        int size = 3;
        foreach(string arg in args) {
            var words = arg.Split(":");
            if(words[0] == "-size") size = (int)double.Parse(words[1]);
        }
        Write($"size = {size}...");

        Random rnd = new Random();
        matrix A = new matrix(size,size);
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                A[i,j] = rnd.NextDouble();
            }
        }
        Write("QRGS decomposing...");
        decomp(A);
        Write("done\n");

        return 0;
    }
}
