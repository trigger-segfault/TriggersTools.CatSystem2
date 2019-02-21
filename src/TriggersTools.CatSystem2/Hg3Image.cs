using System;
using System.Collections.Generic;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Dimensions and other information extracted from an HG-3 image.
	/// </summary>
	public sealed class Hg3Image {
		#region Fields

		/// <summary>
		///  Gets the HG-3 that contains this image set.
		/// </summary>
		[JsonIgnore]
		public Hg3 Hg3 { get; internal set; }
		/// <summary>
		///  Gets the index of the HG-3 image in the HG-3 file. This number comes before the frame index in the file
		///  name.
		/// </summary>
		[JsonIgnore]
		public int ImageIndex { get; internal set; }
		/// <summary>
		///  Gets the file name of the image with the .hg3 extension.
		/// </summary>
		[JsonIgnore]
		public string FileName => Hg3.FileName;

		/// <summary>
		///  Gets the condensed width of the image.
		/// </summary>
		[JsonProperty("width")]
		public int Width { get; private set; }
		/// <summary>
		///  Gets the condensed height of the image.
		/// </summary>
		[JsonProperty("height")]
		public int Height { get; private set; }
		/// <summary>
		///  Gets the total width of the image with <see cref="OffsetX"/> applied.
		/// </summary>
		[JsonProperty("total_width")]
		public int TotalWidth { get; private set; }
		/// <summary>
		/// Gets the height width of the image with <see cref="OffsetY"/> applied.
		/// </summary>
		[JsonProperty("total_height")]
		public int TotalHeight { get; private set; }
		/// <summary>
		///  Gets the horizontal offset of the image from the left.
		/// </summary>
		[JsonProperty("offset_x")]
		public int OffsetX { get; private set; }
		/// <summary>
		///  Gets the vertical offset of the image from the top.
		/// </summary>
		[JsonProperty("offset_y")]
		public int OffsetY { get; private set; }
		/// <summary>
		///  Gets the depth of the original image format in bits.
		/// </summary>
		[JsonProperty("depth_bits")]
		public int DepthBits { get; private set; }

		/// <summary>
		///  Gets the number of frames in the animation.
		/// </summary>
		[JsonProperty("frame_count")]
		public int FrameCount { get; private set; }
		/// <summary>
		///  This is likely a boolean value that determines if transparency is used by the image.
		/// </summary>
		[JsonProperty("has_transparency")]
		public int HasTransparency { get; private set; }
		/// <summary>
		///  Gets the horizontal center of the image. Used for drawing in the game.
		/// </summary>
		[JsonProperty("center")]
		public int Center { get; private set; }
		/// <summary>
		///  Gets the vertical baseline of the image. Used for drawing in the game.
		/// </summary>
		[JsonProperty("baseline")]
		public int Baseline { get; private set; }

		/// <summary>
		///  Gets the offsets to each frame in the HG-3 file's image.
		/// </summary>
		[JsonProperty("frame_offsets")]
		public IReadOnlyList<long> FrameOffsets { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the left margin of the image when expanded.<para/>
		///  This is the same as <see cref="OffsetX"/>.
		/// </summary>
		[JsonIgnore]
		public int MarginLeft => OffsetX;
		/// <summary>
		///  Gets the top margin of the image when expanded.<para/>
		///  This is the same as <see cref="OffsetY"/>.
		/// </summary>
		[JsonIgnore]
		public int MarginTop => OffsetY;
		/// <summary>
		///  Gets the right margin of the image when expanded.<para/>
		///  This is the same as <see cref="TotalWidth"/> - <see cref="Width"/> - <see cref="OffsetX"/>.
		/// </summary>
		[JsonIgnore]
		public int MarginRight => TotalWidth - Width - OffsetX;
		/// <summary>
		///  Gets the bottom margin of the image when expanded.<para/>
		///  This is the same as <see cref="TotalHeight"/> - <see cref="Height"/> - <see cref="OffsetY"/>.
		/// </summary>
		[JsonIgnore]
		public int MarginBottom => TotalHeight - Height - OffsetY;

		/// <summary>
		///  Gets the distance to the center from the left of the image when expanded.<para/>
		///  This is the same as <see cref="Center"/>.
		/// </summary>
		[JsonIgnore]
		public int CenterLeft => Center;
		/// <summary>
		///  Gets the distance to the baseline from the top of the image when expanded.<para/>
		///  This is the same as <see cref="Baseline"/>.
		/// </summary>
		[JsonIgnore]
		public int BaselineTop => Baseline;
		/// <summary>
		///  Gets the distance to the center from the right of the image when expanded.<para/>
		///  This is the same as <see cref="TotalWidth"/> - <see cref="Center"/>.
		/// </summary>
		[JsonIgnore]
		public int CenterRight => TotalWidth - Center;
		/// <summary>
		///  Gets the distance to the baseline from the bottom of the image when expanded.<para/>
		///  This is the same as <see cref="TotalHeight"/> - <see cref="Baseline"/>.
		/// </summary>
		[JsonIgnore]
		public int BaselineBottom => TotalHeight - Baseline;

		/// <summary>
		///  Gets if this HG-3 image has multiple frames. This also means the file name will have a +###+### at the end.
		/// </summary>
		[JsonIgnore]
		public bool IsAnimation => FrameCount != 1;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned HG-3 image for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public Hg3Image() { }
		/// <summary>
		///  Constructs an HG-3 image with the specified file name, image index, <see cref="Hg3.HG3STDINFO"/>, and
		///  bitmap frames.
		/// </summary>
		/// <param name="imageIndex">The frame index of the image.</param>
		/// <param name="stdInfo">The HG3STDINFO struct containing image dimension information.</param>
		/// <param name="frameOffsets">The frame offsets for each frame entry in the HG-3 file.</param>
		/// <param name="hg3">The HG-3 containing this image set.</param>
		internal Hg3Image(int imageIndex, HG3STDINFO stdInfo, long[] frameOffsets, Hg3 hg3) {
			Hg3 = hg3;
			ImageIndex = imageIndex;
			FrameOffsets = Array.AsReadOnly(frameOffsets);
			FrameCount = FrameOffsets.Count; // Todo: Eliminate frame offsets

			Width = stdInfo.Width;
			Height = stdInfo.Height;
			TotalWidth = stdInfo.TotalWidth;
			TotalHeight = stdInfo.TotalHeight;
			OffsetX = stdInfo.OffsetX;
			OffsetY = stdInfo.OffsetY;
			DepthBits = stdInfo.DepthBits;
			HasTransparency = stdInfo.HasTransparency;
			Center = stdInfo.Center;
			Baseline = stdInfo.Baseline;
		}
		/// <summary>
		///  Constructs an HG-3 image with the specified file name, image index, <see cref="Hg3.HG3STDINFO"/>, and
		///  bitmap frames.
		/// </summary>
		/// <param name="imageIndex">The frame index of the image.</param>
		/// <param name="stdInfo">The HG3STDINFO struct containing image dimension information.</param>
		/// <param name="hg3">The HG-3 containing this image set.</param>
		internal Hg3Image(int imageIndex, HG3STDINFO stdInfo, Hg3 hg3) {
			Hg3 = hg3;
			ImageIndex = imageIndex;
			FrameOffsets = Array.AsReadOnly(new long[1]);
			FrameCount = 1; // Todo: Eliminate frame offsets

			Width = stdInfo.Width;
			Height = stdInfo.Height;
			TotalWidth = stdInfo.TotalWidth;
			TotalHeight = stdInfo.TotalHeight;
			OffsetX = stdInfo.OffsetX;
			OffsetY = stdInfo.OffsetY;
			DepthBits = stdInfo.DepthBits;
			HasTransparency = stdInfo.HasTransparency;
			Center = stdInfo.Center;
			Baseline = stdInfo.Baseline;
		}

		#endregion

		#region Helpers

		/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		public string GetFrameFileName(int frmIndex) {
			return Hg3.GetFrameFileName(ImageIndex, frmIndex);
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="Hg3"/> images.</param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		public string GetFrameFilePath(string directory, int frmIndex) {
			return Hg3.GetFrameFilePath(directory, ImageIndex, frmIndex);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3.
		/// </summary>
		/// <returns>The string representation of the HG-3.</returns>
		public override string ToString() => $"{Width}x{Height} Center={Center},{Baseline}";

		#endregion
	}
}
