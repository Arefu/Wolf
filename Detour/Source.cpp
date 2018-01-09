#include "stdafx.h"

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		InitDetours();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}

	return true;
}

void WINAPI InitDetours()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	old_function = (address)(0x1408FB380);
	DetourAttach((PVOID*)&old_function, MyFunction);
	DetourTransactionCommit();
}

__int64 MyFunction()
{
	MessageBox(nullptr, "We're Here Now", "Start", 0);
	return old_function();
}
