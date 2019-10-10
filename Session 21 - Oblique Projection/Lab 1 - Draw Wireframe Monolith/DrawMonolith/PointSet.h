// PointSet.h

#pragma once

#include "stdafx.h"

using std::vector;

class Point2D
{
public:
	Point2D();
	Point2D(double x, double y);
	~Point2D();
	double x, y;
};

class Point3D
{
public:
	Point3D();
    Point3D(double x, double y, double z);
    ~Point3D();
	double x, y, z;
};

class PointSet
{
public:
	PointSet();
	~PointSet();
	Point2D* at(int i);
	void clear();
	void add(double x, double y);
	size_t size();

private:
	vector<Point2D*>* points;
};

class PointSet3D
{
public:
	PointSet3D();
	~PointSet3D();
	Point3D* at(int i);
	void clear();
	void add(double x, double y, double z);
	void add(Point3D* p3d);
	size_t size();

private:
	vector<Point3D*>* points;
};