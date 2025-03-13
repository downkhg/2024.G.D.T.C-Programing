#include <iostream>
using namespace std;
class Cookie
{
	int m_nFlour;
	int m_nSugar;
public:
	Cookie(int flour = 0, int sugar = 0)
	{
		cout << "Cookie:" << this << endl;
		m_nFlour = flour;
		m_nSugar = sugar;
	}
	//소멸자는 객체를 해제할때 사용하므로 굳이 매개변수를 필요치않는다.
	//~Cookie(int flour, int sugar)
	~Cookie()
	{
		cout << "~Cookie:" << this << endl;
	}
};
class MilkCookie : public Cookie
{
	int m_nMilk;
public:
	MilkCookie(int flour, int sugar, int milk) //:Cookie(flour, sugar)
	{
		//Cookie(flour, sugar);
		cout << "MilkCookie:" << this << endl;
		m_nMilk = milk;
	}
	~MilkCookie()
	{
		cout << "~MilkCookie:" << this << endl;
	}
};
class ChocoCookie : public Cookie
{
	int m_nChoco;
public:
	ChocoCookie(int flour, int sugar, int choco) :Cookie(flour, sugar)
	{
		cout << "ChocoCookie:" << this << endl;
		m_nChoco = choco;
	}
	~ChocoCookie()
	{
		cout << "~ChocoCookie:" << this << endl;
	}
};
class AmondChocoCookie : public ChocoCookie
{
	int m_nAmond;
public:
	AmondChocoCookie(int flour, int sugar, int choco, int amond) :ChocoCookie(flour, sugar, choco)
	{
		cout << "AmondChocoCookie:" << this << endl;
		m_nAmond = amond;
	}
	~AmondChocoCookie()
	{
		cout << "~AmondChocoCookie:" << this << endl;
	}
};

//상속받은 자식객체가 생성될때, 부모부분이 먼저생성되고, 자식부분을 호출하여 메모리가 생성된다.
//상속받은 객체가 생성될때 부모의 생성자가 호출되고 자식의 생성자가 호출되어 각각의 메모리부분이 생성된다.
//상속을 받은 객체가 삭제될때 자식의 부분부터 삭제되고, 부모의 부분이 삭제된다.
//상속에 따른 생성자와 소멸자의 호출순서를 통해, 정적할당된 메모리의 구조는 스택에 할당되는 것을 알수있다.
int main()
{
	Cookie cCookie(100, 20);//FB48
	MilkCookie cMilkCookie(100, 20, 20);//FB68
	ChocoCookie cChocoCookie(100, 20, 50);//FB98
	//MilkCookie cMilkCookie[10]{ MilkCookie(100,20,10), };
	//쿠키틀을 10개 준비한다.
	//MilkCookie** pMilkCookies = new MilkCookie * [10];
	//밀크쿠키의 제료를 준비하여 한개씩 틀에 채워넣는다.
	//for (int i = 0; i < 10; i++)
	//{
	//	pMilkCookies[i] = new MilkCookie(100, 20, 20);
	//	//delete pMilkCookies[i];
	//}

	//for (int i = 0; i < 10; i++)
	//{
	//	delete pMilkCookies[i];
	//}
	//delete[] pMilkCookies;

	AmondChocoCookie cAmondChcoCookie(100, 20, 50, 10);//FBC8
}