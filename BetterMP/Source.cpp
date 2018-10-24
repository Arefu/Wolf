#include <iostream>
#include "steam_api.h"
#include <Windows.h>
#include "detours.h"

DWORD WINAPI SteamHijack(LPVOID lpParam);
void SteamP2P();

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	std::cout << "Better Multiplayer";
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread(nullptr, NULL, SteamHijack, nullptr, NULL, nullptr);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}
	return true;
}


DWORD WINAPI SteamHijack(LPVOID lpParam)
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)0x1406183A0, SteamP2P);
	DetourTransactionCommit();
	return 0;
}

void SteamP2P()
{
	unsigned int msgSize = 0;

	while (SteamNetworking()->IsP2PPacketAvailable(&msgSize))
	{
	}
	if (!SteamNetworking()->IsP2PPacketAvailable(&msgSize)) SteamP2P();
}