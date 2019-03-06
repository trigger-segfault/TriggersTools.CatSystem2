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
			string arch = (Environment.Is64BitProcess ? "x64" : "x86");
			string path = Path.Combine(CatUtils.TempDir, arch);
			Directory.CreateDirectory(path);

			// Load the embedded zlib1 dll
			string ResPath = $"zlib1.{arch}.dll";
			string dllPath = Path.Combine(path, "zlib1.dll");
			Embedded.LoadNativeDll(ResPath, dllPath);
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
