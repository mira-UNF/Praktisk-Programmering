Out.txt : main.exe
	mono main.exe -input:input.txt -output:out.txt

main.exe : main.cs
	mcs main.cs

.PHONY: clean
clean:
	rm -f main.exe Out.txt
