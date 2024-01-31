out.txt: hello.exe			    # out.txt depends on hello.exe
	mono hello.exe > out.txt    # run hello.exe

hello.exe: hello.cs		        # hello.exe depends on hello.cs
	mcs hello.cs				# compile hello.cs into hello.exe

clean:						    # no dependencies
	rm -f out.txt hello.exe     # remove secondary files

