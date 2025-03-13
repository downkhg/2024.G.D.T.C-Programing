#include <iostream>

using namespace std;
//싱글톤인스턴스를 참조자로 사용할수없음. 객체가 비어있지않는 참조이므로, 생성시점을 만들수없음.
//예) 생성자를 에서 생성하게하면? -> 생성자는 private라 호출이 불가능하다. 즉 외부에서  객체를 할당할수없다.
//    생성자를 getInstacne에서 생성하면? -> 객체가 비어있는지 비어있지않는지 확인이 불가능하며, 호출때마다 객체를 지우고 생성하게 만든다하더라도 비효율적인 코드가 된다.
class CSingleObject {
	static CSingleObject* m_pInstance;
	int m_nData = 0;
//public: //생성자가 public이라면 객체를 외부에서 생성가능하므로 여러개의 객체가 만들수있다.
	CSingleObject() { cout << typeid(*this).name() << endl; }
//private: //소멸자가 public이라면 객체외부에서 동적할당된 메모리가 삭제될수있다.
	~CSingleObject() { 
		cout << "~" << typeid(*this).name() << endl;
		//Release(); //delete를 호출하면 소멸자가 호출되므로 반복해서 호출된다.
	}
public:
	static CSingleObject* GetInstance()
	{
		if (m_pInstance == NULL) 
			m_pInstance = new CSingleObject();
		return m_pInstance;
	}
	//정적맴버함수는 정적맴버변수에만 접근가능하다.
	//정적맴버함수는 객체가 없을때 호출가능해야한다.
	//그러므로, 정적맴버변수처럼 미리 생성되어있는 변수에는 접근 가능하지만,
	//맴버변수처럼 객체가 있어야 접근 가능한 변수에는 접근이 불가능하여 컴파일 에러가 발생한다.
	//this도 객체가 생성되어야 되므로 호출이 불가능하다.
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
//정적맴버변수는 클래스가 생성되기전에 접근해야하므로, 전역변수처럼 선언되어야, 다른 함수에서 호출할때 접근가능하다.
CSingleObject* CSingleObject::m_pInstance = NULL;

void SingleObjectMain()
{
	cout << "SingleObjectMain() start" << endl;
	//다음과 같은 코드는 생성자가 priavate이므로 생성이 불가능하다
	//CSingleObject cSingleObject;
	//CSingleObject arrSingleObject[3];
	CSingleObject* pSingleObject = NULL;
	CSingleObject& refSingleObject = *pSingleObject->GetInstance();
	//pSingleObject = new CSingleObject(); //객체를 동적할당할때 생성자가 호출되므로, 동적할당이 불가능하다.
	pSingleObject = pSingleObject->GetInstance();
	//pSingleObject->Release(); //스테틱멤버는 객체가 생성되기전에 접근가능하므로 호출이 가능하지만, 상식적인 호출방법은 아님.
	pSingleObject->ShowMessage();//멤버변수가 없을때 호출이 성립하지만, 일반멤버에 접근하면 인스턴스없이 호출이 불가능하여 프로그램이 죽는다.
	CSingleObject::Release(); //정적멤버를 정식으로 호출하는 인터페이스로 적합하다.
	//delete pSingleObject; //싱글톤은 객체내부에서 관리되므로 외부에서 삭제되지않도록해야한다.
	cout << "SingleObjectMain() end" << endl;
}

//1.생성자와 소멸자는 왜 private이여하는가?
//2.정적멤버변수는 왜 전역변수처럼 선언되어야하는가?
//3.static멤버함수는 왜 일반멤버변수에 접근이 불가능하고 static멤버변수에만 접근이 가능한가?
//4.release를 호출하지않고 프로그램이 종료되면 메모리누수가 발생하는 하지만 소멸자를 사용할수 없는 이유?
//5.C++에서 인스턴스를 참조할때 참조자를 사용 할 수 없는 이유는 무엇인가?
//6.C++을 사용할때 메모리를 이해하고 사용해야하는 이유는 무엇인가? 컴파일오류만으로 잘못된 접근을 모두 막을수없기때문.
int main()
{
	cout << "main() start" << endl;
	//_CrtSetBreakAlloc(155); //메모리 누수시 번호를 넣으면 할당하는 위치에 브레이크 포인트를 건다.
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); //메모리 누수 검사

	SingleObjectMain();
	cout << "main() end" << endl;
	return 0;
}