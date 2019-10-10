// FastAnagrams.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "FastAnagrams.h"

using namespace std;

vector<Anagram>* anagrams;

int main()
{
	vector<string>* phrases = new vector<string>{
		"Stop", "Least", "Traces", "Players", "Restrain",
		"Mastering", "Supersonic" };

	anagrams = new vector<Anagram>();

	string line;
	ifstream inputFile("english_dictionary.txt");
	while (getline(inputFile, line))
		anagrams->push_back(Anagram(line));

	// Sort anagrams by the sorted letters
	sort(anagrams->begin(), anagrams->end(),
		[](const Anagram& a, const Anagram& b) ->bool
	{
		return a.letters < b.letters;
	});

	clock_t startTime{ clock() };

	for (const auto& phrase : *phrases) {
		Anagram input{ phrase };
		auto lower = lower_bound(anagrams->begin(), anagrams->end(), input.letters,
			[](const Anagram& a, const Anagram& b) ->bool
		{
			return a.letters < b.letters;
		});
		auto upper = upper_bound(anagrams->begin(), anagrams->end(), input.letters,
			[](const Anagram& a, const Anagram& b) ->bool
		{
			return a.letters < b.letters;
		});
		for (auto i{ lower }; i != upper;++i)
			cout << i->word << endl;
		cout << endl;
	}

	clock_t stopTime{ clock() };

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout << "Total run time (ms): " << totalTime << endl;

	system("pause");
	return 0;
}

Anagram::Anagram() {
}

Anagram::Anagram(string word) {
	transform(word.begin(), word.end(),
		word.begin(), ::tolower);
	this->word = word;
	sort(word.begin(), word.end());
	this->letters = word;
}

Anagram::~Anagram() {

}

