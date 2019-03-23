using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TriggersTools.SharpUtils.Collections;
using TriggersTools.SharpUtils.Enums;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A single frame for an HG-X image.
	/// </summary>
	[JsonObject]
	public sealed class HgxFrame : IEnumerable<Hg3Attribute> {
		#region Fields

		/// <summary>
		///  Gets the HG-X image that contains this frame.
		/// </summary>
		[JsonIgnore]
		public HgxImage HgxImage { get; internal set; }
		/// <summary>
		///  Gets the HG-X type of the image.
		/// </summary>
		[JsonIgnore]
		public HgxFormat HgxType => HgxImage?.Format ?? HgxFormat.None;
		/// <summary>
		///  Gets the file name of the image with the .hg3 extension.
		/// </summary>
		[JsonIgnore]
		public string FileName => HgxImage.FileName;

		/// <summary>
		///  Gets the numeric identifier for the frame.
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; private set; }
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
		[JsonProperty("bit_depth")]
		public int BitDepth { get; private set; }
		
		/// <summary>
		///  This is likely a boolean value that determines if transparency is used by the image.
		/// </summary>
		[JsonProperty("transparent")]
		public bool IsTransparent { get; private set; }
		/// <summary>
		///  Gets the horizontal center of the image. Used for drawing in the game.
		/// </summary>
		[JsonProperty("base_x")]
		public int BaseX { get; private set; }
		/// <summary>
		///  Gets the vertical baseline of the image. Used for drawing in the game.
		/// </summary>
		[JsonProperty("base_y")]
		public int BaseY { get; private set; }

		/// <summary>
		///  Gets the "cptype" value.
		/// </summary>
		[JsonProperty("cptype")]
		public int? Type { get; private set; }
		/// <summary>
		///  Gets the "imgmode" value.
		/// </summary>
		[JsonProperty("imgmode")]
		public int? Mode { get; private set; }

		/// <summary>
		///  Gets the file offset to the frame data.
		/// </summary>
		[JsonProperty("frame_offset")]
		internal long FrameOffset { get; private set; }

		/// <summary>
		///  Gets the display attributes for the image.
		/// </summary>
		[JsonIgnore]
		private IReadOnlyList<Hg3Attribute> attributes;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the display attributes for the image.
		/// </summary>
		[JsonProperty("attributes")]
		public IReadOnlyList<Hg3Attribute> Attributes {
			get => attributes;
			private set {
				attributes = value;
				for (int i = 0; i < attributes.Count; i++)
					attributes[i].Hg3Frame = this;
			}
		}
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
		///  This is the same as <see cref="BaseX"/>.
		/// </summary>
		[JsonIgnore]
		public int BaseLeft => BaseX;
		/// <summary>
		///  Gets the distance to the baseline from the top of the image when expanded.<para/>
		///  This is the same as <see cref="BaseY"/>.
		/// </summary>
		[JsonIgnore]
		public int BaseTop => BaseY;
		/// <summary>
		///  Gets the distance to the center from the right of the image when expanded.<para/>
		///  This is the same as <see cref="TotalWidth"/> - <see cref="BaseX"/>.
		/// </summary>
		[JsonIgnore]
		public int BaseRight => TotalWidth - BaseX;
		/// <summary>
		///  Gets the distance to the baseline from the bottom of the image when expanded.<para/>
		///  This is the same as <see cref="TotalHeight"/> - <see cref="BaseY"/>.
		/// </summary>
		[JsonIgnore]
		public int BaseBottom => TotalHeight - BaseY;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned HG-X frame for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		private HgxFrame() { }
		/// <summary>
		///  Constructs an HG-3 frame with the specified frame info.
		/// </summary>
		/// <param name="frameInfo">The frame information containing all caught HG-3 tags.</param>
		/// <param name="hg3Image">The HG-3 image containing this frame.</param>
		internal HgxFrame(Hg3FrameInfo frameInfo, HgxImage hg3Image) {
			HgxImage = hg3Image;
			Id = frameInfo.Header.Id;
			Hg3Attribute[] attributes = new Hg3Attribute[frameInfo.Ats.Count];
			int i = 0;
			foreach (var pair in frameInfo.Ats) {
				attributes[i] = new Hg3Attribute(pair.Key, pair.Value, this);
				i++;
			}
			Attributes = attributes.AsReadOnly();
			HG3STDINFO stdInfo = frameInfo.StdInfo;
			Width = stdInfo.Width;
			Height = stdInfo.Height;
			TotalWidth = stdInfo.TotalWidth;
			TotalHeight = stdInfo.TotalHeight;
			OffsetX = stdInfo.OffsetX;
			OffsetY = stdInfo.OffsetY;
			BitDepth = stdInfo.DepthBits;
			IsTransparent = stdInfo.IsTransparent != 0;
			BaseX = stdInfo.BaseX;
			BaseY = stdInfo.BaseY;
			Type = frameInfo.CpType?.Type;
			Mode = frameInfo.ImgMode?.Mode;
			FrameOffset = frameInfo.FrameOffset;
		}
		/// <summary>
		///  Constructs an HG-2 frame with the specified frame info.
		/// </summary>
		/// <param name="frameInfo">The frame information.</param>
		/// <param name="hg2Image">The HG-2 image containing this frame.</param>
		internal HgxFrame(Hg2FrameInfo frameInfo, HgxImage hg2Image) {
			HgxImage = hg2Image;
			Attributes = new Hg3Attribute[0];// Array.Empty<Hg3Attribute>();
			HG2IMG img = frameInfo.Img;
			HG2IMG_BASE? imgEx = frameInfo.ImgBase;
			Id = img.Id;
			Width = img.Width;
			Height = img.Height;
			TotalWidth = img.TotalWidth;
			TotalHeight = img.TotalHeight;
			OffsetX = img.OffsetX;
			OffsetY = img.OffsetY;
			BitDepth = img.DepthBits;
			IsTransparent = img.IsTransparent != 0;
			BaseX = imgEx?.BaseX ?? 0;
			BaseY = imgEx?.BaseY ?? 0;
			Type = null;
			Mode = null;
			FrameOffset = frameInfo.FrameOffset;
		}

		#endregion

		#region Accessors

		/// <summary>
		///  Finds the attribute with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the attribute to find.</param>
		/// <returns>The located HG-3 attribute.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  No HG-3 attribute with the <paramref name="id"/> was found.
		/// </exception>
		public Hg3Attribute GetAttributeById(int id) {
			Hg3Attribute attribute = Attributes.FirstOrDefault(f => f.Id == id);
			if (attribute == null)
				throw new KeyNotFoundException($"Could not find the Id \"{id}\"!");
			return attribute;
		}
		/// <summary>
		///  Finds the attribute with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the attribute to find.</param>
		/// <returns>The located HG-3 attribute.-or- null if the Id was not found.</returns>
		public Hg3Attribute FindAttributeById(int id) {
			return Attributes.FirstOrDefault(f => f.Id == id);
		}
		/// <summary>
		///  Tries to get the attribute with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the attribute to find.</param>
		/// <param name="attribute">The output located HG-3 attribute.-or- null if the Id was not found.</param>
		/// <returns>True if the attribute was found, otherwise false.</returns>
		public bool TryGetAttributeById(int id, out Hg3Attribute attribute) {
			attribute = FindAttributeById(id);
			return attribute != null;
		}
		/// <summary>
		///  Gets the index of the attribute with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the attribute to find.</param>
		/// <returns>The index of the located HG-3 attribute.-or- -1 if the Id was not found.</returns>
		public int IndexOfAttributeById(int id) {
			return Attributes.IndexOf(f => f.Id == id);
		}
		/// <summary>
		///  Gets the index of the attribute.
		/// </summary>
		/// <param name="attribute">The HG-X attribute to get the index of.</param>
		/// <returns>The index of the located HG-X attribute.-or- -1 if the attribute was not found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="attribute"/> is null.
		/// </exception>
		public int IndexOfFrame(Hg3Attribute attribute) {
			if (attribute == null)
				throw new ArgumentNullException(nameof(attribute));
			return Attributes.IndexOf(attribute);
		}

		#endregion

		#region ExtractImage

		/// <summary>
		///  Extracts the HG-X frame's image to the specified output directory as a PNG file.
		/// </summary>
		/// <param name="stream">The stream to the HG-X file.</param>
		/// <param name="outputDir">The output directory to save the image to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The file path to the extracted PNG.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  <paramref name="stream"/> is closed.
		/// </exception>
		public string ExtractImageToDirectory(Stream stream, string outputDir, HgxOptions options) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			string pngFile = GetFrameFilePath(outputDir, false);
			ExtractImageToFile(stream, pngFile, options);
			return pngFile;
		}
		/// <summary>
		///  Extracts the HG-X frame's image to the specified output PNG file.
		/// </summary>
		/// <param name="stream">The stream to the HG-X file.</param>
		/// <param name="pngFile">The PNG file path to save the image to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The file path to the extracted PNG.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="pngFile"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  <paramref name="stream"/> is closed.
		/// </exception>
		public void ExtractImageToFile(Stream stream, string pngFile, HgxOptions options) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (pngFile == null)
				throw new ArgumentNullException(nameof(pngFile));
			HgxImage.ExtractImage(stream, this, pngFile, options);
		}

		#endregion

		#region Helpers

		/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="HgxImage.IsAnimation"/> is
		///  false.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		public string GetFrameFileName(bool forcePostfix) {
			return HgxImage.GetFrameFileName(Id, forcePostfix);
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="CatSystem2.HgxImage"/> images.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="HgxImage.IsAnimation"/> is
		///  false.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		public string GetFrameFilePath(string directory, bool forcePostfix) {
			return HgxImage.GetFrameFilePath(directory, Id, forcePostfix);
		}

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the HG-X frames's attributes.
		/// </summary>
		/// <returns>The HG-X attribute enumerator.</returns>
		public IEnumerator<Hg3Attribute> GetEnumerator() => Attributes.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-X frame.
		/// </summary>
		/// <returns>The string representation of the HG-X frame.</returns>
		public override string ToString() {
			return $"{HgxType.ToDescription() ?? "HG-X"} Frame {Id:D4} {Width}x{Height} {BaseX},{BaseY}";
		}

		#endregion
	}
}
