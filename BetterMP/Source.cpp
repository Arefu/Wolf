#include "stdafx.h"


BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	SteamAPI_Init();
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
	ShowConsole();
}

void ShowConsole()
{
	AllocConsole();
	freopen("CONOUT$", "w", stdout);
	std::cout << "Better Multiplayer - Yu-Gi-Oh! Legacy of the Duelist" << std::endl;
	std::cout << "Your SteamID64 Is:  " << SteamUser()->GetSteamID().ConvertToUint64() << std::endl;
	std::cout << "Your Steam Name Is: " << SteamFriends()->GetPersonaName() << std::endl;
}


class LobbyMaker
{
public:
	void MakeLobby();
	SteamAPICall_t hSteamAPICall;
	SteamAPICall_t hSteamAPIReff;

private:
	void OnLobbyMaid(LobbyCreated_t* LobbyCreated, bool Created);
	CCallResult<LobbyMaker, LobbyCreated_t> m_LobbyMadeCallResult;
	STEAM_CALLBACK(LobbyMaker, OnNoConection, SteamServersConnected_t);
};

void LobbyMaker::OnNoConection(SteamServersConnected_t *Struct)
{
	std::cout << "Uh-Oh";
}
void LobbyMaker::MakeLobby()
{
	std::cout << "Attempting To Make Lobby" << std::endl;
	hSteamAPICall = SteamMatchmaking()->CreateLobby(k_ELobbyTypePublic, 2);
	m_LobbyMadeCallResult.Set(hSteamAPICall, this, &LobbyMaker::OnLobbyMaid);
	hSteamAPIReff = hSteamAPICall;
	std::cout << (SteamUtils()->GetAPICallFailureReason(hSteamAPIReff));

	SteamAPI_RunCallbacks();
	this->MakeLobby();
}


DWORD WINAPI SteamHijack(LPVOID)
{

	LobbyMaker().MakeLobby();
	return 0;
}

void LobbyMaker::OnLobbyMaid(LobbyCreated_t* LobbyCreated, bool Created)
{
	if (Created)
	{

	}
	std::cout << "Lobby ID: " << LobbyCreated->m_ulSteamIDLobby << std::endl;
}