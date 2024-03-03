class main {
    public static int Main() {
        for(double x = -3.9; x <= 4.1; x += 1.0/8) {
            System.Console.WriteLine($"{x} {sfuns.gamma(x)}");
        }

        return 0;
    }
}
