// Harmonograph.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	const int timeSteps{ 2500 };
	const double endTime{ 10 };
	const double deltaTime{ endTime / timeSteps };
	double timeAt[timeSteps];
	timeAt[0] = 0.0;

	const double g = 1.8;		// (m/s^2)

	// Define first pendulum
	double omega1[timeSteps];
	double theta1[timeSteps];
	const double length1 = 1.5;
	const double phaseConstant1 = g / length1;
	theta1[0] = 1;
	omega1[0] = 0;

	// Define first pendulum
	double omega2[timeSteps];
	double theta2[timeSteps];
	const double length2 = 1.5;	// (m)
	const double phaseConstant2 = g / length2;
	theta2[0] = 1;
	omega2[0] = 0;

	// Perform Euler-Cromer method to estimate differential equation
	for (int step{}; step < timeSteps - 1; ++step) {
		// First pendulum
		omega1[step + 1] = omega1[step] - phaseConstant1 * theta1[step] * deltaTime;
		theta1[step + 1] = theta1[step] + omega1[step + 1] * deltaTime;
		// Second pendulum
		omega2[step + 1] = omega2[step] - phaseConstant2 * theta2[step] * deltaTime;
		theta2[step + 1] = theta2[step] + omega2[step + 1] * deltaTime;
		// Update time
		timeAt[step + 1] = timeAt[step] + deltaTime;
	}

	// Graph the decay curve using CERN's ROOT libraries
	TApplication* theApp =
		new TApplication("Differential Equations", nullptr, nullptr);

	TCanvas* c1 = new TCanvas("Two Pendulum Harmonograph");
	c1->SetTitle("Two Pendulum Harmonograph - Euler-Cromer Method");

	TGraph* g1 = new TGraph(timeSteps, theta1, theta2);

	g1->SetTitle("Two Pendulum Harmonograph - Euler-Cromer Method;theta (radians);theta (radians)");
	g1->SetMarkerStyle(kDot);
	g1->SetMarkerColor(kBlue);
	g1->SetLineColor(kBlue);
	g1->SetLineWidth(2);
	g1->Draw();

	theApp->Run();

	delete g1;
	delete c1;
	delete theApp;

	return 0;
}
