using System;
using static System.Math;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization; // Import for invariant culture
using System.Diagnostics;
using static System.Console;

class Program{

	// Creating a linspace method
    static double[] Linspace(double start, double end, int num)
    {
        if (num < 2)
            throw new ArgumentException("num must be at least 2 to create a valid range.");

        double step = (end - start) / (num - 1);
        return Enumerable.Range(0, num)
                         .Select(i => start + i * step)
                         .ToArray();
    }

	static void Main(){
	/*Writing the datafile in a plotutils friendly format*/

		double[] t = {1, 2, 3, 4, 6, 9, 10, 13, 15};
            	double[] y = {117, 100, 88, 72, 53, 29.5, 25.2, 15.2, 11.1};
            	double[] dy = {6, 5, 4, 4, 4, 3, 3, 2, 2};


		string dataFile = "data.txt";
        	using (StreamWriter writer = new StreamWriter(dataFile))
        	{
            		// Write data file in (x, y, error) format
            		for (int i = 0; i < t.Length; i++)
          		{
                		writer.WriteLine($"{t[i].ToString(CultureInfo.InvariantCulture)} {y[i].ToString(CultureInfo.InvariantCulture)} {dy[i].ToString(CultureInfo.InvariantCulture)}");
            		}
        	}

		/*Defining the log y and error on log y for the linear fit*/
		vector t_vec = new vector(t.Length);
		vector y_log = new vector(t.Length);
		vector dy_log = new vector(t.Length);

		for(int i=0;i<y_log.size;i++){
			t_vec[i] = t[i];
			y_log[i] = Log(y[i]);
			dy_log[i] = dy[i]/y[i];
		}

		/*Defining the basic functions that wee use to fit*/
		var fs = new Func<double, double>[] {z => 1.0, z => z};

		/*Making the linear fit via the least_squares class*/
		(vector c, matrix cov) = least_squares.lsfit(fs,t_vec,y_log,dy_log);
		//c.print();
		//cov.print();

		double[] t_lin = Linspace(t_vec[0], t_vec[t_vec.size - 1], 100);

		/*Calculating the best fit*/
		vector y_fit = new vector(t_lin.Length);
		vector y_fit_plus = new vector(t_lin.Length);
		vector y_fit_min = new vector(t_lin.Length);

		for(int i=0; i<y_fit.size;i++){
			y_fit[i] = Exp(c[0])*Exp(c[1]*t_lin[i]);

			/*Making the fits of  the plus minus uncertainties*/
			y_fit_plus[i] = Exp(c[0]+Sqrt(cov[0][0]))*Exp((c[1]+Sqrt(cov[1][1]))*t_lin[i]);
			y_fit_min[i] = Exp(c[0]-Sqrt(cov[0][0]))*Exp((c[1]-Sqrt(cov[1][1]))*t_lin[i]);

		}


		using (StreamWriter writer = new StreamWriter("best_fit.txt")){
			for(int i=0; i<t_lin.Length; i++){
				writer.WriteLine($"{t_lin[i].ToString(CultureInfo.InvariantCulture)} {y_fit[i].ToString(CultureInfo.InvariantCulture)}");
			}
		}

		using (StreamWriter writer = new StreamWriter("best_fit_plus.txt")){
    			for (int i = 0; i < t_lin.Length; i++){
        			writer.WriteLine($"{t_lin[i].ToString(CultureInfo.InvariantCulture)} {y_fit_plus[i].ToString(CultureInfo.InvariantCulture)}");
    			}
		}

		using (StreamWriter writer = new StreamWriter("best_fit_min.txt")){
    			for (int i = 0; i < t_lin.Length; i++){
        			writer.WriteLine($"{t_lin[i].ToString(CultureInfo.InvariantCulture)} {y_fit_min[i].ToString(CultureInfo.InvariantCulture)}");
    			}
		}



		double T_half = Round(-Log(2)/c[1],5);  //half life of the radioactive source
		double dT_half = Round(Log(2)*Sqrt(Pow(cov[1][1]/c[1],2)),5); //Error on the half life

		WriteLine("-----------------PART A,B and C---------------------");
		WriteLine($"The half-life estimated from the fit is: {T_half}±{dT_half} days");
		WriteLine("This value is now known to be: 3.66 days (http://nucleardata.nuclear.lu.se/toi/nuclide.asp?iZA=880224), so it matches decently well.");
		WriteLine("The fit can be found in fit_and_data.png where it is plotted as the solid line, the dotted lines are the +-uncertainty on the fit coefficients.");

	}
}
