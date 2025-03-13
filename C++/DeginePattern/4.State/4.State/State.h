#pragma once

class CContext;

//������Ʈ�� ��ӹ޾� �������� ����ϱ����� ���� ��� ����� gonext�� ����ϵ��� �������Ѵ�.
class CState
{
public:
	CState();
	virtual ~CState();
	virtual void GoNext(CContext* pContext) = 0;
};

//Ŭ������ ����: �ɹ��Լ������� �����Ѵ�.
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