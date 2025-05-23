using System;
using static System.Math;

public static class MC {

	/*Plain MC integrator*/
	static public (double,double) plain_integrate(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; for(int i=0;i<dim;i++)V*=b[i]-a[i];
        double sum=0,sum2=0;
	var x=new vector(dim);
	var rnd=new Random();
        for(int i=0;i<N;i++){
                for(int k=0;k<dim;k++)x[k]=a[k]+rnd.NextDouble()*(b[k]-a[k]);
                double fx=f(x); sum+=fx; sum2+=fx*fx;
                }
	double mean = sum/N;
	double sigma = Sqrt(sum2/N-mean*mean);

	var result=(mean*V,sigma*V/Sqrt(N));
	return result;
	}

	/*Corput Sequence*/
	static public double corput(int n, int b = 2){
		double q = 0;
		double bk = 1.0/b;

		while(n > 0){
			q += (n % b) * bk;
			n /= b;
			bk /= b;
		}
		return q;
	}


	/*Halton Sequence*/
	public static int[] primes1 = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37};
	public static int[] primes2 = {41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89};

	static public void halton(int n, int dim, vector k, int[] primes, vector a, vector b){
		for(int i = 0; i < dim; i++){
			k[i] = corput(n,primes[i])*(b[i]-a[i]);
		}
	}



	/*Quasi-random MC integrator*/
	public static (double, double) quasi_integrate(Func<vector, double> f, vector a, vector b, int N){

		int dim = a.size; //finding the relevant dimension
		double  V = 1;
		for(int i = 0; i<dim; i++){V *= (b[i]-a[i]); } //calculating the integration volume

		double seq1 = 0;
		double seq2 = 0; //initializing the two sequences for error estimation

		var k = new vector(dim);

		for(int i = 0; i < N; i++){
			/*Sequence One*/
			halton(i, dim, k, primes1, a, b); //running the actual halton to generate the quasi-random number

			seq1 += f(k);

			/*Sequence Two*/
			halton(i, dim, k, primes2, a, b);

			seq2 += f(k);
		}

		double result = seq1/N*V;

		double error = Abs(seq1-seq2)/N*V;

		return (result, error);
	}

} //End of Class
