using System;
using static System.Math;

public static class integrator {

/*Private adaptive quadrature method*/
	private static (double, double) adaptive_quadrature( Func<double, double> f,double a, double b, double del = 0.001,
	double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN )
	{
		double h = b-a;

		/*Check if it is the first call*/
		if(double.IsNaN(f2)){ f2 = f( a + 2 * h/6); f3 = a + 4 * h/6; }

		double f1 = f(a + h/6);
		double f4 = f(a + 5 * h/6);

		/*Higher order rule*/
		double Q = (2 * f1 + f2 + f3 + 2 * f4)/6 * (b - a);

		/*Lower order rule*/
		double q = ( f1 + f2 + f3 + f4)/4 * (b - a);

		/*Estimating the error*/
		double err = Abs(Q - q);

		/*Checking in the error is within the accepted accuracy, otherwise we split into subintervals*/
		if (err <= del+eps*Abs(Q)) return (Q,err);
		else {
			var (y1,e1) = adaptive_quadrature(f,a,(a+b)/2,del/Sqrt(2),eps,f1,f2);
			var (y2,e2) = adaptive_quadrature(f,(a+b)/2,b,del/Sqrt(2),eps,f3,f4);
			return (y1+y2,(e1+e2)/2.0);
		}

	}

/*Public Clenshaw-Curtis integrator*/
/*Implementing this as eq. 58 in the integration notes*/
	public static (double, double) Clenshaw_Curtis( Func<double, double> f, double a, double b, double del = 0.001,
	double eps = 0.001 )
	{
		/*Doing the variable transformation over an arbitrary integration interval*/
		Func<double, double> fcc = delegate(double theta) {
		return f((a+b)/2+(b-a)/2*Cos(theta))*Sin(theta)*(b-a)/2; //Formula taken from the exercise description
		};

		/*Performing the adaptive quadrature integration with the now transformed function*/
		return adaptive_quadrature(fcc, 0, PI, del, eps);

	}


/*Public integrate method that is the primary call in main.cs*/
	public static (double, double) integrate( Func<double, double> f, double a, double b, double del = 0.001,
	double eps = 0.001, bool CC = false)
	{
		/*Implementing handling of integration over infinite intervals via the formulas from the notes*/

		if(double.IsNegativeInfinity(a) && double.IsPositiveInfinity(b)){
			Func<double, double> f_new = delegate(double x) {
			return f(x/(1-x*x))*(1+x*x)/Pow(1-x*x,2);
			};

			if(CC){ return Clenshaw_Curtis(f_new, -1, 1, del, eps); }

			else {return adaptive_quadrature(f_new, -1, 1, del, eps); }
		}


		if(!double.IsNegativeInfinity(a) && double.IsPositiveInfinity(b)){
			Func<double, double> f_new = delegate(double x){
			return f(a+x/(1-x))/Pow(1-x,2);
			};

			if(CC) { return Clenshaw_Curtis(f_new, 0,1,del,eps); }

			else { return adaptive_quadrature(f_new, 0, 1, del, eps); }
		}


		if(double.IsNegativeInfinity(a) && !double.IsPositiveInfinity(b)){
			Func<double, double> f_new = delegate(double x){
			return f(b+x/(1+x))/Pow(1+x,2);
			};

			if(CC) { return Clenshaw_Curtis(f_new, -1,0,del,eps); }

			else { return adaptive_quadrature(f_new, -1, 0, del, eps); }
		}


		else{
			if(CC)  { return Clenshaw_Curtis(f, a, b, del, eps);  }

			else { return adaptive_quadrature(f, a, b, del, eps); }
		}
	}


/*Public result comparison method*/
	public static bool compare(double goal, double result, double del = 0.001, double eps = 0.001)
	{
		/*Checking absolute accuracy*/
		if(Abs(goal-result) < del){
			return true;
		}

		/*Checking relative accuracy*/
		if(Abs(goal-result)/Max(Abs(goal), Abs(result)) < eps ){
			return true;
		}

		/*If the previous fails it returns false*/
		else{
			return false;
		}
	}


/*Public error function*/
	public static double erf(double z, double del = 0.001, double eps = 0.001)
	{
		if(z<0.0){
			return -erf(-z);
		}

		if(z>=0.0 && z<= 1.0){
			Func<double, double> f = delegate(double x) {return Exp(-x*x);  };
			return 2.0/Sqrt(PI)*integrate(f,0,z, del, eps).Item1; //This just takes only the integral value of the touple
		}

		else{
			Func<double, double> f = delegate(double x) { return Exp(-Pow(z+(1-x)/x, 2))/(x*x); };
			return 1.0-2.0/Sqrt(PI)*integrate(f, 0, 1, del, eps).Item1;
		}
	}




}//End of class

