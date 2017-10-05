#include "stdafx.h"

void Log(MessageType LogType, char* LogMessage)
{
	switch (LogType)
	{
	case 0:
		break;
	case 1:
		break;
	case 2:
		std::cout << "[INFORMATION]: " << LogMessage << std::endl;
		break;
	default:
		throw ERROR;
	}
}