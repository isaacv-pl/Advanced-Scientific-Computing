// DrawCircle.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-5, -5, 5, 5);
	ss.DrawAxes();

	PointSet psCircle;

	int intervals = 50;

	double deltaTheta = 2.0 * M_PI / intervals;

	double radius = 3;

	for (int i{}; i < intervals; ++i)
	{
		double theta = deltaTheta * i;
		double x = radius * cos(theta);
		double y = radius * sin(theta);
		psCircle.add(x, y);
	}

	ss.DrawLines(psCircle, "midnightblue",10,true);

	system("pause");
	return 0;
}

