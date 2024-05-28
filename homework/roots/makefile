CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.convacc.png : in.convacc.gpi out.taskb.convacc.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.convradii.png : in.convradii.gpi out.taskb.convradii.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.taskb.png : in.taskb.gpi out.taskb.dat
	gnuplot -e "outfile='$@' ; data='$(word 2,$^)'" $<

out.taskc.dat : main.exe
	mono $< -taskc 2> out.taskc.log

out.taskb.dat : main.exe
	mono $< -taskb -rmin:0.1 -rmax:10 -guess:-1 > $@ 2> out.taskb.log

out.taska.dat : main.exe
	mono $< -taska 2> out.taska.log

main.exe : main.cs roots.dll ode.dll ; $(MKEXE)
ode.dll : ode.cs rk.cs spline.cs ; $(MKLIB)
roots.dll : roots.cs qrgs.cs matrix.cs vector.cs ; $(MKLIB)

clean :
	$(RM) *.exe *.dll [Oo]ut* *log *.svg

