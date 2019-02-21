using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Native {
	/// <summary>
	///  A static class for zlib1.dll native methods.
	/// </summary>
	internal static class ZLib1 {
		[DllImport("zlib1.dll", EntryPoint = "uncompress", CallingConvention = CallingConvention.Cdecl)]
		public extern static int Uncompress(
			byte[] dst,
			ref int dstLength,
			byte[] src,
			int srcLength);
	}
}
