/// StartGap2.cpp

#include "stdafx.h"

using namespace std;


int main()
{
	int n{ 10000000};
	bool showOutput{ false };

	cout.imbue(std::locale(""));

	cout << "Finding all representations of"
		<< " 4xy + x + y <= " << n << endl;

	int reps{};

	clock_t startTime{ clock() };

	for (int i{ 1 }; i <= n; ++i) {
		bool represented = false;
		int yMax = floor((sqrt(16 * i + 4) - 2) / 8.0);
		for (int y{ 1 }; y <= yMax;++y) {
			double x = (double)(i - y) / (4 * y + 1);
			if (trunc(x)==x) {
				if (!represented) {
					reps++;
					if (showOutput)
						cout << i << "\t";
					represented = true;
				}
				if (showOutput) {
					cout << "(" << floor(x) << ", ";
					cout << y << ")  ";
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

