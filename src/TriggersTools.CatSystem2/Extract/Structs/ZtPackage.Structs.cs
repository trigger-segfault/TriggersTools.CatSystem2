using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header for a ZT package file entry.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
	internal struct ZTENTRYHDR {
		#region Fields

		/// <summary>
		///  The offset to the next file entry. Zero if there is no next entry.
		/// </summary>
		public int OffsetNext;
		/// <summary>
		///  Unknown 4-byte value, ztpack.exe throws a fit when it's changed though.
		/// </summary>
		public int Unknown;
		/// <summary>
		///  The length of this file entry after the header.
		/// </summary>
		public int Length;

		#endregion
	}
	/// <summary>
	///  The entry information for a ZT package file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 272, CharSet = CharSet.Ansi)]
	internal struct ZTENTRY {
		#region Constants

		/// <summary>
		///  The size of this structure in bytes.
		/// </summary>
		public static readonly int CbSize = Marshal.SizeOf<ZTENTRY>();

		#endregion

		#region Fields

		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		public int Unknown;
		/// <summary>
		///  The raw character array filename of the entry.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 256)]
		public byte[] FileNameRaw;
		/// <summary>
		///  Unknown 4-byte value 2, ztpack.exe does not complain when value is changed.
		/// </summary>
		public int Reserved;
		/// <summary>
		///  The size of the compressed file data.
		/// </summary>
		public int CompressedLength;
		/// <summary>
		///  The size of the decompressed file data.
		/// </summary>
		public int DecompressedLength;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the filename of the entry.
		/// </summary>
		public string FileName => FileNameRaw.ToNullTerminatedString(CatUtils.ShiftJIS);

		#endregion
	}
}
