using System;
using static System.Math;

class Program{
	public static int Main(){

	//This makes all the different calculations
	complex z0 = cmath.sqrt(new complex(-1,0));
	Console.WriteLine($"square root of -1 - {z0}, sqrt(-1)  == i -> {z0.approx(complex.I)} \n");

	complex z1 = cmath.sqrt(complex.I);
	Console.WriteLine($"square root of i - {z1}, sqrt(i) == 0.707 + 0.707i -> {z1.approx(new complex(1/Sqrt(2),1/Sqrt(2)))}\n");

	complex z2 = cmath.exp(complex.I);
	Console.WriteLine($"e^i - {z2}, e^i == cos(1)+sin(1)i -> {z2.approx(new complex(Cos(1),Sin(1)))}\n");

	complex z3 = cmath.exp(new complex(0,PI));
	Console.WriteLine($"e^ipi - {z3}, e^ipi == -1 -> {z3.approx(new complex(-1,0))} \n");

	complex z4 = cmath.pow(complex.I,complex.I);
	Console.WriteLine($"i^i -  {z4}, i^i == 0.208 -> {z4.approx(new complex(Exp(-PI/2),0))} \n");

	complex z5 = cmath.log(complex.I);
	Console.WriteLine($"ln(i) - {z5}, ln(i) == i pi/2 -> {z5.approx(new complex(0,PI/2))} \n");

	complex z6 = cmath.sin(new complex(0,PI));
	Console.WriteLine($"sin(i pi) - {z6}, sin(i pi) == 11.54i -> {z6.approx(new complex(0,11.548739357257748377977334315388409))}\n");

	return 0;
	}





}
