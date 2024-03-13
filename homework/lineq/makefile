CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

out.times.png : out.times.data makefile
	echo '\
		set terminal png background "white" ; \
		set out "$@" ; \
		set xlabel "N (size of matrix)" ; \
		set ylabel "real time (s)" ; \
		set title "real time vs N" ; \
		f(x)=A*x**3+bg ; \
		A=5e-9 ; \
		bg=0.02 ; \
		fit f(x) 'out.times.data' via A, bg ; \
		plot "$<" u 1:2 w lp title "real time" , \
		f(x) title "cubic fit f(x)=A*x**3 + bg" ; \
	' | tee log.gpi | gnuplot

out.times.data : time.exe
	for N in $$(seq 20 1 400) ; do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N ; \
	done

time.exe : time.cs qrgs.dll ; $(MKEXE)
main.exe : main.cs qrgs.dll ; $(MKEXE)
qrgs.dll : vector.cs matrix.cs qrgs.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg

