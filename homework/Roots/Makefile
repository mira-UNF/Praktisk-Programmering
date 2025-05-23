# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

all: run $(EXEC) Out.txt

run: $(EXEC)
	mono $(EXEC)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ../classes/ode.cs roots.cs 
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll Out.txt
