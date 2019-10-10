// PointSet.h

#pragma once

#include "stdafx.h"

using std::vector;
using std::pair;

typedef pair<float, float> PointF;

class PointSet
{
public:
	PointSet();
	~PointSet();
	PointF at(int i);
	void add(double x, double y);
	int size();


private:
	vector<PointF>* points;
};

