#include "stdafx.h"

DWORD WINAPI InitIRC(LPVOID)
{
	//Start IRC
	cout << "\"IRC\" Started. Type /HELP For Commands" << endl;
	string CurrentMessage = "/HELP";
	do
	{
		cout << ">: ";
		cin >> CurrentMessage;
		if (CurrentMessage == "/HELP")
		{
			cout << "** HELP **" << endl;
			cout << "/HELP - Print This Message." << endl;
			cout << "/MAKE - Make A Chat/Channel." << endl;
			cout << "/JOIN <KEY> - Replace <KEY> With Your Private Key To Join A Game." << endl;
			cout << "/PART or /QUIT - Quits Game And IRC." << endl;
		}
		else if (CurrentMessage == "/MAKE")
		{
			//
		}
		else if (CurrentMessage == "/PART" || CurrentMessage == "/QUIT")
		{
			exit(0);
		}
	} while (true);
}