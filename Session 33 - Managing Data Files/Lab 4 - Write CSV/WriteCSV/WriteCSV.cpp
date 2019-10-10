// WriteCSV.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	// Emit samples file
	ofstream outfile("atomic_data.csv", ofstream::trunc);

	typedef tuple<string, string, string> atomic_data;

	vector<atomic_data> data;

	data.push_back(make_tuple("12.1017", "Carbon", "C"));
	data.push_back(make_tuple("14.0067", "Nitrogen", "N"));
	data.push_back(make_tuple("15.9994", "Oxygen", "O"));
	data.push_back(make_tuple("18.9984", "Flourine", "F"));

	outfile << "Mass,Name,Symbol" << endl;

	for (const auto& d : data)
	{
		outfile << get<0>(d) << ","
			<< get<1>(d) << ","
			<< get<2>(d) << endl;
	}
	outfile.flush();
	outfile.close();

	return 0;
}

