using System;
using static System.Math;

public class least_squares{

	public static (vector, matrix) lsfit(Func<double,double>[] fs, vector x, vector y, vector dy){

		//Makes the matrix A and the vector b taking the errors into account
		matrix A =  new matrix(x.size, fs.Length);
		vector b  = new vector(x.size);

		//Loops over the entries in A and b respectively and set them according to eq. 32 in the least sq notes
		for(int i=0; i<A.size1;i++){
			for(int j=0; j<A.size2;j++){
			/*Set values of A weighted by errors*/
			A[i,j] = fs[j](x[i])/dy[i];
			}
		}

		/*Set values of b weighted by errors*/
		for(int i=0; i<y.size;i++){
			b[i] = y[i]/dy[i];
		}


		/*We de QR decomp on the matrix A*/
		(matrix Q, matrix R) = QR_GS.decomp(A);

		/*We solve for the optimum coefficients*/
		vector c =  QR_GS.solve(Q,R,b);

		/*We find the errors in the coefficients via the covariance matrix*/
		matrix A_inv = QR_GS.inverse(Q,R);
		matrix cov =  A_inv*A_inv.transpose();

	return (c,cov);


	}
}
