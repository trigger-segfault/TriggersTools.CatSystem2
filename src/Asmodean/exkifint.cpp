// exkifint.cpp (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps extracts data from Windmill's encrypted KIF (*.int) archives.
#include "exkifint.h"

/*void EncryptVCode(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcodeBuffer,
	unsigned long  vcodeLength)
{
	Blowfish bf;
	bf.Set_Key(keyBuffer, keyLength);
	bf.Encrypt(vcodeBuffer, vcodeLength);
}

void DecryptVCode(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcodeBuffer,
	unsigned long  vcodeLength)
{
	Blowfish bf;
	bf.Set_Key(keyBuffer, keyLength);
	bf.Decrypt(vcodeBuffer, vcodeLength);
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
}*/

void InitializeBlowfish(
	Blowfish& blowfish,
	byte* key,
	uint32 keyLength)
{
	blowfish.Set_Key(key, keyLength);
}

void EncryptBlowfish(
	Blowfish blowfish,
	byte* buffer,
	uint32 bufferLength)
{
	blowfish.Encrypt(buffer, bufferLength);
}

void DecryptBlowfish(
	Blowfish blowfish,
	byte* buffer,
	uint32 bufferLength)
{
	blowfish.Decrypt(buffer, bufferLength);
}
