using System.Runtime.InteropServices;
using System.Text;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header structure for a KIFINT archive.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8, CharSet = CharSet.Ansi)]
	internal struct KIFHDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/>.
		/// </summary>
		public const string ExpectedSignature = "KIF";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array signature of the file.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "KIF\0"
		/// <summary>
		///  The number of <see cref="KIFENTRY"/>s in the KIFINT archive.
		/// </summary>
		public int EntryCount;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the signature of the file.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion
	}
	/// <summary>
	///  The entry structure for a KIFINT archive.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 72, CharSet = CharSet.Ansi)]
	internal struct KIFENTRY {
		#region Fields
			
		/// <summary>
		///  The raw character array filename of the entry.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
		public byte[] FileNameRaw;
		/// <summary>
		///  We don't need to pass the <see cref="FileNameRaw"/> during P/Invoke, so we have this info structure,
		///  which is the same as accessing <see cref="Offset"/> and <see cref="Length"/> directly.
		/// </summary>
		[FieldOffset(64)]
		public KIFENTRYINFO Info;
		/// <summary>
		///  The file offset to the entry's data.
		/// </summary>
		[FieldOffset(64)]
		public uint Offset;
		/// <summary>
		///  The file length to the entry's data.
		/// </summary>
		[FieldOffset(68)]
		public int Length;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the filename of the entry.
		/// </summary>
		public string FileName => FileNameRaw.ToNullTerminatedString(CatUtils.ShiftJIS);

		#endregion
	}
	/// <summary>
	///  We don't need to pass the <see cref="KIFENTRY.FileNameRaw"/> during P/Invoke, so we have this info
	///  structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	internal struct KIFENTRYINFO {
		/// <summary>
		///  The file offset to the entry's data.
		/// </summary>
		public uint Offset;
		/// <summary>
		///  The file length to the entry's data.
		/// </summary>
		public int Length;
	}
}
