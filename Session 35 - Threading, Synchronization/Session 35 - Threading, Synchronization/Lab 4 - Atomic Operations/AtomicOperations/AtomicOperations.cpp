// AtomicOperations.cpp
#include "stdafx.h"
using namespace std;

atomic<int> g_counter{};
//mutex g_counter_mutex;

void func() {
	for (int i{}; i < 100000; ++i)
	{
		//lock_guard<mutex> lock(g_counter_mutex);
		g_counter++;
	}
}

int main() {
	for (int i{}; i < 10; ++i) {
		g_counter = 0;
		vector<thread> threadPool;
		for (size_t t{}; t < 20;++t)
			threadPool.push_back(
				thread(func));
		for (auto& t : threadPool)
			t.join();
		cout << g_counter << endl;
	}
	system("pause");
	return 0;
}
