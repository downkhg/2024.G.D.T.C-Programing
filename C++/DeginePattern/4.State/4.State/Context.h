#pragma once

class CState;

class CContext
{
	CState* m_pState;
public:
	CContext();
	~CContext();
	void SetState(CState* pState);
	void GoNext();
};