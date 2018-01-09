#include "stdafx.h" 

INT64 FreeStore();

typedef INT64(__stdcall* Address)();
Address OldFunction = (Address)(0x1406183A0);
BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		DetourAttach((PVOID*)&OldFunction, FreeStore);
		DetourTransactionCommit();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}
	return true;
}

INT64 FreeStore()
{
	return 0;
}