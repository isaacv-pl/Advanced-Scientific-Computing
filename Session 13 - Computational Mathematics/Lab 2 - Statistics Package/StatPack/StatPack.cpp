// StatPack.cpp

#include "stdafx.h"

using namespace std;

int main()
{
	vector<int> data(10000);
	vector<int> freq(101);
	double total{};
	int size = 10000;
	int counter{};
	int flagy{};
	int countoo{};
	double med1{};
	double med2{};
	int flag{1};
	int mode{};
	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_int_distribution<int> distribution(0, 100);

	// Populate vector with random integers
	for (auto &item : data)
		item = distribution(generator);

	// Sort the vector from lowest to highest
	sort(data.begin(), data.end());

	cout.imbue(std::locale(""));
	cout << "Sample Size = "
		<< data.size() << endl;

	// Calculate the mean
	double mean{};
	for (int i{};i<size;++i)
		total += data.at(i);
	mean = total / (double)size;
	cout << "Mean = " << mean << endl;

	// Calculate the variance
	double variance{};
	for (int j{};j<size;++j)
		variance += pow(mean - data.at(j), 2);
	variance /= size;
	cout << "Variance = " << variance << endl;
	cout << "Standard Deviation = " << sqrt(variance) << endl;

	// Calculate the median
	double median{};
	if (size % 2 == 0)
		median = data[size / 2];
	else
	{
		med1 = floor(size / 2);
		med2 = med1 + 1;
		median=(data.at(med1) + data.at(med2))/2;
	}
	cout << "Median = " << median << endl;

	// Calculate the mode
	for (int z{}; z < size; ++z)
	{
		freq.at(data.at(z))++;
		if (freq.at(data.at(z)) >= flag )
		{
			flag = freq.at(data.at(z));
			mode = data.at(z);
		}
	}
	
	for (int y{}; y < 101; ++y)
	{
		if (freq.at(y) == flag)
		{

			if (countoo == size)
				flagy = 1;
			countoo++;
		}
	}
	if (flagy == 1)
		cout << "There is no mode" << endl;
	else
	{
		for (int y{}; y < 101; ++y)
		{
			if (freq.at(y) == flag)
			{
				if (counter == 0)
					cout << "The mode is/are " << y;
				else if (counter != 0)
					cout << ", " << y;
				counter++;
					
			}
		}

	}
	cout << "." << endl;
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

