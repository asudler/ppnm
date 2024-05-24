CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.taskc.png : in.taskc.gpi out.taskc.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.taskb.png : in.taskb.gpi out.taska.dat out.taskb.dat
	gnuplot -e "outfile='$@' ; data1='$(word 2,$^)' ; data2='$(word 3,$^)'" $<

out.taska.png : in.taska.gpi out.taska.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<
	
out.taskc.dat : main.exe
	mono $< -taskc -n:25000 > $@ 2> out.taskc.log

out.taskb.dat : main.exe
	mono $< -taskb -n:25000 -skip:100 -nmin:10 -nmax:100000 \
	> $@ 2> out.taskb.log

out.taska.dat : main.exe
	mono $< -taska -n:25000 -skip:100 -nmin:10 -nmax:100000 \
	> $@ 2> out.taska.log

main.exe : main.cs montecarlo.dll ; $(MKEXE)
montecarlo.dll : montecarlo.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

