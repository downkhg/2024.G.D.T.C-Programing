/*##################################
단일연결리스트(수업용)
파일명: LinkedList_empty.cpp
작성자: 김홍규 (downkhg@gmail.com)
마지막수정날짜: 2019.04.12
버전: 1.10
###################################*/

#include <stdio.h>
#include <stdlib.h> //메모리 동적할당 헤더
#include <crtdbg.h> //메모리 누수 탐지 헤더
//#include  "linkedlistClass.h"

struct SNode {
	int nData;
	SNode* pNext;
};

SNode* CreateNode(SNode* pNode, int data); //노드를 생성하여 리턴한다.
SNode* FindNodeData(SNode* pStart, int data); //해당 데이터를 가진 노드를 찾는다.
SNode* InsertNodeData(SNode* pStart, int data, int insert); //해당 데이터를 가진 노드 뒤에 노드를 추가한다.
void DeleteNodeData(SNode* pStart, int del); //해당데이터를 가진 노드를 삭제한다.
void PrintLinkedList(SNode* pStart); //노드를 순회하며 끝날때까지 출력한다.
void DeleteLinkedList(SNode* pStart); //노드를 순회하며 모든데이터를 삭제한다.
void DeleteLinkedListPtr(SNode** ppStart); //노드를 순회하며 모든데이터를 삭제한다.(시작노드를 가르치키지않도록한다)
void DeleteLinkedListRef(SNode*& pStart); //노드를 순회하며 모든데이터를 삭제한다.(시작노드를 가르치키지않도록한다)
void ReverseLinkedList(SNode* pStart); //

//연결리스트 동적으로 입력받기.(동적할당 설명용)
void InputAdd();

//정상작동 테스트를 위해서, 다음과 같이 기본적인 절차로 오류를 확인한다.
//이 소스에 몇가지 버그가 존재한다.
//이 코드가 정상작동 된 후 발견해볼것!
//main()함수 내 코드는 추가는 가능하지만 삭제는 하지말것!
void main()
{
	//_CrtSetBreakAlloc(81); //메모리 누수시 번호를 넣으면 할당하는 위치에 브레이크 포인트를 건다.
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF); //메모리 누수 검사 

	SNode* pBegin = NULL;
	SNode* pEnd = NULL;

	//노드 추가 테스트
	pEnd = CreateNode(pEnd, 10);
	pBegin = pEnd; //마지막 노드를 알아야 검색이 가능하므로 저장해둔다.

	pEnd = CreateNode(pEnd, 20);
	pEnd = CreateNode(pEnd, 30);
	pEnd = CreateNode(pEnd, 40);
	pEnd = CreateNode(pEnd, 50);

	PrintLinkedList(pBegin);

	SNode* pFind = FindNodeData(pBegin, 40);
	//SNode* pFind = FindNodeData(pBegin, 60); //위결과가 정상적으로 작동했다고 하더라도 예외를 테스트하여 문제를 해결한다.
	if (pFind != NULL)
		printf("Find:%d\n", pFind->nData);

	SNode* pInsert = InsertNodeData(pBegin, 30, 60);//노드 삽입
	if (pInsert != NULL)
		printf("Insert:%d\n", pInsert->nData);
	//pEnd = pInsert; //현재 인터페이스 대로라면 50뒤에 값을 삽입했다면, 이런식으로 사용자가 어디에 추가 했느냐에 따라 end를 수동으로 변경하는것이 일반적.

	PrintLinkedList(pBegin);

	DeleteNodeData(pBegin, 60);//노드 삭제

	PrintLinkedList(pBegin);

	DeleteLinkedListPtr(&pBegin); //모든노드삭제 - 이 함수를 호출하지않을시 메모리가 누수됨.
	//DeleteLinkedListRef(pBegin); //모든노드삭제 - 이 함수를 호출하지않을시 메모리가 누수됨.

	if (pBegin == NULL) printf("delete All");
}

