out.txt : main.exe
	mono $< > $@

main.exe : main.cs
	mcs -target:exe -out:$@ $<

clean : 
	$(RM) out.txt main.exe
