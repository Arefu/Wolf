#include <Windows.h>

DWORD WINAPI MessageBoxThread(LPVOID);

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread(nullptr, NULL, MessageBoxThread, nullptr, NULL, nullptr);
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}

	return true;
}

DWORD WINAPI MessageBoxThread(LPVOID)
{
	MessageBox(nullptr, "Hello From A Thread!", "Hello!", NULL);
	return 0;
}
