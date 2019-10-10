// Blocks.cpp

#include "stdafx.h"
#include "Blocks.h"

using namespace std;

Point2D::Point2D()
{
	x = 0;
	y = 0;
}

Point2D::Point2D(double x, double y)
{
	this->x = x;
	this->y = y;
}

Point2D::~Point2D()
{
	// Empty destructor
}


Block::Block()
{
	center = nullptr;
}

Block::Block(const Block &rhs)
{
	center = new Point2D(rhs.center->x, rhs.center->y);
}

Block::Block(double x, double y)
{
	center = new Point2D(x, y);
}

Block::~Block()
{
	delete center;
}

void Block::Move(double deltaX, double deltaY)
{
	center->x += deltaX;
	center->y += deltaY;
}


BlockList::BlockList()
{
	blocks = new vector<Block>();
	center = new Point2D();
}

BlockList::~BlockList()
{
	blocks->clear();
	delete blocks;
	delete center;
}

int BlockList::Count()
{
	return blocks->size();
}

void BlockList::AddBlock(double x, double y)
{
	blocks->push_back(Block(x, y));
}

void BlockList::MoveBlocks(double deltaX, double deltaY)
{
	for (auto &block : *blocks)
		block.Move(deltaX, deltaY);
}

void BlockList::CalcCenter()
{
	center->x = 0;
	center->y = 0;
	for (const auto &block : *blocks) {
		center->x += block.center->x;
		center->y += block.center->y;
	}
	center->x /= blocks->size();
	center->y /= blocks->size();

	cout << "Blocks: ";
	cout << setw(3) << blocks->size();
	cout << "\tCenter X:";
	cout << setw(7) << setprecision(2) << fixed;
	cout << center->x;
	cout << "\tCenter Y:";
	cout << setw(7) << setprecision(2) << fixed;
	cout << center->y;
	cout << endl;
}





