using System;
using System.IO;
using static System.Console;
using static System.Math;
using System.Globalization; // Import for invariant culture

class main {

	static void Main(){
		/*--------------------------PART A----------------------------*/

		/*Checking the implementation against some integrals*/
		Func<double, double> f;

		WriteLine("Checking some integrals to test the integrator class");
		WriteLine("----------------------------------------------------");

		//Integral of sqrt of x
		f = delegate(double x){ return Sqrt(x); };
		(double I1res, double I1err) = integrator.integrate(f,0,1);
		WriteLine($"Integral 1 --- Sqrt(x) from 0 to 1 ---  2/3 = {I1res} is {integrator.compare(I1res, 2.0/3.0)}");
		WriteLine($"The error is --- {I1err}");
		WriteLine("");
		WriteLine("");

		//Integral of 1/sqrt of x
		f = delegate(double x){return 1/Sqrt(x); };
		(double I2res, double I2err) = integrator.integrate(f,0,1);
		WriteLine($"Integral 2 --- 1/Sqrt(x) from 0 to 1 ---  2 = {I2res} is {integrator.compare(I2res, 2.0)}");
		WriteLine($"The error  is --- {I2err}");
		WriteLine("");
		WriteLine("");

		//Integral of sqrt(1-x^2)
		f = delegate(double x){return 4*Sqrt(1-Pow(x,2)); };
		(double I3res, double I3err) = integrator.integrate(f,0,1);
		WriteLine($"Integral 3 --- 4*Sqrt(1-x^2) from 0 to 1 ---  PI = {I3res} is {integrator.compare(I3res, PI)}");
		WriteLine($"The error is --- {I3err}");
		WriteLine("");
		WriteLine("");

		//Integral of ln(x)/sqrt(x)
		f = delegate(double x){return Log(x)/Sqrt(x); };
		(double I4res, double I4err) = integrator.integrate(f,0,1);
		WriteLine($"Integral 4 --- Log(x)/Sqrt(x) from 0 to 1 --- -4 = {I4res} is {integrator.compare(I4res, -4.0)}");
		WriteLine($"The error is --- {I4err}");
		WriteLine("");
		WriteLine("");


		//Test of error function and related plot

		/*Tabulated values*/
		double[] xtab = {0,0.02,0.04,0.06,0.08,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,2.0}; //Taken from wikipedia
		double[] erftab = {0,0.022564575,0.045111106,0.067621594,0.090078126,0.112462916,0.222702589,0.328626759,0.428392355,0.520499878,0.603856091,0.677801194,0.742100965,0.796908212,0.842700793,0.880205070,0.910313978,0.934007945,0.952285120,0.966105146,0.976348383,0.983790459,0.989090502,0.992790429,0.995322265};
		var tabulated_erf = new StreamWriter("tabulated_erf.txt");

		for(int i=0; i < xtab.Length; i++){
			tabulated_erf.WriteLine($"{xtab[i].ToString(CultureInfo.InvariantCulture)} {erftab[i].ToString(CultureInfo.InvariantCulture)}");
		}
		tabulated_erf.Close();

		var calculated_erf = new StreamWriter("calculated_erf.txt");

		for(int i=0; i < xtab.Length; i++){
			calculated_erf.WriteLine($"{xtab[i].ToString(CultureInfo.InvariantCulture)} {integrator.erf(xtab[i]).ToString(CultureInfo.InvariantCulture)}");
		}
		calculated_erf.Close();

		WriteLine("The comparison between tabulated (circles) and the calculated erf(x) can be found in erf_comparison.png");
		WriteLine("");

		/*Now doing the accuracy test of erf(x)*/
		double target = 0.84270079294971486934;
		double[] accs = {0.1,0.01,0.001,0.0001,0.00001,0.000001,0.0000001};

		var accuracy_erf = new StreamWriter("accuracy_erf.txt");

		for(int i=0; i < accs.Length; i++){
			accuracy_erf.WriteLine($"{Log(accs[i]).ToString(CultureInfo.InvariantCulture)} {Log(Abs(integrator.erf(1,accs[i],0)-target)).ToString(CultureInfo.InvariantCulture)}");
		}
		accuracy_erf.Close();

		WriteLine("The accuracy plot of erf(1) can be found  in accuracy_erf.png");
		WriteLine("");

		/*----------------------------PART B----------------------------*/
		WriteLine("--------------------------------------------------------");
		WriteLine("Checking some integrals using the Clenshaw-Curtis variable transformation");
		WriteLine("");

		//Integral of 1/sqrt(x)
		int count = 0;
		f = delegate(double x) {count ++; return 1.0/Sqrt(x);};
		(double Icc1, double Icc1err) = integrator.Clenshaw_Curtis(f,0,1);
		WriteLine($"Integral of  1/√(x) from 0 to 1 yields {Icc1} with an error of {Icc1err}");
		WriteLine($"This integral was evaluated in {count} iterations, it took python 231");
		WriteLine($"");

		//Integral of ln(x)/√(x)
		count = 0;
		f = delegate(double x) {count ++; return Log(x)/Sqrt(x);};
		(double Icc2, double Icc2err) = integrator.Clenshaw_Curtis(f,0,1);
		WriteLine($"Integral of ln(x)/√(x) from 0 to 1 yields {Icc2} with an error of {Icc2err}");
		WriteLine($"This integral was evalued in {count} iterations, it took python 315");
		WriteLine($"");

		//Integral of e^(-x^2)
		count = 0;
		f = delegate(double x) {count ++; return Exp(-x*x);};
		(double Icc3, double Icc3err) = integrator.integrate(f,double.NegativeInfinity, double.PositiveInfinity, 0.001, 0.001, true);
		WriteLine($"Integral of e^(-x^2) from -infty to +infty yields {Icc3} with an error of {Icc3err}");
		WriteLine($"This integral was evaluated  in {count} iterations, it took python 270");
		WriteLine($"");

	} //End of Main()




} //End of main class

