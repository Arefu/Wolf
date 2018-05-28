#include <Windows.h>

LONG WINAPI MyUnhandledExceptionFilter(struct _EXCEPTION_POINTERS *ExceptionInfo);

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		SetUnhandledExceptionFilter(UnhandledException);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}
	return true;
}
LONG WINAPI UnhandledException(struct _EXCEPTION_POINTERS *ExceptionInfo)
{
	//Create Custom Exception Handler
	return 1;
}
