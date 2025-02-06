using System;

public class vec{
	public double x,y,z; /* the three components of a vector */

	//constructors:
	public vec(){ x=y=z=0; }
	public vec(double x,double y,double z){ this.x=x; this.y=y; this.z=z; }
	//These are responsible for initialising objects of this class, when it is called in the main program

	//operators:
	public static vec operator*(vec v, double c){return new vec(c*v.x,c*v.y,c*v.z);}
	public static vec operator*(double c, vec v){return v*c;}
	public static vec operator+(vec u, vec v){return new vec(u.x+v.x,u.y+v.y,u.z+v.z);}
	public static vec operator-(vec u){return new vec(-u.x,-u.y,-u.z);}
	public static vec operator-(vec u, vec v){return u+(-v);}
	//These redefine the common operations when applied to objects of this class, such as to be vector addition etc.

	//other vector operations
	public static double operator%(vec u, vec v){return u.x*v.x+u.y*v.y+u.z*v.z;}

	//To-string method
	public override string ToString(){ return $"{x} {y} {z}"; }

	//Comparison
	public static bool approx(double a,double b,double acc=1e-6,double eps=1e-6){
		if(Math.Abs(a-b)<acc)return true;
		if(Math.Abs(a-b)<(Math.Abs(a)+Math.Abs(b))*eps)return true;
		return false;
	}

	public bool approx(vec other){
		if(!approx(this.x,other.x))return false;
		if(!approx(this.y,other.y))return false;
		if(!approx(this.z,other.z))return false;
		return true;
	}

	public static bool approx(vec u, vec v) => u.approx(v);





}
