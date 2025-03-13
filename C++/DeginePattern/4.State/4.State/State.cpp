#include "Context.h"
#include "State.h"
#include <iostream>

using namespace std;

CState::CState() { cout << typeid(*this).name() << ":" << this << endl; }
CState::~CState() { cout << "~" << typeid(*this).name() << ":" << this << endl; }

CStateOne::CStateOne() { cout << typeid(*this).name() << ":" << this << endl; }
CStateOne::~CStateOne() { cout << "~" << typeid(*this).name() << ":" << this << endl; }

CStateThree::CStateThree() { cout << typeid(*this).name() << ":" << this << endl; }
CStateThree::~CStateThree() { cout << "~" << typeid(*this).name() << ":" << this << endl; }

CStateTow::CStateTow() { cout << typeid(*this).name() << ":" << this << endl; }
CStateTow::~CStateTow() { cout << "~" << typeid(*this).name() << ":" << this << endl; }

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