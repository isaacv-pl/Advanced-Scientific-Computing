// HarmonicEulerCromer.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	// Set number of time steps in simulation
	const int timeSteps{ 2500 };

	double timeAt[timeSteps];
	double omega[timeSteps];
	double theta[timeSteps];

	const double length = 1.0;	// (m)
	const double g = 9.8;		// (m/s^2)

	const double phaseConstant = g / length;

	// Set initial pendulum angular velocity
	omega[0] = 0.0;

	// Set initial pendulum displacement
	theta[0] = M_PI / 18.0;

	// Duration of simulation (secs)
	const double endTime{ 10 };

	// Calculate time step (delta t)
	const double deltaTime{ endTime / timeSteps };

	// Set initial time
	timeAt[0] = 0.0;

	// Perform Euler-Cromer method to estimate differential equation
	for (int step{}; step < timeSteps - 1; ++step) {
		omega[step + 1] = omega[step] - phaseConstant * theta[step] * deltaTime;
		theta[step + 1] = theta[step] + omega[step+1] * deltaTime;
		timeAt[step + 1] = timeAt[step] + deltaTime;
	}

	// Graph the decay curve using CERN's ROOT libraries
	TApplication* theApp =
		new TApplication("Differential Equations", nullptr, nullptr);

	TCanvas* c1 = new TCanvas("Harmonic Motion - Euler-Cromer Method");
	c1->SetTitle("Simple Pendulum - Euler-Cromer Method");

	TGraph* g1 = new TGraph(timeSteps, timeAt, theta);

	g1->SetTitle("Simple Pendulum - Euler-Cromer Method;time (s);theta (radians)");
	g1->SetLineColor(kBlue);
	g1->SetLineWidth(3);
	g1->Draw();

	theApp->Run();

	delete g1;
	delete c1;
	delete theApp;

	return 0;
}
