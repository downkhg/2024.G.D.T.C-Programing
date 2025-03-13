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