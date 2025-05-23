using System;
using static System.Math;
using static System.Console;
using System.IO;
using System.Globalization; // Import for invariant culture


class main{

	/*Defining Rosenbrock's valley function derivative*/
	/* Rosenbrock's valley function: f(x,y) = (1-x)²+100(y-x²)²  */
	/* Its derivative is: (dfdx, dfdy) = (-2(1-x)-400x(y-x²), 200(y-x²) ) */
	static Func<vector, vector> Rosenbrock = delegate(vector xy) { //Shape of the vector  is (x,y)
		return new vector (-2*(1-xy[0])-400*xy[0]*(xy[1]-xy[0]*xy[0]),200*(xy[1]-xy[0]*xy[0]));
	};
	/*It is expected to have global minimum at (a,a^2) which here is (1,1)*/



	/*Defining Himmelblau's function derivative*/
	/*Himmelblau's function is: f(x,y) = (x^2+y-11)^2+(x+y^2-7)^2*/
	/*Its derivative is: (dfdx, dfdy) = (4x(x^2+y-11)+2(x+y^2-7) ,2(x^2+y-11)+4y(x+y^2-7) )*/
	static Func<vector, vector> Himmelblau = delegate(vector xy) {
		return new vector  (4*xy[0]*(xy[0]*xy[0]+xy[1]-11)+2*(xy[0]+xy[1]*xy[1]-7),2*(xy[0]*xy[0]+xy[1]-11+4*xy[1]*(xy[0]+xy[1]*xy[1]-7)));
	};

	static void Main(){

		/*---------------------------PART A-----------------------------*/
		/*Finding the Rosenbrock roots*/
		vector Rosenbrock_Root = roots.newton(Rosenbrock,new vector(0.5,-0.5));

		WriteLine("----------------PART A------------------");
		WriteLine($"The Rosenbrock valley function was found to have a root at ({Rosenbrock_Root[0]},{Rosenbrock_Root[1]})");
		WriteLine($"Does this match the expected root at (1,1)? {vector.approx(Rosenbrock_Root, new vector(1.0, 1.0), 1e-2, 1e-2) ? "YES" : "NO"}");
		WriteLine("");

		/*Finding the minimas for the Himmelblau function*/
		vector[] start_vectors = {new vector(2.0,1.0), new vector(-2.1,2.9), new vector(-3.0,-2.0), new vector(3.2,-1.5) };
		int i = 0;

		while(i < start_vectors.Length){
			vector Himmelblau_Root = roots.newton(Himmelblau, start_vectors[i]);

			bool test = false;

			if (vector.approx(Himmelblau_Root, new vector(3.0, 2.0), 1e-2, 1e-2)) {test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(-2.805118, 3.131312), 1e-2, 1e-2)) {test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(-3.779310, -3.283186), 1e-2, 1e-2)) { test = true;}
			else if (vector.approx(Himmelblau_Root, new vector(3.584428, -1.848126), 1e-2, 1e-2)) {test = true;}

			WriteLine($"The Himmelblau function was found to have a root at ({Himmelblau_Root[0]},{Himmelblau_Root[1]})");
			WriteLine($"Does this match any expected root? {test ? "YES" : "NO"}");

			i++;

		}

		/*-----------------------------PART B------------------------------*/

		


	} //End of Main()


} //End of main class
