/*##################################
STL(�ڷᱸ�� ������)
���ϸ�: STL_empty.cpp
�ۼ��� : ��ȫ��(downkhg@gmail.com)
������������¥ : 2022.03.09
���� : 1.05
###################################*/
#include <iostream>
#include <vector>
#include <list>
#include <deque>
#include <queue>
#include <stack>
#include <map>
#include <set>
#include <string>
#include <unordered_map>//hash_map -> unordered_map: vs2019���� ����
using namespace std;
//����: �����迭
//0.�迭�� �����Ͱ� ����ɰ����� �̸� Ȯ���Ǿ��ִ�.
//1.�ε����� ���������� �����ϴ�.
//2.�� �ڷ�� �����Ϳ���(�ε���)�� ���� ����/���������� �����ϴ�.
//3.�迭�� ũ�⸦ ��Ÿ���߿� ���氡���ϴ�.
void VectorMain()
{
	vector<int> container(1);//�����̳ʻ����� ũ�⸦ ���������ϴ�.
	container[0] = 10;
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //�迭�� ũ�⸦ �����Ѵ�.
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	vector<int>::iterator it;
	cout << "PrintPtr(" << container.capacity() << "):";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//1.�߰�
	container.push_back(100);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//2.���� 
	it = container.begin(); //10
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	//ã��
	vector<int>::iterator itFind = find(container.begin(), container.end(), 100);
	cout << "itFind[" << &*itFind << "]" << *itFind << endl;
	container.insert(itFind, 50);

	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//3.����
	it = --container.end();
	it = container.begin() + 3;
	cout << "it[" << &*it << "]" << *it << endl;
	container.erase(it);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	// 4.��λ���
	container.clear(); //��λ���
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
}
//���Ḯ��Ʈ
//1.�����ʹ� �������ٸ� �����ϴ�.(����x)
//2.���Ḯ��Ʈ�� �߰�,����,������ O(1)�̴�.
//3.���Ḯ��Ʈ�� ����: ����, ȯ��, ���� stl�� ����Ʈ�� ��� �ش�Ǵ°�?
void ListMain()
{
	list<int> container(1);//�����̳ʻ����� ũ�⸦ ���������ϴ�.
	list<int>::iterator it;
	//container[0] = 10;
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	container.resize(3);
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//1.�߰�
	container.push_back(100);
	cout << "PrintPtr:";
	for (list<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//2.���� 
	it = container.begin(); //10
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	//ã��
	list<int>::iterator itFind = find(container.begin(), container.end(), 100);
	cout << "itFind[" << &*itFind << "]" << *itFind << endl;
	container.insert(itFind, 50);

	cout << "PrintPtr:";
	for (list<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//3.����
	it = --container.end();
	it = container.begin()++;
	cout << "it[" << &*it << "]" << *it << endl;
	container.erase(it);
	cout << "PrintPtr:";
	for (list<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	// 4.��λ���
	container.clear(); //��λ���
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
}
//��ũ: �յڷ� �ڷḦ �߰�/��������, �������ٰ���.
void DequeMain()
{
	deque<int> container(1);//�����̳ʻ����� ũ�⸦ ���������ϴ�.
	container[0] = 10;
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //�迭�� ũ�⸦ �����Ѵ�.
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	deque<int>::iterator it;
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//1.�߰�
	container.push_back(100);
	container.push_front(5);
	cout << "PrintPtr:";
	for (deque<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//2.���� 
	it = container.begin(); //10
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	//ã��
	deque<int>::iterator itFind = find(container.begin(), container.end(), 100);
	cout << "itFind[" << &*itFind << "]" << *itFind << endl;
	container.insert(itFind, 50);

	cout << "PrintPtr:";
	for (deque<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//3.����
	it = --container.end();
	it = container.begin() + 3;
	cout << "it[" << &*it << "]" << *it << endl;
	container.erase(it);
	cout << "PrintPtr:";
	for (deque<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	// 4.��λ���
	container.clear(); //��λ���
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
}
//����: �ڿ��� �߰��ǰ� �ڿ��� ����.
//����Լ����� ���� �Լ��� ȣ���Ҷ����� ���ÿ� ����.
//���ڿ������� -> ���ڹ迭 -> apple -> elppa
void StackMain()
{

}
//ť: �ڿ��� �߰��ϰ� �տ��� ����.
//�޼���ť: �̺�Ʈ�� �߻��� ������� �����ϴ� ����.
//�Էµ� ������� ��ɾ� ó���ϱ�
void QueueMain()
{

}
//�켱����ť: �켱������ ���� ���Ұ� ��������(��)
//�������� �����͸� �־����� � ������� �����Ͱ� �����°�? ū������ ���´�.
void PriorytyQueueMain()
{

}
//��: ���������� �����͸� ã�����ִ�.
//�ش翵��ܾ ������ �ѱ��� ����� ���´�.
void MapMain()
{
	map<string, string> mapDic;

	mapDic["test"] = "����";
	mapDic["pratice"] = "����";
	mapDic["try"] = "����";
	mapDic["note"] = "���";

	cout << mapDic["try"] << endl;
	cout << mapDic["note"] << endl;
}
//��: �������� �����͸� �ִ´�. �����ʹ� ������ ������� �����͸� ã�´�.
void SetMain()
{
	set<int> setData;

	setData.insert(10);
	setData.insert(20);
	setData.insert(30);
	setData.insert(40);

	set<int>::iterator it = setData.find(10);

	if (it != setData.end()) it;
	for (it = setData.begin(); it != setData.end(); it++)
		cout << *it << ",";
	cout << endl;
}
//�ؽø�: �ؽ����̺�
void HashMapMain()
{
	unordered_map<string, string> mapDic;

	mapDic["test"] = "����";
	mapDic["pratice"] = "����";
	mapDic["try"] = "����";
	mapDic["note"] = "���";

	cout << mapDic["try"] << endl;
	cout << mapDic["note"] << endl;
}
void main()
{
	//VectorMain();
	//ListMain();
	DequeMain();
	//StackMain();
	//QueueMain();
	//PriorytyQueueMain();
	//MapMain();
	//SetMain();
}