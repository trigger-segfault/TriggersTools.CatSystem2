using System;
using System.Collections.Generic;
using System.IO;

namespace TriggersTools.CatSystem2.Structs {
	internal enum Hg3ImageType {
		None = 0,
		Image = (1 << 0),
		Jpeg = (1 << 1),
		Alpha = (1 << 2),
		JpegAlpha = Jpeg | Alpha,
	}
	/// <summary>
	///  A container for both an <see cref="HG3TAG"/>, an <typeparamref name="THg3Atom"/>, and a data offset.
	/// </summary>
	/// <typeparam name="THg3Atom">The type of the HG-3 atom info.</typeparam>
	internal class Hg3TagAtom<THg3Atom> {
		#region Fields

		/// <summary>
		///  Gets the tag associated with this atom.
		/// </summary>
		public HG3TAG Tag { get; }
		/// <summary>
		///  Gets the info for this atom, after the tag.
		/// </summary>
		public THg3Atom Atom { get; }
		/// <summary>
		///  Gets the offset to the atom's data.
		/// </summary>
		public long Offset { get; }

		#endregion

		#region Constructors

		public Hg3TagAtom(Stream stream, HG3TAG tag, THg3Atom atom) {
			Tag = tag;
			Atom = atom;
			Offset = stream.Position;
		}

		#endregion
	}
	internal class Hg3FrameInfo {
		#region Fields
		
		public HG3FRAMEHDR Header { get; }
		public HG3STDINFO StdInfo { get; }
		public Hg3TagAtom<HG3IMG> Img { get; set; }
		public Hg3TagAtom<HG3IMG_AL> ImgAl { get; set; }
		public Hg3TagAtom<HG3IMG_JPG> ImgJpg { get; set; }
		public HG3IMGMODE? ImgMode { get; set; }
		public HG3CPTYPE? CpType { get; set; }
		public Dictionary<int, HG3ATS> Ats { get; } = new Dictionary<int, HG3ATS>();

		#endregion

		#region Constructors

		public Hg3FrameInfo(HG3FRAMEHDR frameHdr, HG3STDINFO stdInfo) {
			Header = frameHdr;
			StdInfo = stdInfo;
		}

		#endregion

		#region Properties

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

		public bool HasAtom(object atom) {
			switch (atom) {
			case HG3IMG     _: return Img != null || ImgAl != null || ImgJpg != null;
			case HG3IMG_AL  _: return Img != null || ImgAl != null;
			case HG3IMG_JPG _: return Img != null || ImgJpg != null;
			default: throw new ArgumentException(nameof(atom));
			}
		}
		public void AddAtom(Stream stream, HG3TAG tag, object atom) {
			switch (atom) {
			case HG3IMG     img:    Img    = new Hg3TagAtom<HG3IMG>    (stream, tag, img); break;
			case HG3IMG_AL  imgAl:  ImgAl  = new Hg3TagAtom<HG3IMG_AL> (stream, tag, imgAl); break;
			case HG3IMG_JPG imgJpg: ImgJpg = new Hg3TagAtom<HG3IMG_JPG>(stream, tag, imgJpg); break;
			default: throw new ArgumentException(nameof(atom));
			}
		}

		#endregion
	}
}
