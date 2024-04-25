CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.cubic.png : in.cubic.gp out.cubic.data in.function.data
	gnuplot -e "outfile='$@' ; interpolation='$(word 2,$^)' ; \
    data='$(word 3,$^)'" $<

out.cubic.data : main.exe in.function.data
	mono $< -taskc -nintervals:100 -input:$(word 2,$^) > $@ 2> log.err

out.quadratic.png : in.quadratic.gp out.quadratic.data in.function.data
	gnuplot -e "outfile='$@' ; interpolation='$(word 2,$^)' ; \
    data='$(word 3,$^)'" $<

out.quadratic.data : main.exe in.function.data
	mono $< -taskb -nintervals:100 -input:$(word 2,$^) > $@ 2> log.err

out.linear.png : in.linear.gp out.linear.data in.function.data
	gnuplot -e "outfile='$@' ; interpolation='$(word 2,$^)' ; \
	data='$(word 3,$^)'" $<

out.linear.data : main.exe in.function.data
	mono $< -taska -nintervals:100 -input:$(word 2,$^) > $@

in.function.data : main.exe
	mono $< -makedata -nminusone:10 -xmax:10 > $@

main.exe : main.cs spline.dll ; $(MKEXE)
spline.dll : spline.cs ; $(MKLIB)

clean :
	$(RM) *.data *.exe *.dll [Oo]ut* log* *.svg

