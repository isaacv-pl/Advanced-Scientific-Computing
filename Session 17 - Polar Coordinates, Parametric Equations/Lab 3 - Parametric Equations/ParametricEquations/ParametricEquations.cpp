// ParametricEquations.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

void CalcPolarCurve(PointSet& ps, double loops, double radius);

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-150, -150, 150, 150);

	PointSet ps1, ps2;
	CalcPolarCurve(ps1, 8, 15);
	CalcPolarCurve(ps2, 4, 10);

	ss.DrawLines(ps1, "green", 1, false);
	ss.DrawLines(ps2, "red", 1, false);

	system("pause");
	return 0;
}

void CalcPolarCurve(PointSet& ps, double loops, double radius) {

	int intervals = 997;

	double deltaTheta = 2.0 * M_PI / intervals;

	for (int i = 0; i < intervals; i++)
	{
		double theta = i * deltaTheta;
		double r = radius * sin(loops * theta);
		double x = r * cos(theta);
		double y = r * sin(theta);
		ps.add(x, y);
	}
}

