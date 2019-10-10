// ListCards.cpp

#include "stdafx.h"

using namespace std;

void InitDeck(vector<int>& deck);
void DisplayCards(vector<int>& deck);

int main()
{
	vector<int> deck(52);

	InitDeck(deck);

	DisplayCards(deck);

	system("pause");
	return 0;
}

void InitDeck(vector<int>& deck)// in order to change a stack based variable you must use a reference "&"
{
	for (int i{}; i < deck.size(); ++i)
		deck.at(i) = i;
}

void DisplayCards(vector<int>& deck)// should have used const in order to say that all that is happening is reading the data; no modification.
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