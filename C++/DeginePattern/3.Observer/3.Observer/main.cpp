#include <iostream>
#include <vector>

using namespace std;

class CUnit; //Ŭ������ ����: �̷� Ŭ������ �ִٴ� ���� �˸�.

//�߻�Ŭ����: ���������Լ��� 1���̻� �����Լ�
class CUnit //Ŭ������ ����
{
public:
	float x;
	float y;

	virtual void Move(float x, float y) //Ŭ�����Լ��� ����
	{
		cout << typeid(*this).name() <<".Move(" << x << "," << y << ")" << endl;
	}
	virtual void Attack(CUnit* unit) = 0 //���������Լ�
	{
		cout << typeid(*this).name() << "Attack:" << typeid(*unit).name() << endl;
	}
};

class CCommonder
{
	vector<class CUnit*> listUnit; //Ŭ������ ������ ����
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
		//for (int i = 0; i < listUnit.size(); i++) delete listUnit[i]; //�ش����� �����Ҵ���������� ���������.
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
//�߻�ȭ: �߻�Ŭ������ ��ӹ޾� ������ ��밡���� Ŭ������ ����� ��.
class CMarin : public CUnit
{
public:
	void Attack(CUnit* unit)
	{
		cout << "Attack:" << typeid(*unit).name() << endl;
	}
	//virtual void Move(float x, float y) override //Ŭ�����Լ��� ����
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
	void Attack(CUnit* unit) //override //��������� ��������� ǥ���ϸ� ����.
	{
		Move(unit->x, unit->y);
	}
	//virtual void Move(float x, float y) override//Ŭ�����Լ��� ����
	//{
	//	cout << typeid(*this).name() << ".Move(" << x << "," << y << ")" << endl;
	//}
};
//����: ����ڰ� �Ǽ������ʰ� ����°�.
//0. Ŭ������ �ݵ�� �����ؾ��ϴ°�?
//1. Ŭ������ ������ ���� Ȱ���ϴ°�?
//2. Ŭ������ �����Ҷ� �����͸� ����ؾ��ϴ� ������?
//3. �����̶�� ��ü�� ������Ű���������� ��� �ؾ��ϴ°�?
//4. ���α׷��� ����ɶ� ������ �����ؾ��ϴ°�?
//5. ������ �����Ҷ� � stl�� ����ϴ� ���� ������?
//6. �߻�Ŭ������ �����ΰ�?
//7. �������� Ȱ���Ͽ� ������ �����ϴ� ������ �����ΰ�?
void main()
{
	CCommonder cCommander;
	//CUnit cUnit;//���������Լ��� ��üȭ �� �� ����.
	CMarin cMarin[3];
	CMedic cMedic[2];

	for(int i = 0 ;  i < 3; i++)
		cCommander.SelectUnit(&cMarin[i]);
	for (int i = 0; i < 2; i++)
		cCommander.SelectUnit(&cMedic[i]);

	cCommander.Move(10, 10);
	cCommander.Attack(&cMarin[2]);
}