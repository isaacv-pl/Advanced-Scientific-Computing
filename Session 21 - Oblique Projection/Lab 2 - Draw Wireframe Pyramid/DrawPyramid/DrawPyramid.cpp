// DrawPyramid.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-5, -5, 500, 500);

	ss.SetProjection(29, 0.225);

	// Define the size of the pyramid
	double goldenRatio = (1.0 + sqrt(5)) / 2.0;

	double length = 400;                     // In X direction            
	double height = length / goldenRatio;    // In Y direction
	double width = 400;                      // In Z direction

	// Define the vertices of the pyramid by 3D world coordinates
	PointSet3D* vertices = new PointSet3D();

	vertices->add(0, 0, 0);							// Base Front Left
	vertices->add(length, 0, 0);					// Base Front Right
	vertices->add(length, 0, -width);				// Base Back Right
	vertices->add(0, 0, -width);					// Base Back Left
	vertices->add(length/2,height,-width/2);						// Apex

	// Define the facets of the pyramid by vertex numbers														   
	FacetSet* facets = new FacetSet();

	facets->add(vertices, { 0,1,2,3 });		// Base
	facets->add(vertices, { 0,3,4 });		// Left
	facets->add(vertices, { 0,1,4 });		// Front
	facets->add(vertices, { 1,2,4 });		// Right
	facets->add(vertices, {4,2,3});		// Back

	ss.DrawLines(facets, "green", 3, false, 1000);

	delete facets;
	delete vertices;

	system("pause");
	return 0;
}