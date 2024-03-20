CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.fit.png : out.fit.data experiment.data makefile
	echo '\
        set terminal png background "white" ; \
		set out "$@" ; \
        set xlabel "time (days)" ; \
		set ylabel "ln[activity] (no dim.)" ; \
		set title "exponential decay fit" ; \
		load "$<" ; \
		f(t)=log(a)-(lambda*t) ; \
		plot "$(word 2,$^)" skip 1 u 1:(log($$2)):($$3/$$2) w yerrorbars \
		title "experimental data" , \
		f(x) title "least-squares fit f(x) = ln(a) - lambda*t" ; \
	' | tee log.gpi | gnuplot

out.fit.data : main.exe experiment.data
	mono $< -input:$(word 2,$^) > $@

main.exe : main.cs ls.dll ; $(MKEXE)
ls.dll : ls.cs qrgs.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg
