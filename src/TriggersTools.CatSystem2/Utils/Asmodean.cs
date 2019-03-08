using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  A static class for asmodean.dll native methods.
	/// </summary>
	internal static class Asmodean {
		#region Static Constructors

		static Asmodean() {
			// Make sure the zlib1.dll is extracted because asmodean.dll needs it.
			int unused = 0;
			try {
				Zlib.Compress(Array.Empty<byte>(), ref unused, Array.Empty<byte>(), 0);
			} catch { }
			string arch = (Environment.Is64BitProcess ? "x64" : "x86");
			string path = Path.Combine(CatUtils.TempDir, arch);
			Directory.CreateDirectory(path);

			// Load the embedded asmodean dll
			string ResPath = $"asmodean.{arch}.dll";
			string dllPath = Path.Combine(path, "asmodean.dll");
			Embedded.LoadNativeDll(ResPath, dllPath);
		}

		#endregion

		[DllImport("asmodean.dll", CallingConvention = CallingConvention.Cdecl)]
		public extern static void DecryptEntry(
			ref ulong entryInfo,
			uint fileKey);

		[DllImport("asmodean.dll", CallingConvention = CallingConvention.Cdecl)]
		public extern static void DecryptData(
			byte[] buffer,
			int length,
			uint fileKey);

		[DllImport("asmodean.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void EncryptVCode(
			byte[] keyBuffer,
			int keyLength,
			byte[] vcodeBuffer,
			int vcodeLength);

		[DllImport("asmodean.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public extern static void DecryptVCode(
			byte[] keyBuffer,
			int keyLength,
			byte[] vcodeBuffer,
			int vcodeLength);

		
		/// <summary>
		///  Return codes for <see cref="ProcessImageNative"/>.
		/// </summary>
		public enum ReturnCode : uint {
			/// <summary><see cref="ProcessImageNative"/> returned successfully.</summary>
			Success = 0xFFFFFFFF,
			
			/// <summary>dataBuffer, cmdBuffer, or unrleBuffer ran out of data.</summary>
			UnrleDataIsCorrupt = 0x0001,
			/// <summary>dataBuffer ran out of data.</summary>
			DataBufferTooSmall = 0x0002,
			/// <summary>cmdBuffer ran out of data.</summary>
			CmdBufferTooSmall = 0x0003,
			/// <summary>unrleBuffer ran out of data.</summary>
			UnrleBufferTooSmall = 0x0004,
			/// <summary>
			///  rgbaBuffer length does not fit the unrleBuffer length. Must be at least 1024 bytes in size.
			/// </summary>
			RgbaBufferTooSmall = 0x0005,

			/// <summary>Combined dimensions create too-large of an image.</summary>
			DimensionsTooLarge = 0x0010,
			/// <summary>At least one of the dimensions is zero.</summary>
			InvalidDimensions = 0x0020,
			/// <summary>depthBytes must be between 1 and 4.</summary>
			InvalidDepthBytes = 0x0030,
			
			/// <summary>dataBuffer is null.</summary>
			DataBufferIsNull = 0x0100,
			/// <summary>cmdBuffer is null.</summary>
			CmdBufferIsNull = 0x0200,
			/// <summary>rgbaBuffer is null.</summary>
			RgbaBufferIsNull = 0x0300,
			
			/// <summary>Failed to allocated unrleBuffer.</summary>
			AllocationFailed = 0x1000,
		}
		
		public const int MaxRgbaLength = 16384 * 16384 * 4;
		public const int MinDepthBytes = 1;
		public const int MaxDepthBytes = 4;
		public const int TableSize = 256;

		/*[DllImport("asmodean.dll", EntryPoint = "ProcessImage", CallingConvention = CallingConvention.Cdecl)]
		public extern static void ProcessImageNative(
			byte[] bufferTmp,
			//int length,
			int origLength,
			byte[] cmdBufferTmp,
			//int cmdLength,
			int origCmdLength,
			out IntPtr pRgbaBuffer,
			out int rgbaLength,
			int width,
			int height,
			int depthBytes);*/
		[DllImport("asmodean.dll", EntryPoint = "ProcessImage", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.U4)]
		public extern static ReturnCode ProcessImageNative(
			byte[] dataBuffer,
			int    dataLength,
			byte[] cmdBuffer,
			int    cmdLength,
			byte[] rgbaBuffer,
			int    rgbaLength,
			int    width,
			int    height,
			int    depthBytes,
			int    stride);
	}
}
