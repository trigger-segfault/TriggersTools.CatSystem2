using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Native;

namespace TriggersTools.CatSystem2 {
	partial class Hg3Image {
		private struct BitBuffer {
			#region Fields

			private readonly byte[] buff;
			private int index;
			private int buffIndex;

			#endregion

			public BitBuffer(byte[] buff) {
				this.buff = buff;
				index = 0;
				buffIndex = 0;
			}

			public bool GetBit() {
				unchecked {
					if (index > 7) {
						buffIndex++;
						index = 0;
					}
					
					return ((buff[buffIndex] >> index++) & 1) != 0;
				}
			}

			// Didn't expect to see this in the wild...
			public int GetEliasGammaValue() {
				unchecked {
					int value = 0;
					int digits = 0;

					while (!GetBit()) digits++;

					value = 1 << digits;

					while (digits-- != 0) {
						if (GetBit())
							value |= 1 << digits;
					}

					return value;
				}
			}
		}
		
		private static unsafe void MemorySet(byte* array, int arrayLength, byte value) {
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			int block = 32, index = 0;
			int length = Math.Min(block, arrayLength);

			//Fill the initial array
			while (index < length) {
				array[index++] = value;
			}

			length = arrayLength;
			while (index < length) {
				Buffer.MemoryCopy(array, array + index, arrayLength - index, Math.Min(block, length-index));
				//Buffer.BlockCopy(array, 0, array, index, Math.Min(block, length-index));
				index += block;
				block *= 2;
			}
		}
		
		private static unsafe byte[] Unrle(
			byte* buffer,
			int length,
			byte[] cmdBuffer)
		{
			unchecked {
				fixed (byte* buff = cmdBuffer) {
					int index = 0;
					byte* pBuff = buff;
					bool GetBit() {
						unchecked {
							if (index > 7) {
								pBuff++;
								index = 0;
							}

							return ((*pBuff >> index++) & 1) != 0;
						}
					}
					/*int GetEliasGammaValue() {
						unchecked {
							int value = 0;
							int digits = 0;

							while (!GetBit()) digits++;

							value = 1 << digits;

							while (digits-- != 0) {
								if (GetBit())
									value |= 1 << digits;
							}

							return value;
						}
					}*/
					bool copyFlag = GetBit();
					int outLength;
					//outLength = GetEliasGammaValue();
					{
						int value = 0;
						int digits = 0;
						while (!GetBit()) digits++;
						value = 1 << digits;
						while (digits-- != 0) {
							if (GetBit())
								value |= 1 << digits;
						}

						outLength = value;
					}

					/*BitBuffer cmdBits = new BitBuffer(cmdBuffer);
					bool copyFlag = cmdBits.GetBit();
					outLength = cmdBits.GetEliasGammaValue();*/
					byte[] outBuffer = new byte[outLength];
					fixed (byte* pOutBuffer = outBuffer) {

						for (int i = 0, n = 0; i < outLength; i += n) {
							//n = GetEliasGammaValue();
							int value = 0;
							int digits = 0;
							while (!GetBit()) digits++;
							value = 1 << digits;
							while (digits-- != 0) {
								if (GetBit())
									value |= 1 << digits;
							}
							n = value;
							//n = cmdBits.GetEliasGammaValue();

							if (copyFlag) {
								//memcpy(outBuffer + i, buffer, n);
								//memcpy(pOutBuffer + i, buffer, n);
								Buffer.MemoryCopy(buffer, pOutBuffer + i, outLength - i, n);
								buffer += n;
							}
							else {
								//memset(pOutBuffer + i, 0, n);
								//MemorySet(pOutBuffer + i, n, 0);
								//Already zeroed by C# array init
							}

							copyFlag = !copyFlag;
						}
					}
					return outBuffer;
				}
			}
		}

