CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.interpolate.png : in.interpolate.gpi out.fcn.dat out.interpolate.dat
	gnuplot -e "outfile='$@' ; data1='$(word 2,$^)' ; data2='$(word 3,$^)'" $<

out.interpolate.dat : main.exe out.fcn.dat
	mono $< < $(word 2,$^) -interpolate \
	-n_neurons:12 -n_out:100 -xi:-1 -xf:1 > $@

out.fcn.dat : main.exe
	mono $< -makedata -xi:-1.1 -xf:1.1 -n:69 > $@

main.exe : main.cs ann.dll ; $(MKEXE)
ann.dll : ann.cs minimization.cs vector.cs matrix.cs qrgs.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

