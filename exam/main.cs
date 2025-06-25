using System;
using static System.Math;
using static System.Console;
using System.IO;
using static System.Random;
using System.Globalization; // Import for invariant culture
using System.Collections.Generic;

class main{

	static void Main(){
		WriteLine("--------------------------------Exercise Definition--------------------------------");
		WriteLine("I had number 28 and have thus made the exercise as follows");
		WriteLine("Introduction");
		WriteLine("Least squares method can be used to find patterns (correlations) in a signal which can then be used to extrapolate the signal (see the lecture note).");
		WriteLine("Task");
		WriteLine("Implement a function with the signature vector predict(vector x, int n)");
		WriteLine("that takes the signal vector and the number of terms as input and returns the vector of coefficients that can be used to extrapolate the signal using the linear prediction formula,");
		WriteLine("");
		WriteLine("I have interpreted the above task in an ABC-format in the following way");
		WriteLine("PART A) Implement a least squares prediction method and show that it works, fx. by remaking figure 2 of the notes");
		WriteLine("");
		WriteLine("PART B) Make an estimate of the error of the method");
		WriteLine("");
		WriteLine("PART C) Try to implement the predictor to do signal recovery");
		WriteLine("");

		WriteLine("--------------------------------PART A--------------------------------");

		/*Preparing datasets based on figure 2 in the notes*/
		int N = 100; //Must be even
		int M = N/2;

		Func<int, double> signal_maker = delegate(int k){
			return 2*Sin(0.9*(2*PI*k)/(M-1)) - Sin(2.1*(2*PI*k)/(M-1)) + 0.5*Sin(3.1*(2*PI*k)/(M-1));
		};

		vector complete_signal = new vector(N);

		for(int i = 0; i < N; i++){
			complete_signal[i] = signal_maker(i);
		}

		//Splitting the data in two parts, one for getting the coefficients and one for comparing with extrapolation
		vector fitting_signal = new vector(M);
		vector comparison_signal = new vector(M);

		for(int i = 0; i < M; i++){
			fitting_signal[i] = complete_signal[i];
			comparison_signal[i] = complete_signal[M+i];
		}

		var complete = new StreamWriter("complete_signal.txt");
		for(int i = 0; i < complete_signal.size; i++){
			complete.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)} {complete_signal[i].ToString(CultureInfo.InvariantCulture)}");
		}
		complete.Close();

		/*Using the prediction function and doing signal extrapolation*/
		int n = 6;

		//Finding the vector of coeffiicients
		vector a = ls.predict(fitting_signal, n);

		//Using coefficients to predict the next part of the signal
		vector predicted_signal = ls.extrapolate(fitting_signal,a,M);

		//Writing the extrapolated data
		var predicted = new StreamWriter("predicted_signal.txt");
		for(int i = 0; i < M; i++){
			double x_val = i+M;
			predicted.WriteLine($"{x_val.ToString(CultureInfo.InvariantCulture)} {predicted_signal[i].ToString(CultureInfo.InvariantCulture)}");
		}
		predicted.Close();

		WriteLine("The plot of the complete signal compared to the predicted signal, which is for index 50 to 100, can be");
		WriteLine("found in signal_prediction.png. The complete signal are the circles, the predicted is the solid line.");
		WriteLine("");

		WriteLine("--------------------------------PART B--------------------------------");
		//Now trying to do an error estimation and comparing errors at different n values

		var error_plot = new StreamWriter("error_vs_n.txt");
		for(int j = 1; j < 7; j++){

			//Doing the prediction
			vector a2 = ls.predict(fitting_signal,j);
			vector predicted_signal2 = ls.extrapolate(fitting_signal,a2,M);

			//Calculating the error
			double MSE = 0;

			for(int i = 0; i < predicted_signal2.size; i++){
				double error = predicted_signal2[i]-comparison_signal[i];
				MSE += error*error;
			}

			MSE /= predicted_signal2.size;

			//Writing the datafile
			error_plot.WriteLine($"{j.ToString(CultureInfo.InvariantCulture)} {MSE.ToString(CultureInfo.InvariantCulture)}");
		}
		error_plot.Close();

		WriteLine("To examine the error I have made a plot of mean squared error vs number of coefficients (n)");
		WriteLine("This plot can be found in error_vs_n.png");
		WriteLine("Now, even though it looks very nice it is important to remember that too many parameters");
		WriteLine("can cause overfitting, which worsens the fit quality.");
		WriteLine("");


		WriteLine("--------------------------------PART C--------------------------------");
		//Now I want to try and do signal recovery, so first I have to mess up my signal a bit

		vector corrupted_signal = complete_signal.copy();

		//Setting random places in my signal equal to a corrupted value
		double corrupt_number = -5;
		Random rand = new Random();
		corrupted_signal[rand.Next(0,corrupted_signal.size-1)] = corrupt_number;
		corrupted_signal[rand.Next(0,corrupted_signal.size-1)] = corrupt_number;
		corrupted_signal[rand.Next(0,corrupted_signal.size-1)] = corrupt_number;
		corrupted_signal[rand.Next(0,corrupted_signal.size-1)] = corrupt_number;

		//Writing the corrupted datafile
		var corrupted = new StreamWriter("corrupted_signal.txt");
		for(int i = 0; i < corrupted_signal.size; i++){
			corrupted.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)} {corrupted_signal[i].ToString(CultureInfo.InvariantCulture)}");
		}
		corrupted.Close();

		//Now to do the actual recovery
		int n3 = 6; //setting number of coefficients

		//Filtering the corrupted signal to remove the "bad" points and then fitting to the filtered signal
		List<double> good_values = new List<double>();

		for(int i = 0; i < corrupted_signal.size; i++){
			if(corrupted_signal[i] != corrupt_number){
				good_values.Add(corrupted_signal[i]);
			}
		}

		vector filtered_signal = new vector(good_values.ToArray());

		//Applying the prediction to the filtered signal
		vector a3 = ls.predict(filtered_signal,n3);

		//Looping over the corrupted signal and using prediction to replace any found corrupted values
		vector recovered_signal = corrupted_signal.copy();

		bool continue_recovery;

		int iteration = 0;
		int max_iteration = 10;
		do {
			continue_recovery = false; //anticipating that we don't need to do a loop after this one
			iteration++;
			//Scanning through the initially corrupted signal to see if any bad points remain
			for(int i = n3; i < recovered_signal.size; i++){ //Starting from n3 as we need that many points for prediction
				if(recovered_signal[i] == corrupt_number){
					bool can_predict = true;
					for(int j = 0; j < n3; j++){ //Now we loop through the n points before the bad one
								//to see if they are good, which is needed for prediction
						if(recovered_signal[i-n3+j] == corrupt_number){
							can_predict = false;
							break;
						}
					}

					if(can_predict){ //if we can predict, we make local prediction via eq. 22 from notes
						double prediction = 0;
						for(int j = 0; j < n3; j++){
							prediction += a3[j]*recovered_signal[i-n3+j];
						}
						recovered_signal[i] = prediction;
						continue_recovery = true;
					}
				}
			}
			if(iteration > max_iteration){
			Error.WriteLine("Timeout - Max iterations reached in signal recovery loop!");
			break;
			}
		} while(continue_recovery);

		//Writing datafile for the (hopefully) recovered data
		var recovered = new StreamWriter("recovered_signal.txt");
		for(int i = 0; i < recovered_signal.size; i++){
			recovered.WriteLine($"{i.ToString(CultureInfo.InvariantCulture)} {recovered_signal[i].ToString(CultureInfo.InvariantCulture)}");
		}
		recovered.Close();

		WriteLine("The result of exercise C can be found in recovered_vs_corrupted.png");
		WriteLine("Here, the true complete signal is shown by the circles");
		WriteLine("the corrupted signal is shown by the * connect by -.-.");
		WriteLine("and the recovered signal is the solid line.");
		WriteLine("It looks like it recovers the signal quite well :D");
		WriteLine("Now this might not be super general, as the recovery depends on knowing the corrupted value");
		WriteLine("but perhaps this comes already given from the hardware side or one can just implement a min and max");
		WriteLine("accepted value and treat everything outside of that as corrupted/wrong, I just chose to do it the other way.");

	}// End of Main()


}// End of class main
