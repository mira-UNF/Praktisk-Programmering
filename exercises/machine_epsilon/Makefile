Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs
	mcs main.cs

funs.dll : funs.cs
	mcs -target:library -out:funs.dll funs.cs

main.exe : main.cs funs.dll
	mcs -target:exe -out:main.exe -reference:funs.dll main.cs

.PHONY: clean
clean:
	rm -f main.exe Out.txt
