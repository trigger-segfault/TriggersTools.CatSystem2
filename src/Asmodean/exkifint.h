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

struct KIFENTRYINFO {
	unsigned long  offset;
	unsigned long  length;
};

ASMODEAN_API void DecryptVCode2(
	unsigned char* keyBuffer,
	unsigned long  keyLength,
	unsigned char* vcode2Buffer,
	unsigned long  vcode2Length);

ASMODEAN_API void DecryptEntry(
	KIFENTRYINFO&  entry,
	unsigned long  fileKey);

ASMODEAN_API void DecryptData(
	unsigned char* buffer,
	unsigned long  length,
	unsigned long  fileKey);

#endif /* EXKIFINT_H */