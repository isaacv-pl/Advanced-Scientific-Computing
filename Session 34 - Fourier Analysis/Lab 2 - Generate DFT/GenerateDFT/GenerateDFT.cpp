// GenerateDFT.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

using namespace std;

struct csv_reader : ctype<char> {
	csv_reader() : ctype<char>(get_table()) {}
	static ctype_base::mask const* get_table() {
		static vector<ctype_base::mask> rc(table_size, ctype_base::mask());
		rc[','] = ctype_base::space;
		rc['\n'] = ctype_base::space;
		rc[' '] = ctype_base::space;
		return &rc[0];
	}
};

int main()
{
	// Create vector of tuples to store x, y values
	const auto samples{ make_unique<vector<tuple<double, double>>>() };

	// Read each sample tuple from data file
	ifstream infile("space_signals3.csv");
	double x, y;
	string line;
	stringstream ss;
	ss.imbue(locale(locale(), new csv_reader()));
	while (getline(infile, line)) {
		ss.str(line);
		ss >> x >> y;
		ss.clear();
		samples->push_back(make_tuple(x, y));
	}
	infile.close();

	// Generate discrete Fourier transform
	const size_t sample_count{ samples->size() };
	const size_t term_count{ samples->size() / 2 };
	const auto dft{ make_unique<vector<tuple<double,double>>>() };

	for (size_t term{ 0 }; term < term_count; ++term) {
		double fcos{ 0 }, fsin{ 0 };
		for (size_t sample{ 0 }; sample < sample_count; ++sample) {
			double xs{ get<0>(samples->at(sample)) };
			double ys{ get<1>(samples->at(sample)) };
			fcos += cos(term * xs) * ys;
			fsin += sin(term * xs) * ys;
		}
		fcos /= sample_count;
		fsin /= sample_count;
		if (term > 0) {
			fcos *= 2;
			fsin *= 2;
		}
		dft->push_back(make_tuple(fcos, fsin));
	}

	// Emit DFT data file
	ofstream outfile("dft3.csv");
	outfile.exceptions(ofstream::failbit);
	for (const auto &s : *dft)
	{
		outfile << fixed << get<0>(s) << ", ";
		outfile << fixed << get<1>(s) << endl;
	}
	outfile.flush();
	outfile.close();

	return 0;
}

