CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.linear.png : linear.gp out.linear.data in.function.data
	gnuplot -e "outfile='$@' ; interpolation='$(word 2,$^)' ; \
	data='$(word 3,$^)'" $<

out.linear.data : main.exe in.function.data
	mono $< -parta -nintervals:100 -input:$(word 2,$^) > $@

in.function.data : main.exe
	mono $< -makedata -nminusone:10 -xmax:10 > $@

main.exe : main.cs spline.dll ; $(MKEXE)
spline.dll : spline.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg

