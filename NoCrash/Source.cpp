#include "stdafx.h"

using namespace std;

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		MessageBox(nullptr,
		           "Hello! I Am The Crash Handler For The Wolf Project\nPlease, If Your Game Crashes Report This On The Discord!\nThere Is A Log In The Games Install Directory.",
		           "Yu-Gi-Oh! Crash Handler", 0);
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

LONG WINAPI UnhandledException(struct _EXCEPTION_POINTERS* ExceptionInfo)
{
	std::ofstream ErrorLogFile;
	ErrorLogFile.open("Log.txt");
	if (!ErrorLogFile.is_open())
	{
		stringstream ErrorMessage;
		ErrorMessage << "Something Happened Creating Log File! The Error Is At:\n" << ExceptionInfo
		                                                                              ->ExceptionRecord->
		                                                                              ExceptionAddress <<
			"\nThe Message Is: " << ExceptionCodeToString(ExceptionInfo->ExceptionRecord->ExceptionCode) <<
			"\nPlease Take A Screen Shot And Report Me In The Discord!";
		string PrintableError = ErrorMessage.str();
		MessageBox(nullptr, PrintableError.c_str(), "Uh-Oh!", 0);
	}
	ErrorLogFile << "Yu-Gi-Oh! Crashed. Here Is Some Information.\n";
	ErrorLogFile << "Exception Address: ";
	ErrorLogFile << ExceptionInfo->ExceptionRecord->ExceptionAddress;
	ErrorLogFile << "Error Code: ";
	ErrorLogFile << ExceptionCodeToString(ExceptionInfo->ExceptionRecord->ExceptionCode);
	ErrorLogFile.close();
	return 1;
}

const char* ExceptionCodeToString(DWORD ExceptionCode)
{
	switch (ExceptionCode)
	{
	case EXCEPTION_ACCESS_VIOLATION: return "EXCEPTION_ACCESS_VIOLATION";
	case EXCEPTION_DATATYPE_MISALIGNMENT: return "EXCEPTION_DATATYPE_MISALIGNMENT";
	case EXCEPTION_BREAKPOINT: return "EXCEPTION_BREAKPOINT";
	case EXCEPTION_SINGLE_STEP: return "EXCEPTION_SINGLE_STEP";
	case EXCEPTION_ARRAY_BOUNDS_EXCEEDED: return "EXCEPTION_ARRAY_BOUNDS_EXCEEDED";
	case EXCEPTION_FLT_DENORMAL_OPERAND: return "EXCEPTION_FLT_DENORMAL_OPERAND";
	case EXCEPTION_FLT_DIVIDE_BY_ZERO: return "EXCEPTION_FLT_DIVIDE_BY_ZERO";
	case EXCEPTION_FLT_INEXACT_RESULT: return "EXCEPTION_FLT_INEXACT_RESULT";
	case EXCEPTION_FLT_INVALID_OPERATION: return "EXCEPTION_FLT_INVALID_OPERATION";
	case EXCEPTION_FLT_OVERFLOW: return "EXCEPTION_FLT_OVERFLOW";
	case EXCEPTION_FLT_STACK_CHECK: return "EXCEPTION_FLT_STACK_CHECK";
	case EXCEPTION_FLT_UNDERFLOW: return "EXCEPTION_FLT_UNDERFLOW";
	case EXCEPTION_INT_DIVIDE_BY_ZERO: return "EXCEPTION_INT_DIVIDE_BY_ZERO";
	case EXCEPTION_INT_OVERFLOW: return "EXCEPTION_INT_OVERFLOW";
	case EXCEPTION_PRIV_INSTRUCTION: return "EXCEPTION_PRIV_INSTRUCTION";
	case EXCEPTION_IN_PAGE_ERROR: return "EXCEPTION_IN_PAGE_ERROR";
	case EXCEPTION_ILLEGAL_INSTRUCTION: return "EXCEPTION_ILLEGAL_INSTRUCTION";
	case EXCEPTION_NONCONTINUABLE_EXCEPTION: return "EXCEPTION_NONCONTINUABLE_EXCEPTION";
	case EXCEPTION_STACK_OVERFLOW: return "EXCEPTION_STACK_OVERFLOW";
	case EXCEPTION_INVALID_DISPOSITION: return "EXCEPTION_INVALID_DISPOSITION";
	case EXCEPTION_GUARD_PAGE: return "EXCEPTION_GUARD_PAGE";
	case EXCEPTION_INVALID_HANDLE: return "EXCEPTION_INVALID_HANDLE";
	default: return "UNKOWN";
	}
}
