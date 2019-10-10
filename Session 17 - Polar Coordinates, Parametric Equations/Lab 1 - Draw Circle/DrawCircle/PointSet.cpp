// PointSet.cpp

#include "stdafx.h"
#include "PointSet.h"

using namespace std;

PointSet::PointSet(){
	points = new vector<PointF>();
}

PointSet::~PointSet(){
	delete points;
}

PointF PointSet::at(int i){
	return points->at(i);
}

void PointSet::add(double x, double y){
	points->push_back(make_pair(x, y));
}

int PointSet::size() {
	return points->size();
}


