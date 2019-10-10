// ParaIntegration.cpp

#include "stdafx.h"
using namespace std;

struct TCB {
	thread* t = nullptr;
	double a{};
	double b{};
	int intervals{};
	double integral{};
};

inline double f(double x) {
	return sqrt(1 - pow(x, 2));
}

void simpsons(TCB* tcb)
{
	double sum = f(tcb->b) + f(tcb->a);
	double deltaX = (tcb->b - tcb->a) / tcb->intervals;
	tcb->a += deltaX;
	for (int i{ 1 };i < tcb->intervals;++i) {
		int coeff = 2 * (i % 2 + 1);
		sum += coeff*f(tcb->a);
		tcb->a += deltaX;
	}
	tcb->integral = (deltaX / 3) * sum;
}

int main() {
	cout << setw(15) << "Integral"
		<< setw(10) << "Threads"
		<< setw(6) << "Time" << endl;
	for (int threads{ 1 }; threads <= 10; ++threads) {
		double width = 1.0 / threads;
		clock_t startTime{ clock() };
		vector<TCB*> threadPool;
		for (int tidx{ 0 }; tidx < threads; ++tidx) {
			TCB* tcb = new TCB();
			tcb->a = tidx * width;
			tcb->b = tcb->a + width;
			tcb->intervals = 100000000 / threads;
			tcb->t = new thread(simpsons, tcb);
			threadPool.push_back(tcb);
		}
		double integral{};
		for (auto& tcb : threadPool) {
			tcb->t->join();
			integral += tcb->integral;
			delete tcb->t;
		}
		clock_t stopTime{ clock() };
		threadPool.clear();
		double totalTime{ ((double)(stopTime - startTime)
			/ CLOCKS_PER_SEC) * 1000 };
		cout << setw(15) << setprecision(11)
			<< integral * 4.0
			<< setw(10) << threads
			<< setw(6) << totalTime << endl;
	}
	system("pause");
	return 0;
}
