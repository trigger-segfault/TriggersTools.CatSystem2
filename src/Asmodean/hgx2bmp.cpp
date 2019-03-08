// hgx2bmp.cpp, (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps decompress Windmill's HG-3 (*.hg3) and HG-2 (*.hg2) images.
#include "hgx2bmp.h"
#include <string.h>
#include <stdio.h>

class BitBuffer {
private:
	uint32  index;
	byte*   buffer;
	int32   length;

public:
	BitBuffer(byte* buffer, uint32 length)
		: buffer(buffer), length((int32) length), index(0)
	{
	}

	bool getBit() {
		if (index > 7) {
			// Have we hit the endReached of the buffer?
			if (--length <= 0) {
				// Set to one so we keep hitting this condition.
				length = 0;
				// Return true to force Elias Gamma Value to end.
				// Make sure index stays > 7 to keep throwing this.
				return true;
			}
			buffer++;
			index = 0;
		}

		return (*buffer >> index++) & 1;
	}

	// Didn't expect to see this in the wild...
	uint32 getEliasGammaValue() {
		uint32 value, digits = 0;

		while (!getBit()) digits++;

		value = 1 << digits;

		while (digits--) {
			if (getBit())
				value |= 1 << digits;
		}

		if (length)
			return value;
		return INVALID_ELIAS_GAMMA;
	}
};

ReturnCode Unrle(
	byte*    dataBuffer,
	uint32   dataLength,
	byte*    cmdBuffer,
	uint32   cmdLength,
	byte*&   unrleBuffer,
	uint32&  unrleOutLength)
{
	BitBuffer cmdBits(cmdBuffer, cmdLength);

	bool copyFlag = cmdBits.getBit();

	unrleOutLength = cmdBits.getEliasGammaValue();
	if (unrleOutLength == INVALID_ELIAS_GAMMA)
		return ReturnCode::UnrleDataIsCorrupt;

	// Initialize the array to zeroed bytes all at once,
	// this way we won't need to excessively call memset(0).
	unrleBuffer = new byte[unrleOutLength] { 0 };
	if (unrleBuffer == nullptr)
		return ReturnCode::AllocationFailed;

	uint32 n;
	uint32 unrleLength = unrleOutLength;
	uint32 unrleLeft = unrleOutLength;
	uint32 dataLeft = dataLength;
	for (int32 i = 0; i < unrleLength; i += n) {
		n = cmdBits.getEliasGammaValue();

		if (copyFlag) {
			// Out-of-range checks
			if (unrleLeft < n || dataLeft < n)
				return ReturnCode::UnrleDataIsCorrupt;
			dataLeft -= n;

			memcpy(unrleBuffer + i, dataBuffer, n);
			dataBuffer += n;
		}

		unrleLeft -= n;
		copyFlag = !copyFlag;
	}

	return ReturnCode::Success;
}

/* This encoding tries to optimize for lots of zeros. I think. :) */
byte UnpackValue(byte c) {
	return ((c & 1) ? ((c >> 1) ^ 0xFF) : (c >> 1));
}

void Undeltafilter(
	byte*   unrleBuffer,
	uint32  unrleLength,
	byte*   rgbaBuffer,
	uint32  width,
	uint32  height,
	uint32  depthBytes,
	uint32  stride)
{
	uint32 table1[TABLE_SIZE];// = { 0 };
	uint32 table2[TABLE_SIZE];// = { 0 };
	uint32 table3[TABLE_SIZE];// = { 0 };
	uint32 table4[TABLE_SIZE];// = { 0 };

	for (uint32 i = 0; i < TABLE_SIZE; i++) {
		uint32 val = i & 0xC0;

		val <<= 6;
		val |= i & 0x30;

		val <<= 6;
		val |= i & 0x0C;

		val <<= 6;
		val |= i & 0x03;

		table4[i] = val;
		table3[i] = val << 2;
		table2[i] = val << 4;
		table1[i] = val << 6;
	}

	uint32 sectLength = unrleLength / 4;
	byte*  sect1 = unrleBuffer;
	byte*  sect2 = sect1 + sectLength;
	byte*  sect3 = sect2 + sectLength;
	byte*  sect4 = sect3 + sectLength;

	byte*  outP   = rgbaBuffer;
	byte*  outEnd = rgbaBuffer + unrleLength;

	while (outP < outEnd) {
		uint32 val = table1[*sect1++] | table2[*sect2++] | table3[*sect3++] | table4[*sect4++];

		*outP++ = UnpackValue((byte) (val >> 0));
		*outP++ = UnpackValue((byte) (val >> 8));
		*outP++ = UnpackValue((byte) (val >> 16));
		*outP++ = UnpackValue((byte) (val >> 24));
	}

	for (uint32 x = depthBytes; x < stride; x++) {
		rgbaBuffer[x] += rgbaBuffer[x - depthBytes];
	}

	for (uint32 y = 1; y < height; y++) {
		byte* line = rgbaBuffer + y * stride;
		byte* prev = line - stride;

		for (uint32 x = 0; x < stride; x++) {
			line[x] += prev[x];
		}
	}
}

ReturnCode ProcessImage(
	byte*   dataBuffer,
	uint32  dataLength,
	byte*   cmdBuffer,
	uint32  cmdLength,
	byte*   rgbaBuffer,
	uint32  rgbaLength,
	uint32  width,
	uint32  height,
	uint32  depthBytes,
	uint32  stride)
{
	ReturnCode result = ReturnCode::Success;

	// Murphy's law safety checks
	if (dataBuffer == nullptr)
		return ReturnCode::DataBufferIsNull;
	if (cmdBuffer == nullptr)
		return ReturnCode::CmdBufferIsNull;
	if (rgbaBuffer == nullptr)
		return ReturnCode::RgbaBufferIsNull;

	if (rgbaLength > MAX_RGBA_LENGTH)
		return ReturnCode::DimensionsTooLarge;
	if (width == 0 || height == 0)
		return ReturnCode::InvalidDimensions;
	if (depthBytes == 0 || depthBytes > 4)
		return ReturnCode::InvalidDepthBytes;

	uint32  unrleLength = 0;
	byte*   unrleBuffer = nullptr;
	result = Unrle(dataBuffer, dataLength, cmdBuffer, cmdLength, unrleBuffer, unrleLength);
	if (result != ReturnCode::Success) {
		// Cleanup
		delete[] unrleBuffer;
		return result;
	}
	if (rgbaLength < unrleLength) {
		// Cleanup
		delete[] unrleBuffer;
		return ReturnCode::RgbaBufferTooSmall;
	}

	Undeltafilter(unrleBuffer, unrleLength, rgbaBuffer, width, height, depthBytes, stride);

	// Cleanup
	delete[] unrleBuffer;

	return ReturnCode::Success;
}
