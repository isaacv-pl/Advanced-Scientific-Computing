// JengaCantilever.cpp

#include "stdafx.h"
#include "Blocks.h"

using namespace std;

int main()
{
	BlockList* blocks = new BlockList();

	blocks->AddBlock(7.5, 1.5);
	blocks->AddBlock(1.5, 10.5);
	blocks->AddBlock(10.5, 4.5);
	
	blocks->CalcCenter();

	while (blocks->center->x < 15) {
		blocks->MoveBlocks(3, 3);
		blocks->AddBlock(7.5, 1.5);
		blocks->AddBlock(1.5, 10.5);
		blocks->CalcCenter();
	}

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

