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

#include "asmodean.h"

/*
 * Return codes for ProcessImage
 */
enum class ReturnCode : uint32 {
	/* ProcessImage returned successfully. */
	Success = 0xFFFFFFFF,

	/* dataBuffer, cmdBuffer, or unrleBuffer ran out of data. */
	UnrleDataIsCorrupt = 0x0001,
	/* dataBuffer ran out of data. */
	DataBufferTooSmall = 0x0002,
	/* cmdBuffer ran out of data. */
	CmdBufferTooSmall = 0x0003,
	/* unrleBuffer ran out of data. */
	UnrleBufferTooSmall = 0x0004,
	/* rgbaBuffer length does not fit the unrleBuffer length. Must be at least 1024 bytes in size. */
	RgbaBufferTooSmall = 0x0005,

	/* Combined dimensions create too-large of an image. */
	DimensionsTooLarge = 0x0010,
	/* At least one of the dimensions is zero. */
	InvalidDimensions = 0x0020,
	/* depthBytes must be between 1 and 4. */
	InvalidDepthBytes = 0x0030,

	/* dataBuffer is null. */
	DataBufferIsNull = 0x0100,
	/* cmdBuffer is null. */
	CmdBufferIsNull = 0x0200,
	/* rgbaBuffer is null. */
	RgbaBufferIsNull = 0x0300,

	/* Failed to allocated unrleBuffer. */
	AllocationFailed = 0x1000,
};

// Constants

/* 16384 * 16384 * 4. */
const uint32 MAX_RGBA_LENGTH = 16384 * 16384 * 4;
const uint32 MIN_DEPTH_BYTES = 1;
const uint32 MAX_DEPTH_BYTES = 4;
const uint32 TABLE_SIZE = 256;
const uint32 INVALID_ELIAS_GAMMA = 0xFFFFFFFF;

/* 
 * Process an HG-2 image data or HG-3 image's "img####" tag data.
 * 
 * dataBuffer and cmdBuffer are both decompressed zlib buffers read from the HG-X.
 * rgbaBuffer must be initialized by the program with zeroed bytes before being passed.
 * rgbaBuffer must always be at least 1024 bytes in size.
 */
ASMODEAN_API ReturnCode ProcessImage(
	//byte*   dataBufferTmp,
	//uint32  compressedDataLength,
	//uint32  decompressedDataLength,
	//byte*   cmdBufferTmp,
	//uint32  compressedCmdLength,
	//uint32  decompressedCmdLength,
	byte*   dataBuffer,
	uint32  dataLength,
	byte*   cmdBuffer,
	uint32  cmdLength,
	byte*   rgbaBuffer,
	uint32  rgbaLength,
	uint32  width,
	uint32  height,
	uint32  depthBytes,
	uint32  stride);

#endif /* HGX2BMP_H */