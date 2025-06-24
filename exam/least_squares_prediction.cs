using System;
using static System.Math;

public class ls {

	public static vector predict(vector x, int n){

		/*Setting up the matrices and vectors for the linear system*/

		//Setting up my linear system as Ac=b where c = (a1, a2,.....,an) that we want to solve for
		matrix A = new matrix(x.size-n,n);
		vector b = new vector(x.size-n);

		//Setting up the matrix A according to eq 21 in notes
		for(int i = 0; i < x.size-n; i++){
			for(int j = 0; j < n; j++){
				A[i,j] = x[i+j];
			}
		}

		//Setting up vector b accordign to eq 21 in notes
		for(int i = 0; i < b.size; i++){
			b[i] = x[i+n];
		}


		/*Solving the linear system via least squares using QR decomposition and backsubstitution*/
		//QR decomp of matrix A
		(matrix Q, matrix R) = QR_GS.decomp(A);

		//Finding the coefficients
		vector c = QR_GS.solve(Q,R,b);

	return c; //this returns the vector of coefficients that can then be used to further extrapolate like eq. 22 in the notes
	} //End of predict


	public static vector extrapolate(vector x, vector a, int M){
		int n = a.size;
		int N = x.size;

		//Vector that will contain the extrapolated signal values
		vector extrapolated = new vector(M);

		//Making a buffer vector purely for the calculation via eq. 22
		vector buffer = new vector(n);

		for(int i = 0; i < n; i++){
			buffer[i] = x[N-n+i];
		}

		//Generating the new values via extrapolation
		for(int i = 0; i < M; i++){
			double next_value = 0;
			for(int j = 0; j < n; j++){
				next_value += a[j]*buffer[j];
			}

			extrapolated[i] = next_value;

			//Shifting the buffer following eq 23, 24, ... etc.
			for(int j = 0; j < n-1; j++){
				buffer[j] = buffer[j+1];
			}
			buffer[n-1] = next_value;
		}


		return extrapolated;
	} //End of extrapolate


} //End of class ls
