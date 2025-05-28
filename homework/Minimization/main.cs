using System;
using static System.Math;
using static System.Console;
using System.IO;
using System.Globalization; // Import for invariant culture

class main{

	static Func<vector, double> Rosenbrock =  delegate(vector xy){
		return Pow((1-xy[0]),2)+100*Pow((xy[0]-xy[1]*xy[1]),2);
	};

	static Func<vector, double> Himmelblau = delegate(vector xy){
		return Pow((xy[0]*xy[0]+xy[1]-11),2)+Pow((xy[0]+xy[1]*xy[1]-7),2);
	};


	static void Main(){

		/*----------------PART A------------------*/
		WriteLine("--------------------------------PART A--------------------------------");


		/*Testing Rosenbrock minimum*/
		WriteLine("Testing the minimization on the Rosenbrock function with an expected minimum at (1,1)");

		vector Rosenbrock_start = new vector(0.8,0.8);
		(vector Rosenbrock_min, int iterations) = minimizer.newton(Rosenbrock, Rosenbrock_start);

		WriteLine($"The found minimum is at ({Rosenbrock_min[0]},{Rosenbrock_min[1]}) in {iterations} iterations");
		WriteLine($"Does this match with the expected? - {vector.approx(Rosenbrock_min, new vector(1.0, 1.0), 1e-2, 1e-2) ? "YES" : "NO"}");



		/*Testing Himmelblau minimum*/
		WriteLine("");
		WriteLine("Testing the minimization on the Himmelblau function with several known minima (see wiki)");

		vector Himmelblau_start = new vector(2.7,1.6);
		(vector Himmelblau_min, int iterations2) = minimizer.newton(Himmelblau, Himmelblau_start);

		bool test = false;

		if (vector.approx(Himmelblau_min, new vector(3.0, 2.0), 1e-2, 1e-2)) {test = true;}
		else if (vector.approx(Himmelblau_min, new vector(-2.805118, 3.131312), 1e-2, 1e-2)) {test = true;}
		else if (vector.approx(Himmelblau_min, new vector(-3.779310, -3.283186), 1e-2, 1e-2)) { test = true;}
		else if (vector.approx(Himmelblau_min, new vector(3.584428, -1.848126), 1e-2, 1e-2)) {test = true;}

		WriteLine($"The found minimum is at ({Himmelblau_min[0]},{Himmelblau_min[1]}) in {iterations2} iterations");
		WriteLine($"Does this match any expected minimum? {test ? "YES" : "NO"}");

		/*----------------PART B------------------*/
		WriteLine("--------------------------------PART B--------------------------------");

	} //End of Main()


} //End of main class
