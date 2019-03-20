using System;
using System.Runtime.CompilerServices;
using TriggersTools.CatSystem2.Exceptions;
using TriggersTools.CatSystem2.Utils;

#if NET451
using Buffer = TriggersTools.CatSystem2.Utils.BufferMemoryCopy;
#endif

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region ProcessImageManaged

		/// <summary>
		///  Processes an HG-2 or standard HG-3 image in a managed context.
		/// </summary>
		/// <param name="dataBuffer">The decompressed data buffer.</param>
		/// <param name="cmdBuffer">The decompressed command buffer.</param>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		/// <param name="depthBytes">The depth of the image in bytes.</param>
		/// <param name="stride">The stride of the image.</param>
		/// <returns>The processed pixel array of the image.</returns>
		private static unsafe byte[] ProcessImageManaged(byte[] dataBuffer, byte[] cmdBuffer, int width, int height,
			int depthBytes, int stride)
		{
			byte[] unrleBuffer = Unrle(dataBuffer, cmdBuffer);
			byte[] rgbaBuffer = Undeltafilter(unrleBuffer, width, height, depthBytes, stride);

			return rgbaBuffer;
		}

		#endregion

		#region BitBuffer

		/// <summary>
		///  Gets the next bit from the command bit buffer.
		/// </summary>
		/// <param name="cmd">The pointer to the current byte in the bit buffer.</param>
		/// <param name="cmdLength">The remaining length of the bit buffer.</param>
		/// <param name="cmdBitIndex">The current bit index in the bit buffer.</param>
		/// <returns>True if the bit at the current index was one, otherwise false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool GetCmdBit(ref byte* cmd, ref int cmdLength, ref int cmdBitIndex) {
			if (cmdBitIndex > 7) {
				if (--cmdLength == 0)
					throw new HgxException("dataBuffer, cmdBuffer, or unrleBuffer ran out of data!");
				cmd++;
				cmdBitIndex = 0;
			}

			return ((*cmd >> cmdBitIndex++) & 1) != 0;
		}
		/// <summary>
		///  Gets the next elias gamma value from the command bit buffer.
		/// </summary>
		/// <param name="cmd">The pointer to the current byte in the bit buffer.</param>
		/// <param name="cmdLength">The remaining length of the bit buffer.</param>
		/// <param name="cmdBitIndex">The current bit index in the bit buffer.</param>
		/// <returns>The elias gamma value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe uint GetCmdEliasGammaValue(ref byte* cmd, ref int cmdLength, ref int cmdBitIndex) {
			unchecked {
				uint value = 0;
				int digits = 0;

				while (!GetCmdBit(ref cmd, ref cmdLength, ref cmdBitIndex)) digits++;

				value = 1U << digits;

				while (digits-- != 0) {
					if (GetCmdBit(ref cmd, ref cmdLength, ref cmdBitIndex))
						value |= 1U << digits;
				}

				return value;
			}
		}

		#endregion

		#region Unrle

		private static unsafe byte[] Unrle(byte[] dataBuffer, byte[] cmdBuffer) {
			fixed (byte* pDataBuffer = dataBuffer)
			fixed (byte* pCmdBuffer = cmdBuffer) {
				unchecked {
					// Local BitBuffer
					byte* cmd = pCmdBuffer;
					int cmdLength = cmdBuffer.Length;
					int cmdBitIndex = 0;

					bool copyFlag = GetCmdBit(ref cmd, ref cmdLength, ref cmdBitIndex);

					uint unrleLength = GetCmdEliasGammaValue(ref cmd, ref cmdLength, ref cmdBitIndex);
					byte[] unrleBuffer = new byte[unrleLength];

					fixed (byte* pUnrleBuffer = unrleBuffer) {
						uint dataLeft = (uint) dataBuffer.LongLength;
						uint unrleLeft = (uint) unrleBuffer.LongLength;
						byte* data = pDataBuffer;
						uint n;
						for (uint i = 0; i < unrleLength; i += n) {
							n = GetCmdEliasGammaValue(ref cmd, ref cmdLength, ref cmdBitIndex);

							if (copyFlag) {
								if (unrleLeft < n || dataLeft < n)
									throw new HgxException("dataBuffer, cmdBuffer, or unrleBuffer ran out of data!");
								dataLeft -= n;
								Buffer.MemoryCopy(data, pUnrleBuffer + i, unrleLeft, n);
								data += n;
							}

							unrleLeft -= n;
							copyFlag = !copyFlag;
						}

						return unrleBuffer;
					}
				}
			}
		}

		#endregion

		#region Undeltafilter

		private static unsafe byte[] Undeltafilter(byte[] unrleBuffer, int width, int height, int depthBytes,
			int stride)
		{
			uint[] table1 = new uint[Asmodean.TableSize];
			uint[] table2 = new uint[Asmodean.TableSize];
			uint[] table3 = new uint[Asmodean.TableSize];
			uint[] table4 = new uint[Asmodean.TableSize];

			byte[] rgbaBuffer = new byte[unrleBuffer.Length];

			fixed (uint* pTable1 = table1)
			fixed (uint* pTable2 = table2)
			fixed (uint* pTable3 = table3)
			fixed (uint* pTable4 = table4)
			fixed (byte* pUnrleBuffer = unrleBuffer)
			fixed (byte* pRgbaBuffer = rgbaBuffer) {
				unchecked {
					for (uint i = 0; i < Asmodean.TableSize; i++) {
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

					int sectLength = unrleBuffer.Length / 4;
					byte* sect1 = pUnrleBuffer;
					byte* sect2 = sect1 + sectLength;
					byte* sect3 = sect2 + sectLength;
					byte* sect4 = sect3 + sectLength;

					byte* outP = pRgbaBuffer;
					byte* outEnd = pRgbaBuffer + unrleBuffer.Length;
					
					while (outP < outEnd) {
						uint val = pTable1[*sect1++] | pTable2[*sect2++] | pTable3[*sect3++] | pTable4[*sect4++];

						// UnpackValue(byte)
						// This encoding tries to optimize for lots of zeros. I think. :)
						*outP++ = (byte) ((val & 0x00000001) != 0 ? (((val & 0x000000FF) >>  1) ^ 0xFF) : ((val & 0x000000FF) >>  1));
						*outP++ = (byte) ((val & 0x00000100) != 0 ? (((val & 0x0000FF00) >>  9) ^ 0xFF) : ((val & 0x0000FF00) >>  9));
						*outP++ = (byte) ((val & 0x00010000) != 0 ? (((val & 0x00FF0000) >> 17) ^ 0xFF) : ((val & 0x00FF0000) >> 17));
						*outP++ = (byte) ((val & 0x01000000) != 0 ? (((val             ) >> 25) ^ 0xFF) : ((val             ) >> 25));
					}
					
					for (int x = depthBytes; x < stride; x++) {
						rgbaBuffer[x] += rgbaBuffer[x - depthBytes];
					}

					for (int y = 1; y < height; y++) {
						byte* line = pRgbaBuffer + y * stride;
						byte* prev = line - stride;

						for (int x = 0; x < stride; x++) {
							line[x] += prev[x];
						}
					}
				}
			}

			return rgbaBuffer;
		}

		#endregion
	}
}
