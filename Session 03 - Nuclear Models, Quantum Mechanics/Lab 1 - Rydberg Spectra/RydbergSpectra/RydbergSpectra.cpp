
// RydbergSpectra.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	cout << "Rydberg Formula Hydrogen Spectral Lines" << endl;
	const double cRydberg = 1096775700;
	for (int k{ 1 }; k < 5; ++k)
	{
		for (int j{ k + 1 }; j < k + 6; ++j)
		{
			double lambda = 1 / (cRydberg *((1 / pow(k, 2)) - (1 / pow(j, 2))));
			cout << setw(3) << k;
			cout << setw(10) << setprecision(0) << fixed;
			cout << lambda *100000000000 << "nm" << endl;
		}
	}
	
	#ifdef _WIN3
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

    return 0;
}

