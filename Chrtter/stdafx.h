#pragma once

#include <Windows.h>
#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include <iterator>
#include <algorithm>
#include "detours.h"

void Init();
void InitHook();
bool Start();
std::string strtoup(std::string);
