CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

out.wavefunctions.data : main.exe
	mono $< -partb -rmax:10 -dr:0.05 -wavefunctions > $@

out.convergence.change_rmax.data : main.exe
	$(RM) $@ ; \
	for rmax in $$(seq 1 1 20) ; do \
		mono $< -partb -rmax:$$rmax -dr:0.1 -convergence >> $@ ; \
	done

out.convergence.change_dr.data : main.exe
	$(RM) $@ ; \
	for dr in $$(seq 0.01 0.01 0.3) ; do \
		mono $< -partb -rmax:10 -dr:$$dr >> $@ -convergence ; \
	done

main.exe : main.cs jacobi.dll ; $(MKEXE)
jacobi.dll : matrix.cs vector.cs jacobi.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg
