using System;

class Program{
	static int Main(){

	//Creating a new vector
	var u = new vec(2,1,1);
	var v = new vec(1,2,1);
	//Creation test
	Console.WriteLine($"Test Creation -  vector u = ({u.x},{u.y},{u.z}) and vector v = ({v.x},{v.y},{v.z})\n");

	//Adding two vectors together
	var w = u+v;
	//Addition test
	var t = new vec(u.x+v.z,u.y+v.y,u.z+v.z);
	bool p = vec.approx(t,w);

	Console.WriteLine($"Test Addition - vector w = u+v = ({w.x},{w.y},{w.z}) - test passed? - {p}\n");

	//Inversion test
	var u_inv = -u;

	t = new vec(-u.x,-u.y,-u.z);
	p = vec.approx(t,u_inv);
	Console.WriteLine($"Test Inversion - -u = ({u_inv.x},{u_inv.y},{u_inv.z}) - test passed? - {p}\n");

	//Subtraction test
	w = u-v;

	t = new vec(u.x-v.x,u.y-v.y,u.z-v.z);
	p = vec.approx(t,w);
	Console.WriteLine($"Test Subtraction - w = u-v = ({w.x},{w.y},{w.z}) - test passed? - {p}\n");

	//Constant Multiplication
	w = 2*u;
	var w2 = u*2;

	t = new vec(2*u.x,2*u.y,2*u.z);
	p = vec.approx(t,w);
	Console.WriteLine($"Test Constant Multiplication - w = 2*u = ({w.x},{w.y},{w.z}) and w2 = u*2 = ({w2.x},{w2.y},{w2.z}) - test passed? - {p}\n");

	//Dot product
	var dot = v%u;

	var t2 = u.x*v.x+u.y*v.y+u.z*v.z;
	p = t2==dot;
	Console.WriteLine($"Test Dot Product - v dot u = {dot} - test passed? - {p}\n");

	//Testing override ToString method
	Console.WriteLine($"This should output {u.x},{u.y},{u.z} - it outputs " +  u.ToString());


	return 0;
	}


}
