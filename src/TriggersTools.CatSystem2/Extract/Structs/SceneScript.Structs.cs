using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header structure for a CatScene script file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16, CharSet = CharSet.Ansi)]
	internal struct CATSCENEHDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/>.
		/// </summary>
		public const string ExpectedSignature = "CatScene";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array for the header's signature. This should be "CatScene".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
		public char[] SignatureRaw; // "CatScene"
		/// <summary>
		///  The size of the CatScene compressed data.
		/// </summary>
		public int CompressedSize;
		/// <summary>
		///  The size of the CatScene decompressed data.
		/// </summary>
		public int DecompressedSize;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the header's signature from the null-terminated character array.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion
	}
	/// <summary>
	///  The script header structure for a CatScene script file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	internal struct SCRIPTHDR {
		#region Constants

		/// <summary>
		///  The size of the <see cref="SCRIPTHDR"/> structure.
		/// </summary>
		public readonly static int CbSize = Marshal.SizeOf<SCRIPTHDR>();

		#endregion

		#region Fields

		public int ScriptLength;

		/// <summary>
		///  Bytecode? String map? Anyway, is the count of the first part of the script after the header... 
		///  every single entry 2 uint... in other words 1 entry = 8 bytes
		/// </summary>
		public int UnkCount;

		public int OffsetTable;
		public int StringTable;

		#endregion

		#region Properties

		public int EntryCount => (StringTable - OffsetTable) / 4;

		#endregion
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct SCENELINE {
		#region Fields

		public ushort Type;
		public string Content;

		#endregion

		#region ToString Override

		public override string ToString() => $"{Type} \"{Content}\"";

		#endregion
	}
}
