class main {
    public static bool approx
    (double a, double b, double acc=1e-9, double eps=1e-9){
	    if(System.Math.Abs(b-a) <= acc) return true;
	    if(System.Math.Abs(b-a) <= System.Math.Max(System.Math.Abs(a),System.Math.Abs(b))*eps) return true;
	    return false;
    }

    static int task1() {
        int i = 1;
        while(i+1 > i) { i++; }
        System.Console.Write($"my max int = {i}\n");
        System.Console.Write($"system max value = {int.MaxValue}\n");
        
        i = 1;
        while (i-1 < i) { i++; }
        System.Console.Write($"my min int = {i}\n");
        System.Console.Write($"system min value = {int.MinValue}\n");
        
        return 0;
    } // min and max values
    
    static int task2() {
        double x = 1;
        while(1+x != 1) { x/=2; }
        x*=2;
        System.Console.Write($"my double precision = {x}\n");
        System.Console.Write($"approx sys double precision = {System.Math.Pow(2,-53)}\n");

        float y = 1F;
        while((float)(1F+y) != 1F) { y/=2F; }
        y*=2F;
        System.Console.Write($"my float precision = {y}\n");
        System.Console.Write($"approx sys float precision = {System.Math.Pow(2,-23)}\n");

        return 0;
    } // machine epsilon

    static int task3() {
        double epsilon = System.Math.Pow(2,-52);
        double tiny = epsilon/2;
        double a = 1+tiny+tiny;
        double b = tiny+tiny+1;
        System.Console.Write($"a==b ? {a==b}\n");
        System.Console.Write($"a>1  ? {a>1}\n");
        System.Console.Write($"b>1  ? {b>1}\n");

        System.Console.Write("explanation: rounding differences.\n");
        System.Console.Write("for 'a', 'tiny' is rounded to 0 ");
        System.Console.Write("since there already exists a single-digit number ");
        System.Console.Write("before the addition operation.\n");
        System.Console.Write("for 'b', 'tiny' + 'tiny' is calculated ");
        System.Console.Write("and then added to 1, which gives an answer ");
        System.Console.Write("greater than 0.\n");

        return 0;
    } // calculations beyond machine epsilon

    static int task4() {
        double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
        double d2 = 8*0.1;
        System.Console.WriteLine($"d1={d1:e15}");
        System.Console.WriteLine($"d2={d2:e15}");
        System.Console.WriteLine($"d1==d2 ? => {d1==d2}"); 
        System.Console.WriteLine($"d1 approx d2 ? => {approx(d1,d2)}");

        return 0;
   } // comparing beyond machine epsilon

    static int Main() { 
        task1();
        task2();
        task3();
        task4();
        return 0; 
    }
}
