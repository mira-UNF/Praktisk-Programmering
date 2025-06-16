using System;
using static System.Console;
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




	public static (vector, int) newton(Func<vector,double> f, vector start, double acc = 0.0001, int max_iteration = 1000) //returns minimum, total iterations
	{

		vector x = start.copy();
		int iteration = 0;
		do{
			iteration++; //for counting how many iterations this takes

			if(iteration > max_iteration-1){
				Error.WriteLine("Timeout - maximum iterations reached without proper convergence :(");
				break;
			}

			//Console.Write($"\rIterations -  {iteration}");
			//if(iteration % 10 == 0) {Console.Write($"\rIterations -  {iteration}");}

			/*Finding the Newton step size*/
			vector grad_f = gradient(f,x);

			//if(grad_f.norm() < acc) break; //jobs done

			matrix H = hessian(f,x);

			(matrix Q, matrix R) = QR_GS.decomp(H);
			vector dx = QR_GS.solve(Q,R,-grad_f);

			//Console.WriteLine($"Iteration {iteration}: grad norm = {grad_f.norm()}, dx norm = {dx.norm()}, acc = {acc}");

			if (grad_f.norm() < acc || dx.norm() < acc){
			Console.WriteLine("Converged: small gradient or step");
			break;
			}


			/*Backtracking linesearch*/
			double lam = 1.0;
			do{

				//Console.WriteLine($"  lam = {lam:E}, f(x+lam*dx) = {f(x + lam*dx)}, f(x) = {f(x)}");

				if(lam < 1.0/1024.0) break; //very small step, just accept it

				if(f(x+lam*dx) < f(x)) break; //good step

				lam /= 2;

			}while(true);

			//double fx = f(x);
			//Console.WriteLine($"#{iteration}: f = {fx:F4}, ||grad|| = {grad_f.norm():F4}, step = {lam*dx.norm():E}");

			x = x+lam*dx;

		}while(true);

		return (x,iteration);

	} //End of Newton minimizer method


/* Quasi-Newton method with numerical gradient  */
	public static (vector, int) qnewton(
						Func<vector, double> f, 
						vector start,
						double acc, int max_iteration	
						)
	{
	int counter = 0;
	int second_count = 0;
	int dim = start.size;
	vector x = start.copy();
	matrix B = new matrix(dim, dim);
	B.setid();
	
	//Error.WriteLine("Quasi-Newton routine started..... ");
	while(gradient(f, x).norm() > acc) {
		counter ++;

		if(counter > max_iteration-1){
                                 Error.WriteLine("Timeout - maximum iterations reached without proper convergence :(");
                                 break;
                         }


		vector grad = gradient(f, x);
		vector dx = -B*grad;
		double lambda = 1.0;
		while(true) {
			if(f(x+lambda*dx) < f(x)) {
				/* Accept step and update B */
				x += lambda*dx;
				vector graddx = gradient(f, x);
				vector y = graddx-grad;
				vector u = dx-B*graddx;
				double uy = u%y;
				if(Abs(uy) > Pow(2, -26)) {
					matrix dB = matrix.outer(u, u)/uy;
					B += dB;
				}
				break;	
			}
			lambda /= 2.0;
			double lim = 1.0/1024.0;
			if(lambda < lim) {
				x += lambda*dx;
				B.setid();
				break;
			}
			second_count ++;


		}

	}
	//Error.WriteLine("Quasi-Newton done.");
	return (x, counter);	

	}



	public static (vector, int) analytic_newton(Func<vector,double> f, Func<vector,vector> grad,vector start, double acc = 0.0001,int max_iteration = 1000) //returns minimum, total iterations
	{

		vector x = start.copy();
		int iteration = 0;
		do{
			iteration++; //for counting how many iterations this takes

			if(iteration > max_iteration){
				Error.WriteLine("Timeout - maximum iterations reached without proper convergence :(");
				break;
			}

			//Console.Write($"\rIterations -  {iteration}");
			//if(iteration % 10 == 0) {Console.Write($"\rIterations -  {iteration}");}

			/*Finding the Newton step size*/
			vector grad_f = grad(x);

			//Error.WriteLine($"{grad_f[0]}");

			//if(grad_f.norm() < acc) break; //jobs done

			matrix H = hessian(f,x);

			(matrix Q, matrix R) = QR_GS.decomp(H);
			vector dx = QR_GS.solve(Q,R,-grad_f);

			//Console.WriteLine($"Iteration {iteration}: grad norm = {grad_f.norm()}, dx norm = {dx.norm()}, acc = {acc}");

			if (grad_f.norm() < acc || dx.norm() < acc){
			Console.WriteLine("Converged: small gradient or step");
			break;
			}


			/*Backtracking linesearch*/
			double lam = 1.0;
			do{

				//Console.WriteLine($"  lam = {lam:E}, f(x+lam*dx) = {f(x + lam*dx)}, f(x) = {f(x)}");

				if(lam < 1.0/1024.0) break; //very small step, just accept it

				if(f(x+lam*dx) < f(x)) break; //good step

				lam /= 2;

			}while(true);

			//double fx = f(x);
			//Console.WriteLine($"#{iteration}: f = {fx:F4}, ||grad|| = {grad_f.norm():F4}, step = {lam*dx.norm():E}");

			x = x+lam*dx;

		}while(true);

		return (x,iteration);

	} //End of analytical gradient Newton minimizer method


} //End of Class
