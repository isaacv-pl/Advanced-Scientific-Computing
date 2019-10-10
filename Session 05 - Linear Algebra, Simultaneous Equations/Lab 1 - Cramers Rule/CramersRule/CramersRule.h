// CramersRule.h

#pragma once

double Determinant(double matrix[3][3]);

void OverlayValues(double coeffMatrix[3][3], double valueVector[3],
	int col, double newMatrix[3][3]);

void DisplayEquations(double matrix[3][3], double valueVector[3]);
