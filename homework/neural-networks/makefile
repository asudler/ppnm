CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.taska.dat : main.exe out.fcn.dat
	mono $< < $(word 2,$^) -taska -n_neurons:10 -n_out:69 -xi:-1 -xf:1 > $@

out.fcn.dat : main.exe
	mono $< -makedata -xi:-1.0 -xf:1.0 -n:50 > $@

main.exe : main.cs ann.dll ; $(MKEXE)
ann.dll : ann.cs minimization.cs vector.cs matrix.cs qrgs.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

