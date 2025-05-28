using System;
using static System.Math;
using static System.Console;
using System.IO;
using System.Globalization; // Import for invariant culture
using System.Collections.Generic;

class main{

	static Func<vector, double> Rosenbrock =  delegate(vector xy){
		return Pow((1-xy[0]),2)+100*Pow((xy[0]-xy[1]*xy[1]),2);
	};

	static Func<vector, double> Himmelblau = delegate(vector xy){
		return Pow((xy[0]*xy[0]+xy[1]-11),2)+Pow((xy[0]+xy[1]*xy[1]-7),2);
	};


	static Func<vector, double> Breit_Wigner = delegate(vector param){
		double E = param[0];
		double m = param[1];
		double Gamma = param[2];
		double A = param[3];

		return A/(Pow(E-m,2)+Gamma*Gamma/4);
	};

	public static Func<vector, double> Deviation;

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

		/*Preparing datafile*/
		List<double> E = new List<double> {
  		101, 103, 105, 107, 109, 111, 113, 115, 117, 119,
    		121, 123, 125, 127, 129, 131, 133, 135, 137, 139,
    		141, 143, 145, 147, 149, 151, 153, 155, 157, 159
		};

		List<double> Sig = new List<double> {
    		-0.25, -0.30, -0.15, -1.71, 0.81, 0.65, -0.91, 0.91, 0.96, -2.52,
    		-1.01, 2.01, 4.83, 4.58, 1.26, 1.01, -1.26, 0.45, 0.15, -0.91,
    		-0.81, -1.41, 1.36, 0.50, -0.45, 1.61, -2.21, -1.86, 1.76, -0.50
		};

		List<double> Sig_err = new List<double> {
    		2.0, 2.0, 1.9, 1.9, 1.9, 1.9, 1.9, 1.9, 1.6, 1.6,
    		1.6, 1.6, 1.6, 1.6, 1.3, 1.3, 1.3, 1.3, 1.3, 1.3,
   	 	1.1, 1.1, 1.1, 1.1, 1.1, 1.1, 1.1, 0.9, 0.9, 0.9
		};

		var HiggsData = new StreamWriter("Higgs_data.txt");
		for(int i = 0; i < E.Count; i++){
			HiggsData.WriteLine($"{E[i].ToString(CultureInfo.InvariantCulture)} {Sig[i].ToString(CultureInfo.InvariantCulture)} {Sig_err[i].ToString(CultureInfo.InvariantCulture)}");
		}
		HiggsData.Close();

		/*Defining the deviation function*/
		Deviation = delegate(vector param){
			double m = param[0];
			double Gamma = param[1];
			double A = param[2];
			double sum = 0;

			for(int i = 0; i < E.Count; i++){

				vector input = new vector(E[i],m,Gamma,A);
				sum += Pow((Breit_Wigner(input)-Sig[i])/Sig_err[i],2);
			}
			//WriteLine($"Cost at m={m:F2}, Γ={Gamma:F2}, A={A:F2} → {sum:F3}");
			return sum;
		};

		/*Doing the minimization*/
		vector Higgs_start = new vector(125.9,3.69,24.95);

		(vector Higgs_min, int iterations3) = minimizer.newton(Deviation,Higgs_start);

		WriteLine("The minimizing parameters for the Breit-Wigner fit of the Higgs data is");
		WriteLine($"m = {Higgs_min[0]} [GeV]");
		WriteLine($"Gamma = {Higgs_min[1]} [arb. u.]");
		WriteLine($"A = {Higgs_min[2]} [arb. u.]");
		WriteLine($"These were found in {iterations3} iterations");

		/*Creating datafile for fit*/
		List<double> linspace = new List<double>();
		int n = 100;
		double start = 101;
		double end = 159;
		double step = (end - start) / (n - 1);

		for (int i = 0; i < n; i++)
		{
			linspace.Add(start + i * step);
		}

		var HiggsFit = new StreamWriter("Higgs_fit.txt");
		for(int i = 0; i < linspace.Count; i++){
			vector fit_input = new vector(linspace[i],Higgs_min[0],Higgs_min[1],Higgs_min[2]);
			double fit_val = Breit_Wigner(fit_input);
			HiggsFit.WriteLine($"{linspace[i].ToString(CultureInfo.InvariantCulture)} {fit_val.ToString(CultureInfo.InvariantCulture)}");
		}
		HiggsFit.Close();

	} //End of Main()


} //End of main class

