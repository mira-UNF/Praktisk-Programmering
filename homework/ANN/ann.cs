using System;
using static System.Console;
using static System.Math;

public class ann {

	/*Network parameters*/
	public readonly int n; //number of neurons

	public int counts; //count of number of operations


	/*Activation function and its derivatives/antiderivatives*/
	public Func<double, double> f = x => x*Exp(-Pow(x,2)); //gaussian wavelet
	public Func<double, double> f2 = x => x*x;
	public Func<double, double> deriv = x => Exp(-Pow(x,2))-2*Exp(-Pow(x,2))*Pow(x,2);
	public Func<double, double> dderiv = x => 4*Exp(-Pow(x,2))*Pow(x,3)-6*Exp(-Pow(x,2))*x;
	public Func<double, double> antideriv = x => -0.5*Exp(-Pow(x,2));

	/*Parameters to be minimized*/
	public vector p;
	//vector of parameters ai, bi and wi as from "yi=f((x-ai)/bi)*wi" stacked such that (a1,a2,....,b1,b2,...,w1,w2,....)^T
	//So for n neurons p will be 3*n long

	/*Setter methods*/
	public void set_a(int i, double z) {this.p[i]=z;}
	public void set_b(int i, double z) {this.p[i+n] = z;}
	public void set_w(int i, double z) {this.p[i+2*n] = z;}

	/*Getter methods*/
	public double a(int i) {return this.p[i];}
	public double b(int i) {return this.p[i+n];}
	public double w(int i) {return this.p[i+2*n];}

	/*Constructor*/
	public ann(int n){
		this.n = n;
		p = new vector(3*n);
	}

	public ann(vector ps){
		this.n = ps.size/3;
		this.p = ps;
	}

	/*Network responses*/

	/*Normal response*/
	public double response(double x){
		double sum = 0;
		/*Summing over all neurons for network respons*/
		for(int i = 0; i < n; i++){
			sum += w(i)*f((x-a(i))/b(i));
		}
		return sum;
	}

	/*Derivative response*/
	public double derivativeResponse(double x) {
		double sum = 0;
		for(int i = 0; i < n; i++) {
			/* x' -> x-a/b gives factor 1/b*/
			sum += w(i)/b(i)*deriv((x-a(i))/b(i));
		}
		return sum;

	}

	/*Double derivative response*/
	public double dderivativeResponse(double x) {
		double sum = 0;
		for(int i = 0; i < n; i++) {
			/* x' -> x-a/b gives factor 1/b²*/
			sum += w(i)/Pow(b(i),2)*dderiv((x-a(i))/b(i));
		}
		return sum;

	}

	/*Antiderivative response*/
	public double antiderivativeResponse(double x) {
		double sum = 0;
		for(int i = 0; i < n; i++) {
			/* Substitution from x' -> x-a/b gives factor b */
			sum += w(i)*b(i)*antideriv((x-a(i))/b(i));
		}
		return sum;
	}


	/*The cost_gradient is a method that returns a function, that function takes a vector (p) as input and returns a vector (grad)*/

	public Func<vector,vector> cost_gradient(double[] x, double[] y){
		// Return a function expressing analytic gradient of the first cost function
			Func<vector, vector> grad_at_p_i =  delegate(vector p_i){
				vector grad = new vector(3*n); //initalizing my gradient

				double[] ais = new double[n]; //Initializing neuron parameters
				double[] bis = new double[n]; //here the "i" index refers to the i'th iteration in the minimzation
				double[] wis = new double[n]; //routine.


				for(int j = 0; j < n; j++){ //looping over all the neuron parameters

					ais[j] = p_i[j]; //neuron parameters
					bis[j] = p_i[j+n];
					wis[j] = p_i[j+2*n];

				}

				for(int k = 0; k < x.Length; k++){ //looping over data length to calculate the sum k=1,...k=N

					/*Computing network response at the k'th data point*/

					double fk = 0; //initializing network respons sum
            				double[] zj = new double[n];
            				double[] fj = new double[n];
            				double[] dfj = new double[n];

					for (int j = 0; j < n; j++) {
						double safe_bj = (Math.Abs(bis[j]) < 1e-10) ? 1e-10 : bis[j];

                				zj[j] = (x[k] - ais[j]) / safe_bj;
                				fj[j] = f(zj[j]);
                				dfj[j] = deriv(zj[j]);
                				fk += wis[j] * fj[j];
            				}

            				double err = fk - y[k];


					for (int j = 0; j < n; j++) {
						double safe_bj = (Math.Abs(bis[j]) < 1e-10) ? 1e-10 : bis[j];
                				grad[j] += -2 * err * wis[j] * dfj[j] / safe_bj; // dC/da_j
                				grad[j + n] += 2 * err * wis[j] * dfj[j] * (x[k] - ais[j]) / (safe_bj*safe_bj); // dC/db_j
                				grad[j + 2*n] +=  2 * err * fj[j]; // dC/dw_j
            				}

				}

				return grad;
			}; //end of grad_at_p_i

			return grad_at_p_i;
		} //end of cost_gradient method


	/*Network training*/
	public void train(double[] x, double[] y, double precision = 1e-3, int max_iterations = 1000){
		Error.WriteLine("Training in progress.....");

		Random rng = new Random();
		/*Setting initial parameters*/
		for(int i = 0; i < n; i++) {
			set_w(i,rng.NextDouble() * 2 - 1); //sets w_i = 1
			set_b(i,0.5 + rng.NextDouble()); //sets b_i = 1
			set_a(i,x[0]+(x[x.Length-1]-x[0])*i/(n-1)); //spreads a_i over the range of x
		}

		/*Defining cost function*/
		Func<vector,double> cost = (u) => {
		ann annu = new ann(u);
		double sum=0;
		for(int k=0;k<x.Length;k++)
			sum+=Pow(annu.response(x[k])-y[k],2);
		return sum/x.Length;
		};

		/*Using minimisation method*/
		Func<vector,vector> cost_grad = cost_gradient(x,y);

		/*I could not for the life of me get the analytical gradient to work, but a quasi-newton approach works*/

		//(vector p_trained, int iterations) = minimizer.analytic_newton(cost, cost_grad, this.p, precision, max_iterations);
		//(vector p_trained, int iterations) = minimizer.newton(cost, this.p, precision, max_iterations);
		(vector p_trained, int iterations) = minimizer.qnewton(cost, this.p, precision, max_iterations);
		this.p = p_trained;
		this.counts = iterations;
		WriteLine($"");

		Error.WriteLine($"Training completed with {this.counts} iterations");

	}

} //End of ann class

