using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header for an HG-2 or HG-3 file. Which is used to identify the header size as well as the signature.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12, CharSet = CharSet.Ansi)]
	internal struct HGXHDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/> for an HG-2 image.
		/// </summary>
		public const string ExpectedHg2Signature = "HG-2";
		/// <summary>
		///  The expected value of <see cref="Signature"/> for an HG-3 image.
		/// </summary>
		public const string ExpectedHg3Signature = "HG-3";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array for the header's signature. This should be "HG-2" or "HG-3".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "HG-2", "HG-3"
		/// <summary>
		///  The size of the HG-X header, including the signature. Should always be 12.
		/// </summary>
		public int HeaderSize;
		/// <summary>
		///  HG-3: 0x300.<para/>
		///  HG-2: 0x25 = 88 bytes (full header), 0x20 = 80 bytes (truncated)
		/// </summary>
		public int Type;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the header's signature from the null-terminated character array.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-X header.
		/// </summary>
		/// <returns>The string representation of the HG-X header.</returns>
		public override string ToString() => $"HGXHDR \"{Signature}\"";

		#endregion
	}
	/// <summary>
	///  Compression image data for an HG-X frame processed with unrle and undeltafilter.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	internal struct HGXIMGDATA {
		#region Fields

		/// <summary>
		///  The length of the compressed unrle copy data.
		/// </summary>
		public int CompressedDataLength;
		/// <summary>
		///  The length of the decompressed unrle copy data.
		/// </summary>
		public int DecompressedDataLength;
		/// <summary>
		///  The length of the compressed unrle copy cmd data.
		/// </summary>
		public int CompressedCmdLength;
		/// <summary>
		///  The length of the decompressed unrle copy cmd data.
		/// </summary>
		public int DecompressedCmdLength;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-X process data.
		/// </summary>
		/// <returns>The string representation of the HG-X process data.</returns>
		public override string ToString() => $"HGXIMGDATA {CompressedDataLength} {DecompressedDataLength} " +
											 $"{CompressedCmdLength} {DecompressedCmdLength}";

		#endregion
	}
}
