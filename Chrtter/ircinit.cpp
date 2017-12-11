#include "stdafx.h"
#include "ircdef.h"

using namespace std;

DWORD WINAPI InitIRC(LPVOID)
{
	//Start IRC
	cout << "\"IRC\" Started. Type /HELP For Commands" << endl;
	std::string CurrentMessage, CurrentServer = "127.0.0.1";
	do
	{
		cout << ">: ";
		getline(cin, CurrentMessage);

		if (strtoup(CurrentMessage) == "/HELP")
		{
			cout << "/HELP - Print This Message." << endl;
			cout << "/MAKE - Make A Chat/Channel." << endl;
			cout << "/JOIN <KEY> - Replace <KEY> With Your Private Key To Join A Game Chat." << endl;
			cout << "/PART - Un-Load Chrtter." << endl;
			cout << "/QUIT - Close and DC From IRC, Also Quit Game." << endl;
			cout << "/SERVER - Set Server For Match Making (Default: Localhost)";
		}
		else if (CurrentMessage == "/MAKE")
		{
		}
		else if (CurrentMessage.substr(0, 5) == "/JOIN")
		{
			try
			{
			std::string GameToken = CurrentMessage.substr(CurrentMessage.find_first_of(' '));
			GameToken.erase(::remove_if(GameToken.begin(), GameToken.end(), ::isspace), GameToken.end());
			}
			catch (exception)
			{
				std::cout << "A Key Is Required To Join A Game." << endl;
			}
		}
		else if (CurrentMessage == "/PART")
		{
			FreeConsole();
		}
		else if (CurrentMessage == "/QUIT")
		{
			FreeConsole();
			exit(0);
		}
		else
		{
			cout << "\"" << CurrentMessage << "\" Is Not Recognized As An Command, Type /HELP For A List." << endl;;
		}
	} while (true);
}

std::string strtoup(std::string str)
{
	for (size_t i = 0; i < str.length(); i++)
	{
		str[i] = toupper(str[i]);
	}
	return str;
}
