// GaussianSummation.cpp : Defines the entry point for the console application.

#include "stdafx.h"

using namespace std;

int main()
{
	int n{ 1000 };

	int sumByLooping{};
	int sumByGaussian{};

	for (int i{1}; i < n+1; ++i)
	{
		sumByLooping += i;
	}

	sumByGaussian = (n*(n + 1)) / 2;
	
	cout.imbue(std::locale(""));
	cout << "Manual sum of first ";
	cout << n << " natural numbers = ";
	cout << sumByLooping << endl;

	cout << "Gaussian sum of first ";
	cout << n << " natural numbers = ";
	cout << sumByGaussian << endl;

#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

    return 0;

}

