using System;

public class linearinterpol {

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

	public static double linterp(double[] x, double[] y, double z){
        int i=binsearch(x,z);
        double dx=x[i+1]-x[i]; if(!(dx>0)) throw new Exception("uups...");
        double dy=y[i+1]-y[i];
        return y[i]+dy/dx*(z-x[i]);
        }

	public static  double lininterpInteg(double[] x, double[] y, double z){
	int i_cutoff = binsearch(x,z);
	double sum = 0, dx, dy, pi;

	/*Numerical Integration of the interpolation*/
	for(int i = 0; i <= i_cutoff; i++){
	dx = x[i+1]-x[i];
	dy = y[i+1]-y[i];
	pi = dy/dx;
	if(i == i_cutoff) { dx = z - x[i_cutoff]; }
	sum += y[i]*dx+pi*(0.5*dx*dx);
	}
	return sum;
	}

}
