out.txt : main.exe input.txt
	mono main.exe < input.txt 1> out.txt 2> log

main.exe : main.cs genlist.dll
	mcs -out:$@ -target:exe -reference:genlist.dll main.cs

extra.exe : extra.cs genlist.dll
	mcs -out:$@ -target:exe -reference:genlist.dll extra.cs

genlist.dll : genlist.cs
	mcs -out:$@ -target:library genlist.cs

clean :
	$(RM) *.dll *.exe log* [Oo]ut*

