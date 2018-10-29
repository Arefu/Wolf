#pragma once

#include <iostream>
#include <Windows.h>

#include "detours.h"
#include "steam_api.h"

typedef INT64 (__stdcall* address)();

__int64 SteamP2P();


void ShowConsole();
//Setup Detour Information
void HookGame();
//Call From DLLMain
void InitBetterMP();

DWORD WINAPI SteamHijack(LPVOID);