// BaselProblem.cpp : Defines the entry point for the console application.

#include "stdafx.h"

using namespace std;

int main()
{
	double sum;
	
	cout.imbue(std::locale(""));

	for (int limit{ 1000 }; limit <= 10000; limit += 1000) {
		sum = 0;
		for (int n{ 1 }; n < limit; ++n)
			sum += 1.0 / pow(n, 2.0);
		cout << "Sum of reciprocals of positive integers squared <= ";
		cout << setw(6) << limit << " = ";
		cout << setprecision(14) << sum << endl;
	}

	cout << "\nMagic Number = " << sqrt(sum * 6) << endl;

#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

	return 0;
}

