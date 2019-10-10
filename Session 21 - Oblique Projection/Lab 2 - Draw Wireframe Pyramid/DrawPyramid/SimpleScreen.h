// SimpleScreen.h

#pragma once

#include "stdafx.h"

using std::vector;

// Point 2D
class Point2D
{
public:
	Point2D();
	Point2D(double x, double y);
	~Point2D();
	double x, y;
};

// Point 3D
class Point3D
{
public:
	Point3D();
	Point3D(double x, double y, double z);
	~Point3D();
	double x, y, z;
};

// PointSet
class PointSet
{
public:
	PointSet();
	~PointSet();
	Point2D* at(size_t i);
	void clear();
	void add(double x, double y);
	size_t size();
private:
	vector<Point2D*>* points;
};

// PointSet3D
class PointSet3D
{
public:
	PointSet3D();
	~PointSet3D();
	Point3D* at(size_t i);
	void clear();
	void add(double x, double y, double z);
	void add(Point3D* p3d);
	size_t size();
private:
	vector<Point3D*>* points;
};

// Facet
class Facet
{
public:
	Facet();
	Facet(PointSet3D* allVertices, vector<size_t> vertexNumbers);
	~Facet();
	Point3D* at(size_t i);
	PointSet3D* all();
	void clear();
	size_t size();
private:
	PointSet3D* points;
};

// FacetSet
class FacetSet
{
public:
	FacetSet();
	~FacetSet();
	Facet* at(size_t i);
	PointSet3D* vertices(size_t facetNumber);
	void add(PointSet3D* allVertices, vector<size_t> vertexNumbers);
	void clear();
	size_t size();
private:
	vector<Facet*>* facets;
};

// SimpleScreen
class SimpleScreen
{
public:
	SimpleScreen();
	~SimpleScreen();

	void SetWorldRect(double xMin, double yMin,
		double xMax, double yMax);

	void SetProjection(double degrees = 45, double correction = 1);

	void Clear();

	void DrawAxes(std::string clr = "black", float width = 1);

	void DrawLines(PointSet* ps, std::string clr, float width = 1,
		bool close = true, bool fill = false, long delay = 0);

	void DrawLines(FacetSet* facets, std::string clr, float width = 1,
		bool fill = false, long delay = 0);

private:
	void CalcScreenPoints(PointSet* ps);
	void CalcScreenPoints3D(PointSet3D* ps3d);

	const int screenWidth = 501;
	const int screenHeight = 501;
	ALLEGRO_DISPLAY* display;
	double worldXmin, worldYmin, worldXmax, worldYmax;
	double worldWidth, worldHeight;
	double scaleX, scaleY;

	vector<Point2D*>* points;

	double obliqueAngle;
	double obliqueCos;
	double obliqueSin;
	double aspectCorrection;
};

