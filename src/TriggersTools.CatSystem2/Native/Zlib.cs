using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Native {
	/// <summary>
	///  A static class for zlib1.dll native methods.
	/// </summary>
	internal static class Zlib {
		#region Static Constructors
		
		static Zlib() {
			// Load the embedded zlib1 dll
			string dllName = "zlib1.dll";
			string dllPath = Path.Combine(CatUtils.TempDir, dllName);
			Embedded.LoadNativeDll(dllName, dllPath);
		}

		#endregion

		[DllImport("zlib1.dll", EntryPoint = "compress", CallingConvention = CallingConvention.Cdecl)]
		public extern static int Compress(
			byte[] dst,
			ref int dstLength,
			byte[] src,
			int srcLength);
		[DllImport("zlib1.dll", EntryPoint = "uncompress", CallingConvention = CallingConvention.Cdecl)]
		public extern static int Uncompress(
			byte[] dst,
			ref int dstLength,
			byte[] src,
			int srcLength);
	}
}
