set terminal png background "white"
set out outfile
set xlabel "x"
set ylabel "y"

f(x)=exp(-x)*sin(x)
F(x)=-0.5*exp(-x)*(cos(x)+sin(x))
set title "Linear Interpolation, f(x) = exp(-x)*sin(x)"
plot f(x) title "function", \
F(x) title "antiderivative (analytical)", \
data w p title "raw data", \
interpolation u 1:2 w p title "linear interpolation", \
interpolation u 1:3 w p title "linear integration"

