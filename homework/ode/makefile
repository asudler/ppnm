CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.threebody_interp.gif : in.threebody_interp.gpi out.threebody_interp.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.threebody_interp.dat : main.exe makefile
	mono $< -threebody -dxi1:0.4662036850 -dyi1:0.4323657300 \
	-dxi2:-0.93240737 -dyi2:-0.86473146 -dxi3:0.4662036850 \
	-dyi3:0.4323657300 -xi1:-0.97000436 -yi1:0.24308753 \
	-xi3:0.97000436 -yi3:-0.24308753 -ti:0 -tf:7 -dt:0.25 \
	> $@ 2> err.log

out.threebody_raw.dat : main.exe makefile
	mono $< -threebody -dxi1:0.4662036850 -dyi1:0.4323657300 \
	-dxi2:-0.93240737 -dyi2:-0.86473146 -dxi3:0.4662036850 \
	-dyi3:0.4323657300 -xi1:-0.97000436 -yi1:0.24308753 \
	-xi3:0.97000436 -yi3:-0.24308753 -ti:0 -tf:7 \
	> $@ 2> err.log

out.planets.png : in.planets.gpi out.planet1.dat out.planet2.dat out.planet3.dat
	gnuplot -e "outfile='$@' ; data1='$(word 2,$^)' ; data2='$(word 3,$^)' ; \
	data3='$(word 4,$^)'" $<

out.planet3.dat : main.exe
	mono $< -planet -greps:0.01 -yi:1 -dyi:-0.5 -ti:0 -tf:50 -dt:0.1 \
    > $@ 2> err.log

out.planet2.dat : main.exe
	mono $< -planet -greps:0 -yi:1 -dyi:-0.5 -ti:0 -tf:10 -dt:0.1 \
    > $@ 2> err.log

out.planet1.dat : main.exe
	mono $< -planet -greps:0 -yi:1 -dyi:0 -ti:0 -tf:10 -dt:0.1 \
	> $@ 2> err.log

out.lotkavolterra.png : in.lotkavolterra.gpi out.lotkavolterra.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.lotkavolterra.dat : main.exe
	mono $< -lotkavolterra -a:1.5 -b:1.0 -c:3.0 -d:1.0 \
	-xi:10 -yi:5 -ti:0 -tf:15 > $@ 2> err.log

out.oscillator.png : in.oscillator.gpi out.oscillator.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.oscillator.dat : main.exe
	mono $< -oscillator -b:0.25 -c:5.0 -yi:3.04159 -dyi:0 -ti:0 -tf:10 \
	> $@ 2> err.log

main.exe : main.cs ode.dll ; $(MKEXE)
ode.dll : ode.cs rk.cs spline.cs ; $(MKLIB)

clean :
	$(RM) *.data *.exe *.dll [Oo]ut* *.log *.svg

