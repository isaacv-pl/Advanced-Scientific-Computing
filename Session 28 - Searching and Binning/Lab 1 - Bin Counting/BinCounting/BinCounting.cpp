// BinCounting.cpp

#include "stdafx.h"

using namespace std;

vector<double>* items;

// Forward declarations for function signatures
void FindItemsSlow();
bool FindRangeSlow(double rangeLow, double rangeHigh, int& indexLow, int& indexHigh);

void FindItemsFast();
bool FindRangeFast(double rangeLow, double rangeHigh, int& indexLow, int& indexHigh);
int BinarySearch(double value, int indexLow, int indexHigh);


int main()
{
	items = new vector<double>(3000000, 0);

	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_real_distribution<double> distribution(0.0, 1.0);

	cout.imbue(std::locale(""));
	cout << "For " << items->size() << " items:" << endl;

	for (auto &item : *items)
		item = distribution(generator);

	sort(items->begin(), items->end());

	FindItemsSlow();

	FindItemsFast();

	delete items;

	system("pause");
	return 0;
}


void FindItemsSlow()
{
	int indexLow, indexHigh, itemsFound = 0;

	cout << "\nUsing sequential scan method:" << endl;

	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_real_distribution<double> distribution(0.0, 1.0);

	clock_t startTime{ clock() };

	for (int i = 0; i < 10; i++)
	{
		double rangeLow = distribution(generator) / 2;
		double rangeHigh = distribution(generator) / 2 + 0.5;

		bool found = FindRangeSlow(rangeLow, rangeHigh,
			indexLow, indexHigh);

		if (found) itemsFound += (indexHigh - indexLow);
	}

	clock_t stopTime{ clock() };

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout << "\tItems found = " << itemsFound << endl;;
	cout << "Total run time (ms): " << totalTime << endl;
}


bool FindRangeSlow(double rangeLow, double rangeHigh,
	int& indexLow, int& indexHigh)
{
	for (int i{}; i < items->size(); ++i)
	{
		if (items->at(i) >= rangeLow)
		{
			indexLow = i;
			break;
		}
	}
	for (int i{1}; i < items->size(); ++i)
	{
		if (items->at(items->size() - i) < rangeHigh)
		{
			indexHigh = items->size() - i+1;
			break;
		}
	}

	return true;
}


void FindItemsFast()
{
	int indexLow, indexHigh, itemsFound = 0;

	cout << "\nUsing binary search method:" << endl;

	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_real_distribution<double> distribution(0.0, 1.0);

	clock_t startTime{ clock() };

	for (int i = 0; i < 10; i++)
	{
		double rangeLow = distribution(generator) / 2;
		double rangeHigh = distribution(generator) / 2 + 0.5;

		bool found = FindRangeFast(rangeLow, rangeHigh,
			indexLow, indexHigh);

		if (found) itemsFound += (indexHigh - indexLow);
	}

	clock_t stopTime{ clock() };

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout << "\tItems found = " << itemsFound << endl;;
	cout << "Total run time (ms): " << totalTime << endl;
}


bool FindRangeFast(double rangeLow, double rangeHigh, int& indexLow, int& indexHigh)
{
	indexLow = BinarySearch(rangeLow, 0, items->size() - 1);

	indexHigh = BinarySearch(rangeHigh, 0, items->size() - 1);

	if (indexLow == -1) return false;

	return true;
}


int BinarySearch(double value, int indexLow, int indexHigh)
{
	if (indexLow <= indexHigh)
	{
		int indexMiddle = (indexHigh + indexLow) / 2;
		if (items->at(indexMiddle) > value)
			return BinarySearch(value, indexLow, indexMiddle - 1);
		else
			return BinarySearch(value, indexMiddle + 1, indexHigh);
	}
	return indexLow;
}


