// DrawSphere.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-200, -200, 200, 200);

	ss.SetProjection(29, 0.225);

	ss.SetCameraLocation(30000, 60000, 120000);

	// Set the radius of the sphere
	double radius = 175;

	// Calculate the angle deltas
	double intervals = 37;
	double deltaPhi = M_PI / intervals;         // Latitudes
	double deltaTheta = 2 * M_PI / intervals;   // Longitudes

	// Step phi around a half-circle to set each vertex "Y" coordinate
	for (double phi = 0; phi < M_PI; phi += deltaPhi)
	{
		PointSet3D* vertices = new PointSet3D();
		vertices->add(0, radius * cos(phi), 0);
		vertices->add(0, radius * cos(phi), 0);
		vertices->add(0, radius * cos(phi + deltaPhi), 0);
		vertices->add(0, radius * cos(phi + deltaPhi), 0);

		// Calculate radius of each line of latitude
		double rA = radius * sin(phi);
		double rB = radius * sin(phi + deltaPhi);

		// Step theta around a full circle to set each vertex "X" and "Z" coordinate
		for (double theta = 0; theta < M_PI * 2; theta += deltaTheta)
		{
			vertices->at(0)->x = rA * sin(theta);
			vertices->at(0)->z = -rA * cos(theta);

			vertices->at(1)->x = rA * sin(theta + deltaTheta);
			vertices->at(1)->z = -rA * cos(theta + deltaTheta);

			vertices->at(2)->x = rB * sin(theta + deltaTheta);
			vertices->at(2)->z = -rB * cos(theta + deltaTheta);

			vertices->at(3)->x = rB * sin(theta);
			vertices->at(3)->z = -rB * cos(theta);

			// At the North pole (phi == 0) vertex 0 and 1 are the same points,
			// so we use a different vertex number ordering to designate a
			// more meaningful surface normal for those particular facets
			Facet* f = (phi > 0) ? new Facet(vertices, { 0, 1, 2, 3 })
				: new Facet(vertices, { 2, 3, 0, 1 });
			
			ss.DrawFacet(f, al_map_rgb(0,0,0), al_map_rgb(0,0,255), 1, true, true, 2);
		}
	}
	ss.Update();

	system("pause");
	return 0;
}

