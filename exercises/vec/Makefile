Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs
	mcs main.cs

vec.dll: vec.cs
	mcs -target:library -out:vec.dll vec.cs

main.exe: main.cs vec.dll
	mcs -target:exe -out:main.exe -reference:vec.dll main.cs
