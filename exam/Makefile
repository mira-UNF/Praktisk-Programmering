# Executable name
EXEC = main.exe
# C# source file
SRC = main.cs

SIGNAL = complete_signal.txt
PREDICT = predicted_signal.txt
PLOT_PREDICT = signal_prediction.png
ERROR = error_vs_n.txt
PLOT_ERROR = error_vs_n.png

all: run $(EXEC) $(SIGNAL) $(PREDICT) $(PLOT_PREDICT) $(ERROR) $(PLOT_ERROR) Out.txt

run: $(EXEC)
	mono $(EXEC) > Out.txt

$(EXEC): $(SRC) matrix.dll
	mcs -target:exe -out:main.exe main.cs -reference:matrix.dll

# Generating the plot
$(PLOT_PREDICT): $(SIGNAL) $(PREDICT)
	graph -T png -x 0 100 -y -3 4 -X "Index" -Y "Signal" -L "Least Squares Extrapolation" --bitmap-size 800x600 \
		-S 1 -m 1 $(PREDICT) \
		-S 4 -m 0 $(SIGNAL) \
	> $(PLOT_PREDICT)

$(PLOT_ERROR): $(ERROR)
	graph -T png -x 1 6 -y -1 6 -X "n value" -Y "Mean Squared Error" -L "Error vs n" --bitmap-size 800x600 \
		-S 1 -m 1 $(ERROR) \
	> $(PLOT_ERROR)

# Compile the matrix library
matrix.dll: ../homework/classes/matrix.cs ../homework/classes/genlist.cs ../homework/classes/vector.cs ../homework/classes/QR_GS.cs least_squares_prediction.cs   
	mcs -target:library -out:matrix.dll $^

.PHONY: clean
clean:
	rm -f main.exe matrix.dll *.txt *.png
