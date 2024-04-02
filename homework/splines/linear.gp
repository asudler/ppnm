set terminal svg background "white"
set out outfile
set xlabel "x"
set ylabel "y"

f(x)=exp(-x/10)*sin(x)
F(x)=-(10./101.)*exp(-x/10)*(10*cos(x)+sin(x))+1
set title "Linear Interpolation, f(x) = exp(-x/10)*sin(x)"
plot f(x) title "function", \
F(x) title "antiderivative (analytical)", \
data w p title "raw data", \
interpolation u 1:2 w p title "linear interpolation", \
interpolation u 1:3 w p title "linear integration"

