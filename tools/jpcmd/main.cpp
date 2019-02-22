#include <Windows.h>
#include "resource.h"

int main() {
	SetConsoleTitleA("Japanese Command Prompt");
	// Shift-JIS
	SetConsoleOutputCP(932);
	SetConsoleCP(932);
	system("cmd");
}