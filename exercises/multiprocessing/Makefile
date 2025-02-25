#Out.txt : main.exe
#	mono main.exe > Out.txt

N=1e8
#TIME = time --portability --append --output $@
#TIME = /usr/bin/time --portability --append --output $@

TIME = /usr/bin/time -p

Out.txt : main.exe Makefile
	>$@
	$(TIME) mono $< -terms:$N -threads:1 >>$@
	$(TIME) mono $< -terms:$N -threads:2 >>$@
	$(TIME) mono $< -terms:$N -threads:3 >>$@
	$(TIME) mono $< -terms:$N -threads:4 >>$@

main.exe : main.cs
	mcs main.cs

.PHONY: clean
clean:
	rm -f main.exe Out.txt