//여기서 부터 기능을 구현한다.
//기존코드는 손대지말고, 추가만 하여 현 프로그램 정상 작동하도록할것.
SNode* CreateNode(SNode* pNode, int data)
{
	SNode* pTemp = new SNode(); //(SNode*)malloc(sizeof(SNode))
	pTemp->nData = data;
	pTemp->pNext = NULL;
	if (pNode != NULL)//NULL == NULL -> !T -> F
		pNode->pNext = pTemp;
	//pTemp->pNext = pNode;
	return  pTemp;
}

SNode* FindNodeData(SNode* pStart, int data)
{
	SNode* pNode = pStart;

	while (pNode)
	{
		if (pNode->nData != data)
			pNode = pNode->pNext;
		else
			break;
	}

	return pNode;
}

SNode* InsertNodeData(SNode* pStart, int data, int insert)
{
	SNode* pNode = pStart;
	SNode* pInsert = NULL;

	pNode = FindNodeData(pStart, data);

	if (pNode)
	{
		pInsert = new SNode();
		//0x06->pNext = 0x04 
		pInsert->pNext = pNode->pNext;
		pNode->pNext = pInsert;
		pInsert->nData = insert;
	}

	return pInsert;
}

void DeleteNodeData(SNode* pStart, int del)
{
	SNode* pPre = NULL;
	SNode* pNode = pStart;

	while (pNode)
	{
		if (pNode->nData != del)//10!=60 => T //20!=60 => T //30!=60 => T//60 != ->F
		{
			//pNode값이 변경되기전에 이전으로 지정한다.
			pPre = pNode;
			pNode = pNode->pNext;
		}
		else
		{
			//0x03->pNext = 0x04
			pPre->pNext = pNode->pNext;
			delete pNode;
			break;
		}
	}
}

void PrintLinkedList(SNode* pStart)
{
	SNode* pNode = pStart;
	printf("data:");
	while (pNode)
	{
		printf("%d", pNode->nData);
		pNode = pNode->pNext;

		if (pNode != NULL)
			printf(",");
	}
	printf("\n");
}

void DeleteLinkedList(SNode* pStart)
{
	SNode* pNode = pStart;
	SNode* pDel = NULL;

	while (pNode)//T
	{
		pDel = pNode;
		pNode = pNode->pNext;
		delete(pDel);
	}
	pStart = NULL; //매개변수를 바꿔도 소용없음.
}

void DeleteLinkedListPtr(SNode** ppStart)
{
	SNode* pNode = *ppStart;
	SNode* pDel = NULL;

	while (pNode)//T
	{
		pDel = pNode;
		pNode = pNode->pNext;
		delete(pDel);
	}
	*ppStart = NULL; //매개변수를 바꿔도 소용없음.
}

void DeleteLinkedListRef(SNode*& pRefStart)
{
	SNode* pNode = pRefStart;
	SNode* pDel = NULL;

	while (pNode)//T
	{
		pDel = pNode;
		pNode = pNode->pNext;
		delete(pDel);
	}
	pRefStart = NULL; //매개변수를 바꿔도 소용없음.
}

void InputAdd()
{
	SNode* pStart = NULL;
	SNode* pNode = NULL;
	int nData = 0;

	//동적할당을 하면 프로그램이 사용자에 의해서 사용되는 메모리가 결정된다.
	//쉽게말해서, 컴파일단계에서 100개를 만들고 쓴다면, 
	//사용하지않더라도 100개의 메모리를 사용할수밖에없다.
	//그리고, 100개 이상의 메모리도 사용할수없다.
	//그러나, 동적할당을 하면 사용자가 추가한 메모리만큼만 메모리가 사용되고 
	//메모리용량이 허용하는 한 추가가 된다.
	while (nData != -1)
	{
		scanf("%d", &nData);
		pNode = CreateNode(pNode, nData);

		if (pNode == NULL)
		{
			printf("더 이상 사용할수 있는 메모리가 없습니다!");
		}

		if (pStart == NULL)
			pStart = pNode;

		PrintLinkedList(pStart);
	}

	DeleteLinkedList(pStart); //모든노드삭제 - 이 함수를 호출하지않을시 메모리가 누수됨.
}