using System;
using System.Collections.Generic;
using System.IO;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The type of image that is extracted from an <see cref="Hg3FrameInfo"/>.
	/// </summary>
	internal enum Hg3ImageType {
		/// <summary>We shouldn't hit this. No image was detected.</summary>
		None = 0,
		/// <summary>A standard <see cref="HG3IMG"/> tag type is extracted.</summary>
		Image = (1 << 0),
		/// <summary>Raw jpeg data is extracted.</summary>
		Jpeg = (1 << 1),
		/// <summary>An <see cref="HG3IMG_AL"/> tag type is extracted.</summary>
		Alpha = (1 << 2),
		/// <summary>An <see cref="HG3IMG_JPG"/> data and <see cref="HG3IMG_AL"/> tag type is extracted.</summary>
		JpegAlpha = Jpeg | Alpha,
	}
	/// <summary>
	///  A container for both an <see cref="HG3TAG"/>, an <typeparamref name="THg3Img"/>, and a data offset.
	/// </summary>
	/// <typeparam name="THg3Img">The type of the HG-3 image info.</typeparam>
	internal class Hg3TagImg<THg3Img> {
		#region Fields

		/// <summary>
		///  Gets the tag associated with this atom.
		/// </summary>
		public HG3TAG Tag { get; }
		/// <summary>
		///  Gets the info for this atom, after the tag.
		/// </summary>
		public THg3Img Data { get; }
		/// <summary>
		///  Gets the offset to the atom's data.
		/// </summary>
		public long Offset { get; }
		/// <summary>
		///  Gets the optional Id for <see cref="HG3IMG"/> tags.
		/// </summary>
		public int Id { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the image/tag combo with the current stream position, the tag, and the image data.
		/// </summary>
		/// <param name="stream">The stream to get the current position from.</param>
		/// <param name="tag">The tag for this image.</param>
		/// <param name="data">The image data for this image.</param>
		/// <param name="id">the optional Id for <see cref="HG3IMG"/> tags.</param>
		public Hg3TagImg(Stream stream, HG3TAG tag, THg3Img data, int id) {
			Tag = tag;
			Data = data;
			Offset = stream.Position;
			Id = id;
		}

		#endregion
	}
	/// <summary>
	///  Information on an HG-3 image and its attributes that was collected.
	/// </summary>
	internal class Hg3FrameInfo {
		#region Fields
		
		/// <summary>
		///  The header for the frame.
		/// </summary>
		public HG3FRAMEHDR Header { get; }
		/// <summary>
		///  The dimensions and other specs for the image.
		/// </summary>
		public HG3STDINFO StdInfo { get; }
		/// <summary>
		///  The tag for <see cref="HG3IMG"/> standard image data.
		/// </summary>
		public Hg3TagImg<HG3IMG> Img { get; set; }
		/// <summary>
		///  The tag for <see cref="HG3IMG_AL"/> alpha image data.
		/// </summary>
		public Hg3TagImg<HG3IMG_AL> ImgAl { get; set; }
		/// <summary>
		///  The tag for <see cref="HG3IMG_AL"/> jpeg image.
		/// </summary>
		public Hg3TagImg<HG3IMG_JPG> ImgJpg { get; set; }
		/// <summary>
		///  The image mode, if a tag was found.
		/// </summary>
		public HG3IMGMODE? ImgMode { get; set; }
		/// <summary>
		///  The cp type, if a tag was found. I think this tag always exists.
		/// </summary>
		public HG3CPTYPE? CpType { get; set; }
		/// <summary>
		///  The image attributes that were found.
		/// </summary>
		public Dictionary<int, HG3ATS> Ats { get; } = new Dictionary<int, HG3ATS>();

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the HG-3 frame info from the frame header and std image info.
		/// </summary>
		/// <param name="frameHdr">The header for the frame that contains the Id.</param>
		/// <param name="stdInfo">The image dimensions and other specs.</param>
		public Hg3FrameInfo(HG3FRAMEHDR frameHdr, HG3STDINFO stdInfo) {
			Header = frameHdr;
			StdInfo = stdInfo;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets the type of image to extract from this frame info.
		/// </summary>
		public Hg3ImageType Type {
			get {
				Hg3ImageType type = Hg3ImageType.None;
				if (Img    != null) type |= Hg3ImageType.Image;
				if (ImgJpg != null) type |= Hg3ImageType.Jpeg;
				if (ImgAl  != null) type |= Hg3ImageType.Alpha;
				return type;
			}
		}

		#endregion

		#region Atom

		/// <summary>
		///  Gets if this frame info already has the specified image tag type.
		/// </summary>
		/// <param name="data">The image data type to check for.</param>
		/// <returns>True if the image type already exists.</returns>
		public bool HasAtom(object data) {
			switch (data) {
			case HG3IMG     _: return Img != null || ImgAl != null || ImgJpg != null;
			case HG3IMG_AL  _: return Img != null || ImgAl != null;
			case HG3IMG_JPG _: return Img != null || ImgJpg != null;
			default: throw new ArgumentException(nameof(data));
			}
		}
		/// <summary>
		///  Adds a new image tag and data to the frame.
		/// </summary>
		/// <param name="stream">The stream to get the current position from.</param>
		/// <param name="tag">The tag for this image.</param>
		/// <param name="data">The image data for this image.</param>
		/// <param name="id">the optional Id for <see cref="HG3IMG"/> tags.</param>
		public void AddTagImg(Stream stream, HG3TAG tag, object data, int id) {
			switch (data) {
			case HG3IMG     img:    Img    = new Hg3TagImg<HG3IMG>    (stream, tag, img, id); break;
			case HG3IMG_AL  imgAl:  ImgAl  = new Hg3TagImg<HG3IMG_AL> (stream, tag, imgAl, id); break;
			case HG3IMG_JPG imgJpg: ImgJpg = new Hg3TagImg<HG3IMG_JPG>(stream, tag, imgJpg, id); break;
			default: throw new ArgumentException(nameof(data));
			}
		}

		#endregion
	}
}
