using System;
using static System.Math;

public static class Jacobi{


	//This multiplies with the Jacobian matrix from the right
	public static void timesJ(matrix A, int p, int q, double theta){

		double c = Cos(theta), s = Sin(theta);

		for(int i = 0; i < A.size1 ; i++){
			double aip=A[i,p],aiq=A[i,q];
			A[i,p]=c*aip-s*aiq;
			A[i,q]=s*aip+c*aiq;
		}
	}

	//This multipliese with the Jacobian matrix from the left
	public static void Jtimes(matrix A, int p, int q, double theta){

		double c=Cos(theta),s=Sin(theta);

		for(int j=0;j<A.size1;j++){
			double apj=A[p,j],aqj=A[q,j];
			A[p,j]= c*apj+s*aqj;
			A[q,j]=-s*apj+c*aqj;
		}
	}




	public static (vector, matrix, matrix) cyclic(matrix M){

		int n = M.size1;
		matrix A = M.copy();
		matrix V=matrix.id(M.size1);
		vector w=new vector(M.size1);


		bool changed;

		//We keep applying rotations until the elements we are trying to zero dont change anymore, which means we have converged.
		do{
			changed=false;
			for(int p=0;p<n-1;p++){

			for(int q=p+1;q<n;q++){

				//Getting the relevant matrix elements
				double apq=A[p,q], app=A[p,p], aqq=A[q,q];

				//Calculating the theta needed to zero theem
				double theta=0.5*Atan2(2*apq,aqq-app);
				double c=Cos(theta),s=Sin(theta);

				//Calculating the new matrix elements
				double new_app=c*c*app-2*s*c*apq+s*s*aqq;
				double new_aqq=s*s*app+2*s*c*apq+c*c*aqq;

				//If the new elements are not equal to the old ones, we do the rotation
				if(new_app!=app || new_aqq!=aqq)
				{
					changed=true;
					timesJ(A,p,q, theta); // A←A*J
					Jtimes(A,p,q,-theta); // A←JT*A
					timesJ(V,p,q, theta); // V←V*J
				}
			}
			}
		}while(changed);

		for(int i = 0; i < V.size1; i++){
			w[i] = V[i,i];
		}

		return (w, V, A);
	}
}
