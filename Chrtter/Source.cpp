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

void InitConsole()
{
	if (ConsoleAlloc == false)
	{
		ConsoleAlloc = true;
		AllocConsole();
		freopen("CONIN$", "r", stdin);
		freopen("CONOUT$", "w", stdout);
		freopen("CONOUT$", "w", stderr);
		CreateThread(nullptr, NULL, InitIRC, nullptr, NULL, nullptr);
	}
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