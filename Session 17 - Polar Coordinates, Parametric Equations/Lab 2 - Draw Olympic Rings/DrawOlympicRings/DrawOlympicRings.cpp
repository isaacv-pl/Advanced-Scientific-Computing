// DrawOlympicRings.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

void DrawCircle(SimpleScreen& ss, double centerX, double centerY,
	double radius, string clr, int width);

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-20, -20, 20, 20);

	double radius = 5;

	// You must determine proper offsets
	double ringOffsetFullX = radius*5/2;
	double ringOffsetHalfX = radius*5/4;
	double ringOffsetY = radius;

	int width = 15;

	DrawCircle(ss,0, 0, radius, "black", width);
	DrawCircle(ss,-ringOffsetFullX, 0, radius, "blue", width);
	DrawCircle(ss,ringOffsetFullX, 0, radius, "red", width);
	DrawCircle(ss,-ringOffsetHalfX, -ringOffsetY, radius, "yellow", width);
	DrawCircle(ss,ringOffsetHalfX, -ringOffsetY, radius, "green", width);

	system("pause");
	return 0;
}

void DrawCircle(SimpleScreen& ss, double centerX, double centerY,
	double radius, string clr, int width)
{
	PointSet psCircle;

	int intervals = 97;

	double deltaTheta = 2.0 * M_PI / intervals;

	for (int i{}; i < intervals; ++i)
	{
		double theta = deltaTheta * i;
		double x = centerX + radius * cos(theta);
		double y = centerY + radius * sin(theta);
		psCircle.add(x, y);
	}

	ss.DrawLines(psCircle, clr.c_str(),width);
}