// ExpandStdCF.cpp : Defines the entry point for the console application.

#include "stdafx.h"

using namespace std;

int main()
{
	const int maxTerms = 20;	

	array<unsigned int, maxTerms> cf{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

	array<unsigned long, maxTerms + 2> h;
	array<unsigned long, maxTerms + 2> k;
	h[0] = 0;
	h[1] = 1;
	k[0] = 1;
	k[1] = 0;
	double convergent;

	cout << "Using " << maxTerms << " terms, ";
	cout << "the continued fraction expansion is:" << endl;
	cout << setw(5) << "a";
	cout << right << setw(15) << "h";
	cout << right << setw(15) << "k";
	cout << setw(20) << "convergent" << endl;
	for (int n{ 0 }; n < 20; ++n)
	{
		cout << setw(5) << cf [n];
		if (n == 0)
		{
			h[2] = h[1] + h[0];// I don't think I really remember how this part of the function works?
			k[2] = k[1] + k[0];
		}
		else
		{
			h[n + 2] = cf[n] * h[n + 1] + h[n];
			k[n + 2] = cf[n] * k[n + 1] + k[n];
		}
		cout << right << setw(15) <<h[n + 2];
		cout << right << setw(15) << k[n + 2];
		convergent = ((double)h[n + 2]) / k[n + 2];

		cout << setw(20) << setprecision(14) <<  convergent <<  endl;
	}

	


#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

    return 0;
}

