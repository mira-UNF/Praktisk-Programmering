# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

DATA_LINTERP = linterp_data.txt
SPLINE_LINTERP = linterp_spline.txt
INTEG_LINTERP = linterp_integral.txt
PLOT_LINTERP = linterp.png
QSPLINE_DATA = qspline_data.txt
QSPLINE = qspline.txt
QSPLINE_INTEG = qspline_integ.txt
QSPLINE_DERIV = qspline_deriv.txt
PLOT_QSPLINE = qspline.png

all: $(EXEC) run $(PLOT_LINTERP) $(PLOT_QSPLINE)

run: $(EXEC)
	rm -f $(DATA_LINTERP)  # Remove old file
	mono $(EXEC)           # Generate new data


# Compile the C# program
$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Generate the data files by running the compiled program
$(DATA_LINTERP) $(SPLINE_LINTERP) $(QSPLINE) $(QSPLINE_DATA) $(QSPLINE_INTEG) $(QSPLINE_DERIV) $(INTEG_LINTERP): $(EXEC)
	@echo "Running $(EXEC) to generate data..."
	mono $(EXEC)

# Generating the plot
$(PLOT_LINTERP): $(DATA_LINTERP) $(SPLINE_LINTERP) $(INTEG_LINTERP)
	graph -T png -x -1 10 -y -2 2 -X "x-data" -Y "y-data" -L "Linear Spline Test" --bitmap-size 800x600 \
	-S 3 -m 0 $(DATA_LINTERP) \
	-S 1 -m 1 $(SPLINE_LINTERP) \
	-S 1 -m 2 $(INTEG_LINTERP) \
	> $(PLOT_LINTERP) 

$(PLOT_QSPLINE): $(QSPLINE_DATA) $(QSPLINE) $(QSPLINE_INTEG) $(QSPLINE_DERIV)
	graph -T png -x -1 10 -y -2 2 -X "x-data" -Y "y-data" -L "Quadratic Spline Test" --bitmap-size 800x600 \
	-S 3 -m 0 $(QSPLINE_DATA) \
	-S 1 -m 1 $(QSPLINE) \
	-S 1 -m 2 $(QSPLINE_INTEG) \
	-S 1 -m 3 $(QSPLINE_DERIV) \
	> $(PLOT_QSPLINE) 


# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/vector.cs ../classes/QR_GS.cs linearinterpol.cs qspline.cs
	mcs -target:library -out:matrix.dll $^



# Clean build artifacts
.PHONY: clean
clean:
	rm -f main.exe $(PLOT_LINTERP) $(PLOT_QSPLINE) $(DATA_LINTERP) $(QSPLINE) $(QSPLINE_DATA) $(QSPLINE_INTEG) $(QSPLINE_DERIV) $(SPLINE_LINTERP) $(INTEG_LINTERP) matrix.dll  
