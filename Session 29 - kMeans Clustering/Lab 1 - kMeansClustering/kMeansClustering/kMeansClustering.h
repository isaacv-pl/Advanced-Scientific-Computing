// kMeansClustering.h

#pragma once

#include "stdafx.h"

using std::string;
using std::vector;

class Cluster {
public:
	Cluster();
	Cluster(int index);
	~Cluster();
	double x;
	double y;
	string clr;
	int population;
};

class DataPoint {
public:
	DataPoint();
	DataPoint(double x, double y);
	~DataPoint();
	double x;
	double y;
	Cluster* c;
};



