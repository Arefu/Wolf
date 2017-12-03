#include "stdafx.h"

void Log(MessageType LogType, char* LogMessage)
{
	switch (LogType)
	{
	case 0:
		std::cout << "[ERROR]: " << LogMessage << std::endl;
		std::cout << "Program Needs To Quit Due To Error...";
		exit(1);
		break;
	case 1:
		std::cout << "[WARNING]: " << LogMessage << std::endl;
		break;
	case 2:
		std::cout << "[INFORMATION]: " << LogMessage << std::endl;
		break;
	default:
		throw ERROR;
	}
}