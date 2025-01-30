using System;

class Program{
	static void Main(){

	//square root of 2
	double  res0 = Math.Sqrt(2);
	Console.WriteLine($"The square root of 2 is {res0}");

	//check of result
	double check = Math.Pow(res0,2.0);
	Console.WriteLine($"this should equal 2 it is {check}");

	//2 to the power of 1/5
	double res1 = Math.Pow(2,1.0/5.0);
	Console.WriteLine($"2 to the power of 1/5 is {res1}");

	//e to the power of pi
	double res2 = Math.Pow(Math.E,Math.PI);
	Console.WriteLine($"e to the power of pi is {res2}");

	//pi to the power of e
	double res3 = Math.Pow(Math.PI,Math.E);
	Console.WriteLine($"pi to the power of e is {res3}");
	
	for (int i = 1; i <= 10; i++){

		//gamme function
		double res4 = sfuns.fgamma(i);
		Console.WriteLine($"Gamme function of {i} is {res4}"); 
		}

	}

}
