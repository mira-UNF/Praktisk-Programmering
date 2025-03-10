using static System.Console;

public static class QR_GS{

	//This method takes the input of a matrix A and decomposes it into QR, where Q and R are returned as a tuple
	public static (matrix, matrix) decomp(matrix A){

		matrix Q = A.copy(); //makese a copy of the matrix A which will be orthogonalized in a minute
		matrix R = new matrix(A.size2,A.size2); //This creates the matrix R, which is a square matrix with size mxm for A being an nxm matrix


		//Orthogonalization
		for(int i=0; i<A.size2; i++){
			R[i,i] = Q[i].norm(); //Sets the i,i diagonal entry in R equal to the norm of thee i'th column in Q
			Q[i] /= R[i,i]; //normalizes the i'th column in Q (as R[i,i] is exactly the norm of that column per the above line)
			for(int j=i+1; j<A.size2; j++){
				R[i,j] = Q[i].dot(Q[j]);
				Q[j] -= Q[i]*R[i,j];
			}
		}
		return (Q, R);
	}



	//This method solves the equation QRx = b and returns the vector x - this is eq. to Rx = QTb
	public static vector solve(matrix Q, matrix R, vector b){

		vector QTb = Q.transpose()*b;

		//This does the back substitution
		for(int i=QTb.size-1; i>=0; i--){
			double sum = 0;
			for(int  k=i+1; k<QTb.size;k++) sum+=R[i,k]*QTb[k];
			QTb[i] = (QTb[i]-sum)/R[i,i];
		}

	return QTb;
	}


	//This returns the determinant of matrix A, which in this QR decomp is equal to det(R)
	public static double det(matrix R){
		double det = 1;
		for(int i=0;i<R.size1;i++){
			det *= R[i,i];
		}

	return det;
	}

	//This returns the inverse of matrix A via the decomp QR
	public static matrix inverse(matrix Q, matrix R){
		//we have to solve n linear equations of the form Ax_i = e_i where a matrix made from columns x_i then form the inverse
		//this is solved via backsubstitution

		matrix inverse =  new matrix(Q.size2,Q.size1);

		vector e_i = new vector(Q.size1);

		for(int entry  = 0; entry<Q.size1; entry++){
			e_i[entry] = 1;
			inverse[entry] = solve(Q,R,e_i);
			e_i[entry] = 0;
		}


	return inverse;
	}

}
