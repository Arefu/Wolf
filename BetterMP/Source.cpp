#include "stdafx.h"

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		InitBetterMP();
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
	default:
		break;
	}
	return true;
}


void InitBetterMP()
{
	HookGame();
	ShowConsole();
}

void ShowConsole()
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	std::cout << "Better Multiplayer - Yu-Gi-Oh! Legacy of the Duelist" << std::endl;
}

void HookGame()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)0x1406183A0, SteamP2P);
	DetourTransactionCommit();
}

void SteamP2P()
{
	unsigned int msgSize = 0;

	//while (SteamNetworking()->IsP2PPacketAvailable(&msgSize))
	//{
	//}
	//if (!SteamNetworking()->IsP2PPacketAvailable(&msgSize)) SteamP2P();
}