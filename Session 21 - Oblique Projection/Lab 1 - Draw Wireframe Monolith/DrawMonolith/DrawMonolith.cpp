// DrawMonolith.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-50, -50, 400, 400);

	// Define the size of the monolith
	const double length = 150;    // In X direction
	const double height = 300;    // In Y direction
	const double width = 50;      // In Z direction

	// Define the vertices of the monolith by 3D world coordinates
	PointSet3D* vertices = new PointSet3D();

	vertices->add(0, 0, 0);					// Front Left Bottom
	vertices->add(length, 0, 0);			// Front Right Bottom
	vertices->add(length, 0, -width);		// Back Right Bottom
	vertices->add(0, 0, -width);			// Back Left Bottom
	vertices->add(0, height, 0);			// Front Left Top
	vertices->add(length, height, 0);		// Front Right Top
	vertices->add(length, height, -width);	// Back Right Top
	vertices->add(0, height, -width);		// Back Left Top

	// Define the facets of the monolith by vertex numbers														   
	FacetSet* facets = new FacetSet();

	facets->add(vertices, { 0,1,2,3 });		// Bottom
	facets->add(vertices, { 4,5,6,7 });		// Top
	facets->add(vertices, { 0,4,7,3 });		// Left
	facets->add(vertices, { 1,2,6,5 });		// Right
	facets->add(vertices, { 0,1,5,4 });		// Front
	facets->add(vertices, { 2,3,7,6 });		// Back

	ss.DrawLines(facets, "green", 3, false, 2000);

	delete facets;
	delete vertices;

	system("pause");
	return 0;
}

