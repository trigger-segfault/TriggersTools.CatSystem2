﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2.Native {
	/// <summary>
	///  A static class for asmodean.dll native methods.
	/// </summary>
	internal static class Asmodean {
		[DllImport("asmodean.dll", CallingConvention = CallingConvention.Cdecl)]
		public extern static void DecryptEntry(
			ref KIFENTRYINFO entry,
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

		[DllImport("asmodean.dll", EntryPoint = "ProcessImage", CallingConvention = CallingConvention.Cdecl)]
		public extern static void ProcessImageNative(
			byte[] bufferTmp,
			int length,
			int origLength,
			byte[] cmdBufferTmp,
			int cmdLength,
			int origCmdLength,
			out IntPtr pRgbaBuffer,
			out int rgbaLength,
			int width,
			int height,
			int depthBytes);
	}
}
