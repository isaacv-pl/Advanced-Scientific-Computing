// Blocks.h

#pragma once

using namespace std;

class Point2D
{
public:
	Point2D();
	Point2D(double x, double y);
	~Point2D();
	double x, y;
};

class Block
{
public:
	Block();
	Block(const Block &rhs);
	Block(double x, double y);
	~Block();
	void Move(double deltaX, double deltaY);
	Point2D* center;
};

class BlockList
{
public:
	BlockList();
	~BlockList();
	int Count();
	void CalcCenter();
	void AddBlock(double x, double y);
	void MoveBlocks(double deltaX, double deltaY);
	Point2D* center;
private:
	vector<Block>* blocks;
};
