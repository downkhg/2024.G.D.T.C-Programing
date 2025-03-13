#include <iostream>

using namespace std;
//�̱����ν��Ͻ��� �����ڷ� ����Ҽ�����. ��ü�� ��������ʴ� �����̹Ƿ�, ���������� ���������.
//��) �����ڸ� ���� �����ϰ��ϸ�? -> �����ڴ� private�� ȣ���� �Ұ����ϴ�. �� �ܺο���  ��ü�� �Ҵ��Ҽ�����.
//    �����ڸ� getInstacne���� �����ϸ�? -> ��ü�� ����ִ��� ��������ʴ��� Ȯ���� �Ұ����ϸ�, ȣ�⶧���� ��ü�� ����� �����ϰ� ������ϴ��� ��ȿ������ �ڵ尡 �ȴ�.
class CSingleObject {
	static CSingleObject* m_pInstance;
	int m_nData = 0;
//public: //�����ڰ� public�̶�� ��ü�� �ܺο��� ���������ϹǷ� �������� ��ü�� ������ִ�.
	CSingleObject() { cout << typeid(*this).name() << endl; }
//private: //�Ҹ��ڰ� public�̶�� ��ü�ܺο��� �����Ҵ�� �޸𸮰� �����ɼ��ִ�.
	~CSingleObject() { 
		cout << "~" << typeid(*this).name() << endl;
		//Release(); //delete�� ȣ���ϸ� �Ҹ��ڰ� ȣ��ǹǷ� �ݺ��ؼ� ȣ��ȴ�.
	}
public:
	static CSingleObject* GetInstance()
	{
		if (m_pInstance == NULL) 
			m_pInstance = new CSingleObject();
		return m_pInstance;
	}
	//�����ɹ��Լ��� �����ɹ��������� ���ٰ����ϴ�.
	//�����ɹ��Լ��� ��ü�� ������ ȣ�Ⱑ���ؾ��Ѵ�.
	//�׷��Ƿ�, �����ɹ�����ó�� �̸� �����Ǿ��ִ� �������� ���� ����������,
	//�ɹ�����ó�� ��ü�� �־�� ���� ������ �������� ������ �Ұ����Ͽ� ������ ������ �߻��Ѵ�.
	//this�� ��ü�� �����Ǿ�� �ǹǷ� ȣ���� �Ұ����ϴ�.
	static void Release()
	{
		if (m_pInstance)
		{
			cout <<"CSingleObject.Release!" << endl;
			delete m_pInstance;
			m_pInstance = NULL;
		}
		else
			cout << "CSingleObject empty instance!";
	}
	void ShowMessage()
	{
		cout << typeid(*this).name() << ".ShowMessage:" << m_nData << endl;
	}
};
//�����ɹ������� Ŭ������ �����Ǳ����� �����ؾ��ϹǷ�, ��������ó�� ����Ǿ��, �ٸ� �Լ����� ȣ���Ҷ� ���ٰ����ϴ�.
CSingleObject* CSingleObject::m_pInstance = NULL;

void SingleObjectMain()
{
	cout << "SingleObjectMain() start" << endl;
	//������ ���� �ڵ�� �����ڰ� priavate�̹Ƿ� ������ �Ұ����ϴ�
	//CSingleObject cSingleObject;
	//CSingleObject arrSingleObject[3];
	CSingleObject* pSingleObject = NULL;
	CSingleObject& refSingleObject = *pSingleObject->GetInstance();
	//pSingleObject = new CSingleObject(); //��ü�� �����Ҵ��Ҷ� �����ڰ� ȣ��ǹǷ�, �����Ҵ��� �Ұ����ϴ�.
	pSingleObject = pSingleObject->GetInstance();
	//pSingleObject->Release(); //����ƽ����� ��ü�� �����Ǳ����� ���ٰ����ϹǷ� ȣ���� ����������, ������� ȣ������ �ƴ�.
	pSingleObject->ShowMessage();//��������� ������ ȣ���� ����������, �Ϲݸ���� �����ϸ� �ν��Ͻ����� ȣ���� �Ұ����Ͽ� ���α׷��� �״´�.
	CSingleObject::Release(); //��������� �������� ȣ���ϴ� �������̽��� �����ϴ�.
	//delete pSingleObject; //�̱����� ��ü���ο��� �����ǹǷ� �ܺο��� ���������ʵ����ؾ��Ѵ�.
	cout << "SingleObjectMain() end" << endl;
}

//1.�����ڿ� �Ҹ��ڴ� �� private�̿��ϴ°�?
//2.������������� �� ��������ó�� ����Ǿ���ϴ°�?
//3.static����Լ��� �� �Ϲݸ�������� ������ �Ұ����ϰ� static����������� ������ �����Ѱ�?
//4.release�� ȣ�������ʰ� ���α׷��� ����Ǹ� �޸𸮴����� �߻��ϴ� ������ �Ҹ��ڸ� ����Ҽ� ���� ����?
//5.C++���� �ν��Ͻ��� �����Ҷ� �����ڸ� ��� �� �� ���� ������ �����ΰ�?
//6.C++�� ����Ҷ� �޸𸮸� �����ϰ� ����ؾ��ϴ� ������ �����ΰ�? �����Ͽ��������� �߸��� ������ ��� ���������⶧��.
int main()
{
	cout << "main() start" << endl;
	//_CrtSetBreakAlloc(155); //�޸� ������ ��ȣ�� ������ �Ҵ��ϴ� ��ġ�� �극��ũ ����Ʈ�� �Ǵ�.
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); //�޸� ���� �˻�

	SingleObjectMain();
	cout << "main() end" << endl;
	return 0;
}