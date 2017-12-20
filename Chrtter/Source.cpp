#include "stdafx.h"
#include "ircdef.h"

using namespace std;

BOOL ConsoleAlloc = false;
BOOL RanHook = false;
typedef INT64 (__stdcall* Address)();
Address OldFunction = (Address)(0x1408FAF50);

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
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

void Init()
{
	if (ConsoleAlloc == false)
	{
		AllocConsole();
		freopen("CONIN$", "r", stdin);
		freopen("CONOUT$", "w", stdout);
		freopen("CONOUT$", "w", stderr);
		ConsoleAlloc = true;

		//Start IRC On Thread.
		CreateThread(nullptr, NULL, InitIRC, nullptr, NULL, nullptr);
	}
}

void InitHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&OldFunction, Start);
	DetourTransactionCommit();
}

bool Start()
{
	Init();
	return OldFunction();
}
