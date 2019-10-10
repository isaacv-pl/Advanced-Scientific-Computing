// GaussianNormal.cpp

#include "stdafx.h"

using namespace std;

auto pi = TMath::Pi();

int main()
{
	TApplication* theApp = new TApplication("Lab 2", nullptr, nullptr);

	TCanvas* c1 = new TCanvas();
	c1->SetTitle("Canvas 1");

	seed_seq seed { 2016 };
	default_random_engine generator{ seed };
	normal_distribution<double> normal(50, 2);

	TH1D* h1 = new TH1D("Gaussian", "Normal Curve", 100, 40, 60);
	for (int i = 0; i < 1000000; i++){
		double x = normal(generator);
		h1->Fill(x);
	}

	h1->Draw();

	theApp->Run();

    return 0;
}

