using System;
using static System.Math;
using static System.Console;
using System.IO;
using System.Globalization; // Import for invariant culture


class main{

	/*Defining Rosenbrock's valley function derivative*/
	/* Rosenbrock's valley function: f(x,y) = (1-x)²+100(y-x²)²  */
	/* Its derivative is: (dfdx, dfdy) = (-2(1-x)-400x(y-x²), 200(y-x²) ) */
	static Func<vector, vector> Rosenbrock = delegate(vector xy) { //Shape of the vector  is (x,y)
		return new vector (-2*(1-xy[0])-400*xy[0]*(xy[1]-xy[0]*xy[0]),200*(xy[1]-xy[0]*xy[0]));
	};
	/*It is expected to have global minimum at (a,a^2) which here is (1,1)*/



	/*Defining Himmelblau's function derivative*/
	/*Himmelblau's function is: f(x,y) = (x^2+y-11)^2+(x+y^2-7)^2*/
	/*Its derivative is: (dfdx, dfdy) = (4x(x^2+y-11)+2(x+y^2-7) ,2(x^2+y-11)+4y(x+y^2-7) )*/
	static Func<vector, vector> Himmelblau = delegate(vector xy) {
		return new vector  (4*xy[0]*(xy[0]*xy[0]+xy[1]-11)+2*(xy[0]+xy[1]*xy[1]-7),2*(xy[0]*xy[0]+xy[1]-11+4*xy[1]*(xy[0]+xy[1]*xy[1]-7)));
	};

