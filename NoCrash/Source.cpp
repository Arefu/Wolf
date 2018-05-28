#include <Windows.h>
#include <fstream>
#include <detours.h>

LONG WINAPI UnhandledException(struct _EXCEPTION_POINTERS *ExceptionInfo);
const char *ExceptionCodeToString(DWORD ExceptionCode);

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		MessageBox(NULL, "Uh-Oh, Hi there?", "Uh-Oh!", 0);
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
	char ErrorAddress[MAX_PATH];
	sprintf_s(ErrorAddress, "Exception Address: %p\n\n", ExceptionInfo->ExceptionRecord->ExceptionAddress);
	char ErrorCode[MAX_PATH];
	sprintf_s(ErrorCode, ExceptionCodeToString(ExceptionInfo->ExceptionRecord->ExceptionCode));

	std::ofstream ErrorLogFile;
	ErrorLogFile.open("C:\\Log.txt");
	if (!ErrorLogFile.is_open())
	{
		MessageBox(NULL, "Something Happened Creating Log File!", "Uh-Oh!", 0);
	}
	ErrorLogFile << "Yu-Gi-Oh! Crashed. Here Is Some Information.\n";
	ErrorLogFile << ErrorAddress;
	ErrorLogFile << "Error Code: ";
	ErrorLogFile << ErrorCode;
	ErrorLogFile.close();
	return 1;
}

const char *ExceptionCodeToString(DWORD ExceptionCode)
{
	switch (ExceptionCode)
	{
	case EXCEPTION_ACCESS_VIOLATION:			return "EXCEPTION_ACCESS_VIOLATION";
	case EXCEPTION_DATATYPE_MISALIGNMENT:		return "EXCEPTION_DATATYPE_MISALIGNMENT";
	case EXCEPTION_BREAKPOINT:					return "EXCEPTION_BREAKPOINT";
	case EXCEPTION_SINGLE_STEP:					return "EXCEPTION_SINGLE_STEP";
	case EXCEPTION_ARRAY_BOUNDS_EXCEEDED:		return "EXCEPTION_ARRAY_BOUNDS_EXCEEDED";
	case EXCEPTION_FLT_DENORMAL_OPERAND:		return "EXCEPTION_FLT_DENORMAL_OPERAND";
	case EXCEPTION_FLT_DIVIDE_BY_ZERO:			return "EXCEPTION_FLT_DIVIDE_BY_ZERO";
	case EXCEPTION_FLT_INEXACT_RESULT:			return "EXCEPTION_FLT_INEXACT_RESULT";
	case EXCEPTION_FLT_INVALID_OPERATION:		return "EXCEPTION_FLT_INVALID_OPERATION";
	case EXCEPTION_FLT_OVERFLOW:				return "EXCEPTION_FLT_OVERFLOW";
	case EXCEPTION_FLT_STACK_CHECK:				return "EXCEPTION_FLT_STACK_CHECK";
	case EXCEPTION_FLT_UNDERFLOW:				return "EXCEPTION_FLT_UNDERFLOW";
	case EXCEPTION_INT_DIVIDE_BY_ZERO:			return "EXCEPTION_INT_DIVIDE_BY_ZERO";
	case EXCEPTION_INT_OVERFLOW:				return "EXCEPTION_INT_OVERFLOW";
	case EXCEPTION_PRIV_INSTRUCTION:			return "EXCEPTION_PRIV_INSTRUCTION";
	case EXCEPTION_IN_PAGE_ERROR:				return "EXCEPTION_IN_PAGE_ERROR";
	case EXCEPTION_ILLEGAL_INSTRUCTION:			return "EXCEPTION_ILLEGAL_INSTRUCTION";
	case EXCEPTION_NONCONTINUABLE_EXCEPTION:	return "EXCEPTION_NONCONTINUABLE_EXCEPTION";
	case EXCEPTION_STACK_OVERFLOW:				return "EXCEPTION_STACK_OVERFLOW";
	case EXCEPTION_INVALID_DISPOSITION:			return "EXCEPTION_INVALID_DISPOSITION";
	case EXCEPTION_GUARD_PAGE:					return "EXCEPTION_GUARD_PAGE";
	case EXCEPTION_INVALID_HANDLE:				return "EXCEPTION_INVALID_HANDLE";
	default:									return "UNKOWN";
	}
}
