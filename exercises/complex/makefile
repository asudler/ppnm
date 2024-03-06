CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKDLL = mcs -target:library -out:$@ $(DLLS) $(CODE)

all: Out.txt
	cat $<
Out.txt: main.exe
	mono $< > $@
main.exe: main.cs cmath.dll ; $(MKEXE)
cmath.dll: cmath.cs complex.cs ; $(MKDLL)

clean:
	$(RM) [Oo]ut* *.dll *.exe
