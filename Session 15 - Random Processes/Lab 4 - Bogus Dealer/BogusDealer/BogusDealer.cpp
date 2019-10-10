// BogusDealer.cpp

#include "stdafx.h"

using namespace std;

void InitDeck(vector<int>& deck);
void DealCards(vector<int>& deck);
void DisplayCards(vector<int>& deck);

int main()
{
	vector<int> deck(52);

	InitDeck(deck);

	DealCards(deck);

	DisplayCards(deck);

	system("pause");
	return 0;
}

void InitDeck(vector<int>& deck)
{
	for (int i{}; i < deck.size(); ++i)
		deck.at(i) = i;
}


void DealCards(vector<int>& deck)
{
	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_int_distribution<int> distribution(0, 51);

	for (auto& card : deck)
		card = distribution(generator);
}

void DisplayCards(vector<int>& deck)
{
	const vector<string> suit{ "Clubs", "Diamonds",
		"Hearts", "Spades" };

	const vector<string> rank{ "Deuce", "Three", "Four",
		"Five", "Six", "Seven",
		"Eight", "Nine", "Ten",
		"Jack", "Queen", "King",
		"Ace" };

	for (int i{}; i < deck.size();++i) {
		int card = deck.at(i);
		cout << "Card in position " << i
			<< " is the " << rank.at(card % 13)
			<< " of " << suit.at(card / 13) << endl;
	}
}