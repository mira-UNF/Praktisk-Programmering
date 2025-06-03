# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

SHO = simple_harmonic.txt
PLOT_SHO = simple_harmonic.png

DAMP = damped_harmonic.txt 
PLOT_DAMP = damped_harmonic.png

CIRC = circular_orbit.txt
PLOT_CIRC = circular_orbit.png

ELLIP = elliptical_orbit.txt
PLOT_ELLIP = elliptical_orbit.png

PRECESS = precession_orbit.txt
PLOT_PRECESS = precession_orbit.png

all: Out.txt  $(SHO) $(PLOT_SHO) $(DAMP) $(PLOT_DAMP) $(CIRC) $(PLOT_CIRC) $(ELLIP) $(PLOT_ELLIP) $(PRECESS) $(PLOT_PRECESS)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

$(SHO): $(EXEC)
	@echo "Running $(EXEC) to generate data..."
	mono $(EXEC)

$(PLOT_SHO): $(SHO)
	graph -T png -x -1 10 -y -2 2 -X "x-data" -Y "y-data" -L "Simple Harmonic" --bitmap-size 800x600 \
	-S 1 -m1 $(SHO) \
	>$(PLOT_SHO)

$(PLOT_DAMP): $(DAMP)
	graph -T png -x -1 10 -y -2 2 -X "x-data" -Y "y-data" -L "Damped  Harmonic" --bitmap-size 800x600 \
	-S 1 -m1 $(DAMP) \
	>$(PLOT_DAMP)

$(PLOT_CIRC): $(CIRC)
	graph -T png -x -4 4 -y -4 4 -X "x-data" -Y "y-data" -L "Circular Orbit" --bitmap-size 800x800 \
	-S 1 -m1 $(CIRC) \
	>$(PLOT_CIRC)

$(PLOT_ELLIP): $(ELLIP)
	graph -T png -x -4 4 -y -4 4 -X "x-data" -Y "y-data" -L "Elliptical Orbit" --bitmap-size 800x800 \
	-S 1 -m1 $(ELLIP) \
	>$(PLOT_ELLIP)

$(PLOT_PRECESS): $(PRECESS)
	graph -T png -x -4 4 -y -4 4 -X "x-data" -Y "y-data" -L "Precession Orbit" --bitmap-size 800x800 \
	-S 1 -m1 $(PRECESS) \
	>$(PLOT_PRECESS)

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ode.cs
	mcs -target:library -out:matrix.dll $^



# Clean build artifacts
.PHONY: clean
clean:
	rm -f main.exe matrix.dll $(SHO) $(PLOT_SHO) $(DAMP) $(PLOT_DAMP) $(CIRC) $(PLOT_CIRC) $(ELLIP) $(PLOT_ELLIP) $(PRECESS) $(PLOT_PRECESS)
