using System;
using static System.Console;
using static System.Math;

class main {
    static int task1() {
        // compute sqrt(2)
        double x=Math.Sqrt(2);
        Write($"sqrt(2)={x}\n");
        Write($"(check: sqrt(2)^2={Math.Pow(x,2)})\n");

        // compute 2^(1/5)
        x=Math.Pow(2, 0.2);
        Write($"2^(1/5)={x}\n");
        Write($"(check: (2^(1/5))^5={Math.Pow(x,5)})\n");

        // compute e^pi
        x=PI;
        Write($"e^pi={Exp(x)}\n");
        Write($"(check: Log(e^pi)={Log(Exp(x))})\n");

        // compute pi^e
        x=E;
        Write($"pi^e={Pow(PI,x)}\n");
        Write($"(check: Log_pi(pi^e)={Log(Pow(PI,x),PI)})\n");

        return 0;  
    }

    static int task2() {
        double prod=1;
        for(int x=1; x<10; x++) {
            Write($"fgamma({x})={sfuns.fgamma(x)}\n");
            prod*=x;
        }

        return 0;
    }

    static int task3() {
        double prod=1;
        for(int x=1; x<10; x++) {
            Write($"lnfgamma({x})={sfuns.lnfgamma(x)}\n");
            prod*=x;
        }

        return 0;
    }

    static int Main() {
        task1();
        task2();
        task3();
        return 0;
    }
}
