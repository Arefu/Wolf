#include "stdafx.h" 

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		InitConsole();
		InitIRC();
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
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	freopen("CONIN$", "r", stdin);
	SetConsoleTitle("Yu-Gi-Oh! Legacy Of The Duelist: Chrtter");
}

void WINAPI InitIRC()
{
	Log(Information, "Starting CHRTTER...");
	//Start IRC
}
