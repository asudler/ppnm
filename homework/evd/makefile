CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

out.wavefunctions.png : out.wavefunctions.data makefile
	echo '\
		set terminal png background "white" ; \
		set out "$@" ; \
		set xlabel "r (Bohr radius)" ; \
		set ylabel "radial hydrogen wavefunction" ; \
		f0(x) = 2*x*exp(-x) ; \
		f1(x) = -x*(1-x/2)*exp(-x/2)/sqrt(2) ; \
		f2(x) = 2*x*(1-2*x/3 + 2*x*x/27)*exp(-x/3)/sqrt(27) ; \
		plot "$<" u ($$0*0.05):1 skip 2 w lp title "numeric ground state" , \
		"" u ($$0*0.05):2 skip 2 w lp title "numeric 1st excited state" , \
		"" u ($$0*0.05):3 skip 2 w lp title "numeric 2nd excited state" , \
		f0(x) w l lw 2 title "analytic ground state" , \
		f1(x) w l lw 2 title "analytic 1st excited state" , \
		f2(x) w l lw 2 title "analytic 2nd excited state" ; \
	' | tee log.gpi | gnuplot

out.wavefunctions.data : main.exe
	mono $< -partb -rmax:20 -dr:0.05 -wavefunctions > $@

out.convergence.change_rmax.png : out.convergence.change_rmax.data makefile
	echo '\
		set terminal png background "white" ; \
		set out "$@" ; \
		set xlabel "rmax (Bohr radius)" ; \
		set ylabel "E0 (Hartree)" ; \
		set title "convergence check with rmax" ; \
		plot "$<" every ::1 u 1:3 w lp ; \
	' | tee log.gpi | gnuplot

out.convergence.change_rmax.data : main.exe
	$(RM) $@ ; \
	for rmax in $$(seq 1 1 20) ; do \
		mono $< -partb -rmax:$$rmax -dr:0.1 -convergence >> $@ ; \
	done

out.convergence.change_dr.png : out.convergence.change_dr.data makefile
	echo '\
		set terminal png background "white" ; \
		set out "$@" ; \
		set xlabel "dr (Bohr radius)" ; \
		set ylabel "E0 (Hartree)" ; \
		set title "convergence check with dr" ; \
		plot "$<" every ::1 u 2:3 w lp ; \
	' | tee log.gpi | gnuplot

out.convergence.change_dr.data : main.exe
	$(RM) $@ ; \
	for dr in $$(seq 0.01 0.01 0.3) ; do \
		mono $< -partb -rmax:10 -dr:$$dr >> $@ -convergence ; \
	done

main.exe : main.cs jacobi.dll ; $(MKEXE)
jacobi.dll : matrix.cs vector.cs jacobi.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg
