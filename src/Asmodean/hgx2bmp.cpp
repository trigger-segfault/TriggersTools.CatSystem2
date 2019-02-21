// hgx2bmp.cpp, (for P/Invoke), 2018/6/5
// originally code by asmodean

// contact: 
//   web:   http://asmodean.reverse.net
//   email: asmodean [at] hush.com
//   irc:   asmodean on efnet (irc.efnet.net)

// This code helps decompress Windmill's HG-3 (*.hg3) and HG-2 (*.hg2) images.
#include "hgx2bmp.h"
#include <string.h>
#include <Windows.h>
#include <stdio.h>
#include "zlib.h"

class bitbuff_t {
public:
	bitbuff_t(unsigned char* buff, unsigned long len)
		: buff(buff),
		len(len),
		index(0) {
	}

	bool get_bit(void) {
		if (index > 7) {
			buff++;
			len--;
			index = 0;
		}

		return (*buff >> index++) & 1;
	}

	// Didn't expect to see this in the wild...
	unsigned long get_elias_gamma_value(void) {
		unsigned long value = 0;
		unsigned long digits = 0;

		while (!get_bit()) digits++;

		value = 1 << digits;

		while (digits--) {
			if (get_bit()) {
				value |= 1 << digits;
			}
		}

		return value;
	}

private:
	unsigned long  index;
	unsigned char* buff;
	unsigned long  len;
};

// This encoding tries to optimize for lots of zeros. I think. :)
unsigned char unpack_val(unsigned char c) {
	unsigned char z = c & 1 ? 0xFF : 0;
	return (c >> 1) ^ z;
}

void unrle(
	unsigned char*  buffer,
	unsigned long   length,
	unsigned char*  cmdBuffer,
	unsigned long   cmdLength,
	unsigned char*& outBuffer,
	unsigned long&  outLength)
{
	bitbuff_t cmdBits(cmdBuffer, cmdLength);

	bool copyFlag = cmdBits.get_bit();

	outLength = cmdBits.get_elias_gamma_value();
	outBuffer = new unsigned char[outLength];

	unsigned long n = 0;
	for (unsigned long i = 0; i < outLength; i += n) {
		n = cmdBits.get_elias_gamma_value();

		if (copyFlag) {
			memcpy(outBuffer + i, buffer, n);
			buffer += n;
		}
		else {
			memset(outBuffer + i, 0, n);
		}

		copyFlag = !copyFlag;
	}
}

void undeltafilter(
	unsigned char* buffer,
	unsigned long  length,
	unsigned char* outBuffer,
	unsigned long  width,
	unsigned long  height,
	unsigned long  depthBytes)
{
	unsigned long table1[256] = { 0 };
	unsigned long table2[256] = { 0 };
	unsigned long table3[256] = { 0 };
	unsigned long table4[256] = { 0 };

	for (unsigned long i = 0; i < 256; i++) {
		unsigned long val = i & 0xC0;

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

	unsigned long  sect_len = length / 4;
	unsigned char* sect1 = buffer;
	unsigned char* sect2 = sect1 + sect_len;
	unsigned char* sect3 = sect2 + sect_len;
	unsigned char* sect4 = sect3 + sect_len;

	unsigned char* outP = outBuffer;
	unsigned char* outEnd = outBuffer + length;

	while (outP < outEnd) {
		unsigned long val = table1[*sect1++] | table2[*sect2++] | table3[*sect3++] | table4[*sect4++];

		*outP++ = unpack_val((unsigned char)(val >> 0));
		*outP++ = unpack_val((unsigned char)(val >> 8));
		*outP++ = unpack_val((unsigned char)(val >> 16));
		*outP++ = unpack_val((unsigned char)(val >> 24));
	}

	unsigned long stride = width * depthBytes;

	for (unsigned long x = depthBytes; x < stride; x++) {
		outBuffer[x] += outBuffer[x - depthBytes];
	}

	for (unsigned long y = 1; y < height; y++) {
		unsigned char* line = outBuffer + y * stride;
		unsigned char* prev = outBuffer + (y - 1) * stride;

		for (unsigned long x = 0; x < stride; x++) {
			line[x] += prev[x];
		}
	}
}

void ProcessImage(
	unsigned char*  bufferTmp,
	unsigned long   length,
	unsigned long   origLength,
	unsigned char*  cmdBufferTmp,
	unsigned long   cmdLength,
	unsigned long   origCmdLength,
	unsigned char*& rgbaBuffer,
	unsigned long&  rgbaLength,
	unsigned long   width,
	unsigned long   height,
	unsigned long   depthBytes)
{
	unsigned char* buffer = new unsigned char[origLength];
	uncompress(buffer, &origLength, bufferTmp, length);

	unsigned char* cmdBuffer = new unsigned char[origCmdLength];
	uncompress(cmdBuffer, &origCmdLength, cmdBufferTmp, cmdLength);

	unsigned long  outLength = 0;
	unsigned char* outBuffer = nullptr;
	unrle(buffer, origLength, cmdBuffer, origCmdLength, outBuffer, outLength);

	rgbaLength = outLength;
	rgbaBuffer = (unsigned char*)GlobalAlloc(GMEM_FIXED, rgbaLength);
	//rgbaBuffer = new unsigned char[rgbaLength];
	/*if (outLength == width * height * 4) {
		undeltafilter(outBuffer, outLength, rgbaBuffer, width, height, depthBytes);
		//printf("\n%i          ", (outLength - width * height * 4));
		//Beep(500, 2000);
	}*/
	undeltafilter(outBuffer, outLength, rgbaBuffer, width, height, depthBytes);

	delete[] outBuffer;
	delete[] cmdBuffer;
	delete[] buffer;
}
