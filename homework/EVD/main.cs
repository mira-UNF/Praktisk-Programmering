using static System.Console;
using System.IO;

public class Program{

	public static void Main(string[] args){

	// Default values
        double dr = 0.3;
        double rmax = 5.0;


	/* --------------- PART A -----------------*/

		//Generating a square

		var rnd = new System.Random(7);
		int size = 5;
		matrix A = new matrix(size,size);

		//generating a random matrix
		for(int row = 0; row < size; row++){
			for (int col = row; col < size; col++){
				A[row,col] = rnd.NextDouble();
				A[col,row] = A[row,col];
			}
		}

		//See if A is symmetric
		WriteLine("------We have the symmetric matrix A------");
		A.print();

		//Run jacobi eigval stuff

		(vector w, matrix V, matrix A_after) = Jacobi.cyclic(A);

		//TEST D = V_T A V
		WriteLine("-------- PART A - TESTING JACOBI ALGORITHM ---------");
		matrix D = V.transpose()*A*V;
		WriteLine($"Is V^T A V = D? {A_after.approx(D)}");
		WriteLine($"Is V D V^T = A? {A.approx(V*D*V.transpose())}");
		WriteLine($"Is V^T V = 1? {matrix.id(V.size1).approx(V.transpose()*V)}");
		WriteLine($"Is V V^T = 1? {matrix.id(V.size1).approx(V*V.transpose())}");


	/* ----------------- PART B --------------------*/

		//Making the Hamiltonian and solving the system

		matrix H = Hamiltonian(dr, rmax);
		H.print();




	}


	public static matrix Hamiltonian(double dr, double rmax){
		int npoints = (int)(rmax/dr)-1;
		vector r = new vector(npoints);
		for(int i=0;i<npoints;i++)r[i]=dr*(i+1);
			matrix H = new matrix(npoints,npoints);
			for(int i=0;i<npoints-1;i++){
   				H[i,i]  =-2*(-0.5/dr/dr);
   				H[i,i+1]= 1*(-0.5/dr/dr);
   				H[i+1,i]= 1*(-0.5/dr/dr);
  			}
			H[npoints-1,npoints-1]=-2*(-0.5/dr/dr);
			for(int i=0;i<npoints;i++)H[i,i]+=-1/r[i];

	return H;
	}



}
