// SimpleScreen.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

// Point2D
Point2D::Point2D() {
}

Point2D::Point2D(double x, double y) {
	this->x = x;
	this->y = y;
}

Point2D::~Point2D() {
}


// Point3D
Point3D::Point3D() {
}

Point3D::Point3D(double x, double y, double z) {
	this->x = x;
	this->y = y;
	this->z = z;
}

Point3D::~Point3D() {
}


// PointSet
PointSet::PointSet() {
	points = new vector<Point2D*>();
}

PointSet::~PointSet() {
	points->clear();
	delete points;
}

Point2D* PointSet::at(size_t i) {
	return points->at(i);
}

void PointSet::clear() {
	points->clear();
}

void PointSet::add(double x, double y) {
	points->push_back(new Point2D(x, y));
}

size_t PointSet::size() {
	return points->size();
}


// PointSet3D
PointSet3D::PointSet3D() {
	points = new vector<Point3D*>();
}

PointSet3D::~PointSet3D() {
	points->clear();
	delete points;
}

Point3D* PointSet3D::at(size_t i) {
	return points->at(i);
}

void PointSet3D::clear() {
	points->clear();
}

void PointSet3D::add(double x, double y, double z) {
	points->push_back(new Point3D(x, y, z));
}

void PointSet3D::add(Point3D* p3d)
{
	points->push_back(p3d);
}

size_t PointSet3D::size() {
	return points->size();
}


//Facet
Facet::Facet()
{
	points = new PointSet3D();
}

Facet::Facet(PointSet3D* allVertices, vector<size_t> vertexNumbers)
	:Facet()
{
	for (size_t v : vertexNumbers)
		points->add(allVertices->at(v));
}

Facet::~Facet()
{
	points->clear();
	delete points;
}

Point3D* Facet::at(size_t i) {
	return points->at(i);
}

PointSet3D* Facet::all() {
	return points;
}

void Facet::clear() {
	points->clear();
}

size_t Facet::size() {
	return points->size();
}


// FacetSet
FacetSet::FacetSet() {
	facets = new vector<Facet*>();
}

FacetSet::~FacetSet(){
	facets->clear();
	delete facets;
}

Facet* FacetSet::at(size_t i){
	return facets->at(i);
}


PointSet3D* FacetSet::vertices(size_t facetNumber){
	return facets->at(facetNumber)->all();
}

void FacetSet::add(PointSet3D* allVertices, vector<size_t> vertexNumbers){
	facets->push_back(new Facet(allVertices, vertexNumbers));
}

void FacetSet::clear(){
	facets->clear();
}

size_t FacetSet::size() {
	return facets->size();
}


// SimpleScreen
SimpleScreen::SimpleScreen() {
	al_init();
	al_init_primitives_addon();
	display = al_create_display(screenWidth, screenHeight);
	Clear();
	points = new vector<Point2D*>();
	SetProjection();
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

void SimpleScreen::SetProjection(double degrees, double correction)
{
	obliqueAngle = degrees * M_PI / 180;
	obliqueCos = cos(obliqueAngle);
	obliqueSin = sin(obliqueAngle);
	aspectCorrection = correction;
}

void SimpleScreen::Clear() {
	//al_clear_to_color(al_map_rgb(0, 0, 0));
	al_clear_to_color(al_map_rgb(212, 208, 200));
}

void SimpleScreen::CalcScreenPoints(PointSet* ps)
{
	points->clear();
	for (size_t i{}; i < ps->size(); ++i) {
		// Convert WORLD coordinates to SCREEN coordinates
		double screenX = (ps->at(i)->x - worldXmin) * scaleX;
		double screenY = screenHeight - (ps->at(i)->y - worldYmin) * scaleY;
		// Add this SCREEN coordinate to array of points
		points->push_back(new Point2D(screenX, screenY));
	}
}

void SimpleScreen::CalcScreenPoints3D(PointSet3D* ps3d)
{
	points->clear();
	for (size_t i{}; i < ps3d->size(); ++i) {
		// Convert WORLD coordinates to SCREEN coordinates
		double screenX = (ps3d->at(i)->x - 
			ps3d->at(i)->z * obliqueCos * aspectCorrection - worldXmin) * scaleX;
		double screenY = screenHeight - 
			(ps3d->at(i)->y - ps3d->at(i)->z * obliqueSin - worldYmin) * scaleY;
		// Add this SCREEN coordinate to array of points
		points->push_back(new Point2D(screenX, screenY));
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

void SimpleScreen::DrawLines(PointSet* ps, string clr, float width,
	bool close, bool fill, long delay)
{
	CalcScreenPoints(ps);
	if (fill) close = true;
	if (close) {
		vector<float> verts;
		for (size_t i{};i < points->size();++i) {
			verts.push_back(points->at(i)->x);
			verts.push_back(points->at(i)->y);
		}
		if (fill) {
			al_draw_filled_polygon(&verts[0], ps->size(), al_color_name(clr.c_str()));
		}
		else {
			//al_draw_polygon(&verts[0], ps->size(), ALLEGRO_LINE_JOIN_NONE, al_color_name(clr.c_str()), width, 0);
			al_draw_polygon(&verts[0], ps->size(), ALLEGRO_LINE_JOIN_ROUND, al_color_name(clr.c_str()), width, 0);
		}
	}
	else {
		for (size_t i{}; i < points->size() - 1;++i) {
			al_draw_line(points->at(i)->x, points->at(i)->y,
				points->at(i + 1)->x, points->at(i + 1)->y, al_color_name(clr.c_str()), width);
			if (delay > 0) {
				al_flip_display();
				this_thread::sleep_for(chrono::milliseconds(delay));
			}
		}
	}
	al_flip_display();
}

void SimpleScreen::DrawLines(FacetSet* facets, std::string clr, float width, bool fill, long delay)
{
	for (size_t f{}; f < facets->size();++f) {
		CalcScreenPoints3D(facets->at(f)->all());
		vector<float> verts;
		for (size_t i{};i < points->size();++i) {
			verts.push_back(points->at(i)->x);
			verts.push_back(points->at(i)->y);
		}
		if (fill){
			al_draw_filled_polygon(&verts[0], points->size(), al_color_name(clr.c_str()));
		}
		else {
			//al_draw_polygon(&verts[0], points->size(), ALLEGRO_LINE_JOIN_NONE, al_color_name(clr.c_str()), width, 0);
			al_draw_polygon(&verts[0], points->size(), ALLEGRO_LINE_JOIN_ROUND, al_color_name(clr.c_str()), width, 0);
		}
		if (delay > 0) {
			al_flip_display();
			this_thread::sleep_for(chrono::milliseconds(delay));
		}
	}
	al_flip_display();
}



