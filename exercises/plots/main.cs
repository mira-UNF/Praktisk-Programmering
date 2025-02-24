using System;
using static System.Math;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization; // Import for invariant culture
using System.Diagnostics;


class Program
{
    public static double erf(double x)
    {
        /// single precision error function (Abramowitz and Stegun, from Wikipedia)
        if (x < 0) return -erf(-x);
        double[] a = { 0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429 };
        double t = 1 / (1 + 0.3275911 * x);
        double sum = t * (a[0] + t * (a[1] + t * (a[2] + t * (a[3] + t * a[4])))); /* the right thing */
        return 1 - sum * Exp(-x * x);
    }

    public static double sgamma(double x){
	if(x<0)return PI/Sin(PI*x)/sgamma(1-x);
	if(x<9)return sgamma(x+1)/x;
	double lnsgamma=Log(2*PI)/2+(x-0.5)*Log(x)-x
    	+(1.0/12)/x-(1.0/360)/(x*x*x)+(1.0/1260)/(x*x*x*x*x);
	return Exp(lnsgamma);
    }

   static double lngamma(double x){
	if(x<=0) throw new ArgumentException("lngamma: x<=0");
	if(x<9) return lngamma(x+1)-Log(x);
	return x*Log(x+1/(12*x-1/x/10))-x+Log(2*PI/x)/2;
   }


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

    public static int Main()
    {
        // Creating an array (fixed size) of inputs from the Wikipedia tabulated values
        double[] erf_x_tab = { 0.00, 0.02, 0.04, 0.06, 0.08, 0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.8, 0.9, 1.00 };
        double[] erf_tab = { 0.00, 0.022, 0.045, 0.068, 0.090, 0.112, 0.223, 0.329, 0.428, 0.520, 0.604, 0.678, 0.742, 0.797, 0.843 };
	double[] gamma_x_tab = { 0.01, 0.02, 0.04, 0.06, 0.08, 0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.8, 0.9, 1.00 };
	double[] gamma_tab = { 99.43, 49.44, 24.46, 16.15, 12.00, 9.51, 4.59, 2.99, 2.22, 1.49, 1.30, 1.16, 1.07, 1.00 };
	double[] lngamma_tab = { 4.60, 3.90, 3.20, 2.78, 2.48, 2.25, 1.52, 1.10, 0.80, 0.40, 0.26, 0.15, 0.07, 0.00 };
        // Creating a linspace of values
        double[] x_lin = Linspace(0.1, 1.0, 100);

        // Creating the strings to be written to txt later on
	//First for the error function
        StringBuilder erf_data_tab = new StringBuilder();
        for (int i = 0; i < erf_x_tab.Length; i++)
        {
            erf_data_tab.AppendLine($"{erf_x_tab[i].ToString(CultureInfo.InvariantCulture)} {erf_tab[i].ToString(CultureInfo.InvariantCulture)}");
        }

        StringBuilder erf_data_lin = new StringBuilder();
        for (int i = 0; i < x_lin.Length; i++)
        {
            erf_data_lin.AppendLine($"{x_lin[i].ToString(CultureInfo.InvariantCulture)} {erf(x_lin[i]).ToString(CultureInfo.InvariantCulture)}");
        }

	//Now for the gamma function
	StringBuilder gamma_data_tab = new StringBuilder();
	for (int i = 0; i < gamma_x_tab.Length; i++)
	{
		gamma_data_tab.AppendLine($"{gamma_x_tab[i].ToString(CultureInfo.InvariantCulture)} {gamma_tab[i].ToString(CultureInfo.InvariantCulture)}");
	}

	StringBuilder gamma_data_lin = new StringBuilder();
	for (int i = 0; i < x_lin.Length; i++)
	{
		gamma_data_lin.AppendLine($"{x_lin[i].ToString(CultureInfo.InvariantCulture)} {sgamma(x_lin[i]).ToString(CultureInfo.InvariantCulture)}");
	}

	//Now for the lngamma function
	StringBuilder lngamma_data_tab = new StringBuilder();
	for (int i = 0; i < gamma_x_tab.Length; i++)
	{
		lngamma_data_tab.AppendLine($"{gamma_x_tab[i].ToString(CultureInfo.InvariantCulture)} {lngamma_tab[i].ToString(CultureInfo.InvariantCulture)}");
	}

	StringBuilder lngamma_data_lin = new StringBuilder();
	for (int i = 0; i < x_lin.Length; i++)
	{
		lngamma_data_lin.AppendLine($"{x_lin[i].ToString(CultureInfo.InvariantCulture)} {lngamma(x_lin[i]).ToString(CultureInfo.InvariantCulture)}");
	}


        // Writing data to a file
        File.WriteAllText("erf_tabulated_points.txt", erf_data_tab.ToString());
	File.WriteAllText("erf_smooth_curve.txt", erf_data_lin.ToString());

	File.WriteAllText("gamma_tabulated_points.txt", gamma_data_tab.ToString());
	File.WriteAllText("gamma_smooth_curve.txt", gamma_data_lin.ToString());

	File.WriteAllText("lngamma_tabulated_points.txt", lngamma_data_tab.ToString());
	File.WriteAllText("lngamma_smooth_curve.txt", lngamma_data_lin.ToString());

        return 0; // Indicate successful execution
    }
}
