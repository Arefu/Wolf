#pragma once

#include <iostream>
#include <Windows.h>

#include "steam_api.h"


void ShowConsole();
void InitBetterMP();

DWORD WINAPI SteamHijack(LPVOID);
DWORD WINAPI WatchForLobby(LPVOID);