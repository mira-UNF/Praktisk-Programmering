# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

DATA = Higgs_data.txt
FIT = Higgs_fit.txt
PLOT = Higgs.png 

all: run $(EXEC) $(DATA) $(FIT) $(PLOT) Out.txt

run: $(EXEC)
	mono $(EXEC)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Generating the plot
$(PLOT): $(DATA) $(FIT)
	graph -T png -x 100 160 -y -6 14 -X "Energy [GeV]" -Y "Signal [arb.]" -L "Higgs fit" --bitmap-size 800x600 \
		-S 1 -m 1 $(FIT) \
		-S 4 -m 0 -I e $(DATA) \
	> $(PLOT)

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ../classes/ode.cs minimization.cs  
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll *.txt *.png
