using System;
using System.Collections;
using System.Collections.Generic;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;
using System.Linq;
using TriggersTools.SharpUtils.Collections;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A single frame for an HG-3 image.
	/// </summary>
	[JsonObject]
	public sealed class Hg3Frame : IEnumerable<Hg3Attribute> {
		#region Fields

		/// <summary>
		///  Gets the HG-3 image that contains this frame.
		/// </summary>
		[JsonIgnore]
		public Hg3Image Hg3Image { get; internal set; }
		/// <summary>
		///  Gets the file name of the image with the .hg3 extension.
		/// </summary>
		[JsonIgnore]
		public string FileName => Hg3Image.FileName;

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
		[JsonProperty("depth_bits")]
		public int DepthBits { get; private set; }
		
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
		///  Constructs an unassigned HG-3 frame for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public Hg3Frame() { }
		/// <summary>
		///  Constructs an HG-3 frame with the specified frame info.
		/// </summary>
		/// <param name="frameInfo">The frame information containing all caught HG-3 tags.</param>
		/// <param name="hg3Image">The HG-3 image containing this frame.</param>
		internal Hg3Frame(Hg3FrameInfo frameInfo, Hg3Image hg3Image) {
			Hg3Image = hg3Image;
			Id = frameInfo.Header.Id;
			var attributes = frameInfo.Ats.Select(pair => new Hg3Attribute(pair.Key, pair.Value, this));
			Attributes = Array.AsReadOnly(attributes.ToArray());
			HG3STDINFO stdInfo = frameInfo.StdInfo;
			Width = stdInfo.Width;
			Height = stdInfo.Height;
			TotalWidth = stdInfo.TotalWidth;
			TotalHeight = stdInfo.TotalHeight;
			OffsetX = stdInfo.OffsetX;
			OffsetY = stdInfo.OffsetY;
			DepthBits = stdInfo.DepthBits;
			IsTransparent = stdInfo.IsTransparent != 0;
			BaseX = stdInfo.BaseX;
			BaseY = stdInfo.BaseY;
			Type = frameInfo.CpType?.Type;
			Mode = frameInfo.ImgMode?.Mode;
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
		/// <param name="attribute">The HG-3 attribute to get the index of.</param>
		/// <returns>The index of the located HG-3 attribute.-or- -1 if the attribute was not found.</returns>
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

		#region Helpers

		/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="Hg3Image.IsAnimation"/> is
		///  false.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		public string GetFrameFileName(bool forcePostfix) {
			return Hg3Image.GetFrameFileName(Id, forcePostfix);
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="CatSystem2.Hg3Image"/> images.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="Hg3Image.IsAnimation"/> is
		///  false.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		public string GetFrameFilePath(string directory, bool forcePostfix) {
			return Hg3Image.GetFrameFilePath(directory, Id, forcePostfix);
		}

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the HG-3 frames's attributes.
		/// </summary>
		/// <returns>The HG-3 attribute enumerator.</returns>
		public IEnumerator<Hg3Attribute> GetEnumerator() => Attributes.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3.
		/// </summary>
		/// <returns>The string representation of the HG-3.</returns>
		public override string ToString() => $"HG-3 Frame {Id:D4} {Width}x{Height} {BaseX},{BaseY}";

		#endregion
	}
}
