out1.txt : task1.exe
	mono $< -numbers:3.1415926,0,6.28 > $@

task1.exe : task1.cs
	mcs -target:exe -out:$@ $<

out2.txt : task2.exe
	echo 1 2 3 4 5 | mono $< 2> $@

task2.exe : task2.cs
	mcs -target:exe -out:$@ $<

out3.txt : task3.exe
	mono $< -input:input.txt -output:$@

task3.exe : task3.cs
	mcs -target:exe -out:$@ $<

clean :
	$(RM) out1.txt out2.txt out3.txt task1.exe task2.exe task3.exe

