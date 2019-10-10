// DrawTorus.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	SimpleScreen ss;
	ss.SetWorldRect(-200, -200, 200, 200);

	ss.SetProjection(29, 0.225);

	ss.SetCameraLocation(30000, 60000, 120000);

	// Define the two radii of the torus
	double r1 = 140;     // "Donut hole" radius
	double r2 = 30;      // "Cross-sectional" radius

	// Calculate the angle deltas
	double intervals = 37;
	double deltaPhi = M_PI / intervals;         // Latitudes
	double deltaTheta = 2 * M_PI / intervals;   // Longitudes

	// Step the phi angle counter-clockwise through a full circle (expressed in radians)
	for (double phi = 0; phi < M_PI * 2; phi += deltaPhi) {

		// Step the theta angle counter-clockwise through a full circle (expressed in radians)
		for (double theta = 0; theta < M_PI * 2; theta += deltaTheta) {

			// Create a vertex array to hold the four points of this facet
			// Note:  The vertices are numbered in a counterclockwise direction

			PointSet3D* vertices = new PointSet3D();

			vertices->add(-sin(phi) * (r1 + r2 / 2 * cos(theta)),
				r2 * sin(theta), -cos(phi) * (r1 + r2 / 2 * cos(theta)));

			vertices->add(-sin(phi + deltaPhi) * (r1 + r2 / 2 * cos(theta)),
				r2 * sin(theta), -cos(phi + deltaPhi) * (r1 + r2 / 2 * cos(theta)));

			vertices->add(-sin(phi + deltaPhi) * (r1 + r2 / 2 * cos(theta + deltaTheta)),
				r2 * sin(theta + deltaTheta), -cos(phi + deltaPhi) * (r1 + r2 / 2 * cos(theta + deltaTheta)));

			vertices->add(-sin(phi) * (r1 + r2 / 2 * cos(theta + deltaTheta)),
				r2 * sin(theta + deltaTheta), -cos(phi) * (r1 + r2 / 2 * cos(theta + deltaTheta)));

			Facet* f = (phi > 0) ? new Facet(vertices, { 0, 1, 2, 3 })
				: new Facet(vertices, { 2, 3, 0, 1 });

			ss.DrawFacet(f, al_map_rgb(212, 130, 55), al_map_rgb(212, 190, 55), 1, true, true, 20);
		}
	}

	ss.Update();

	system("pause");
	return 0;
}
