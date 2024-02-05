out.txt : main.exe
	mono $< > $@

main.exe : main.cs sfuns.dll
	mcs -target:exe -reference:sfuns.dll -out:main.exe main.cs

sfuns.dll : sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs

clean :
	rm -f main.exe out.txt sfuns.dll

