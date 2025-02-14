out.txt : main.exe
	mono main.exe -input:input.txt -output:out.txt

.PHONY: clean
clean:
	rm -f main.exe out.txt

list.dll: list.cs
	mcs -target:library -out:list.dll list.cs

main.exe: main.cs list.dll
	mcs -target:exe -out:main.exe -reference:list.dll main.cs
