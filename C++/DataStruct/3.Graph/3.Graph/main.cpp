#include <iostream>
#include <vector>
#include <unordered_map>
#include <algorithm>
#include <queue>

using namespace std;

struct SNode
{
	char cData;
	bool isVisit = false;

	bool CheckVisit()
	{
		if (isVisit == false)
		{
			isVisit = true;
			return false;
		}
		else
			return true;
	}

	vector<SNode*> vecNodes;
	SNode(char data)
	{
		cData = data;
	}

	void Add(SNode* pNode)
	{
		vecNodes.push_back(pNode);
	}

	SNode* GetNode(int idx)
	{
		return vecNodes[idx];
	}

	int NodeSize()
	{
		return vecNodes.size();
	}
};

void TraverseDFS(SNode* pNode)
{
	if (pNode->CheckVisit()) { cout << pNode->cData << " is Visit!" << endl; return; }
	cout << pNode->cData << endl;
	vector<SNode*>& vecNoeds = pNode->vecNodes;
	for (vector<SNode*>::iterator it = vecNoeds.begin(); it != vecNoeds.end(); it++)
	{
		SNode* idjNode = *it;
		TraverseDFS(idjNode);
	}
}

void PrintDFS(SNode* pStart)
{
	cout << "###### DFS Start! ######" << endl;
	TraverseDFS(pStart);
	cout << "###### DFS End! ######" << endl;
}

bool VisitBFS(queue<SNode*> &queVisit, SNode* pNode)
{
	if (!pNode->CheckVisit())
	{
		queVisit.push(pNode);
		return true;
	}
	else
		cout << pNode->cData << " is Visit!" << endl;
	return false;
}

void PrintBFS(SNode* pStart)
{
	queue<SNode*> queVisit;
	SNode* pNode = pStart;
	cout << "###### BFS Start! ######" << endl;
	if (VisitBFS(queVisit, pNode))
		cout << pNode->cData << endl;
	do{
		vector<SNode*>& vecNoeds = pNode->vecNodes;
		for (vector<SNode*>::iterator it = vecNoeds.begin(); it != vecNoeds.end(); it++)
		{
			SNode* idjNode = *it;
			if (VisitBFS(queVisit, idjNode))
				cout << idjNode->cData << endl;
		}
		
		//cout << pNode->cData <<" is Visit End & pop!" << endl;
		queVisit.pop();
		if (queVisit.empty())
		{
			break;	
		}
		else
			pNode = queVisit.front();
	} while (pNode);
	cout << "###### BFS End! ######" << endl;
}

void GraphMain()
{
	unordered_map<char, SNode*> graphNodes;

	graphNodes['A'] = new SNode('A');
	graphNodes['B'] = new SNode('B');
	graphNodes['C'] = new SNode('C');
	graphNodes['D'] = new SNode('D');
	graphNodes['E'] = new SNode('E');
	graphNodes['F'] = new SNode('F');
	graphNodes['G'] = new SNode('G');
	graphNodes['H'] = new SNode('H');

	graphNodes['A']->Add(graphNodes['B']);

	graphNodes['B']->Add(graphNodes['D']);
	graphNodes['B']->Add(graphNodes['F']);

	graphNodes['C']->Add(graphNodes['B']);

	graphNodes['D']->Add(graphNodes['H']);

	graphNodes['E']->Add(graphNodes['C']);
	graphNodes['E']->Add(graphNodes['H']);

	graphNodes['F']->Add(graphNodes['D']);
	graphNodes['F']->Add(graphNodes['G']);
	graphNodes['F']->Add(graphNodes['H']);

	graphNodes['G']->Add(graphNodes['E']);
	graphNodes['G']->Add(graphNodes['H']);
	graphNodes['G']->Add(graphNodes['F']);

	//PrintDFS(graphNodes['A']);
	PrintBFS(graphNodes['A']);
	//모든노드를 방문이 끝나면 초기화를 한다.
	for (pair<char, SNode*> it : graphNodes) {
		it.second->isVisit = false; 
	}

	
}

int main()
{
	GraphMain();
	return 0;
}