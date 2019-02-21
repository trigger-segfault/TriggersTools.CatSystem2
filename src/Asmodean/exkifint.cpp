// exkifint.cpp (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps extracts data from Windmill's encrypted KIF (*.int) archives.
#include "exkifint.h"
#include "blowfish.h"

void DecryptVCode2(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcode2Buffer,
	unsigned long  vcode2Length)
{
	Blowfish bf;
	bf.Set_Key(keyBuffer, keyLength);
	bf.Decrypt(vcode2Buffer, (vcode2Length + 7) & ~7);
}

void DecryptEntry(
	KIFENTRYINFO& entry,
	unsigned long fileKey)
{
	Blowfish bf;
	bf.Set_Key((unsigned char*)&fileKey, 4);
	bf.Decrypt((unsigned char*)&entry.offset, 8);
}

void DecryptData(
	unsigned char* buffer,
	unsigned long  length,
	unsigned long  fileKey)
{
	Blowfish bf;
	bf.Set_Key((unsigned char*)&fileKey, 4);
	bf.Decrypt(buffer, (length / 8) * 8);
}
