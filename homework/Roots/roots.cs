using System;
using static System.Math;

public static class roots {

	public static vector newton (Func<vector, vector> f,
					vector start,
					double acc = 1e-2,
					vector dx = null)
	{
	vector x=start.copy();
	vector fx=f(x),z,fz;
	int iteration = 0;
	do{ /* Newton's iterations */
		iteration++;
		if(fx.norm() < acc) break; /* job done */
		matrix J=jacobian(f,x,fx,dx);
		(matrix Q, matrix R) = QR_GS.decomp(J);
		vector Dx = QR_GS.solve(Q,R,-fx); /* Newton's step */
		double lam=1;
		do{ /* linesearch */
			z=x+lam*Dx;
			fz=f(z);
			if( fz.norm() < (1-lam/2)*fx.norm() ) break;
			if( lam < 1.0/32 ) break;
			lam/=2;
			}while(true);
		x=z; fx=fz;

		if (iteration % 1000 == 0) {Console.Write($"\rIterations -  {iteration}");}

	}while(true);

	return x;

	} //End of newton method



	public static matrix jacobian (Func<vector,vector> f,vector x,vector fx=null,vector dx=null)
	{
	if(dx == null) dx = x.map(xi => Abs(xi)*Pow(2,-26));
	if(fx == null) fx = f(x);
	matrix J=new matrix(x.size);
	for(int j=0;j < x.size;j++){
		x[j]+=dx[j];
		vector df=f(x)-fx;
		for(int i=0;i < x.size;i++) J[i,j]=df[i]/dx[j];
		x[j]-=dx[j];
		}
	return J;
	} //End of jacobian method




} //End of Class
