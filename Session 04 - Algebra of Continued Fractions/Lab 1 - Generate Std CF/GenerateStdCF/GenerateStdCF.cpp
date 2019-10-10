// GenerateStdCF.cpp
//

#include "stdafx.h"

using namespace std;

int main()
{
	// The Golden Ratio = 1.6180339887...
	double x = (1 + sqrt(5)) / 2;

	//double x = sqrt(113);

	const int maxTerms = 20;

	cout << "To " << maxTerms << " terms, ";
	cout << "the standard continued fraction for ";
	cout << setprecision(14) << x << " is:\n" << endl;

	cout << "{" << setprecision(0) << fixed << floor(x) << ",";
	x -= floor(x);
	for (int i{ 0 }; i < 20; ++i)
	{	
		x=(1/x)-(floor(1 / x));
		if (i < 19)
			cout << x << ",";
		else
			cout << x;
	}

	cout << "}" << endl;

#ifdef _WIN32
	cout << endl;
	system("pause");
#else
	cout << endl << "Press ENTER to continue . . .";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif

    return 0;
}

