#include <Windows.h>

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
		MessageBox(nullptr, "Hello! I'm A MessageBox.", "MessageBox Example!", NULL);
		break;

	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		MessageBox(nullptr, "Goodbye! I Was A MessageBox.", "MessageBox Example!", NULL);
		break;

	default:
		return EXIT_SUCCESS;
	}

	return EXIT_SUCCESS;
}