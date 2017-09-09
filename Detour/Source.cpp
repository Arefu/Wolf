
#include <Detours.h>
#include <Windows.h>

long MyFunc(long Arg);

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
		Detours::X64::DetourFunction(reinterpret_cast<PBYTE>(0x14086A250), (PBYTE)&MyFunc);
		break;
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}

	return true;
}

long MyFunc(long Arg)
{
	MessageBox(nullptr, ":", "", 0);
	return Arg;
}