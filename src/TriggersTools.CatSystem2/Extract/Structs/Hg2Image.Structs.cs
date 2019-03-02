using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/*/// <summary>
	///  The header for an HG-3 file. Which is used to identify the header size as well as the signature.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct HG2HDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/>.
		/// </summary>
		public const string ExpectedSignature = "HG-2";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array for the header's signature. This should be "HG-2".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "HG-2"
		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		public int HeaderSize;
		/// <summary>
		///  0x25 = 88 bytes (full header), 0x20 = 80 bytes (truncated)
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
		///  Gets the string representation of the HG-2 header.
		/// </summary>
		/// <returns>The string representation of the HG-2 header.</returns>
		public override string ToString() => $"HG2HDR \"{Signature}\"";

		#endregion
	}*/
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct HG2IMG {
		#region Fields

		/// <summary>
		///  The condensed width of the image.
		/// </summary>
		public int Width;
		/// <summary>
		///  The condensed height of the image.
		/// </summary>
		public int Height;

		/// <summary>
		///  The depth of the image format in bits.
		/// </summary>
		public int DepthBits;

		public int Unknown1;
		public int Unknown2;

		public HGXIMGDATA Data;
		/*public int DataLength; // 32
		public int OriginalDataLength;
		public int CmdLength;
		public int OriginalCmdLength;*/
		/// <summary>
		///  Always 40?
		/// </summary>
		public int ExtraLength; // 48
		/// <summary>
		///  The numeric identifier for the image.
		/// </summary>
		public int Id;

		/// <summary>
		///  The total width of the image with <see cref="OffsetX"/> applied.
		/// </summary>
		public int TotalWidth;
		/// <summary>
		/// The height width of the image with <see cref="OffsetY"/> applied.
		/// </summary>
		public int TotalHeight;
		/// <summary>
		///  The horizontal offset of the image from the left.
		/// </summary>
		public int OffsetX;
		/// <summary>
		///  The vertical offset of the image from the top.
		/// </summary>
		public int OffsetY;

		/// <summary>
		///  This is likely a boolean value that determines if transparency is used by the image.
		/// </summary>
		public int IsTransparent;
		/// <summary>
		///  The offset from the start of this structure.
		/// </summary>
		public int OffsetNext;

		/*/// <summary>
		///  The horizontal center of the image. Used for drawing in the game.
		/// </summary>
		public int BaseX;
		/// <summary>
		///  The vertical baseline of the image. Used for drawing in the game.
		/// </summary>
		public int BaseY;*/

		#endregion
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct HG2IMG_EX {
		#region Fields

		public int BaseX;
		public int BaseY;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-2 header.
		/// </summary>
		/// <returns>The string representation of the HG-2 header.</returns>
		public override string ToString() => $"HG2HDR_EX";

		#endregion
	}
}
