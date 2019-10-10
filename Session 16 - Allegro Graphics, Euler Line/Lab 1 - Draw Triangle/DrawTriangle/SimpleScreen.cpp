// SimpleScreen.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

SimpleScreen::SimpleScreen() {
	al_init();
	al_init_primitives_addon();
	display = al_create_display(screenWidth, screenHeight);
	Clear();
	points = new vector<PointF>();
}

SimpleScreen::~SimpleScreen() {
	delete points;
	al_destroy_display(display);
	al_shutdown_primitives_addon();
}

void SimpleScreen::SetWorldRect(double xMin, double yMin,
	double xMax, double yMax) {
	worldXmin = xMin;
	worldYmin = yMin;
	worldXmax = xMax;
	worldYmax = yMax;
	worldWidth = xMax - xMin;
	worldHeight = yMax - yMin;
	scaleX = screenWidth / worldWidth;
	scaleY = screenHeight / worldHeight;
}

void SimpleScreen::Clear() {
	//al_clear_to_color(al_map_rgb(0, 0, 0));
	al_clear_to_color(al_map_rgb(212, 208, 200));
}

void SimpleScreen::CalcScreenPoints(PointSet& ps)
{
	points->clear();
	for (int i{}; i < ps.size(); ++i) {
		// Convert WORLD coordinates to SCREEN coordinates
		double screenX = (ps.at(i).first - worldXmin) * scaleX;
		double screenY = screenHeight - (ps.at(i).second - worldYmin) * scaleY;

		// Add this SCREEN coordinate to array of points
		points->push_back(make_pair(screenX, screenY));
	}
}

void SimpleScreen::DrawAxes(string clr, float width) {
	// Draw X axis
	double screenY0 = screenHeight + worldYmin * scaleY;
	al_draw_line(1, screenY0, screenWidth, screenY0, al_color_name(clr.c_str()), width);

	// Draw Y axis
	double screenX0 = -worldXmin * scaleX;
	al_draw_line(screenX0, 1, screenX0, screenHeight, al_color_name(clr.c_str()), width);

	al_flip_display();
}

void SimpleScreen::DrawLines(PointSet& ps, string clr, float width, bool close, bool fill)
{
	CalcScreenPoints(ps);
	if (fill) close = true;
	if (close) {
		vector<float> verts;
		for (int i{};i < points->size();++i) {
			verts.push_back(points->at(i).first);
			verts.push_back(points->at(i).second);
		}
		//if (close) {
		//	verts.push_back(points->at(0).first);
		//	verts.push_back(points->at(0).second);
		//}
		if (fill) {
			//al_draw_filled_polygon(&verts[0], verts.size(), al_color_name(clr.c_str()));
			al_draw_filled_polygon(&verts[0], ps.size(), al_color_name(clr.c_str()));
		}
		else {
			//al_draw_polygon(&verts[0], verts.size(), ALLEGRO_LINE_JOIN_NONE, al_color_name(clr.c_str()), width, 0);
			al_draw_polygon(&verts[0], ps.size(), ALLEGRO_LINE_JOIN_ROUND, al_color_name(clr.c_str()), width, 0);
		}
	}
	else {
		for (int i{}; i < points->size() - 1;++i)
			al_draw_line(points->at(i).first, points->at(i).second,
				points->at(i + 1).first, points->at(i + 1).second, al_color_name(clr.c_str()), width);
	}
	al_flip_display();
}
