// SlowAnagrams.cpp

#include "stdafx.h"

using namespace std;

vector<string>* words;

template <typename T>
string Concat(vector<T>* set) {
	string c{};
	for (const auto& item : *set)
		c += item;
	return c;
}

template <typename T>
void Swap(vector<T>* set, int a, int b) {
	T tmp = set->at(a);
	set->at(a) = set->at(b);
	set->at(b) = tmp;
}

template <typename T>
void Permute(vector<T>* set, int level) {
	// Heap's Algorithm
	if (level == 0) {
		// At this point, set contains a new permutation
		words->push_back(Concat(set));
	}
	else {
		for (int i{ 0 }; i < level;++i) {
			Permute(set, level - 1);
			Swap(set, level % 2 == 1 ? 0 : i, level - 1);
		}
	}
}

int main()
{
	vector<string>* phrases = new vector<string>{
		"Stop", "Least","Traces", "Players", "Restrain","Mastering","Supersonic" };

	vector<string>* dict = new vector<string>();
	string line;
	ifstream inputFile("english_dictionary.txt");
	while (getline(inputFile, line))
		dict->push_back(line);

	words = new vector<string>();

	clock_t startTime{ clock() };
	for (auto& phrase : *phrases) {

		// Convert phrase to all lowercase
		transform(phrase.begin(), phrase.end(),
			phrase.begin(), ::tolower);

		// Create vector of individual characters
		vector<char> letters;
		for (auto s : phrase)
			letters.push_back(s);

		// Add all permutations of letters to words vector
		words->clear();
		Permute<char>(&letters, letters.size());

		// Remove duplicate permutations
		sort(words->begin(), words->end());
		auto last = unique(words->begin(), words->end());
		words->erase(last, words->end());

		// Display only words that are found in the dictionary
		for (const auto& word : *words) {
			if (binary_search(dict->begin(), dict->end(), word))
				cout << word << endl;
		}
		cout << endl;
	}
	clock_t stopTime{ clock() };

	double totalTime{ ((double)(stopTime - startTime)
		/ CLOCKS_PER_SEC) * 1000 };

	cout << "Total run time (ms): " << totalTime << endl;

	system("pause");
	return 0;
}

