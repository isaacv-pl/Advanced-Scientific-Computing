// LeastSquares.h

#pragma once

double Determinant(double matrix[3][3]);

void OverlayValues(double coeffMatrix[3][3], double valueVector[3],
	int col, double newMatrix[3][3]);

void DisplayEquations(double matrix[3][3], double valueVector[3]);

double PowerSum(double v[8], int power);

double PowerSum(double v1[8], int power, double v2[8]);