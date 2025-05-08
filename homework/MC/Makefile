# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs
UCIRC_EST = unit_circle_estimated.txt
UCIRC_ACT = unit_circle_actual.txt
UCIRC_PLOT = unit_circle.png
UCIRC_QRAND = unit_circle_qrand_estimated.txt
COMP_PLOT = comparison.png

all: run $(EXEC) $(UCIRC_EST) $(UCIRC_ACT) $(UCIRC_PLOT) $(UCIRC_QRAND) $(COMP_PLOT) Out.txt

run: $(EXEC)
	mono $(EXEC)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll


$(UCIRC_PLOT): $(UCIRC_EST) $(UCIRC_ACT)
	graph -T png -x 0 4000 -y 0 1 -X "number of points N" -Y "error" -L "Unit Circle Error Scaling" --bitmap-size 800x600 \
	-S1 -m1 $(UCIRC_EST) \
	-S1 -m2 $(UCIRC_ACT) \
	>$(UCIRC_PLOT)

$(COMP_PLOT): $(UCRIC_QRAND) $(UCIRC_EST)
	graph -T png -x 0 4000 -y 0 1 -X "number of points N" -Y "error" -L "Comparison of quasi random and 1/Sqrt(N) scaling" --bitmap-size 1400x1200 \
	-S1 -m1 $(UCIRC_EST) \
	-S1 -m2 $(UCIRC_QRAND) \
	>$(COMP_PLOT)

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs MC.cs
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll Out.txt $(UCIRC_EST) $(UCIRC_ACT) $(UCIRC_PLOT) $(UCIRC_QRAND) $(COMP_PLOT)
