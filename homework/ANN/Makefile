# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

DATA = training_data.txt
RESP = network_response.txt
PLOTA = plot_A.png

all: $(EXEC) Out.txt $(PLOTA) $(DATA) $(RESP)

Out.txt: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Generating the plot
$(PLOTA): $(DATA) $(RESP)
	graph -T png -x -1.2 1.2 -y -1.2 1.2 -X "x data" -Y "y data" -L "ANN training" --bitmap-size 800x600 \
		-S 1 -m 1 $(RESP) \
		-S 4 -m 0 $(DATA) \
	> $(PLOTA)

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ../classes/ode.cs minimization.cs ann.cs  
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll *.txt *.png
