#include <iostream>

using namespace std;
class CContext;
class CState;
class CStateOne;
class CStateTow;
class CStateThree;

class CContext
{
	CState* m_pState = NULL;
public:
	CContext() { cout << typeid(*this).name() <<":"<< this << endl; }
	~CContext() { delete m_pState; cout << "~" << typeid(*this).name() << ":" << this << endl; }
	void SetState(CState* pState);
	void GoNext();
};

//스테이트는 상속받아 공통으로 사용하기위해 쓰며 모든 장면이 gonext를 사용하도록 만들어야한다.
class CState
{
public:
	CState() { cout << typeid(*this).name() << ":" << this << endl; }
	virtual ~CState() { cout <<"~"<< typeid(*this).name() << ":" << this << endl; }
	virtual void GoNext(CContext* pContext) = 0;
};
//클래스의 선언: 맴버함수변수를 선언한다.
class CStateThree : public CState
{
public:
	CStateThree() { cout << typeid(*this).name() << ":" << this << endl; }
	~CStateThree() { cout << "~" << typeid(*this).name() << ":" << this << endl; }
	virtual void GoNext(CContext* pContext);
};

class CStateTow : public CState
{
public:
	CStateTow() { cout << typeid(*this).name() << ":" << this << endl; }
	~CStateTow() { cout << "~" << typeid(*this).name() << ":" << this << endl; }
	virtual void GoNext(CContext* pContext);
};

class CStateOne : public CState
{
public:
	CStateOne() { cout << typeid(*this).name() << ":" << this << endl; }
	~CStateOne() { cout << "~" << typeid(*this).name() << ":" << this << endl; }
	virtual void GoNext(CContext* pContext);
};

//클래스함수의 외부정의: 클래스의 선언보다 사용이 함수에서 정의가 먼저인 경우 모든 클래스가 선언된뒤에, 멤버함수를 정의해야 사용 가능하다.
void CContext::SetState(CState* pState)
{
	if (m_pState) { delete m_pState; m_pState = NULL; }
	m_pState = pState;
}

void CContext::GoNext()
{
	m_pState->GoNext(this);
}

void CStateThree::GoNext(CContext* pContext)
{
	pContext->SetState(new CStateOne());
}

void CStateTow::GoNext(CContext* pContext)
{
	pContext->SetState(new CStateThree());
}

void CStateOne::GoNext(CContext* pContext)
{
	pContext->SetState(new CStateTow());
}

void main()
{
	//_CrtSetBreakAlloc(155); //메모리 누수시 번호를 넣으면 할당하는 위치에 브레이크 포인트를 건다.
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); //메모리 누수 검사

	CContext cContext;

	cContext.SetState(new CStateOne());
	cContext.GoNext();
	cContext.GoNext();
	cContext.GoNext();
}