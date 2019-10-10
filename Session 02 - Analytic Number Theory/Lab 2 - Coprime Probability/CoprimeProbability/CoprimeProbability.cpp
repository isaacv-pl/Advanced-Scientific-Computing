// CoprimeProbability.cpp

#include "stdafx.h"

using namespace std;

int GCD(int a, int b);

int main()
{
	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_int_distribution<int> distribution(1, 100000);

	double maxIterations{ 1000000 };
	double coprimePairs{};

	for (double i{}; i < maxIterations;++i) 
	{
		int a = distribution(generator);
		int b = distribution(generator);
		if (GCD(a, b) == 1)
			coprimePairs++;
	}

	double coprimeProbability = coprimePairs / maxIterations;

	cout << "Probability two random integers are coprime = ";
	cout << setprecision(14) << coprimeProbability << endl;

	cout << "\nHidden constant:\t" << sqrt(6 / coprimeProbability) << endl;

#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

	return 0;
}

int GCD(int a, int b)
{
	//return b == 0 ? a : GCD(b, a%b);
	int difference;
	if (a > b)
		difference = a - b;
	else if (b > a)
		difference = b - a;
	else
		return b;
	while (difference != 0)
	{
		if (b > difference)
		{
			b = a;
			difference = b;
		}
		else if (b < difference)
			difference = a;
		difference=a-b;
	}
	return b;
}


