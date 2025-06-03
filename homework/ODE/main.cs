using System;
using System.IO;
using static System.Console;
using static System.Math;
using System.Globalization; // Import for invariant culture

class main{

	/*Defines the function for the simple harmonic oscillator*/
	static Func<double, vector, vector> SHO = delegate(double x, vector y) {
		return new vector (y[1], -y[0]);
	};

	/*Defines the function for the damped simple harmonic oscillator*/
	static Func<double, vector, vector> DAMP = delegate(double x, vector y) {
		double b = 0.25; double c = 5;
		return new vector (y[1], -b*y[1]-c*Sin(y[0]));
	};

	/*Defining the function for the newtonian orbits*/
	static Func<double, vector, vector> NEWT = delegate(double x, vector y) {
		double y0 = y[0];
		double y1 = y[1];
		double eps = 0;

		vector dydx = new vector(2);

		dydx[0] = y1;
		dydx[1] = 1 - y0 + eps*y0*y0;

		return dydx;
	};

	/*Defining the function for the relativist orbit*/
	static Func<double, vector, vector> REL = delegate(double x, vector y) {
    		double y0 = y[0];
    		double y1 = y[1];
    		double eps = 0.01;

		vector dydx = new vector(2);

    		dydx[0] = y1;
    		dydx[1] = 1 - y0 + eps*y0*y0;

    		return dydx;
	};


	static void Main(){

		/*-------------------------PART A------------------------*/

		/*Initializing my lists*/
		var xs = new genlist<double>(); //This is essentially my time variable
		var ys = new genlist<vector>();

		/*Solving the simple harmonic oscillator system*/
		var simple_harmonic = new  StreamWriter("simple_harmonic.txt");
		vector y = ode.driver(SHO,0, new vector(0,1),4*PI,xs,ys);

		/*Writing datafile for the solved equation*/
		for(int i=0; i < xs.size; i++){
			simple_harmonic.WriteLine($"{xs[i].ToString(CultureInfo.InvariantCulture)} {ys[i][0].ToString(CultureInfo.InvariantCulture)}");
		}
		simple_harmonic.Close();

		/*Initializing my lists again*/
		xs = new genlist<double>();
		ys = new genlist<vector>();

		/*Solving the damped harmonic oscillator*/
		var damped_harmonic = new StreamWriter("damped_harmonic.txt");
		vector y2 = ode.driver(DAMP, 0, new vector(0,1), 4*PI, xs, ys);

		for(int i=0; i < xs.size; i++){
			damped_harmonic.WriteLine($"{xs[i].ToString(CultureInfo.InvariantCulture)} {ys[i][0].ToString(CultureInfo.InvariantCulture)}");
		}
		damped_harmonic.Close();

		/*-------------------------PART B--------------------------*/

		/*Initializing my lists*/
		xs = new genlist<double>();
		ys = new genlist<vector>();

		/*Solving the circular orbit system*/
		double u0 = 1; //Initial conditions
		double u_prime0 = 1e-1; //Making this slightly larger than zero due to problems with step-size
		var circular_orbit = new StreamWriter("circular_orbit.txt");
		vector y3 = ode.driver(NEWT, 0, new vector(u0,u_prime0), 4*PI, xs, ys);

		/*Writing the datafile for the circular orbit*/

		/*I do the calculation of the actual orbits here*/
		double xprint;
		double yprint;
		for(int i=0; i < xs.size; i++){
			xprint = (1/ys[i][0])*Cos(xs[i]);
			yprint  = (1/ys[i][0])*Sin(xs[i]);

			circular_orbit.WriteLine($"{xprint.ToString(CultureInfo.InvariantCulture)} {yprint.ToString(CultureInfo.InvariantCulture)}");
		}
		circular_orbit.Close();

		/*Initializing my lists*/
		xs = new genlist<double>();
		ys = new genlist<vector>();

		/*Solving the elliptical orbit system*/
		u0 = 1; //Initial conditions
		u_prime0 = -0.5;
		var elliptical_orbit = new StreamWriter("elliptical_orbit.txt");
		vector y4 = ode.driver(NEWT, 0, new vector(u0,u_prime0), 4*PI, xs, ys);

		/*Writing the datafile for the elliptical orbit*/

		for(int i=0; i < xs.size; i++){
			xprint = (1/ys[i][0])*Cos(xs[i]);
			yprint = (1/ys[i][0])*Sin(xs[i]);

			elliptical_orbit.WriteLine($"{xprint.ToString(CultureInfo.InvariantCulture)} {yprint.ToString(CultureInfo.InvariantCulture)}");
		}
		elliptical_orbit.Close();

		/*Initializing lists*/
		xs = new genlist<double>();
		ys = new genlist<vector>();

		/*Solving the elliptical orbit with precession*/
		u0 = 1; //Initial conditions
		u_prime0 = -0.5;
		var precession_orbit = new StreamWriter("precession_orbit.txt");
		vector y5 = ode.driver(REL, 0, new vector(u0,u_prime0), 12*PI, xs, ys);

		/*Writing the datafile for the precession orbit*/

		for(int i=0; i < xs.size; i++){
			xprint = (1/ys[i][0])*Cos(xs[i]);
			yprint = (1/ys[i][0])*Sin(xs[i]);

			precession_orbit.WriteLine($"{xprint.ToString(CultureInfo.InvariantCulture)} {yprint.ToString(CultureInfo.InvariantCulture)}");
		}
		precession_orbit.Close();


		WriteLine("--------------------PART A----------------------");
		WriteLine("The solution to the simple harmonic oscillator and the damped harmonic oscillator can be found in simple_harmonic.png and damped_harmonic.png respectively.");
		WriteLine("--------------------PART B----------------------");
		WriteLine("The solutions to the different orbits can be found in circular_orbit.png, elliptical_orbit.png and precesseion_orbit.png");

	}



}
