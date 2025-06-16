using System;
using static System.Math;
using static System.Console;
using System.IO;
using System.Globalization; // Import for invariant culture

class main{
	static void Main(){

		WriteLine("---------------------------PART A-------------------------------");
		WriteLine("This tests a simple neural network based on Newton minimisation");
		WriteLine("It is implemented using a numerical gradient approach");
		WriteLine("The graph for part A can be found in plot_A.png");
		WriteLine("Here the circles are the training data and the solid line is the network response");
		WriteLine("");
		WriteLine("PS - I could not get the analytical to work, but a quasi-newton worked fine, hope it is ok");

		/*Preparing training data*/
		Func<double, double> f = z => Cos(5 * z - 1) * Exp(-z * z); //Function to approximate

		int length = 30; //number of datapoints
		double a = -1.0; //start of interval
		double b = 1.0; //end of interval

		double[] x = new double[length]; //x data
		double[] y = new double[length]; //y data

		var training_data = new StreamWriter("training_data.txt");
		for(int i = 0; i<length; i++){
			x[i] = a + (b-a)*i/(length-1); //creating even spread over interval
			y[i] = f(x[i]);
			training_data.WriteLine($"{x[i].ToString(CultureInfo.InvariantCulture)} {y[i].ToString(CultureInfo.InvariantCulture)}");
		}
		training_data.Close();

		/*Setting up and training a neural network with 3 hidden layer neurons*/
		ann ann_Newton = new ann(3); //creating the neural network

		ann_Newton.train(x,y,1e-6,5000); //training the network on the data

		/*Outputting network response*/
		var network_response = new StreamWriter("network_response.txt");
		for(int i = 0; i<length; i++){
			network_response.WriteLine($"{x[i].ToString(CultureInfo.InvariantCulture)} {ann_Newton.response(x[i]).ToString(CultureInfo.InvariantCulture)}");
		}
		network_response.Close();

		WriteLine("");
		WriteLine("---------------------------PART B-------------------------------");
		WriteLine("The neural network can also output derivative, double derivative and anti derivative responses");
		WriteLine("The different derivatives can be found in the graphs named plot_deriv.png, plot_dderiv.png etc.");
		WriteLine("The normal function can be found in plot_B.png");
		WriteLine("Again the circles are the data points/tabulated values and the solid line is the network response");

		/*Preparing training data*/
		Func<double, double> fB = z => Cos(z); //Function to approximate

		int lengthB = 30; //number of datapoints
		double aB = -PI; //start of interval
		double bB = PI; //end of interval

		double[] xB = new double[lengthB]; //x data
		double[] yB = new double[lengthB]; //y data

		/*Data to compare the different derivatives and anti derivates to*/
		Func<double, double> dfB = z => -Math.Sin(z);      // First derivative
		Func<double, double> ddfB = z => -Math.Cos(z);     // Second derivative
		Func<double, double> adfB = z => Math.Sin(z);      // Antiderivative

		var deriv_data = new StreamWriter("deriv_data.txt");
		var dderiv_data = new StreamWriter("dderiv_data.txt");
		var aderiv_data = new StreamWriter("aderiv_data.txt");
		var training_dataB = new StreamWriter("training_dataB.txt");

		for(int i = 0; i<lengthB; i++){
			xB[i] = aB + (bB-aB)*i/(lengthB-1); //creating even spread over interval
			yB[i] = fB(xB[i]);
			double yd = dfB(xB[i]);
			double ydd = ddfB(xB[i]);
			double yad = adfB(xB[i]);

			training_dataB.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {yB[i].ToString(CultureInfo.InvariantCulture)}");
			deriv_data.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {yd.ToString(CultureInfo.InvariantCulture)}");
			dderiv_data.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {ydd.ToString(CultureInfo.InvariantCulture)}");
			aderiv_data.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {yad.ToString(CultureInfo.InvariantCulture)}");


		}
		training_dataB.Close();
		deriv_data.Close();
		dderiv_data.Close();
		aderiv_data.Close();


		/*Setting up and training neural network*/
		ann ann_NewtonB = new ann(3); //creating the neural network

                ann_NewtonB.train(xB,yB,1e-6,5000); //training the network on the data


		/*Outputting network responses*/
		var network_responseB = new StreamWriter("network_responseB.txt");
		var deriv_response = new StreamWriter("deriv_response.txt");
		var dderiv_response = new StreamWriter("dderiv_response.txt");
		var aderiv_response = new StreamWriter("aderiv_response.txt");

		for(int i = 0; i<length; i++){
			network_responseB.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {ann_NewtonB.response(xB[i]).ToString(CultureInfo.InvariantCulture)}");
			deriv_response.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {ann_NewtonB.derivativeResponse(xB[i]).ToString(CultureInfo.InvariantCulture)}");
			dderiv_response.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {ann_NewtonB.dderivativeResponse(xB[i]).ToString(CultureInfo.InvariantCulture)}");
			aderiv_response.WriteLine($"{xB[i].ToString(CultureInfo.InvariantCulture)} {(ann_NewtonB.antiderivativeResponse(xB[i])-ann_NewtonB.antiderivativeResponse(0)).ToString(CultureInfo.InvariantCulture)}");

		}
		network_responseB.Close();
		deriv_response.Close();
		dderiv_response.Close();
		aderiv_response.Close();


	} //end of Main()


} //end of class main
