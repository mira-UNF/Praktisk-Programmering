This folder contains my exam project for the practical programming and numerical methods course. I had number 28 in the spreadsheet and have thus completed exercise number 28. The exercise description can be found below as well as my interpretation of it, this can also be found in the top of the "Out.txt" file.

To run the program simply do a "make clean && make" and it should build everything from scratch again :D

Least-Squares Signal Extrapolation
-------------------
Least-squares signal extrapolation (linear prediction)
Introduction
Least squares method can be used to find patterns (correlations) in a signal which can then be used to extrapolate the signal (see the lecture note).

Task
Implement a function with the signature
vector predict(vector x, int n)
that takes the signal vector and the number of terms as input and returns the vector of coefficients that can be used to extrapolate the signal using the "linear prediction" formula

I have interpreted the above task in an ABC-format in the following way
PART A) Implement a least squares prediction method and show that it works, fx. by remaking figure 2 of the notes

PART B) Make an estimate of the error of the method

PART C) Try to implement the predictor to do signal recovery


-------------------

Description of solution
-------------------
I have implemented a least squares extrapolation in the "least_squares_prediction.cs" file and have used it to recreate figure 2 from the notes. I have then investigated the error of the extrapolation as a function of the number of coefficients, n, and finally I have implemented it also in the context of missing data recovery. Here, I sift through the datafile and use points coming before a missing data point to extrapolate the missing point. A description of the output of the program can be found in the "Out.txt" file.

Self-evaluation
-------------------
As I have completed all A,B and C sub-exercises I would judge the project to be 10/10 points.
