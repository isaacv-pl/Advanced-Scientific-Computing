// ReadCSV.cpp

#include "stdafx.h"

using namespace std;

struct csv_reader : ctype<char> {
	csv_reader() : ctype<char>(get_table()) {}
	static ctype_base::mask const* get_table() {
		static vector<ctype_base::mask>
			rc(table_size, ctype_base::mask());
		rc[','] = ctype_base::space;
		rc['\n'] = ctype_base::space;
		rc[' '] = ctype_base::space;
		return &rc[0];
	}
};


int main()
{
	typedef tuple<string, string, string> atomic_data;
	vector<atomic_data> data;

	string s1, s2, s3;

	string line;
	stringstream ss;
	ss.imbue(locale(locale(), new csv_reader()));

	// Read samples file
	ifstream infile("atomic_data.csv");
	while (getline(infile, line)) {
		ss.str(line);
		ss >> s1 >> s2 >> s3;
		ss.clear();
		data.push_back(make_tuple(s1, s2, s3));
	}
	infile.close();

	for (const auto& d : data)
		cout << setw(10) << get<0>(d)
		<< setw(12) << get<1>(d)
		<< setw(10) << get<2>(d) << endl;

	system("pause");
	return 0;
}

