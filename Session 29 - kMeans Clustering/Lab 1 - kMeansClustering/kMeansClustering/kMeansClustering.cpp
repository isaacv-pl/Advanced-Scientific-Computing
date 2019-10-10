// kMeansClustering.cpp

#include "stdafx.h"
#include "SimpleScreen.h"
#include "kMeansClustering.h"

using namespace std;

SimpleScreen* ss;
vector<DataPoint*>* dataPoints;
vector<Cluster*>* clusters;

void InitDataPoints(bool includeOutlier);
void InitClusters(int numClusters);
void DrawClusters();
bool UpdateClusters();

int main()
{
	ss = new SimpleScreen();
	ss->SetWorldRect(-5, -5, 45, 45);

	InitDataPoints(true);

	InitClusters(5);

	DrawClusters();

	while (true){
		ALLEGRO_EVENT ev = ss->Wait();
		if (ev.type == ALLEGRO_EVENT_DISPLAY_CLOSE)
			break;

		if (ev.type == ALLEGRO_EVENT_MOUSE_BUTTON_DOWN) {
			if (UpdateClusters())
				cout << "Cluster has converged!" << endl;
			DrawClusters();
		}
	}

	delete ss;

	clusters->clear();
	delete clusters;
	
	dataPoints->clear();
	delete dataPoints;

	return 0;
}


// Cluster
Cluster::Cluster() {
	x = 0;
	y = 0;
	clr = "";
	population = 0;
}

Cluster::Cluster(int index)
	:Cluster()
{
	if (index == 0) clr = "red";
	if (index == 1) clr = "blue";
	if (index == 2) clr = "green";
	if (index == 3) clr = "orange";
	if (index == 4) clr = "yellow";
	if (index == 5) clr = "white";
}

Cluster::~Cluster() {

}

// DataPoint
DataPoint::DataPoint() {
	x = 0;
	y = 0;
	c = nullptr;
}

DataPoint::DataPoint(double x, double y)
	:DataPoint() {
	this->x = x;
	this->y = y;
}

DataPoint::~DataPoint() {

}

void InitDataPoints(bool includeOutlier) {
	
	// Populate data point vector
	double radius{ 5 };
	seed_seq seed{ 2016 };
	default_random_engine gen{ seed };
	uniform_real_distribution<double> center1x(-5.0, 45);
	uniform_real_distribution<double> center1y(-5.0, 45);
	uniform_real_distribution<double> center2x(-5.0, 45);
	uniform_real_distribution<double> center2y(-5.0, 45);
	uniform_real_distribution<double> center3x(-5.0, 45);
	uniform_real_distribution<double> center3y(-5.0, 45);
	uniform_real_distribution<double> center4x(-5.0, 45);
	uniform_real_distribution<double> center4y(-5.0, 45);
	uniform_real_distribution<double> center5x(-5.0, 45);
	uniform_real_distribution<double> center5y(-5.0, 45);
	uniform_real_distribution<double> center6x(-5.0, 45);
	uniform_real_distribution<double> center6y(-5.0, 45);
	uniform_real_distribution<double> boundaryx(radius);

	dataPoints = new vector<DataPoint*>();
	dataPoints->push_back(new DataPoint(center1x(gen), center1y(gen)));
	dataPoints->push_back(new DataPoint(center2x(gen), center2y(gen)));
	dataPoints->push_back(new DataPoint(center3x(gen), center3y(gen)));
	dataPoints->push_back(new DataPoint(center4x(gen), center4y(gen)));
	dataPoints->push_back(new DataPoint(center5x(gen), center5y(gen)));
	dataPoints->push_back(new DataPoint(center6x(gen), center6y(gen)));
	dataPoints->push_back(new DataPoint(center1x(gen), center1y(gen)));
	dataPoints->push_back(new DataPoint(center1x(gen), center1y(gen)));
	dataPoints->push_back(new DataPoint(center1x(gen), center1y(gen)));
	dataPoints->push_back(new DataPoint(center1x(gen), center1y(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));
	dataPoints->push_back(new DataPoint(xy(gen), xy(gen)));

	if (includeOutlier) {
		// A data outlier
		dataPoints->push_back(new DataPoint(5, 40));
	}
}

void InitClusters(int numClusters) {
	// Populate cluster vector
	clusters = new vector<Cluster*>();
	for (int i{}; i < numClusters; ++i) {
		clusters->push_back(new Cluster(i));
	}

	// Assign each data point an initial cluster
	for (size_t i{}; i < dataPoints->size(); ++i) {
		Cluster* c = clusters->at(i % clusters->size());
		dataPoints->at(i)->c = c;
		c->population++;
	}
}

void DrawClusters() {
	ss->Clear();
	ss->DrawAxes();
	
	// Draw each data point
	for (auto dp : *dataPoints)
		ss->DrawRectangle(dp->c->clr, dp->x, dp->y, 1, 1, 0, true);
	
	// Draw each cluster's geometric center (the "mean")
	for (auto c : *clusters)
		ss->DrawCircle(c->x, c->y, 1, c->clr, 2);

	ss->Update();
}

bool UpdateClusters() {
	// Assume the k-means clustering is now stable until
	// proven otherwise by either moving means or reassignments
	bool converged = true;

	// Phase I:
	// Calculate new geometric mean of each cluster
	// based upon current data point assignments
	for (auto c : *clusters) {
		DataPoint pt;
		size_t count{};
		for (auto dp : *dataPoints) {
			if (dp->c == c) {
				pt.x += dp->x;
				pt.y += dp->y;
				count++;
			}
		}
		pt.x /= count;
		pt.y /= count;

		// Move cluster center (mean) if necessary
		if (pt.x != c->x || pt.y != c->y) {
			c->x = pt.x;
			c->y = pt.y;
			converged = false;
		}
	}

	// Phase II:
	// Assign data points to nearest cluster
	for (auto dp : *dataPoints) {
		double distMin = numeric_limits<double>::max();
		Cluster* cNearest = nullptr;
		for (auto c : *clusters) {
			double dist = sqrt(pow(dp->x - c->x, 2) +
				pow(dp->y - c->y, 2));
			if (dist < distMin) {
				distMin = dist;
				cNearest = c;
			}
		}
		if (cNearest != dp->c) {
			// Reassign data point to the new cluster but only
			// if this is not the last point the current cluster.
			// Reassinging should never result in an empty cluster
			if (dp->c->population > 1) {
				dp->c->population--;
				dp->c = cNearest;
				dp->c->population++;
				converged = false;
			}
		}
	}

	return converged;
}