# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

RMAX = rmax.txt
PLOT_RMAX = rmax.png

RMIN = rmin.txt
PLOT_RMIN =  rmin.png

ACC = acc.txt
PLOT_ACC = acc.png

EPS = eps.txt
PLOT_EPS = eps.png

WN = wavefunction_n.txt
WA = wavefunction_a.txt
PLOT_WF = wavefunction.png
EREF = true_groundstate.txt

all: run $(EXEC) Out.txt $(EREF) $(RMAX) $(PLOT_RMAX) $(RMIN) $(PLOT_RMIN) $(ACC) $(PLOT_ACC) $(EPS) $(PLOT_EPS) $(WA) $(WN) $(PLOT_WF)

run: $(EXEC)
	mono $(EXEC)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

$(PLOT_RMAX): $(RMAX) $(EREF)
	graph -T png -x 0 10 -y -2 2 -X "rmax" -Y "energy" -L "rmax convergence" --bitmap-size 800x600 \
	-S 3 -m0 $(RMAX) \
	-S 1 -m1 $(EREF) \
	>$(PLOT_RMAX)

$(PLOT_RMIN): $(RMIN) $(EREF)
	graph -T png -x 0 1 -y -2 2 -X "rmin" -Y "energy" -L "rmin convergence" --bitmap-size 800x600 \
	-S 3 -m0 $(RMIN) \
	-S 1 -m1 $(EREF) \
	>$(PLOT_RMIN)

$(PLOT_ACC): $(ACC) $(EREF)
	graph -T png -x 0 0.1 -y -2 2 -X "acc" -Y "energy" -L "acc convergence" --bitmap-size 800x600 \
	-S 3 -m0 $(ACC) \
	-S 1 -m1 $(EREF) \
	>$(PLOT_ACC)

$(PLOT_EPS): $(EPS) $(EREF)
	graph -T png -x 0 0.1 -y -2 2 -X "eps" -Y "energy" -L "eps convergence" --bitmap-size 800x600 \
	-S 3 -m0 $(EPS) \
	-S 1 -m1 $(EREF) \
	>$(PLOT_EPS)

$(PLOT_WF): $(WN) $(WA)
	graph -T png -x -1 10 -y -2 2 -X "r" -Y "f" -L "Wavefunction" --bitmap-size 800x600 \
	-S 3 -m0 $(WN) \
	-S 1 -m1 $(WA) \
	>$(PLOT_WF)


# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ../classes/ode.cs roots.cs 
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll *.txt *.png
