#include <Detours.h>
#include <Windows.h>

void MyFunc();
void MyBox();
static bool HasRun = false;


BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
		MyFunc();
		break;
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}

	return false;
}

void WINAPI MyFunc()
{
	if (HasRun == false)
	{
		HasRun = true;
		MessageBox(nullptr, "We're Starting", "Start", 0);
		Detours::X64::DetourFunction((PBYTE)0x14097B8F0, (PBYTE)&MyBox,Detours::X64Option::USE_PUSH_RET);
		MessageBox(nullptr, "We're Finish", "Start", 0);
	}
}

void MyBox()
{
	MessageBox(nullptr, "We're Here Now", "Start", 0);
}