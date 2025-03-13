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

//������Ʈ�� ��ӹ޾� �������� ����ϱ����� ���� ��� ����� gonext�� ����ϵ��� �������Ѵ�.
class CState
{
public:
	CState() { cout << typeid(*this).name() << ":" << this << endl; }
	virtual ~CState() { cout <<"~"<< typeid(*this).name() << ":" << this << endl; }
	virtual void GoNext(CContext* pContext) = 0;
};
//Ŭ������ ����: �ɹ��Լ������� �����Ѵ�.
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

//Ŭ�����Լ��� �ܺ�����: Ŭ������ ���𺸴� ����� �Լ����� ���ǰ� ������ ��� ��� Ŭ������ ����ȵڿ�, ����Լ��� �����ؾ� ��� �����ϴ�.
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
	//_CrtSetBreakAlloc(155); //�޸� ������ ��ȣ�� ������ �Ҵ��ϴ� ��ġ�� �극��ũ ����Ʈ�� �Ǵ�.
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); //�޸� ���� �˻�

	CContext cContext;

	cContext.SetState(new CStateOne());
	cContext.GoNext();
	cContext.GoNext();
	cContext.GoNext();
}