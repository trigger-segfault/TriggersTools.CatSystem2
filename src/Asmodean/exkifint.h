// exkifint.h (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps extracts data from Windmill's encrypted KIF (*.int) archives.
#pragma once

#ifndef EXKIFINT_H
#define EXKIFINT_H

#include "asmodean.h"
#include "blowfish.h"

/*struct KIFENTRYINFO {
	unsigned long  offset;
	unsigned long  length;
};

ASMODEAN_API void EncryptVCode(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcodeBuffer,
	unsigned long  vcodeLength);

ASMODEAN_API void DecryptVCode(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcodeBuffer,
	unsigned long  vcodeLength);

ASMODEAN_API void DecryptEntry(
	KIFENTRYINFO&  entry,
	unsigned long  fileKey);

ASMODEAN_API void DecryptData(
	unsigned char* buffer,
	unsigned long  length,
	unsigned long  fileKey);*/

ASMODEAN_API void InitializeBlowfish(
	Blowfish& blowfish,
	byte* key,
	uint32 keyLength);

ASMODEAN_API void EncryptBlowfish(
	Blowfish blowfish,
	byte* buffer,
	uint32 bufferLength);

ASMODEAN_API void DecryptBlowfish(
	Blowfish blowfish,
	byte* buffer,
	uint32 bufferLength);

#endif /* EXKIFINT_H */