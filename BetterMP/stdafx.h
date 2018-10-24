#pragma once

#include <iostream>
#include <Windows.h>

#include "detours.h"
#include "steam_api.h"



DWORD WINAPI SteamHijack(LPVOID lpParam);
void SteamP2P();


void ShowConsole();
//Setup Detour Information
void HookGame();
//Call From DLLMain
void InitBetterMP();