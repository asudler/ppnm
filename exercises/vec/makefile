out.txt : main.exe
	mono $< > $@

main.exe : main.cs vec.dll
	mcs -target:exe -reference:vec.dll -out:main.exe main.cs

vec.dll : vec.cs
	mcs -target:library -out:vec.dll vec.cs

clean :
	$(RM) vec.dll main.exe out.txt


