// StartGap.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

using namespace std;


int main()
{
	int n{ 100};
	bool showOutput{ true };

	cout.imbue(std::locale(""));

	cout << "Finding all representations of"
		<< " 4xy + x + y <= " << n << endl;

	int reps{};

	clock_t startTime{ clock() };

	for (int i{ 1 }; i <= n; ++i) 
	{
		bool represented = false;
		for (int y{ 1 }; y <= i;++y) 
		{
			for (int x{ y }; x <= i;++x) 
			{
				if (4 * x * y + x + y == i) 
				{
					if (!represented) 
					{
						reps++;
						if (showOutput)
							cout << i << "\t";
						represented = true;
					}
					if (showOutput) {
						cout << "(" << x << ", ";
						cout << y << ")  ";
					}
				}
			}
		}
		if (showOutput && represented) cout << endl;
	}

	clock_t stopTime{ clock() };

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout << "Total run time (ms): " << totalTime << endl;

	cout << "Total representations = " << reps << endl;

	cout << "Probability of a representation = "
		<< (double)reps / n << endl;


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

