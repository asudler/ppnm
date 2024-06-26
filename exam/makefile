CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.time.png : in.time.gpi ../homework/lineq/out.time.dat out.time.dat
	gnuplot -e "outfile='$@' ; qrdat='$(word 2,$^)' ; \
	qrfit='out.qrfit.log' ; ludat='$(word 3,$^)' ; lufit='out.lufit.log'" \
	$<

out.time.dat : main.exe
	for N in $$(seq 20 5 600) ; do \
		time --format "$$N %e" --output $@ --append \
		mono $< -time -n:$$N ; \
	done

out.debug.dat : main.exe
	mono $< -debug -n:25 2> $@

main.exe : main.cs lu.dll ; $(MKEXE)
lu.dll : lu.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg
