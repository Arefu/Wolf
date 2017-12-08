#include "stdafx.h" 

BOOL ConsoleAlloc = false;
BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	BOOL RanHook = false;

	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		if (RanHook == false)
		{
			InitHook();
			RanHook = true;
		}
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}

	return true;
}

void WINAPI InitConsole()
{
	if (ConsoleAlloc == false)
	{
		ConsoleAlloc = true;
		AllocConsole();
		freopen_s((FILE**)stdout, "CONOUT$", "r", stdout);
		freopen_s((FILE**)stdout, "CONOUT$", "w", stdout);
		InitIRC();
	}
}

void WINAPI InitIRC()
{
	//Start IRC
	std::cout << "\"IRC\" Started. Type /HELP For Commands" << std::endl;
	CreateThread(nullptr, NULL, MessageListen, nullptr, NULL, nullptr);
}

DWORD WINAPI MessageListen(LPVOID)
{
	std::string CurrentMessage;
		std::cout << ">: ";
		std::getline(std::cin, CurrentMessage);

		if (CurrentMessage == "/HELP")
		{
			std::cout << "** HELP **" << std::endl;
			std::cout << "/HELP - Print This Message." << std::endl;
			std::cout << "/MAKE - Make A Chat/Channel." << std::endl;
			std::cout << "/JOIN <KEY> - Replace <KEY> With Your Private Key To Join A Game." << std::endl;
			std::cout << "/PART or /QUIT - Quits Game And IRC." << std::endl;
		}
		else if (CurrentMessage == "/MAKE")
		{
			//
		}
		else if (CurrentMessage == "/PART" || CurrentMessage == "/QUIT")
		{
			exit(0);
		}
		return 0;
}

void InitHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	OldFunction = (Address)(0x1408FAF50);
	DetourAttach((PVOID*)&OldFunction, Main);
	DetourTransactionCommit();

}

bool Main()
{
	InitConsole();


	return OldFunction();
}