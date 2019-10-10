// GenRandomWalkTXT.cpp

#include "stdafx.h"

using namespace std;


int main()
{
	TRandom* r = new TRandom(2016);

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

	for (auto& w : walks) {
		Position pos;
		w.positions.push_back(pos);
		for (int step{ 1 };step < steps;++step) {
			pos.x = w.positions.at(step - 1).x
				+ ((r->Uniform(1) < .5) ? -1 : 1);
			pos.y = w.positions.at(step - 1).y
				+ ((r->Uniform(1) < .5) ? -1 : 1);
			pos.z = w.positions.at(step - 1).z
				+ ((r->Uniform(1) < .5) ? -1 : 1);
			w.positions.push_back(pos);
		}
	}

	ofstream outfile("RandomWalk.txt",
		ofstream::trunc);

	for (int step{}; step < steps;++step) {
		for (const auto& w : walks)
			outfile
			<< " " << w.positions.at(step).x
			<< " " << w.positions.at(step).y
			<< " " << w.positions.at(step).z;
		outfile << endl;
	}

	outfile.close();

	return 0;
}

