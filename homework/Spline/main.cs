using System;
using static System.Math;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization; // Import for invariant culture
using System.Diagnostics;
using static System.Console;




class main {
	static void Main()  {

	/*Initializing my arrays*/
		int n = 10;
		double[] x = new double[n];
		double[] y = new double[n];


/*----------------------------------LINEAR  SPLINE-----------------------------------*/
	/*Creating a datafile to use for the data points we want to interpolate between*/
		string dataFile = "linterp_data.txt";
        	using (StreamWriter writer = new StreamWriter(dataFile))
        	{
            		// Write data file in (x, y, error) format
            		for (int i = 0; i < n; i++)
          		{
				x[i] = i;
				y[i] = Cos(i);
                		writer.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)} {Cos(i).ToString(CultureInfo.InvariantCulture)}");
            		}
        	}


	/*Creating a datafile for use for the interpolation and the integration*/
		var spline_stream = new StreamWriter("linterp_spline.txt");
		var integral_stream = new StreamWriter("linterp_integral.txt");

		int N = 300; //Dividing  up the space between each point for finer interpolation
		double step = (x[n-1]-x[0])/(N-1);
		//Pretty sure this doesn't matter for linear piecewise interp. but now I have it...
		//nvm it matters for the integration of course
		for (int i = 0; i < N; i++)
		{
			double z = x[0] + i*step;
			double s = linearinterpol.linterp(x,y,z);
			double integ = linearinterpol.lininterpInteg(x,y,z);

			spline_stream.WriteLine($"{z.ToString(CultureInfo.InvariantCulture)} {s.ToString(CultureInfo.InvariantCulture)}");
			integral_stream.WriteLine($"{z.ToString(CultureInfo.InvariantCulture)} {integ.ToString(CultureInfo.InvariantCulture)}");

		}

		spline_stream.Close();
		integral_stream.Close();


/*-------------------------------QUADRATIC SPLINE------------------------------*/
	/*Initializing my arrays*/
		int nq = 10;
		double[] xq = new double[nq];
		double[] yq = new double[nq];


	/*Creating datafile to do quadratic interpolation between*/
		var data_stream = new StreamWriter("qspline_data.txt");

		for (int i = 0; i < nq; i++)
		{
			xq[i] = i;
			yq[i] = Sin(i);

			data_stream.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)} {Sin(i).ToString(CultureInfo.InvariantCulture)}");

		}

		data_stream.Close();

	/*Creating the values for the quadratic interpolation, integration and derivative*/
		var qspline_stream = new StreamWriter("qspline.txt");
		var qinteg_stream = new StreamWriter("qspline_integ.txt");
		var qderiv_stream = new StreamWriter("qspline_deriv.txt");

		int Nq = 300;
		double stepq = (xq[nq-1]-xq[0])/(Nq-1);

		qspline q = new qspline(xq, yq);

		for (int i = 0; i < N; i++)
		{
			double zq = xq[0] + i*stepq;
			double sq = q.evaluate(zq);
			double integq = q.integral(zq);
			double derivq = q.derivative(zq);

			qspline_stream.WriteLine($"{zq.ToString(CultureInfo.InvariantCulture)} {sq.ToString(CultureInfo.InvariantCulture)}");
			qinteg_stream.WriteLine($"{zq.ToString(CultureInfo.InvariantCulture)} {integq.ToString(CultureInfo.InvariantCulture)}");
			qderiv_stream.WriteLine($"{zq.ToString(CultureInfo.InvariantCulture)} {derivq.ToString(CultureInfo.InvariantCulture)}");

		}

		qspline_stream.Close();
		qinteg_stream.Close();
		qderiv_stream.Close();

	}
}
