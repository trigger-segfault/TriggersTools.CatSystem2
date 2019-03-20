using System.Runtime.InteropServices;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  Standard HG-2 image information.
	/// </summary>
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

		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		public int Unknown1;
		/// <summary>
		///  Unknown 4-byte value 2.
		/// </summary>
		public int Unknown2;

		/// <summary>
		///  The image compression data.
		/// </summary>
		public HGXIMGDATA Data;
		/// <summary>
		///  This is possibly the offset to the image data. Always 40.
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
		///  The offset from the start of this structure to the next image entry. Zero when there are no more images.
		/// </summary>
		public int OffsetNext;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-2 image.
		/// </summary>
		/// <returns>The string representation of the HG-2 image.</returns>
		public override string ToString() => $"HG2IMG W={Width} H={Height}";

		#endregion
	}
	/// <summary>
	///  Extract HG-2 image base information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct HG2IMG_BASE {
		#region Constants

		/// <summary>
		///  The size of this structure in bytes.
		/// </summary>
		public static readonly int CbSize = Marshal.SizeOf<HG2IMG_BASE>();

		#endregion

		#region Fields

		/// <summary>
		///  The horizontal center of the image. Used for drawing in the game.
		/// </summary>
		public int BaseX;
		/// <summary>
		///  The vertical baseline of the image. Used for drawing in the game.
		/// </summary>
		public int BaseY;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-2 image base.
		/// </summary>
		/// <returns>The string representation of the HG-2 image base.</returns>
		public override string ToString() => $"HG2IMG_BASE X={BaseX} Y={BaseY}";

		#endregion
	}
}
