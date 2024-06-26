CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.time.png : out.time.dat makefile
	echo '\
		set terminal png background "white" ; \
		set out "$@" ; \
		set xlabel "N (size of matrix)" ; \
		set ylabel "real time (s)" ; \
		set title "real time vs N" ; \
		f(x)=A*x**3+bg ; \
		A=5e-9 ; \
		bg=0.02 ; \
		fit f(x) "$<" via A, bg ; \
		plot "$<" u 1:2 w lp title "real time" , \
		f(x) title "cubic fit f(x)=A*x**3 + bg" ; \
	' | tee log.gpi | gnuplot

out.time.dat : main.exe
	for N in $$(seq 20 5 600) ; do \
		time --format "$$N %e" --output $@ --append \
		mono $< -time -n:$$N ; \
	done

out.debug.dat : main.exe
	mono $< -debug -n:25 -m:10

main.exe : main.cs qrgs.dll ; $(MKEXE)
qrgs.dll : qrgs.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg
