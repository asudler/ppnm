CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

out.erf.gnuplot.svg : erf.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x" ;\
		set ylabel "erf(x)" ;\
		set title "error function" ;\
		set tics out ;\
		set grid ;\
		plot [-3.9:4.1] \
			"$<" with lines, \
			"erf.data.txt" using 1:2 with points pointtype 6, \
			"erf.data.txt" using (-($$1)):(-($$2)) with points pointtype 6 ;\
	' | tee log.gpi  | gnuplot

out.gamma.gnuplot.svg : gamma.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x" ;\
		set ylabel "gamma(x)" ;\
		set title "gamma function" ;\
		set tics out ;\
		set grid ;\
		plot [-4:4.5] \
			"$<" with lines, \
			"gamma.data.txt" using 1:2 with points pointtype 6, \
	' | tee log.gpi  | gnuplot

out.lngamma.gnuplot.svg : lngamma.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x" ;\
		set ylabel "ln(gamma(x))" ;\
		set title "ln gamma function" ;\
		set tics out ;\
		set grid ;\
		plot [0:4.5] \
			"$<" with lines, \
			"lngamma.data.txt" using 1:2 with points pointtype 6, \
	' | tee log.gpi  | gnuplot

erf.txt : erf.exe
	mono $< > $@

gamma.txt : gamma.exe
	mono $< > $@

lngamma.txt : lngamma.exe
	mono $< > $@

sfuns.dll : sfuns.cs ; $(MKLIB)
erf.exe : erf.cs sfuns.dll ; $(MKEXE)
gamma.exe : gamma.cs sfuns.dll ; $(MKEXE)
lngamma.exe : lngamma.cs sfuns.dll ; $(MKEXE)

clean : 
	$(RM) *.exe *.dll [Oo]ut* log* *.svg
