CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)

out.lotkavolterra.dat : main.exe
	mono $< -lotkavolterra -a:1.5 -b:1.0 -c:3.0 -d:1.0 \
	-xi:10 -yi:5 -ti:0 -tf:15 > $@ 2> err.log

out.oscillator.dat : main.exe
	mono $< -oscillator -b:0.25 -c:5.0 -yi:3.04159 -dyi:0 -ti:0 -tf:10 \
	> $@ 2> err.log

main.exe : main.cs ode.dll ; $(MKEXE)
ode.dll : ode.cs rk.cs ; $(MKLIB)

clean :
	$(RM) *.data *.exe *.dll [Oo]ut* log* *.svg

