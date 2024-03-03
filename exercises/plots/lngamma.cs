class main {
    public static int Main() {
        for(double x = 0.1; x <= 4.1; x += 1.0/8) {
            System.Console.WriteLine($"{x} {sfuns.lngamma(x)}");
        }

        return 0;
    }
}
