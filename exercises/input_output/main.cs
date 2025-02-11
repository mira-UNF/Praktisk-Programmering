using System;
using System.IO;
using static System.Math;

class Program{
	public static int Main(string[] args){
		string infile=null,outfile=null;
		foreach(var arg in args){
			var words=arg.Split(':');
			if(words[0]=="-input")infile=words[1];
			if(words[0]=="-output")outfile=words[1];
		}
		if( infile==null || outfile==null) {
			Console.Error.WriteLine("wrong filename argument");
			return 1;
		}

		var instream =new System.IO.StreamReader(infile);
		var outstream=new System.IO.StreamWriter(outfile,append:false);
		for(string line=instream.ReadLine();line!=null;line=instream.ReadLine()){
			double x=double.Parse(line);
			outstream.WriteLine($"{x} {Sin(x)} {Cos(x)}");
        	}
		instream.Close();
		outstream.Close();

	return 0;
	}


















//static void Main(){
	//	char[] split_delimiters = {' ','\t','\n'};
	//	var split_options = StringSplitOptions.RemoveEmptyEntries;
	//	for( string line = Console.ReadLine(); line != null; line =Console.ReadLine() ){
	//		var numbers = line.Split(split_delimiters,split_options);
	//		foreach(var number in numbers){
	//			double x = double.Parse(number);
	//			Console.Error.WriteLine($"{x} {Math.Sin(x)} {Math.Cos(x)}");
        //        	}
        //	}


	//}
















	//public static void Main(string[] args){
	//foreach(var arg in args){
	//	var words = arg.Split(':');
	//	if(words[0]=="-numbers"){
	//		var numbers=words[1].Split(',');
	//		foreach(var number in numbers){
	//			double x = double.Parse(number);
	//			Console.WriteLine($"{x} {Math.Sin(x)} {Math.Cos(x)}");
	//			}
	//		}
	//	}
	//}
}
