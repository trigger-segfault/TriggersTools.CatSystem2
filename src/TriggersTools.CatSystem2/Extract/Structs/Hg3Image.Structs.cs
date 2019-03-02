using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
		///  The raw character array for the header's signature. This should be "HG-3".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "HG-3"
		/// <summary>
		///  The size of the HG-3 header, including the signature. Should always be 12.
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
		///  Gets the string representation of the HG-3 header.
		/// </summary>
		/// <returns>The string representation of the HG-3 header.</returns>
		public override string ToString() => $"HGXHDR \"{Signature}\"";

		#endregion
	}
	/// <summary>
	///  The frame header structure for an HG-3 file. Which is used to get the additional offset for the next entry and
	///  the identifier of the current entry.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	internal struct HG3FRAMEHDR {
		#region Fields

		/// <summary>
		///  The offset from the current stream position to the next frame. Zero if there is no more frames.
		/// </summary>
		public int OffsetNext;
		/// <summary>
		///  The numeric identifier for the image.
		/// </summary>
		public int Id;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 offset.
		/// </summary>
		/// <returns>The string representation of the HG-3 offset.</returns>
		public override string ToString() => $"HG3OFFSET OffsetNext={OffsetNext} Id={Id}";

		#endregion
	}
	/// <summary>
	///  A tag for a specific entry in an HG-3 file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16, CharSet = CharSet.Ansi)]
	internal struct HG3TAG {
		#region Fields

		/// <summary>
		///  The raw character array for the tag's signature.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
		public char[] SignatureRaw;
		/// <summary>
		///  Gets the offset of the next tag in the HG-3 stream.
		/// </summary>
		public int OffsetNext;
		/// <summary>
		///  Gets the length of the tag's structure preceding this tag in the stream.
		/// </summary>
		public int Length;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the tag's signature from the null-terminated character array.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 entry tag.
		/// </summary>
		/// <returns>The string representation of the HG-3 entry tag.</returns>
		public override string ToString() => $"\"{Signature}\" OffsetNext={OffsetNext} Length={Length}";

		#endregion
	}
	/// <summary>
	///  Standard HG-3 image info, this tag uses the tag name
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 40)]
	internal struct HG3STDINFO {
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
		/// <summary>
		///  The horizontal offset of the image from the left.
		/// </summary>
		public int OffsetX;
		/// <summary>
		///  The vertical offset of the image from the top.
		/// </summary>
		public int OffsetY;
		/// <summary>
		///  The total width of the image with <see cref="OffsetX"/> applied.
		/// </summary>
		public int TotalWidth;
		/// <summary>
		/// The height width of the image with <see cref="OffsetY"/> applied.
		/// </summary>
		public int TotalHeight;
		/// <summary>
		///  This is likely a boolean value that determines if transparency is used by the image.
		/// </summary>
		public int IsTransparent;
		/// <summary>
		///  The horizontal center of the image. Used for drawing in the game.
		/// </summary>
		public int BaseX;
		/// <summary>
		///  The vertical baseline of the image. Used for drawing in the game.
		/// </summary>
		public int BaseY;

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return tagSignature.StartsWith("stdinfo");
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 standard image info.
		/// </summary>
		/// <returns>The string representation of the HG-3 standard image info.</returns>
		public override string ToString() => $"stdinfo {Width}x{Height} Count={IsTransparent}";

		#endregion
	}
	/// <summary>
	///  The tag structure for a standard HG-3 image. Is identified as "img####".
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24)]
	internal struct HG3IMG {
		#region Constants

		/// <summary>
		///  The regex used to match tag signatures of this type.
		/// </summary>
		private static readonly Regex SignatureRegex = new Regex(@"img(?'num'\d{4})");

		#endregion

		#region Fields

		//[FieldOffset(0)]
		public int Unknown;
		//[FieldOffset(4)]
		public int Height;
		//[FieldOffset(8)]
		public HGXIMGDATA Data;
		/*[FieldOffset(8)]
		public int DataLength;
		[FieldOffset(12)]
		public int OriginalDataLength;
		[FieldOffset(16)]
		public int CmdLength;
		[FieldOffset(20)]
		public int OriginalCmdLength;*/

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return SignatureRegex.IsMatch(tagSignature);
		}
		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <param name="number">The output number assigned to this tag.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature, out int number) {
			Match match = SignatureRegex.Match(tagSignature);
			if (match.Success)
				number = int.Parse(match.Groups["num"].Value);
			else
				number = 0;
			return match.Success;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the standard HG-3 image.
		/// </summary>
		/// <returns>The string representation of the standard HG-3 image.</returns>
		public override string ToString() => $"img#### Data={Data.DataLength} Cmd={Data.CmdLength}";

		#endregion
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	internal struct HGXIMGDATA {
		#region Fields

		public int DataLength;
		public int OriginalDataLength;
		public int CmdLength;
		public int OriginalCmdLength;

		#endregion
	}
	/// <summary>
	///  The tag structure for a standard HG-3 image alpha. Is identified as "img_al".
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	internal struct HG3IMG_AL {
		#region Fields

		/// <summary>
		///  The compressed length of the alpha data.
		/// </summary>
		public int Length;
		/// <summary>
		///  The decompressed length of the alpha data.
		/// </summary>
		public int OriginalLength;

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return tagSignature == "img_al";
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 AL image.
		/// </summary>
		/// <returns>The string representation of the HG-3 AL image.</returns>
		public override string ToString() => $"img_al Length={Length} OriginalLength={OriginalLength}";

		#endregion
	}
	/// <summary>
	///  The tag structure for a standard HG-3 JPEG image. Is identified as "img_jpg".<para/>
	///  This image can be compiled with <see cref="HG3IMG_AL"/> as an alpha mask to create a full image.<para/>
	///  This should not be read from the stream beacuse this has no fields but will result in a byte being read
	///  anyways.
	/// </summary>
	internal struct HG3IMG_JPG {
		#region Fields

		// There are no fields, this tag denotes a raw JPEG image.

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return tagSignature == "img_jpg";
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 JPEG image.
		/// </summary>
		/// <returns>The string representation of the HG-3 JPEG image.</returns>
		public override string ToString() => $"img_jpg";

		#endregion
	}
	/// <summary>
	///  The tag structure for a standard HG-3 image mode. Is identified as "imgmode".
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
	internal struct HG3IMGMODE {
		#region Fields

		/// <summary>
		///  Unknown 4-byte value.
		/// </summary>
		public int Mode;

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return tagSignature == "imgmode";
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 image mode.
		/// </summary>
		/// <returns>The string representation of the HG-3 image mode.</returns>
		public override string ToString() => $"imgmode Mode={Mode:X8}";

		#endregion
	}
	/// <summary>
	///  The tag structure for a standard HG-3 image type. Is identified as "cptype".
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
	internal struct HG3CPTYPE {
		#region Fields

		/// <summary>
		///  Unknown 4-byte value.
		/// </summary>
		public int Type;

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return tagSignature == "cptype";
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 image mode.
		/// </summary>
		/// <returns>The string representation of the HG-3 image mode.</returns>
		public override string ToString() => $"cptype Type={Type:X8}";

		#endregion
	}
	/// <summary>
	///  The tag structure for an HG-3 attribute. Is identified as "ats####".
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 20)]
	internal struct HG3ATS {
		#region Constants

		/// <summary>
		///  The regex used to match tag signatures of this type.
		/// </summary>
		private static readonly Regex SignatureRegex = new Regex(@"ats(?'num'\d{4})");

		#endregion

		#region Fields

		/// <summary>
		///  The x coordinate of the attribute.
		/// </summary>
		public int X;
		/// <summary>
		///  The y coordinate of the attribute.
		/// </summary>
		public int Y;
		/// <summary>
		///  The width coordinate of the attribute.
		/// </summary>
		public int Width;
		/// <summary>
		///  The height coordinate of the attribute.
		/// </summary>
		public int Height;
		/// <summary>
		///  The display color coordinate of the attribute.
		/// </summary>
		public int Color;

		#endregion

		#region HasTagSignature

		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature) {
			return SignatureRegex.IsMatch(tagSignature);
		}
		/// <summary>
		///  Gets if the tag's signature matches that if this structure.
		/// </summary>
		/// <param name="tagSignature">The <see cref="HG3TAG.Signature"/>.</param>
		/// <param name="number">The output number assigned to this tag.</param>
		/// <returns>True if the signatures match.</returns>
		public static bool HasTagSignature(string tagSignature, out int number) {
			Match match = SignatureRegex.Match(tagSignature);
			if (match.Success)
				number = int.Parse(match.Groups["num"].Value);
			else
				number = 0;
			return match.Success;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the standard HG-3 image.
		/// </summary>
		/// <returns>The string representation of the standard HG-3 image.</returns>
		public override string ToString() => $"ats#### X={X} Y={Y} W={Width} H={Height} C={Color:X8}";

		#endregion
	}
}
