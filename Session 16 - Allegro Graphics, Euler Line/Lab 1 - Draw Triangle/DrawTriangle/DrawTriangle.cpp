
// DrawTriangle.cpp

#include "stdafx.h"
#include "SimpleScreen.h"

using namespace std;

int main()
{
	seed_seq seed{ 2016 };
	default_random_engine generator{ seed };
	uniform_real_distribution<double> da(-5.0, 0);
	uniform_real_distribution<double> db(5.0, 10.0);
	uniform_real_distribution<double> dc(0, 5.0);
	uniform_real_distribution<double> dd(-5.0, 0);//A redefined
	uniform_real_distribution<double> de(5.0, 10.0);//B redefined
	SimpleScreen ss;
	ss.SetWorldRect(-5, -5, 10, 10);
	ss.DrawAxes();
	double pointa = -3;// da(generator);
	double pointb = 8;//db(generator);
	double pointc = 2;//dc(generator);
	double pointd = -2;//dd(generator);
	double pointe = 9;//de(generator);
	
	PointSet ps;
	ps.add(pointa, pointd);//A
	ps.add(pointb, pointa);//B
	ps.add(pointc, pointe);//C
	PointSet CentroidB;
	CentroidB.add((pointa + pointc) / 2, (pointd + pointe) / 2);
	CentroidB.add(pointb, pointa);
	PointSet CentroidPointB;
	CentroidPointB.add((pointa + pointc) / 2, (pointd + pointe) / 2);
	CentroidPointB.add((pointa + pointc) / 2, (pointd + pointe) / 2+.01);
	PointSet CentroidA;
	CentroidA.add((pointb + pointc) / 2, (pointa + pointe) / 2);
	CentroidA.add(pointa, pointd);
	PointSet CentroidPointA;
	CentroidPointA.add((pointb + pointc) / 2, (pointa + pointe) / 2);
	CentroidPointA.add((pointb + pointc) / 2, (pointa + pointe) / 2+.01);
	PointSet CentroidC;
	CentroidC.add((pointa + pointb) / 2, (pointd+pointa)/2);
	CentroidC.add(pointc, pointe);
	PointSet CentroidPointC;
	CentroidPointC.add((pointa + pointb) / 2, (pointd + pointa) / 2);
	CentroidPointC.add((pointa + pointb) / 2, (pointd + pointa) / 2+.01);
	double orthoslopeC{};
	double orthpa{};
	double orthpb{};
	double sumc{};
	double y{};
	double y1{};
	orthoslopeC = 1/(((pointa - pointd) / (pointb - pointa))*-1);
	double slopeAB = ((pointa - pointd) / (pointb - pointa));
	sumc = pointe - pointd;
	double newslope = (orthoslopeC / slopeAB)-1;
	double newconstant = (((sumc / slopeAB) + (newslope+1)*pointc*-1) + pointa)*-1;
	orthpb=newconstant / newslope;
	orthpa = orthoslopeC*(orthpb - pointc) + pointe;

	PointSet OrthocenterC;
	OrthocenterC.add(orthpb, orthpa);
	OrthocenterC.add(pointc, pointe);
	PointSet OrthopointC;
	OrthopointC.add(orthpb, orthpa);
	OrthopointC.add(orthpb, orthpa+.01);
	double orthoslopeB{};
	double orthpc{};
	double orthpd{};
	orthoslopeB = 1 / (((pointe - pointd) / (pointc - pointa))*-1);
	double slopeAC = (pointe - pointd) / (pointc - pointa);
	sumc = pointa - pointe;
	newslope = (orthoslopeB / slopeAC) - 1;
	newconstant = (((sumc / slopeAC) + (newslope + 1)*pointb*-1) + pointc)*-1;
	orthpd = newconstant / newslope;
	orthpc = orthoslopeB*(orthpd - pointb) + pointa;

	PointSet OrthocenterB;
	OrthocenterB.add(orthpd, orthpc);
	OrthocenterB.add(pointb, pointa);
	PointSet OrthopointB;
	OrthopointB.add(orthpd, orthpc);
	OrthopointB.add(orthpd, orthpc+.01);
	double orthoslopeA{};
	double orthpe{};
	double orthpf{};
	orthoslopeA= 1 / (((pointa-pointe)/(pointb-pointc))*-1);
	double slopeBC = (pointa - pointe) / (pointb - pointc);
	sumc = pointd - pointa;
	newslope = (orthoslopeA / slopeBC) - 1;
	newconstant = (((sumc / slopeBC) + (newslope + 1)*pointa*-1) + pointb)*-1;
	orthpf = newconstant / newslope;
	orthpe = orthoslopeA*(orthpf - pointa) + pointd;

	PointSet OrthocenterA;
	OrthocenterA.add(pointa, pointd);
	OrthocenterA.add(orthpf, orthpe);
	PointSet OrthopointA;
	OrthopointA.add(orthpf, orthpe);
	OrthopointA.add(orthpf, orthpe+.01);

	//circumcenter portion
	newslope = orthoslopeC-slopeBC;
	newconstant = (slopeBC*pointc*-1) + pointe - ((orthoslopeC*(pointa + pointb) / 2 * -1) + ((pointa + pointd) / 2));
	double circumA = newconstant / newslope;
	double circumB = slopeBC*(circumA - pointc) + pointe;

	PointSet CircumcenterAB;
	CircumcenterAB.add((pointa + pointb) / 2, (pointd + pointa) / 2);
	CircumcenterAB.add(circumA, circumB);

	newslope = orthoslopeA - slopeAC;
	newconstant = (slopeAC*pointc*-1) + pointe - ((orthoslopeA*((pointb + pointc) / 2) * -1) + ((pointa + pointe) / 2));
	circumA = newconstant / newslope;
	circumB = slopeAC*(circumA - pointc) + pointe;

	PointSet CircumcenterBC;
	CircumcenterBC.add((pointb + pointc) / 2, (pointa + pointe) / 2);
	CircumcenterBC.add(circumA, circumB);

	newslope = orthoslopeB - slopeBC;
	newconstant = (slopeBC*pointc*-1) + pointe - (((orthoslopeB*(pointa + pointc) / 2)*-1) + ((pointd + pointe) / 2));
	circumA = newconstant / newslope;
	circumB = slopeBC*(circumA - pointc) + pointe;

	PointSet CircumcenterAC;
	CircumcenterAC.add((pointa + pointc) / 2, (pointd + pointe) / 2);
	CircumcenterAC.add(circumA, circumB);
	newslope = orthoslopeC - orthoslopeA;
	newconstant = (orthoslopeA*pointa*-1) + pointd - ((orthoslopeC*pointc*-1) + pointc);
	double orthcenterA = newconstant / newslope;
	double orthcenterB = orthoslopeC*(orthcenterA - pointc) + pointe;
	PointSet LastA;
	LastA.add((pointa + orthcenterA) / 2, (pointb + orthcenterB) / 2);
	LastA.add((pointa + orthcenterA) / 2, (pointb + orthcenterB) / 2+.01);
	PointSet LastB;
	LastB.add((pointb + orthcenterA) / 2, (pointa + orthcenterB) / 2);
	LastB.add((pointb + orthcenterA) / 2, (pointa + orthcenterB) / 2+.01);
	PointSet LastC;
	LastC.add((pointc + orthcenterA) / 2, (pointe + orthcenterB) / 2);
	LastB.add((pointb + orthcenterA) / 2, (pointa + orthcenterB) / 2+.01);


	//order in which you declare points is the order in which the computer will draw the lines.
	ss.DrawLines(ps, "tvblack",5,true,false);
	ss.DrawLines(CentroidC, "orange", 5, false , false);
	ss.DrawLines(CentroidA, "orange", 5, false, false);
	ss.DrawLines(CentroidB, "orange", 5, false, false);
	ss.DrawLines(OrthocenterC, "midnightblue", 5, false , false);
	ss.DrawLines(OrthocenterB, "midnightblue", 5, false, false);
	ss.DrawLines(OrthocenterA, "midnightblue", 5, false, false);
	ss.DrawLines(CircumcenterAB, "green", 5, false, false);
	ss.DrawLines(CircumcenterBC, "green", 5, false, false);
	ss.DrawLines(CircumcenterAC, "green", 5, false, false);
	ss.DrawLines(CentroidPointA, "red", 20, false, false);
	ss.DrawLines(CentroidPointB, "red", 20, false, false);
	ss.DrawLines(CentroidPointC, "red", 20, false, false);
	ss.DrawLines(OrthopointA, "red", 20, false, false);
	ss.DrawLines(OrthopointB, "red", 20, false, false);
	ss.DrawLines(OrthopointC, "red", 20, false, false);
	ss.DrawLines(LastA, "red", 20, true, true);
	ss.DrawLines(LastB, "red",20, true, true);
	ss.DrawLines(LastC, "red", 20, true, true);
	//pointset alias, color name, width ,(closed boolean), (paint fill boolean)
	system("pause");
	return 0;
}

