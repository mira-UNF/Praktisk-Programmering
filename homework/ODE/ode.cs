using System;
using static System.Math;

public static class ode{

	public static (vector, vector) rkstep12(
	Func<double, vector, vector> f, double x, vector y, double h) /*Inputs for solver*/

	{
		/*The calculations here are formulas 32-34 in the ODE notes taken for alpha = 1/2 */
		vector k0 = f(x,y);
		vector k1 = f(x+h/2, y+k0*(h/2));
		vector y_next = y+k1*h;
		vector error = (k1-k0)*h;

	return (y_next,error);
	}


	public static vector driver(
	Func<double, vector, vector> F,
	double a, /*initial point*/
	vector ya, /*initial value*/
	double b, /*final point*/
	genlist<double> xlist = null,
	genlist<vector> ylist = null,
	bool enforce = false,
	double h = 0.01, /*step size*/
	double acc = 0.01, /* desired absolute accuracy*/
	double eps = 0.01 /*desired relative accuracy*/
	){ /*Inputs for driver*/

		/*Checks if a > b and throws an exception if it is*/
		if (a > b) throw new ArgumentException("Error - Initial point, a, larger than final point, b.");

		/*Sets up my initial conditions*/
		double x = a;
		vector y = ya.copy();

		/*Logs the solution at each step unless both lists are null*/
		/*This is for use in plotting the results of the numerical solution afterwards*/
		if (xlist != null && ylist != null){
			xlist.add(x);
			ylist.add(y);
		}


		//*While loop that runs until the if statement containing return is fulfilled *//
		//* which then breaks the loop*//
		while(true){
			if(x>=b) return y; //If x is greater than or equal to the final point we break
			if(x+h>b) h = b-x; //calculating the final step

			var (yh,err_y) = rkstep12(F,x,y,h); //propagates through RK12

			double tol = (acc+eps*yh.norm()) * Sqrt(h/(b-a));
			double err = err_y.norm();

			//Make stepsize dependent on the enforce bool


			if(err<=tol){ // accept step
				x+=h; y=yh;
				xlist.add(x);
				ylist.add(y);
			}

			if(err>0) h *= Min( Pow(tol/err,0.25)*0.95 , 2); // readjust stepsize
			else h*=2;

		}



	}

}
