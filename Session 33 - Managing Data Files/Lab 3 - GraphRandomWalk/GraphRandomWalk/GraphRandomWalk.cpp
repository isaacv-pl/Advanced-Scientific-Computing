// GraphRandomWalk.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

using namespace std;

int main(int argc, char* argv[])
{
	// Open file passed as command line argument
	if (argc < 2) {
		cout << "Error: Please specify a file name";
		return -1;
	}

	string filename(argv[1]);

	// Determine format of input file
	// based upon filename extension
	bool binaryInput = (filename.find(".bin") != string::npos)
		? true : false;

	ifstream infile(filename);

	if (!infile.is_open()) {
		cout << "Error: Unable to open " << filename;
		return -1;
	}

	const unsigned int steps = 250;

	const unsigned int walkers = 7;

	struct Position {
		double x{ 0 };
		double y{ 0 };
		double z{ 0 };
	};

	struct Path {
		vector<Position> positions;
	};

	vector<Path> walks(walkers);

	for (int step{}; step < steps;++step) {
		for (auto& w : walks) {
			Position pos;
			if (binaryInput)
				infile.read((char*)&pos, sizeof(Position));
			else
				infile >> pos.x >> pos.y >> pos.z;
			w.positions.push_back(pos);
		}
	}

	TApplication* theApp =	new TApplication("3D Random Walk",
		nullptr, nullptr);
	
	TCanvas *c1 = new TCanvas(filename.c_str(),
		filename.c_str(), 500, 500);

	TView *view = TView::CreateView(1);
	view->SetRange(-15, -15, -15, 15, 15, 15);

	Color_t color{};

	for (const auto& w : walks) {
		vector<double> x(steps);
		vector<double> y(steps);
		vector<double> z(steps);
		for (int step{}; step < steps;++step) {
			x.at(step) = w.positions.at(step).x;
			y.at(step) = w.positions.at(step).y;
			z.at(step) = w.positions.at(step).z;
		}
		TPolyLine3D* line = new TPolyLine3D(steps,
			&x[0], &y[0], &z[0]);
		line->SetLineColor(++color);
		line->SetLineWidth(2);
		line->Draw();
	}

	theApp->Run();

	return 0;
}

