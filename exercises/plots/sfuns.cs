public static class sfuns{
    public static double erf(double x){
        // single precision error function (Abramowitz and Stegun, from Wikipedia)
        if(x < 0) return -erf(-x);
        double[] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};
        double t = 1/(1 + 0.3275911*x);
        double sum = t*(a[0]+t*(a[1]+t*(a[2]+t*(a[3]+t*a[4])))); // smh
        return 1 - sum*System.Math.Exp(-x*x);
    }
    public static double gamma(double x){
        if(x<0) return System.Math.PI/System.Math.Sin(System.Math.PI*x)/gamma(1-x);
        if(x<9) return gamma(x+1)/x;
        double lngamma = x*System.Math.Log(x+1/(12*x-1/x/10))-x+System.Math.Log(2*System.Math.PI/x)/2;
        return System.Math.Exp(lngamma);
    }
    public static double lngamma(double x){
        if(x<=0) throw new System.ArgumentException("lngamma: x<=0");
        if(x<9) return lngamma(x+1)-System.Math.Log(x);
        return x*System.Math.Log(x+1/(12*x-1/x/10))-x+System.Math.Log(2*System.Math.PI/x)/2;
    }
} // sfuns
