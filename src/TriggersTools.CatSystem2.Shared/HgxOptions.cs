using System;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Options that can be used when extracting HG-X images.
	/// </summary>
	[Flags]
	public enum HgxOptions {
		/// <summary>
		///  No special options when extracting HG-X images.
		/// </summary>
		None =  0,
		/// <summary>
		///  Expand the image to it's full size. HG-X images are saved in a smaller size to cut out unneeded
		///  transparent areas.<para/>
		///  Images can be expanded while extracting, or you can use <see cref="HgxFrame.TotalWidth"/>,
		///  <see cref="HgxFrame.TotalHeight"/>, <see cref="HgxFrame.OffsetX"/>, and <see cref="HgxFrame.OffsetY"/>.
		/// </summary>
		Expand = (1 << 0),
		/// <summary>
		///  Vertically flip the image. HG-X images can be saved in a flipped format, but there is no information
		///  identifying that it is flipped.
		/// </summary>
		Flip = (1 << 1),
	}
}
