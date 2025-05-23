using System;
using static System.Math;


public class qspline{
	//Setting up arrays for calculating b and c
	public double[] x, y, p, b, c, c_up, c_down;
	public int length;


	public qspline(double[] xs, double[] ys){
		/*Initializing my arrays in order to calculate b and c for the polynomial*/
		length = xs.Length;

		//Copying the x and y arrays
		x = new double[length];
		y = new double[length];

		for(int i = 0; i < length; i++){
			x[i] = xs[i];
			y[i] = ys[i];
		}

		//We have n-1 unknown coefficients
		b = new double[length-1];
		p = new double[length-1];
		c = new double[length-1];
		c_up =  new double[length-1];
		c_down = new double[length-1];

		//Choosing the one arbitrarily chosen element
		c_up[0] = 0;
		c_down[length-2] = 0;
		b[0] = (y[1]-y[0])/(x[1]-x[0]);
		p[0] = b[0];

		//Forward recursion
		for(int i=1; i<length-1; i++){

			double dx = x[i+1]-x[i];
			double dy = y[i+1]-y[i];

			p[i] = dy/dx;

			c_up[i] = 1/dx*(p[i]-p[i-1]-c_up[i-1]*(x[i]-x[i-1]));
		}

		//Backward recursion
		for(int i=length-3; i>0; i--){
			double dx = x[i+1]-x[i];

			c_down[i] = 1/dx*(p[i+1]-p[i]-c_down[i+1]*(x[i+2]-x[i+1]));
		}

		//Averaging the two recursions
		for(int i = 0; i<length-1; i++){
			c[i] = (c_up[i]+c_down[i])/2;
		}


		//Calculating b-array
		for(int i = 0; i<length-1; i++){
			b[i] = p[i]-c[i]*(x[i+1]-x[i]);
		 }
	}

	public static int binsearch(double[] x, double z)
	{/* locates the interval for z by bisection */
	if( z<x[0] || z>x[x.Length-1] ) throw new Exception("binsearch: bad z");
	int i=0, j=x.Length-1;
	while(j-i>1){
		int mid=(i+j)/2;
		if(z>x[mid]) i=mid; else j=mid;
		}
	return i;
	}

	public double evaluate(double z){

		int i = binsearch(x,z);

	return y[i]+b[i]*(z-x[i])+c[i]*Pow(z-x[i],2);
	}

	public double derivative(double z){
		int i = binsearch(x,z);
		double dx = z - x[i];
	return b[i] + 2*c[i]*dx;
	}


	public double integral(double z){
		int i_cutoff = binsearch(x, z);
		double sum = 0, dx;

		for (int i = 0; i <= i_cutoff; i++) {
    			dx = x[i + 1] - x[i];
    			if (i == i_cutoff) dx = z - x[i_cutoff];
    			sum += y[i]*dx+0.5*b[i]*Pow(dx,2)+1.0/3.0*c[i]*Pow(dx,3);
		}

	return sum;
	}


}
