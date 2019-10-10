// Facet.h

#pragma once

#include "stdafx.h"
#include "PointSet.h"

using std::vector;

class Facet
{
public:
	Facet();
	Facet(PointSet3D* vertices, vector<int> vertexNumbers);
	~Facet();
	Point3D at(int i);
	PointSet3D* points;
	void clear();
	size_t size();
};

//class FacetSet
//{
//public:
//	FacetSet();
//}



