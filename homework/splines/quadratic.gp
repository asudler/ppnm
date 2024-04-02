set terminal png background "white"
set out outfile
set xlabel "x"
set ylabel "y"

df(x)=exp(-x/10)*(cos(x)-0.1*sin(x))
f(x)=exp(-x/10)*sin(x)
F(x)=-(10./101.)*exp(-x/10)*(10*cos(x)+sin(x))+1
set title "Quadratic Spline, f(x) = exp(-x)*sin(x)"
plot f(x) title "function", \
df(x) title "derivative", \
F(x) title "antiderivative (analytical)", \
data w p title "raw data", \
interpolation u 1:2 w p title "quadratic spline", \
interpolation u 1:3 w p title "spline differentiation", \
interpolation u 1:4 w p title "spline integration"
