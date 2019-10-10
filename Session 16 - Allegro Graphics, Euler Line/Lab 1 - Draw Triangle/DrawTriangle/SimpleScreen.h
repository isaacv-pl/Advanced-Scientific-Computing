// SimpleScreen.h

#pragma once

#include "stdafx.h"
#include "PointSet.h"

class SimpleScreen
{
public:
	SimpleScreen();
	~SimpleScreen();

	void SetWorldRect(double xMin, double yMin,
		double xMax, double yMax);

	void Clear();

	void DrawAxes(std::string clr = "black", float width = 1);

	void DrawLines(PointSet & ps, std::string, float width = 1,
		bool close = true, bool fill = false);


private:
	void CalcScreenPoints(PointSet& ps);

	const int screenWidth = 501;
	const int screenHeight = 501;
	ALLEGRO_DISPLAY* display;
	double worldXmin, worldYmin, worldXmax, worldYmax;
	double worldWidth, worldHeight;
	double scaleX, scaleY;

	vector<PointF>* points;
};

