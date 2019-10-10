// FastAnagrams.h

#pragma once

#include "stdafx.h"

using std::string;

class Anagram {
public:
	Anagram();
	Anagram(string word);
	~Anagram();

	string word;
	string letters;
};



