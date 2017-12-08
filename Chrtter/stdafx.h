#pragma once

#include <Windows.h>
#include <iostream>
#include <string>
#include "detours.h"
#include "ircdef.h"

void InitConsole();

void InitHook();

typedef INT64(__stdcall* Address)();
Address OldFunction = NULL;
bool Main();

using namespace std; //What are you going to do about it?