		/*private static unsafe void Unrle(
			byte* buffer,
			int length,
			byte[] cmdBuffer,
			int cmdLength,
			out byte[] outBuffer,
			out int outLength) {
			unchecked {
				fixed (byte* buff = cmdBuffer) {
					int index = 0;
					byte* pBuff = buff;
					bool GetBit() {
						unchecked {
							if (index > 7) {
								pBuff++;
								index = 0;
							}

							return ((*pBuff >> index++) & 1) != 0;
						}
					}
					int GetEliasGammaValue() {
						unchecked {
							int value = 0;
							int digits = 0;

							while (!GetBit()) digits++;

							value = 1 << digits;

							while (digits-- != 0) {
								if (GetBit())
									value |= 1 << digits;
							}

							return value;
						}
					}
					bool copyFlag = GetBit();
					//outLength = GetEliasGammaValue();
					{
						int value = 0;
						int digits = 0;
						while (!GetBit()) digits++;
						value = 1 << digits;
						while (digits-- != 0) {
							if (GetBit())
								value |= 1 << digits;
						}

						outLength = value;
					}

					BitBuffer cmdBits = new BitBuffer(cmdBuffer);
					bool copyFlag = cmdBits.GetBit();
					outLength = cmdBits.GetEliasGammaValue();
					outBuffer = new byte[outLength];
					fixed (byte* pOutBuffer = outBuffer) {

						for (int i = 0, n = 0; i < outLength; i += n) {
							//n = GetEliasGammaValue();
							int value = 0;
							int digits = 0;
							while (!GetBit()) digits++;
							value = 1 << digits;
							while (digits-- != 0) {
								if (GetBit())
									value |= 1 << digits;
							}
							n = value;
							//n = cmdBits.GetEliasGammaValue();

							if (copyFlag) {
								//memcpy(outBuffer + i, buffer, n);
								//memcpy(pOutBuffer + i, buffer, n);
								Buffer.MemoryCopy(buffer, pOutBuffer + i, outLength - i, n);
								buffer += n;
							}
							else {
								//memset(pOutBuffer + i, 0, n);
								//MemorySet(pOutBuffer + i, n, 0);
								//Already zeroed by C# array init
							}

							copyFlag = !copyFlag;
						}
					}
				}
			}
		}*/

		/*private static unsafe void unrle(
			byte* buffer,
			int length,
			byte[] cmdBuffer,
			int cmdLength,
			out byte[] outBuffer,
			out int outLength) {
			//bitbuff_t cmdBits = new bitbuff_t((IntPtr) cmdBuffer, cmdLength);
			//bitbuff_t cmdBits = new bitbuff_t(cmdBuffer);

			fixed (byte* buff = cmdBuffer) {
				//int len;
				int index = 0;
				//int buffIndex;

				if (index > 7) {
					buff++;
					len--;
					index = 0;
				}

				return (*buff >> index++) & 1;

				bool copyFlag = cmdBits.get_bit();

				outLength = cmdBits.get_elias_gamma_value();
				outBuffer = new byte[outLength];
				fixed (byte* pOutBuffer = outBuffer) {

					for (int i = 0, n = 0; i < outLength; i += n) {
						n = cmdBits.get_elias_gamma_value();

						if (copyFlag) {
							//memcpy(outBuffer + i, buffer, n);
							Buffer.MemoryCopy(buffer, pOutBuffer + i, outLength - i, n);
							buffer += n;
						}
						else {
							//memset(pOutBuffer + i, 0, n);
							//MemorySet(pOutBuffer + i, n, 0);
							//Already zeroed by C# array init
						}

						copyFlag = !copyFlag;
					}
				}
			}
		}*/


		// This encoding tries to optimize for lots of zeros. I think. :)
		private static unsafe byte UnpackVal(uint c) {
			unchecked {
				c &= 0xFF;
				if ((c & 1) != 0)
					return (byte) ((c >> 1) ^ 0xFF);
				else
					return (byte) (c >> 1);
				//uint z = ((c & 1) != 0 ? 0xFF : 0U);
				//return (byte) (((c >> 1) & 0x7F) ^ z);
			}
		}

