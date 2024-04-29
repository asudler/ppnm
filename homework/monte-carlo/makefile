CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.taska.dat : main.exe
	mono $< -taska -n:500 > $@ 2> out.taska.log

main.exe : main.cs montecarlo.dll ; $(MKEXE)
montecarlo.dll : montecarlo.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

