# Interesting Functions

### Function Address

* `sub_1408FB3D0(__int64 a1)`

##### Points of Interest: 

Calls: `SteamAPI_GetHSteamPipe`

Returns: `(*(int (__fastcall **)(__int64, const char *))(*(_QWORD *)v1 + 224i64))(v1, "LobbyInvite");`

Reason: Fires whenever the user loads the "Invite Player" section.

***

### Function Address

* `sub_140689290(HINSTANCE a1, __int64 a2, __int128 _XMM0)`

##### Points of Interest: 

Calls: `SteamAPI_GetHSteamPipe`, `RegisterClassW`, `CreateWindowExW`, `LoadCursorW`, ` LoadIconW`

Returns: `return sub_14097B230((unsigned __int64)&v11 ^ v15);`

Reason: Proposed function that loads the Yu-Gi-Oh main screen.

***

### Function Address

* `sub_14068AA90(__int64 a1)`

##### Points of Interest: 

Calls: `MessageBoxW`, `SteamAPI_Init`, `timeBeginPeriod`, `timeEndPeriod`


Returns: `return sub_14097B230((unsigned __int64)&v7 ^ v11);`

Reason: SteamAPI Init is called, so this looks to be the function called to open the game and load YGO_DATA.DAT._

***

# Parameters
* `-vsync` unlocks the frame rate.

### Help
To view these functions fully load the game up in IDA or any other 64bit compatibly debugger, then jump to the offset of the function.
