#include <iostream>
#include <vector>

using namespace std;

void CustumVectorMain();

namespace custum
{
	template<typename Type>
	class vector {
		Type* array;
		int nSize;
	public:
		vector(int size = 0)
		{
			nSize = size;
		}
		size_t capacity()
		{
			return nSize;
		}
		size_t size()
		{
			return nSize;
		}
		void resize(int size)
		{

		}
		int& operator[](int idx)
		{
			return array[idx];
		}
		Type* begin()
		{
			return &array[0];
		}
		Type* end()
		{
			return &array[nSize];
		}
		void push_back(Type data)
		{

		}
		void insert(Type* where, Type data)
		{

		}
		Type* erase(Type* it)
		{
			return it;
		}
		void clear()
		{

		}
	};
}

void VectorMain()
{
	vector<int> container(1);//컨테이너생성시 크기를 지정가능하다.
	container[0] = 10;
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //배열의 크기를 지정한다.
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	vector<int>::iterator it;
	cout << "PrintPtr(" << container.capacity() << "):";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//1.추가
	container.push_back(100);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//2.삽입 
	it = container.begin(); //10
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	//찾기
	vector<int>::iterator itFind = find(container.begin(), container.end(), 100);
	cout << "itFind[" << &*itFind << "]" << *itFind << endl;
	container.insert(itFind, 50);

	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//3.삭제
	it = --container.end();
	it = container.begin() + 3;
	cout << "it[" << &*it << "]" << *it << endl;
	container.erase(it);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (vector<int>::iterator it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	// 4.모두삭제
	container.clear(); //모두삭제
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
}

void main()
{
	CustumVectorMain();
	//VectorMain();
}

void CustumVectorMain()
{
	custum::vector<int> container(1);//컨테이너생성시 크기를 지정가능하다.
	container[0] = 10;
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //배열의 크기를 지정한다.
	cout << "Print(" << container.capacity() << "):";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	int* it;
	cout << "PrintPtr(" << container.capacity() << "):";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//1.추가
	container.push_back(100);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (int* it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//2.삽입 
	it = container.begin(); //10
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	it++;
	cout << "it[" << &*it << "]" << *it << endl;
	//찾기
	int* itFind = find(container.begin(), container.end(), 100);
	cout << "itFind[" << &*itFind << "]" << *itFind << endl;
	container.insert(itFind, 50);

	cout << "PrintPtr(" << container.capacity() << "):";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	//3.삭제
	it = container.end();
	it--;
	it = container.begin() + 3;
	cout << "it[" << &*it << "]" << *it << endl;
	container.erase(it);
	cout << "PrintPtr(" << container.capacity() << "):";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	// 4.모두삭제
	container.clear(); //모두삭제
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
}

