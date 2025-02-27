TIME = /usr/bin/time -p

Out.txt : main.exe 
	mono main.exe > Out.txt

matrix.dll: ../classes/matrix.cs ../classes/vector.cs QR_GS.cs
	mcs -target:library -out:matrix.dll $^

main.exe: main.cs matrix.dll
	mcs -target:exe -out:main.exe -reference:matrix.dll main.cs


.PHONY: clean
clean:
	rm -f main.exe Out.txt
