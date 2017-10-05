#pragma once

#include <Windows.h>
#include <iostream>

void InitConsole();
void InitIRC();

//Log.cpp
enum MessageType { Error = 0, Warning = 1, Information = 2 };
void Log(MessageType, char*);