// FactorQuad.cpp

#include "stdafx.h"


using namespace std;

int main()
{
	int J{ 115425 };
	int K{ 3254121 };
	int L{ 379020 };
	int a{0};
	int b{0};
	int c{0};
	int d{0};
	int flag{};
	

	std::cout << "Given the quadratic:" << endl
		<< J << "x^2 + " << K << "x + " << L
		<< " = 0" << endl << endl
		<< "The factors are:"
		<< endl << endl;

	for (int i{ 1 }; i <= J; ++i)
	{
		if (J%i == 0)
		{
			a = i;
			c = J/a;
			for (int j{ 1 }; j <= L; ++j)
			{
				if (L%j == 0)
				{
					b = j;
					d = L/b;
					if (K == a*d + b*c)
					{
						std::cout << "(" << a << "x + " << b << ") (" << c << "x + " << d << ")" << endl << endl;
						flag = 1;
					}
				}
			}
		}
		if (flag == 0&& i == J)
			std::cout << "You have a prime polynomial. Stop it."<< endl;
	}

#ifdef _WIN32
		std::cout << endl;
		std::system("pause");
#else
		cout << endl << "Press ENTER to continue . . .";
		cin.ignore(numeric_limits<streamsize>::max(), '\n');
#endif


	return 0;
}

