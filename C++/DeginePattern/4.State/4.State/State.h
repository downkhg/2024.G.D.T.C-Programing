#pragma once

class CContext;

//스테이트는 상속받아 공통으로 사용하기위해 쓰며 모든 장면이 gonext를 사용하도록 만들어야한다.
class CState
{
public:
	CState();
	virtual ~CState();
	virtual void GoNext(CContext* pContext) = 0;
};

//클래스의 선언: 맴버함수변수를 선언한다.
class CStateThree : public CState
{
public:
	CStateThree();
	virtual ~CStateThree();
	virtual void GoNext(CContext* pContext);
};

class CStateTow : public CState
{
public:
	CStateTow();
	virtual ~CStateTow();
	virtual void GoNext(CContext* pContext);
};

class CStateOne : public CState
{
public:
	CStateOne();
	virtual ~CStateOne();
	virtual void GoNext(CContext* pContext);
};