CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.higgs.png : in.higgs.gpi in.higgs.dat out.taskb.dat out.taskc.dat
	gnuplot -e "outfile='$@' ; data1='$(word 2,$^)' ; data2='$(word 3,$^)' ; \
	data3='$(word 4,$^)'" $<
	
out.taskc.dat : main.exe
	mono $< -taskc -infile:in.higgs.dat -skip_header:1 \
	-m0:126.8 -Gamma0:1 -A0:5 > $@ 2> out.taskc.log

out.taskb.dat : main.exe
	mono $< -taskb -infile:in.higgs.dat -skip_header:1 \
	-m0:126.8 -Gamma0:1 -A0:5 > $@

out.taska.log : main.exe
	mono $< -taska 2> $@

main.exe : main.cs minimization.dll ; $(MKEXE)
minimization.dll : minimization.cs qrgs.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg
