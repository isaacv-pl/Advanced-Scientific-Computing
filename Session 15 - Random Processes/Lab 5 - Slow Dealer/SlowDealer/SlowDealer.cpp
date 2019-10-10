// SlowDealer.cpp

#include "stdafx.h"

using namespace std;

void InitDeck(vector<int>& deck);
void DealCards(vector<int>& deck);
void DisplayCards(vector<int>& deck);

seed_seq seed{ 2016 };
default_random_engine generator{ seed };
uniform_int_distribution<int> distribution(0, 51);

int main()
{
	vector<int> deck(52);
	vector<int> check(52);

	const int maxDeal{ 10000 };

	clock_t startTime{ clock() };

	for (int deal{};deal < maxDeal;++deal) {
		InitDeck(deck);
		DealCards(deck);
	}

	clock_t stopTime{ clock() };

	DisplayCards(deck);

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout.imbue(std::locale(""));
	cout << "Total deals: " << maxDeal << endl;
	cout << "Total run time (ms): " << totalTime << endl;

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
	// Fix this buggy algorithm; So we need a helper vector
	/*vector<bol> alreadyDeal (52, false);
	for(auto& card : deck) {
		card = distribution(generator);
		while(already Dealt.at(card){
			card= distribution(generator);
			}
		alreadyDealt.at(card)=true;
			*/
	int counter = 0;
	int flag{0};
	for (auto& card : deck)
	{
		card = distribution(generator);
		while (flag == 0)
		{
			for (int i{}; i < 52; ++i)
			{

				if (card == deck.at(i))
				{
					card = distribution(generator);
					i = 0;
				}
				if (i == 51 && card != deck.at(i))
				{
					flag = 1;
				}
				if (i + 1 == counter)
				{
					++i;
				}
			}
		}
		counter++;
	}
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