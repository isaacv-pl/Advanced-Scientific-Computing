// GenerateIDFT.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

using namespace std;

typedef vector<tuple<double, double>> VT2D;

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

unique_ptr<VT2D> ReadTupleFile(string filename)
{
	auto v{ make_unique<VT2D>() };
	ifstream infile(filename);
	if (!infile.is_open()) throw exception();
	double x, y;
	string line;
	stringstream ss;
	ss.imbue(locale(locale(), new csv_reader()));
	while (getline(infile, line)) {
		ss.str(line);
		ss >> x >> y;
		ss.clear();
		v->push_back(make_tuple(x, y));
	}
	infile.close();
	return v;
}

void WriteTupleFile(string filename, shared_ptr<VT2D> vec)
{
	ofstream outfile(filename);
	outfile.exceptions(ofstream::failbit);
	for (const auto &v : *vec)
	{
		outfile << fixed << get<0>(v) << ", ";
		outfile << fixed << get<1>(v) << endl;
	}
	outfile.flush();
	outfile.close();
}

int main()
{
	const auto samples = ReadTupleFile("samples.csv");
	const auto dft = ReadTupleFile("dft.csv");

	const auto idft{ make_shared<VT2D>() };

	size_t term_count{ dft->size() };
	for (const auto &sample : *samples) {
		double xs{ get<0>(sample) }, ys{ get<1>(sample) }, yt{ 0 };
		for (size_t term{ 0 }; term < term_count; ++term) {
			yt += get<0>(dft->at(term)) * cos(term * xs);
			yt += get<1>(dft->at(term)) * sin(term * xs);
		}
		idft->push_back(make_tuple(ys, yt));
	}

	WriteTupleFile("idft.csv", idft);

    return 0;
}

