using System;
using static System.Math;

public static class minimizer {

	public static vector gradient(Func<vector,double> f, vector x) //returns the gradient of f at x
	{

		double fx = f(x);
		vector grad_f = new vector(x.size);

		for(int i = 0; i < x.size; i++)
		{
			double dxi = (1+Abs(x[i]))*Pow(2,-26); //infinitessimal step
			x[i] += dxi;
			grad_f[i] = (f(x)-fx)/dxi; //calculates (f(x+dx)-f(x))/dx
			x[i] -= dxi;
		}

		return grad_f;

	} //End of gradient method




	public static matrix hessian(Func<vector,double> f, vector x) //returns the hessian matrix of f at x
	{

		matrix H = new matrix(x.size, x.size);
		vector grad_f = gradient(f, x);

		for(int j = 0; j < x.size; j++)
		{
			double dxj = (1+Abs(x[j]))*Pow(2,-13); //infinitessimal step
			x[j] += dxj;
			vector dgrad = gradient(f,x) - grad_f;

			for(int i = 0; i < x.size; i++)
			{
				H[i,j] = dgrad[i]/dxj;
			}
			x[j] -= dxj;

		}

		return H;

	} //End of hessian method




	public static (vector, int) newton(Func<vector,double> f, vector start, double acc = 0.0001) //returns minimum, total iterations
	{

		vector x = start.copy();
		int iteration = 0;
		do{
			iteration++; //for counting how many iterations this takes

			if(iteration > 1000){
				Console.WriteLine("Timeout error - maximum iterations reached without convergence :(");
				break;
			}

			// if(iteration % 10 == 0) {Console.Write($"\rIterations -  {iteration}");}

			/*Finding the Newton step size*/
			vector grad_f = gradient(f,x);

			if(grad_f.norm() < acc) break; //jobs done

			matrix H = hessian(f,x);

			(matrix Q, matrix R) = QR_GS.decomp(H);
			vector dx = QR_GS.solve(Q,R,-grad_f);

			/*Backtracking linesearch*/
			double lam = 1.0;
			do{

				if(lam < 1/1024) break; //very small step, just accept it

				if(f(x+lam*dx) < f(x)) break; //good step

				lam /= 2;

			}while(true);

			x = x+lam*dx;

		}while(true);

		return (x,iteration);

	} //End of Newton minimizer method




} //End of Class
