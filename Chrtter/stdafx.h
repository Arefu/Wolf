#pragma once

#include <Windows.h>
#include <iostream>
#include <string>
#include "detours.h"

void InitConsole();
void InitIRC();
void InitHook();
DWORD WINAPI MessageListen(LPVOID);

typedef INT64(__stdcall* Address)();
Address OldFunction = NULL;
bool Main();