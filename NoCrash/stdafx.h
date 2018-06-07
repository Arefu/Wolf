#pragma once

#include <Windows.h>
#include <fstream>
#include <string>
#include <sstream>

LONG WINAPI UnhandledException(struct _EXCEPTION_POINTERS *ExceptionInfo);
const char *ExceptionCodeToString(DWORD ExceptionCode);