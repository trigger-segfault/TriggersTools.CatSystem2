using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;

#if NET451
namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  Implement .NET 4.6's Buffer.MemoryCopy.
	/// </summary>
	internal static class BufferMemoryCopy {

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlockCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count) {
			Array.Copy(src, srcOffset, dst, dstOffset, count);
		}*/

		[DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
		private static unsafe extern void CopyMemory(void* dest, void* src, IntPtr count);

		[SecurityCritical]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy) {
			if (sourceBytesToCopy > destinationSizeInBytes) {
				throw new ArgumentOutOfRangeException("sourceBytesToCopy is greater than destinationSizeInBytes!");
			}
			Memmove((byte*) destination, (byte*) source, sourceBytesToCopy);
		}

		private unsafe static void Memmove(byte* dest, byte* src, long _len) {
			uint len = (uint) _len;
            const uint CopyThreshold = 2048;

			// P/Invoke into the native version when the buffers are overlapping.

			if (((uint) dest - (uint) src < len) || ((uint) src - (uint) dest < len)) goto PInvoke;

			byte* srcEnd = src + len;
			byte* destEnd = dest + len;

			if (len <= 16) goto MCPY02;
			if (len > 64) goto MCPY05;

			MCPY00:
			// Copy bytes which are multiples of 16 and leave the remainder for MCPY01 to handle.
			Contract.Assert(len > 16 && len <= 64);
            *(Block16*)dest = *(Block16*)src;                   // [0,16]
			if (len <= 32) goto MCPY01;
            *(Block16*)(dest + 16) = *(Block16*)(src + 16);     // [0,32]
			if (len <= 48) goto MCPY01;
            *(Block16*)(dest + 32) = *(Block16*)(src + 32);     // [0,48]

		MCPY01:
			// Unconditionally copy the last 16 bytes using destEnd and srcEnd and return.
			Contract.Assert(len > 16 && len <= 64);
            *(Block16*)(destEnd - 16) = *(Block16*)(srcEnd - 16);
			return;

		MCPY02:
			// Copy the first 8 bytes and then unconditionally copy the last 8 bytes and return.
			if ((len & 24) == 0) goto MCPY03;
			Contract.Assert(len >= 8 && len <= 16);

			if (Environment.Is64BitProcess) {
				*(long*) dest = *(long*) src;
				*(long*) (destEnd - 8) = *(long*) (srcEnd - 8);
			}
			else {
				*(int*) dest = *(int*) src;
				*(int*) (dest + 4) = *(int*) (src + 4);
				*(int*) (destEnd - 8) = *(int*) (srcEnd - 8);
				*(int*) (destEnd - 4) = *(int*) (srcEnd - 4);
			}
			return;

		MCPY03:
			// Copy the first 4 bytes and then unconditionally copy the last 4 bytes and return.
			if ((len & 4) == 0) goto MCPY04;
			Contract.Assert(len >= 4 && len < 8);
			*(int*) dest = *(int*) src;
			*(int*) (destEnd - 4) = *(int*) (srcEnd - 4);
			return;

		MCPY04:
			// Copy the first byte. For pending bytes, do an unconditionally copy of the last 2 bytes and return.
			Contract.Assert(len < 4);
			if (len == 0) return;
			*dest = *src;
			if ((len & 2) == 0) return;
			*(short*) (destEnd - 2) = *(short*) (srcEnd - 2);
			return;

		MCPY05:
			// PInvoke to the native version when the copy length exceeds the threshold.
			if (len > CopyThreshold) {
				goto PInvoke;
			}
			// Copy 64-bytes at a time until the remainder is less than 64.
			// If remainder is greater than 16 bytes, then jump to MCPY00. Otherwise, unconditionally copy the last 16 bytes and return.
			Contract.Assert(len > 64 && len <= CopyThreshold);
			uint n = len >> 6;

		MCPY06:
            *(Block64*)dest = *(Block64*)src;
			dest += 64;
			src += 64;
			n--;
			if (n != 0) goto MCPY06;

			len %= 64;
			if (len > 16) goto MCPY00;
            *(Block16*)(destEnd - 16) = *(Block16*)(srcEnd - 16);
			return;

		PInvoke:
			CopyMemory(dest, src, new IntPtr(len));
		}
		
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		private struct Block16 { }

		[StructLayout(LayoutKind.Sequential, Size = 64)]
		private struct Block64 { }
	}
}
#endif