	static void Main(){

		/*---------------------------PART A-----------------------------*/
		/*Finding the Rosenbrock roots*/
		vector Rosenbrock_Root = roots.newton(Rosenbrock,new vector(0.5,-0.5));

		WriteLine("----------------PART A------------------");
		WriteLine($"The Rosenbrock valley function was found to have a root at ({Rosenbrock_Root[0]},{Rosenbrock_Root[1]})");
		WriteLine($"Does this match the expected root at (1,1)? {vector.approx(Rosenbrock_Root, new vector(1.0, 1.0), 1e-2, 1e-2) ? "YES" : "NO"}");
		WriteLine("");

		/*Finding the minimas for the Himmelblau function*/
		vector[] start_vectors = {new vector(2.0,1.0), new vector(-2.1,2.9), new vector(-3.0,-2.0), new vector(3.2,-1.5) };
		int i = 0;

		while(i < start_vectors.Length){
			vector Himmelblau_Root = roots.newton(Himmelblau, start_vectors[i]);

			bool test = false;

			if (vector.approx(Himmelblau_Root, new vector(3.0, 2.0), 1e-2, 1e-2)) {test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(-2.805118, 3.131312), 1e-2, 1e-2)) {test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(-3.779310, -3.283186), 1e-2, 1e-2)) { test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(3.584428, -1.848126), 1e-2, 1e-2)) {test = true;}

			WriteLine($"The Himmelblau function was found to have a root at ({Himmelblau_Root[0]},{Himmelblau_Root[1]})");
			WriteLine($"Does this match any expected root? {test ? "YES" : "NO"}");

			i++;

		}

		/*-----------------------------PART B------------------------------*/

		/*Initalizing parameters*/
		double rmin = 0.1;
		double rmax = 10.0;
		double acc = 0.01;
		double eps = 0.01;

		/*Initializing generic lists to hold the last integration run for later comparison with anaylytical solution*/
		genlist<double> xs = new genlist<double>();
		genlist<vector> ys = new genlist<vector>();

		/*Defining function that outputs the wavefunction value at the boundary as a function of guessed energy*/
		/*Idea is that the function here solves for the wavefunction for a given E and outputs the value of the*/
		/*wavefunction at rmax, then we use newton to find the roots of this function, aka what E yields a*/
		/*wavefunction that goes to zero at rmax.*/

		Func<vector, vector> wavefunction_solver = delegate(vector E_guess)
		{
			double E = E_guess[0]; //energy guess

			Func<double, vector, vector> radial_SE = delegate(double r, vector y){ //radial SE to be solved
				return new vector(y[1], -2*(1/r+E)*y[0]);
			};

			vector init = new vector(rmin-rmin*rmin, 1-2*rmin); //initial conditions as stated in the exercise

			vector solution = ode.driver(radial_SE,rmin,init,rmax,xs,ys, acc: acc, eps: eps);

			return new vector(solution[0]);
		};


		vector result = roots.newton(wavefunction_solver, new vector(-1.0));

		xs = new genlist<double>();
		ys = new genlist<vector>();

		vector final_wavefunction = wavefunction_solver(new vector(result[0]));

		var out_wave = new StreamWriter("wavefunction_n.txt");
		for(int j = 0; j < xs.size; j++) {
			out_wave.WriteLine($"{xs[j].ToString(CultureInfo.InvariantCulture)}	{ys[j][0].ToString(CultureInfo.InvariantCulture)}");
		}
		out_wave.Close();

		WriteLine("");
		WriteLine("----------------PART B------------------");
		WriteLine($"The found root is: E0 = {result[0]}");
		WriteLine("Whereas the expected result is -0.5.");

		WriteLine("A plot of the numerical wavefunction (dots) vs the analytical one (line) can be found in wavefunction.png");

		var out_wave2 = new StreamWriter("wavefunction_a.txt");
		for(int j = 0; j < xs.size; j++){
			out_wave2.WriteLine($"{xs[j].ToString(CultureInfo.InvariantCulture)} {(xs[j]*Exp(-xs[j])).ToString(CultureInfo.InvariantCulture)}");
		}
		out_wave2.Close();

		var out_energy = new StreamWriter("true_groundstate.txt");
		for(int j = 0; j < 6; j++){
			out_energy.WriteLine($"{((double) j).ToString(CultureInfo.InvariantCulture)} {(-0.5).ToString(CultureInfo.InvariantCulture)}");
		}
		out_energy.Close();

		/*Investigating convergences*/
		WriteLine("");
		WriteLine("The test of convergence can be found in the seperate .png files named after each parameter that is varied");
		WriteLine("The analytical value is plotted as a line, while the numerical values are plotted as dots");

		int n = 5; //maximum number of iterations for convergence tests
		double E_guess2 = -0.8;

		/*rmax test*/
		var out_rmax = new StreamWriter("rmax.txt");
		WriteLine("Doing rmax convergence test...");
		for(int j = 1; j < n+1; j++ ){
			WriteLine($"{j} out of {n}");
			rmax = (double) j; //here the (double) explicitely typecasts j as a double
			vector result_rmax = roots.newton(wavefunction_solver, new vector(E_guess2));
			out_rmax.WriteLine($"{rmax.ToString(CultureInfo.InvariantCulture)} {result_rmax[0].ToString(CultureInfo.InvariantCulture)}");
		}
		out_rmax.Close();
		WriteLine("");
		WriteLine("rmax convergence test done");

		rmax = 10.0;

		/*rmin test*/
		var out_rmin = new StreamWriter("rmin.txt");
		WriteLine("");
		WriteLine("Doing  rmin convergence test...");
		for(int j = 1; j < n+1; j++ ){
			WriteLine($"{j} out of {n}");
			rmin = (double) j/10.0;
			vector result_rmin = roots.newton(wavefunction_solver, new vector(-1.0));
			out_rmin.WriteLine($"{rmin.ToString(CultureInfo.InvariantCulture)} {result_rmin[0].ToString(CultureInfo.InvariantCulture)}");
		}
		out_rmin.Close();
		WriteLine("");
		WriteLine("rmin convergence test done");

		rmin = 0.1;

		/*acc test*/
		WriteLine("");
		WriteLine("Doing acc convergence test...");
		var out_acc = new StreamWriter("acc.txt");
		for(int j = 1; j < n+1; j++ ){
			WriteLine($"{j} out of {n}");
			acc = (double) j/100;
			vector result_acc = roots.newton(wavefunction_solver, new vector(-1.0));
			out_acc.WriteLine($"{acc.ToString(CultureInfo.InvariantCulture)} {result_acc[0].ToString(CultureInfo.InvariantCulture)}");
		}
		out_acc.Close();
		WriteLine("");
		WriteLine("acc convergence test done");

		acc = 0.01;

		/*eps test*/
		WriteLine("");
		WriteLine("Doing eps convergence test...");
		var out_eps = new StreamWriter("eps.txt");
		for(int j = 1; j < n+1; j++ ){
			WriteLine($"{j} out of {n}");
			eps = (double) j/100;
			vector result_eps = roots.newton(wavefunction_solver, new vector(-1.0)); 
			out_eps.WriteLine($"{eps.ToString(CultureInfo.InvariantCulture)} {result_eps[0].ToString(CultureInfo.InvariantCulture)}");
		}
		out_eps.Close();
		WriteLine("");
		WriteLine("eps convergence test done");

	} //End of Main()


} //End of main class