		private static unsafe void Undeltafilter(
			byte* buffer,
			int length,
			byte* outBuffer,
			int width,
			int height,
			int depthBytes)
		{
			uint[] table1 = new uint[256];
			uint[] table2 = new uint[256];
			uint[] table3 = new uint[256];
			uint[] table4 = new uint[256];
			fixed (uint* pTable1 = table1)
			fixed (uint* pTable2 = table2)
			fixed (uint* pTable3 = table3)
			fixed (uint* pTable4 = table4) {
				unchecked {
					for (uint i = 0; i < 256; i++) {
						uint val = i & 0xC0;

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

					int sect_len = length / 4;
					byte* sect1 = buffer;
					byte* sect2 = sect1 + sect_len;
					byte* sect3 = sect2 + sect_len;
					byte* sect4 = sect3 + sect_len;

					byte* outP = outBuffer;
					byte* outEnd = outBuffer + length;

					while (outP < outEnd) {
						//uint val = table1[*sect1++] | table2[*sect2++] | table3[*sect3++] | table4[*sect4++];
						uint val = pTable1[*sect1++] | pTable2[*sect2++] | pTable3[*sect3++] | pTable4[*sect4++];
						uint c;

						c = val & 0xFF;
						if ((c & 1) != 0)
							*outP++ = (byte) ((c >> 1) ^ 0xFF);
						else
							*outP++ = (byte) (c >> 1);
						c = (val >> 8) & 0xFF;
						if ((c & 1) != 0)
							*outP++ = (byte) ((c >> 1) ^ 0xFF);
						else
							*outP++ = (byte) (c >> 1);
						c = (val >> 16) & 0xFF;
						if ((c & 1) != 0)
							*outP++ = (byte) ((c >> 1) ^ 0xFF);
						else
							*outP++ = (byte) (c >> 1);
						c = (val >> 24) & 0xFF;
						if ((c & 1) != 0)
							*outP++ = (byte) ((c >> 1) ^ 0xFF);
						else
							*outP++ = (byte) (c >> 1);

						/**outP++ = unpack_val((val >> 0));
						*outP++ = unpack_val((val >> 8));
						*outP++ = unpack_val((val >> 16));
						*outP++ = unpack_val((val >> 24));*/
					}

					int stride = width * depthBytes;

					for (int x = depthBytes; x < stride; x++) {
						outBuffer[x] += outBuffer[x - depthBytes];
					}

					for (int y = 1; y < height; y++) {
						byte* line = outBuffer + y * stride;
						byte* prev = line - stride;

						for (int x = 0; x < stride; x++) {
							line[x] += prev[x];
						}
					}
				}
			}
		}


		
		/*private static unsafe void ProcessImageInternal(
			byte[] bufferTmp,
			int length,
			int origLength,
			byte[] cmdBufferTmp,
			int cmdLength,
			int origCmdLength,
			out byte[] rgbaBuffer,
			out int rgbaLength,
			int width,
			int height,
			int depthBytes)*/
		private static unsafe byte[] ProcessImageInternal(
			byte[] buffer,
			byte[] cmdBuffer,
			int width,
			int height,
			int depthBytes)
		{
			//byte[] buffer = ZlibStream.UncompressBuffer(bufferTmp);
			//byte[] buffer = new byte[origLength];
			//IntPtr pOrigLength = new IntPtr(origLength);
			//IntPtr pLength = new IntPtr(length);
			//Zlib.Uncompress(buffer, ref origLength, bufferTmp, length);
			//Zlib.Uncompress(buffer, ref origLength, bufferTmp, length);
			//unsigned char* buffer = new unsigned char[origLength];
			//uncompress(buffer, &origLength, bufferTmp, length);

			//byte[] cmdBuffer = ZlibStream.UncompressBuffer(cmdBufferTmp);
			//byte[] cmdBuffer = new byte[origCmdLength];
			//Zlib.Uncompress(cmdBuffer, ref origCmdLength, cmdBufferTmp, cmdLength);
			//unsigned char* cmdBuffer = new unsigned char[origCmdLength];
			//uncompress(cmdBuffer, &origCmdLength, cmdBufferTmp, cmdLength);
			//byte[] outBuffer;
			fixed (byte* pBuffer = buffer) {
				//fixed (byte* pCmdBuffer = cmdBuffer) {
				//Unrle(pBuffer, buffer.Length, cmdBuffer, cmdBuffer.Length, out byte[] outBuffer, out int outLength);
				byte[] outBuffer = Unrle(pBuffer, buffer.Length, cmdBuffer);
				byte[] rgbaBuffer = new byte[outBuffer.Length];
				//rgbaBuffer = (unsigned char*)GlobalAlloc(GMEM_FIXED, rgbaLength);
				//rgbaBuffer = new unsigned char[rgbaLength];
				/*if (outLength == width * height * 4) {
					undeltafilter(outBuffer, outLength, rgbaBuffer, width, height, depthBytes);
					//printf("\n%i          ", (outLength - width * height * 4));
					//Beep(500, 2000);
				}*/
				fixed (byte* pOutBuffer = outBuffer)
				fixed (byte* pRgbaBuffer = rgbaBuffer)
					Undeltafilter(pOutBuffer, outBuffer.Length, pRgbaBuffer, width, height, depthBytes);
				//undeltafilter(outBuffer, outLength, rgbaBuffer, width, height, depthBytes);
				return rgbaBuffer;
			}
		}
	}
}
