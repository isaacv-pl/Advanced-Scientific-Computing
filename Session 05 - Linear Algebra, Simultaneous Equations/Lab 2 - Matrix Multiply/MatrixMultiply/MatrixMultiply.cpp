// MatrixMultiply.cpp : Defines the entry point for the console application.

#include "stdafx.h"

using namespace std;

void Multiply(int a[2][3], int b[3][2],
	int c[2][2]);

int main()
{
	int a[2][3]{ {4,5,8},{1,9,7} };
	int b[3][2]{ { 2,4},{6,1},{5,9} };

	int c[2][2];

	Multiply(a, b, c);

	for (int i{}; i < 2; ++i) {
		for (int j{}; j < 2; ++j)
			cout << setw(6) << c[i][j];
		cout << endl;
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

void Multiply(int a[2][3], int b[3][2],
	int c[2][2])
{
	
	 for(int i{}; i<2; ++i)
	{
		for(int j{}; j<2;++j)
		{
		int sum{};
			for(int v{}; v<3;++v)
			{
				sum+=a[i][v]*b[v][j];
			}
			c[i][j]=sum;
		}
	}
	
}