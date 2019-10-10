// RaceCondition.cpp

#include "stdafx.h"

using namespace std;

void DisplayThreadId() {
	cout << " {threadid = "
		<< this_thread::get_id()
		<< "}" << endl;
}

void func(string msg, string symbol, int count) {
	cout << msg << " enter func()";
	DisplayThreadId();
	for (int i{}; i < count; ++i)
		cout << symbol;
	cout << endl << msg << "exit func()";
	DisplayThreadId();
}

int main() {
	cout << "enter main()";
	DisplayThreadId();
	thread t1(func, "Thread 1", "a", 10);
	thread t2(func, "Thread 2", "b", 20);
	t1.join();
	t2.join();
	cout << "exit main()";
	DisplayThreadId();
	system("pause");
	return 0;
}