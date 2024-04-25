CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.taskb.dat : main.exe
	mono $< -taskb 2> out.taskb.log

out.erf.png : erf.gp erf_wikipedia.dat out.taska.dat
	gnuplot -e "outfile='$@' ; mydata='$(word 3,$^)' ; \
	wikidata='$(word 2,$^)'" $<

out.taska.dat : main.exe
	mono $< -taska -n:500 -start:-4 -end:4 > $@ 2> out.taska.log

main.exe : main.cs integrate.dll ; $(MKEXE)
integrate.dll : integrate.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

