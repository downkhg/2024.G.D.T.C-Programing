#include <iostream>
#include <vector>

using namespace std;

class CUnit; //클래스의 선언: 이런 클래스가 있다는 것을 알림.

//추상클래스: 순수가상함수를 1개이상 가진함수
class CUnit //클래스의 정의
{
public:
	float x;
	float y;

	virtual void Move(float x, float y) //클래스함수의 정의
	{
		cout << typeid(*this).name() <<".Move(" << x << "," << y << ")" << endl;
	}
	virtual void Attack(CUnit* unit) = 0 //순수가상함수
	{
		cout << typeid(*this).name() << "Attack:" << typeid(*unit).name() << endl;
	}
};

class CCommonder
{
	vector<class CUnit*> listUnit; //클래스의 선언을 포함
	int idx = 0;
	//vector<CUnit*> listUnit;
public:
	CCommonder(int listSize = 12)
	{
		cout << typeid(*this).name() << endl;
		listUnit.resize(listSize);
	}
	~CCommonder()
	{
		cout << "~" << typeid(*this).name() << endl;
		//for (int i = 0; i < listUnit.size(); i++) delete listUnit[i]; //해당대상이 동적할당되지않으면 지울수없음.
		listUnit.clear();
	}
	void SelectUnit(CUnit* unit)
	{
		//listUnit.push_back(unit);
		listUnit[idx] = unit;
		idx++;
	}
	void DeselectUnit(CUnit* unit)
	{
		auto it = find(listUnit.begin(), listUnit.end(), unit);
		listUnit.erase(it);
		idx--;
	}
	CUnit* ChoiceUnit(CUnit* unit)
	{
		auto it = find(listUnit.begin(), listUnit.end(), unit);
		return *it;
	}
	CUnit* ChoiceUnit(int idx)
	{
		return listUnit[idx];
	}
	void Move(float x, float y)
	{
		for (int i = 0; i < idx; i++)
			listUnit[i]->Move(x, y);
	}
	void Attack(CUnit* unit)
	{
		for (int i = 0; i < idx; i++)
			listUnit[i]->Attack(unit);
	}
};
//추상화: 추상클래스를 상속받아 실제로 사용가능한 클래스를 만드는 것.
class CMarin : public CUnit
{
public:
	void Attack(CUnit* unit)
	{
		cout << "Attack:" << typeid(*unit).name() << endl;
	}
	//virtual void Move(float x, float y) override //클래스함수의 정의
	//{
	//	cout << typeid(*this).name() << ".Move(" << x << "," << y << ")" << endl;
	//}
};

class CMedic : public CUnit
{
public:
	void Heal(CUnit* unit)
	{
		cout << "Heal:" << typeid(*unit).name() << endl;
	}
	void Attack(CUnit* unit) //override //없어도되지만 명시적으로 표기하면 좋음.
	{
		Move(unit->x, unit->y);
	}
	//virtual void Move(float x, float y) override//클래스함수의 정의
	//{
	//	cout << typeid(*this).name() << ".Move(" << x << "," << y << ")" << endl;
	//}
};
//설계: 사용자가 실수하지않게 만드는것.
//0. 클래스를 반드시 설계해야하는가?
//1. 클래스의 선언은 언제 활용하는가?
//2. 클래스를 참조할때 포인터를 사용해야하는 이유는?
//3. 유닛이라는 객체를 생성시키지않으려면 어떻게 해야하는가?
//4. 프로그램이 종료될때 유닛을 삭제해야하는가?
//5. 유닛을 관리할때 어떤 stl을 사용하는 것이 좋은가?
//6. 추상클래스는 무엇인가?
//7. 다형성을 활용하여 유닛을 구현하는 이유는 무엇인가?
void main()
{
	CCommonder cCommander;
	//CUnit cUnit;//순수가상함수는 객체화 할 수 없다.
	CMarin cMarin[3];
	CMedic cMedic[2];

	for(int i = 0 ;  i < 3; i++)
		cCommander.SelectUnit(&cMarin[i]);
	for (int i = 0; i < 2; i++)
		cCommander.SelectUnit(&cMedic[i]);

	cCommander.Move(10, 10);
	cCommander.Attack(&cMarin[2]);
}