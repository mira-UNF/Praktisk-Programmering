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

		/*Preparing training data*/

		/*Setting up and training neural network*/

		/*Outputting network responses*/


	} //end of Main()


} //end of class main
