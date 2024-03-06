CODE = $(filter %.cs, $^)
LIBS = $(addprefix -reference:, $(filter %.dll, $^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$nthreads %e %U"

out.times.svg : out.times.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "number of threads" ;\
		set ylabel "real time" ;\
		set title "running time as a fcn of number of threads" ;\
		plot "$<" u 1:2 w lp title "real time" ;\
		plot "$<" u 1:3 w lp title "user time" ;\
	' | tee log.gpi | gnuplot
out.parallelfor.svg : out.parallelfor.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "number of threads" ;\
		set ylabel "real time" ;\
		set title "running time as a fcn of number of threads" ;\
		plot "$<" u 1:2 w lp title "real time" ;\
		plot "$<" u 1:3 w lp title "user time" ;\
	' | tee log.gpi | gnuplot
out.linq.svg : out.linq.txt makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "number of threads" ;\
		set ylabel "real time" ;\
		set title "running time as a fcn of number of threads" ;\
		plot "$<" u 1:2 w lp title "real time" ;\
		plot "$<" u 1:3 w lp title "user time" ;\
	' | tee log.gpi | gnuplot

N = 1e8
out.times.txt : main1.exe
	for nthreads in 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16; do\
        $(TIME) mono main1.exe -nthreads:$$nthreads -nterms:$(N); \
    done
out.parallelfor.txt : main2.exe
	for nthreads in 1 2 3 4 ; do\
        $(TIME) mono main2.exe -nthreads:$$nthreads -nterms:$(N); \
    done
out.linq.txt : main3.exe
	for nthreads in 1 2 3 4 ; do\
        $(TIME) mono main2.exe -nthreads:$$nthreads -nterms:$(N); \
    done

main1.exe : main1.cs ; $(MKEXE)
main2.exe : main2.cs ; $(MKEXE)
main3.exe : main3.cs ; $(MKEXE)

clean :
	$(RM) *.exe *.dll [Oo]ut* log* *.svg

