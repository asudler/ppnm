CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.taska.dat : main.exe
	mono $< -taska 2> out.taska.log

main.exe : main.cs roots.dll ; $(MKEXE)
roots.dll : roots.cs qrgs.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

