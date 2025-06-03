using static System.Console;

public class Program{


	public static void Main(){

		var rnd = new System.Random(7);

		//Generating  the size of the nxm matrix A
		int n = 6;
		int m = 5;

		//Initializing my matrix A
		matrix A  = new matrix(n,m);


		//generating a random matrix

		for(int row = 0; row<n; row++){
			for (int col = 0; col < m; col++){
				A[row,col] = rnd.NextDouble();
			}
		}

		WriteLine("--------------------PART A--------------------");

		//TEST RND MATRIX
		//A.print("A");


		//TEST DECOMP
		(matrix Q, matrix R) = QR_GS.decomp(A);
		//Q.print("Q");

		//Is R upper triangular?
		WriteLine("Is R upper triangular?");
		R.print("R");

		//Is Q*R = A?
		WriteLine($"Is QR=A? {A.approx(Q*R)}");

		//Is Q orthogonal?
		matrix I = matrix.id(m);
		matrix QTQ = Q.transpose()*Q;

		//I.print("I");
		//QTQ.print("QTQ");

		WriteLine($"Is Q orthognal? {I.approx(QTQ)}");



		//TEST SOLVE
		A = new matrix(5,5);

		//generating a random matrix
		for(int row = 0; row<5; row++){
			for (int col = 0; col < 5; col++){
				A[row,col] = rnd.NextDouble();
			}
		}

		vector b = new vector(5);
		for(int i = 0; i<5; i++){
			b[i] = rnd.NextDouble();
		}

		(Q, R) = QR_GS.decomp(A);

		vector x = QR_GS.solve(Q,R,b);

		WriteLine($"Does x solve Ax=b? {b.approx(A*x)}");


		//DETERMINANT
		WriteLine($"determinant of A is {QR_GS.det(R)}");


		WriteLine("------------------PART B-------------------");

		//TEST INVERSE
		A = new matrix(5,5);

		//generating a random matrix
		for(int row = 0; row<5; row++){
			for(int col = 0; col < 5; col++){
				A[row,col] = rnd.NextDouble();
			}
		}


		(Q, R) = QR_GS.decomp(A);

		matrix B = QR_GS.inverse(Q, R);

		WriteLine($"Is the matrix B the inverse of A? {I.approx(A*B)}");


		//A*B = I
		WriteLine($"Is A*B = I? {I.approx(A*B)}");

	}




}


