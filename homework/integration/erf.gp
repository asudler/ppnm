set terminal pngcairo size 1024, 768 background "white"
set out outfile
set xlabel "z"
set ylabel "erf(z)"
set key outside right

set title "Error Function"
plot mydata w l title "erf(z), my implementation", \
wikidata w p title "erf(z), from wikipedia table"

