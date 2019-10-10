// SinusoidAliasing.cpp

#include "stdafx.h"

using namespace std;

auto pi = TMath::Pi();

int main()
{
	TApplication* theApp = new TApplication("Lab 1", nullptr, nullptr);

	TCanvas* c1 = new TCanvas();
	c1->SetTitle("Canvas 1");

	const int n{51 };
	double x[n], y[n];

	double deltaX = 2 * pi / n;	

	for (int i{}; i < n;++i) {
		x[i] = i * deltaX;
		y[i] = sin(5*x[i]);
	}

	TGraph* g1 = new TGraph(n, x, y);
	
	string title = "Sinsuoid From " + to_string(n) + " Samples";
	title += ";X Axis Label;Y Axis Label";

	g1->SetTitle(title.c_str());
	g1->SetLineColor(2);
	g1->Draw();

	theApp->Run();

	return 0;
}

