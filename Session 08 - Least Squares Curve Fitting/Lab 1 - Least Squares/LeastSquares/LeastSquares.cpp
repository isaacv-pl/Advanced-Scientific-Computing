// LeastSquares.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include "LeastSquares.h"

using namespace std;


int main()
{
	double vecX[8]{ 1, 2, 3, 4, 5, 6, 7, 8 };
	double vecY[8]{ 205, 430, 677, 945, 1233, 1542, 1872, 2224 };

	double sumX = PowerSum(vecX, 1);
	double sumX2 = PowerSum(vecX, 2);
	double sumX3 = PowerSum(vecX, 3);
	double sumX4 = PowerSum(vecX, 4);

	double sumY = PowerSum(vecY, 1);

	double sumXY = PowerSum(vecX, 1, vecY);
	double sumX2Y = PowerSum(vecX, 2, vecY);

	double coeffMatrix[3][3]{
		{ sumX4, sumX3, sumX2},
		{ sumX3, sumX2, sumX },
		{ sumX2, sumX, 8 } };

	double valueVector[3]{ sumX2Y,sumXY,sumY };

	DisplayEquations(coeffMatrix, valueVector);

	double detCoeff = Determinant(coeffMatrix);

	if (detCoeff == 0) {
		cout << "This system is linearly dependent and not uniquely solveable!" << endl;
		return -1;
	}


	double mA[3][3];
	OverlayValues(coeffMatrix, valueVector, 0, mA);
	double detA = Determinant(mA);

	double mB[3][3];
	OverlayValues(coeffMatrix, valueVector, 1, mB);
	double detB = Determinant(mB);

	double mC[3][3];
	OverlayValues(coeffMatrix, valueVector, 2, mC);
	double detC = Determinant(mC);

	//cout.imbue(std::locale(""));

	cout << "DetCoeff = " << detCoeff << endl;
	cout << endl;

	cout << "DetA = " << setw(14) << setprecision(4) << fixed << detA << endl;
	cout << "DetB = " << setw(14) << setprecision(4) << fixed << detB << endl;
	cout << "DetC = " << setw(14) << setprecision(4) << fixed << detC << endl;
	cout << endl;

	double a = detA / detCoeff;
	double b = detB / detCoeff;
	double c = detC / detCoeff;

	cout << "a = " << setw(10) << setprecision(4) << fixed << a << endl;
	cout << "b = " << setw(10) << setprecision(4) << fixed << b << endl;
	cout << "c = " << setw(10) << setprecision(4) << fixed << c << endl;

	cout << endl << "Actual vs. Estimate" << endl;
	cout << setw(6) << "X";
	cout << setw(12) << "Act";
	cout << setw(12) << "Est";
	cout << setw(10) << "% Err";
	cout << endl;

	for (int i{}; i < 8; ++i) {
		double yp = a * pow(vecX[i], 2) + b * vecX[i] + c;
		double err = abs(vecY[i] - yp) / vecY[i];
		cout << setw(6) << i;
		cout << setw(12) << vecY[i];
		cout << setw(12) << yp;
		cout << setw(10) << setprecision(4) << err * 100 << " %";
		cout << endl;
	}



#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

	return 0;
}

double Determinant(double matrix[3][3])
{
	double a = matrix[0][0];
	double b = matrix[0][1];
	double c = matrix[0][2];
	double d = matrix[1][0];
	double e = matrix[1][1];
	double f = matrix[1][2];
	double g = matrix[2][0];
	double h = matrix[2][1];
	double i = matrix[2][2];

	double det = a * (e * i - f * h)
		- (b * (d * i - f * g))
		+ c * (d * h - e * g);

	return det;
}

void OverlayValues(double coeffMatrix[3][3], double valueVector[3],
	int col, double newMatrix[3][3])
{
	// Copy existing coeffMatrix to newMatrix
	for (int i{};i < 3;++i)
		for (int j{};j < 3; ++j)
			newMatrix[i][j] = coeffMatrix[i][j];

	// Overlay the valueVector on the specified column
	for (int i{};i < 3;++i)
		newMatrix[i][col] = valueVector[i];
}

void DisplayEquations(double coeffMatrix[3][3], double valueVector[3])
{
	for (int i{}; i < 3; ++i) {
		if (coeffMatrix[i][0] != 0) {
			if (coeffMatrix[i][0] < 0)
				cout << "-";
			if (abs(coeffMatrix[i][0]) != 1)
				cout << abs(coeffMatrix[i][0]);
			cout << "x ";
		}
		if (coeffMatrix[i][1] != 0) {
			if (coeffMatrix[i][1] < 0)
				cout << "-";
			else
				cout << "+ ";
			if (abs(coeffMatrix[i][1]) != 1)
				cout << abs(coeffMatrix[i][1]);
			cout << "y ";
		}
		if (coeffMatrix[i][2] != 0) {
			if (coeffMatrix[i][2] < 0)
				cout << "-";
			else
				cout << "+ ";
			if (abs(coeffMatrix[i][2]) != 1)
				cout << abs(coeffMatrix[i][2]);
			cout << "z ";
		}
		cout << "= " << valueVector[i] << endl;
	}
	cout << endl;
}

double PowerSum(double v[8], int power)
{
	double s{};
	for (int i{};i < 8;++i)
		s += pow(v[i], power);
	return s;
}

double PowerSum(double v1[8], int power, double v2[8])
{
	double s{};
	for (int i{};i < 8;++i)
		s += (pow(v1[i], power) * v2[i]);
	return s;
}