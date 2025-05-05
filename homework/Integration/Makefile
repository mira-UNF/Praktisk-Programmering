# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs
TAB_ERF = tabulated_erf.txt
CALC_ERF = calculated_erf.txt
PLOT_ERF = erf_comparison.png
ACC_ERF = accuracy_erf.txt
PLOT_ACC = accuracy_erf.png

all: $(EXEC) $(TAB_ERF) $(CALC_ERF) $(PLOT_ERF) $(ACC_ERF) $(PLOT_ACC)

$(TAB_ERF) $(CALC_ERF) $(ACC_ERF): $(EXEC)
	mono $(EXEC)

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs integrator.cs
	mcs -target:library -out:matrix.dll $^


$(PLOT_ERF): $(TAB_ERF) $(CALC_ERF)
	graph -T png -x 0 2 -y 0 1 -X "x" -Y "erf(x)" -L "erf(x) comparison plot" --bitmap-size 800x600 \
		-S 4 -m 0 $(TAB_ERF) \
		-S 1 -m 1 $(CALC_ERF) \
	>$(PLOT_ERF)

$(PLOT_ACC): $(ACC_ERF)
	graph -T png -x -17 -1 -y -25 -4 -X "Log(accuracy)" -Y "Log(erf(1)-reference)" -L "Value as a function of accuracy" --bitmap-size 800x600 \
		-S 1 -m 1 $(ACC_ERF) \
	>$(PLOT_ACC)

# Clean build artifacts
.PHONY: clean
clean:
	rm -f main.exe matrix.dll $(TAB_ERF) $(CALC_ERF) $(PLOT_ERF) $(ACC_ERF) $(PLOT_ACC)
