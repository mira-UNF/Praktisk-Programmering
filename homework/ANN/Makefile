# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

DATA = training_data.txt
RESP = network_response.txt
PLOTA = plot_A.png

DATAB = training_dataB.txt
RESPB = network_responseB.txt
PLOTB = plot_B.png

DERIV = deriv_data.txt
DDERIV = dderiv_data.txt
ADERIV = aderiv_data.txt

DRESP = deriv_response.txt
DDRESP = dderiv_response.txt
ADRESP = aderiv_response.txt

DPLOT = plot_deriv.png
DDPLOT = plot_dderiv.png
ADPLOT = plot_aderiv.png

all: $(EXEC) Out.txt $(PLOTA) $(PLOTB) $(DPLOT) $(DDPLOT) $(ADPLOT) $(DATA) $(RESP)

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

$(PLOTB): $(DATAB) $(RESPB)
	graph -T png -x -3.14 3.14 -y -1.2 1.2 -X "x data" -Y "y data" -L "ANN training" --bitmap-size 800x600 \
		-S 1 -m 1 $(RESPB) \
		-S 4 -m 0 $(DATAB) \
	> $(PLOTB)

$(DPLOT): $(DERIV) $(DRESP)
	graph -T png -x -3.14 3.14 -y -1.2 1.2 -X "x data" -Y "y data" -L "ANN derivative" --bitmap-size 800x600 \
		-S 1 -m 1 $(DRESP) \
		-S 4 -m 0 $(DERIV) \
	> $(DPLOT)

$(DDPLOT): $(DDERIV) $(DRESP)
	graph -T png -x -3.14 3.14 -y -1.2 1.2 -X "x data" -Y "y data" -L "ANN double derivative" --bitmap-size 800x600 \
		-S 1 -m 1 $(DDRESP) \
		-S 4 -m 0 $(DDERIV) \
	> $(DDPLOT)

$(ADPLOT): $(ADERIV) $(ADRESP)
	graph -T png -x -3.14 3.14 -y -1.2 1.2 -X "x data" -Y "y data" -L "ANN antiderivative" --bitmap-size 800x600 \
		-S 1 -m 1 $(ADRESP) \
		-S 4 -m 0 $(ADERIV) \
	> $(ADPLOT)

# Compile the matrix library
matrix.dll: ../classes/matrix.cs ../classes/genlist.cs ../classes/vector.cs ../classes/QR_GS.cs ../classes/ode.cs minimization.cs ann.cs  
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll *.txt *.png
