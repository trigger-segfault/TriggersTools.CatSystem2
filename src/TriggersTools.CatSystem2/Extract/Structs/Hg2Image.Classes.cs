using System.IO;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  Frame information for an HG-2 image.
	/// </summary>
	internal class Hg2FrameInfo {
		#region Fields

		/// <summary>
		///  Gets the basic information for the image.
		/// </summary>
		public HG2IMG Img { get; }
		/// <summary>
		///  Gets the information for the image's base.
		/// </summary>
		public HG2IMG_BASE? ImgBase { get; }
		/// <summary>
		///  Gets the position to extract the image at.
		/// </summary>
		public long Offset { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the HG-2 frame info with the current stream position, the image data and the optional image
		///  base data.
		/// </summary>
		/// <param name="stream">The stream to get the current position from.</param>
		/// <param name="img">The basic image data.</param>
		/// <param name="data">The optional image's base information.</param>
		public Hg2FrameInfo(Stream stream, HG2IMG img, HG2IMG_BASE? imgBase) {
			Img = img;
		}

		#endregion
	}
}
