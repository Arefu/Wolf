#include "stdafx.h"

static bool Attached = false;
DWORD WINAPI LongPoll(LPVOID);
BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	if (!Attached)
		Attached = true;
	{
		MessageBox(nullptr, "DllMain", "", 0);
		switch (ul_reason_for_call)
		{
		case DLL_PROCESS_ATTACH:
			MessageBox(nullptr, "DLL_PROCESS_ATTACH", "", 0);
			InitBetterMP();
			break;
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
		default:
			break;
		}
	}
	return true;
}


void InitBetterMP()
{
	MessageBox(nullptr, "InitBetterMP", "", 0);
	CreateThread(nullptr, NULL, SteamHijack, nullptr, NULL, nullptr);
}

void ShowConsole()
{
	MessageBox(nullptr, "ShowConsole", "", 0);
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	std::cout << "Better Multiplayer - Yu-Gi-Oh! Legacy of the Duelist" << std::endl;
	std::cout << "Your SteamID Is: " << SteamUser()->GetSteamID().ConvertToUint64() << std::endl;
	
}

void HookGame()
{
	MessageBox(nullptr, "HookGame", "", 0);
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	address old_function = (address)(0x1406183A0);
	DetourAttach((PVOID*)&old_function, SteamP2P);
	DetourTransactionCommit();
}

__int64 SteamP2P()
{

	MessageBox(nullptr, "SteamHijack", "", 0);

	CreateThread(nullptr, NULL, LongPoll, nullptr, NULL, nullptr);
	address old_function = (address)(0x1406183A0);
	DetourDetach((PVOID*)&old_function, SteamP2P);
	return 0;
}

DWORD WINAPI SteamHijack(LPVOID)
{
	ShowConsole();
	HookGame();
	return 0;
}

DWORD WINAPI LongPoll(LPVOID)
{
	do
	{
		unsigned int msgSize = 0;
		if (SteamNetworking()->IsP2PPacketAvailable(&msgSize))
		{
			while (SteamNetworking()->IsP2PPacketAvailable(&msgSize))
			{
				void *packet = malloc(msgSize);
				CSteamID steamIDRemote;
				uint32 bytesRead = 0;
				if (SteamNetworking()->ReadP2PPacket(packet, msgSize, &bytesRead, &steamIDRemote))
				{
					SteamMatchmaking()->JoinLobby(steamIDRemote);
				}
				free(packet);
			}

		}
	} while (true);
	return 0;
}
