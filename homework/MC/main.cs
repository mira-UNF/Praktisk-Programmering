using System;
using static System.Console;
using static System.Math;
using System.Globalization; // Import for invariant culture
using System.IO;

class main {

	static void Main() {
		/*------------------------PART A---------------------------*/

		/*Calculating area of unit circle to test MC plain integration*/

		/*Defining my unit circle*/
		Func<vector, double> f;

		f = delegate(vector x) {
		if(x.norm() <= 1) return 1;
		else return 0;
		}; //This checks whether you are inside a unit circle or not


		/*Setting integration limits*/
		vector a = new vector(-1,-1); //This is (x_lower bound, y_lower bound)
		vector b = new vector(1,1); // similar to above but upper bound

		/*List of number of points in order to compare actual vs estimated error*/

		int[] Ns = new int[] {100,200,300,400,500,750,1000,1250,1500,2000,2500,3000,3500};

		/*The exact value of the  integral*/
		double exact = PI;

		/*Creating a streamwriter to make the datafile*/
		var unit_circle_estimated = new StreamWriter("unit_circle_estimated.txt"); //File for the estimated error vs N
		var unit_circle_actual = new StreamWriter("unit_circle_actual.txt"); //Actual error vs N

		for(int i = 0; i < Ns.Length; i++){
			(double result, double error) = MC.plain_integrate(f,a,b,Ns[i]);

			unit_circle_estimated.WriteLine($"{Ns[i].ToString(CultureInfo.InvariantCulture)} {error.ToString(CultureInfo.InvariantCulture)}");
			unit_circle_actual.WriteLine($"{Ns[i].ToString(CultureInfo.InvariantCulture)} {Abs(exact-result).ToString(CultureInfo.InvariantCulture)}");
		}
		unit_circle_estimated.Close();
		unit_circle_actual.Close();

		WriteLine("-----------------------PART A-------------------------");
		WriteLine("The unit circle area integration error can be found in unit_circle.png, the solid line is the estimated error, the dotted line is the actual error");

		/*Trying to calculate the funky integral*/

		/*Function definition*/
		f = delegate(vector k){
			return Pow(PI,-3)*Pow(1-Cos(k[0])*Cos(k[1])*Cos(k[2]),-1);
		};

		/*Boundary conditions*/
		vector a2 = new vector(0,0,0);
		vector b2 = new vector(PI,PI,PI);

		/*Number of points*/
		int N = 4000;

		(double result2, double error2) = MC.plain_integrate(f,a2,b2,N);

		WriteLine("The difficult singular integral should evaluate to approx 1.3932039296856768591842462603255");
		WriteLine($"The MC integrator evaluates it to be {result2} with an error of {error2} and {N} points");

		/*-------------------------------PART B--------------------------------*/

		/*Estimating the area of the unit circle now using the quasi random method*/

		f = delegate(vector x) {
    		if(x.norm() <= 1) return 1;
    		else return 0;
		}; // This checks whether you are inside a unit circle or not

		var unit_circle_qrand_estimated = new StreamWriter("unit_circle_qrand_estimated.txt");

		for(int i = 0; i < Ns.Length; i++){
			(double result3, double error3) = MC.quasi_integrate(f, a, b, Ns[i]);

			unit_circle_qrand_estimated.WriteLine($"{Ns[i].ToString(CultureInfo.InvariantCulture)} {error3.ToString(CultureInfo.InvariantCulture)}");
		}
		unit_circle_qrand_estimated.Close();

		WriteLine("---------------------PART B---------------------");

		WriteLine("The comparison between the plain and quasi random MC integration can be found in comparison.png");
		WriteLine("The solid line is 1/Sqrt(N) and the dashed line is the quasi random method");

	} //End of Main()




} //End of main class
