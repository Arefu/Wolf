# Celtic Guardian

This is my "Utilities DLL" it stores functions I don't want to re-write. You will need to compile this as all my C# projects will depend on it.

Current Functions:
```c
Log(string Message, Event LogLevel, bool ShouldQuit = false, int ExitCode = 0);
IsExt(string File,string Extension);
GetIntFromByteArray(byte[] Data);
ByteArrayToString(byte[] Data, bool TrimDelim = true);
GetText(byte[] Message, bool RemoveNull = true);
GetRealTextFromByteArray(byte[] Data, bool RemovewhiteSpace = false);
HexToDec(string HexValue);
```