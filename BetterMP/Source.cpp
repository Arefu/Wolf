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
	CreateThread(nullptr, NULL, SteamHijack, nullptr, NULL, nullptr);
}

void ShowConsole()
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	std::cout << "Better Multiplayer - Yu-Gi-Oh! Legacy of the Duelist" << std::endl;
	std::cout << "Your SteamID64 Is: " << SteamUser()->GetSteamID().ConvertToUint64() << std::endl;
}

DWORD WINAPI SteamHijack(LPVOID)
{
	ShowConsole();

	CreateThread(nullptr, NULL, WatchForLobby, nullptr, NULL, nullptr);
	return 0;
}

DWORD WINAPI WatchForLobby(LPVOID)
{
	unsigned int msgSize = 0;
	while (true)
	{
		if (!SteamNetworking()->IsP2PPacketAvailable(&msgSize)) continue;

		CSteamID steamIDRemote;
		void *packet = malloc(msgSize);
		unsigned int bytesRead = 0;
		SteamNetworking()->ReadP2PPacket(packet, msgSize, &bytesRead, &steamIDRemote);
		std::cout << "Found Lobby! SteamID Of Host: " << steamIDRemote.ConvertToUint64();
	}
}

/*

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
}*/
