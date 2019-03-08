using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header structure for a FES script file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16, CharSet = CharSet.Ansi)]
	internal struct FESHDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/>.
		/// </summary>
		public const string ExpectedSignature = "FES";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array for the header's signature. This should be "FES".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "FES\0"
		/// <summary>
		///  The size of the CatScene compressed data.
		/// </summary>
		public int CompressedLength;
		/// <summary>
		///  The size of the CatScene decompressed data.
		/// </summary>
		public int DecompressedLength;
		/// <summary>
		///  Reserved for future use.
		/// </summary>
		public int Reserved;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the header's signature from the null-terminated character array.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion
	}
}
