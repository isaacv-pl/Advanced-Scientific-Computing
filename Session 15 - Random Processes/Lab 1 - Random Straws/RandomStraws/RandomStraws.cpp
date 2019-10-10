// RandomStraws.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_real_distribution<double> distribution(0.0, 1.0);

	double maxIterations = 10000001; 
	double totalStraws = 0;
	int counter{};
	double strawrun{};

	for (double iteration = 0; iteration < maxIterations; iteration++)
	{
		while (strawrun < 1)
		{
			strawrun += distribution (generator);
			counter++;
		}
		totalStraws += counter;
		counter = 0;
		strawrun = 0;

	}
	double meanStrawsPerUnitLength = totalStraws / maxIterations;

	cout << "Mean straws per iteration: "
		<< setprecision(14) << fixed<< meanStrawsPerUnitLength << endl;

	cout << "Base of natural logarithm: " <<
		setprecision(14) << fixed << exp(1) << endl;

	//Wait for key press
	{
#ifdef _WIN32
		cout << endl;
		system("pause");
#else
		cout << endl << "Press ENTER to continue . . .";
		cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif
	}

    return 0;
}

