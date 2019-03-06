// hgx2bmp.h, (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps decompress Windmill's HG-3 (*.hg3) and HG-2 (*.hg2) images.
#pragma once

#ifndef HGX2BMP_H
#define HGX2BMP_H

#include <Windows.h>
#include "asmodean.h"

/* Make sure to free returned rgbaBuffer! */
ASMODEAN_API void ProcessImage(
	unsigned char*  bufferTmp,
	//unsigned long   length,
	unsigned long   origLength,
	unsigned char*  cmdBufferTmp,
	//unsigned long   cmdLength,
	unsigned long   origCmdLength,
	unsigned char*& rgbaBuffer,
	unsigned long&  rgbaLength,
	unsigned long   width,
	unsigned long   height,
	unsigned long   depthBytes);

#endif /* HGX2BMP_H */