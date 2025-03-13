#include "Context.h"
#include "State.h"
#include <iostream>

using namespace std;

CContext::CContext() 
{ 
	cout << typeid(*this).name() << ":" << this << endl; 
}
CContext::~CContext() 
{ 
	delete m_pState; cout << "~" << typeid(*this).name() << ":" << this << endl; 
}

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