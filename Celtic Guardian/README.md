# Celtic Guardian

This is my "Utilities DLL" it stores functions I don't want to re-write. You will need to compile this as all my C# projects will depend on it.

Current Utility Functions:
```csharp
Log(string Message, Event LogLevel, bool ShouldQuit = false, int ExitCode = 0);
IsExt(string File,string Extension);
GetIntFromByteArray(byte[] Data);
ByteArrayToString(byte[] Data, bool TrimDelim = true);
GetText(byte[] Message, bool RemoveNull = true);
GetRealTextFromByteArray(byte[] Data, bool RemovewhiteSpace = false);
HexToDec(string HexValue);
GiveFileSize(long Value, int DecimalPlaces = 1);
```

Current Data Classes
```csharp 
public FileData(int Item1, int Item2, string Item3);
```