using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Image and frame information about an HG-3 file with assocaited images.
	/// </summary>
	[JsonObject]
	public sealed partial class Hg3 : IReadOnlyCollection<Hg3Image> {
		#region Constants

		/// <summary>
		///  The current file version for the HG-3.
		/// </summary>
		public const int CurrentVersion = 2;

		#endregion

		#region Fields

		/// <summary>
		///  Gets the file version of the loaded HG-3.
		/// </summary>
		[JsonProperty("version")]
		public int Version { get; private set; }

		/// <summary>
		///  Gets the file name of the HG-3 file with the .hg3 extension.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/*/// <summary>
		///  The size of the HG-3 header.
		/// </summary>
		[JsonProperty("header_size")]
		public int HeaderSize { get; private set; }*/
		/// <summary>
		///  Unknown 4-byte value.
		/// </summary>
		[JsonProperty("unknown")]
		public int Unknown { get; private set; }
		/// <summary>
		///  Gets if the HG-3 frames were saved while expended.
		/// </summary>
		[JsonProperty("expanded")]
		public bool Expanded { get; private set; }
		/// <summary>
		///  Gets the list of images associated with this HG-3 file.
		/// </summary>
		[JsonIgnore]
		private IReadOnlyList<Hg3Image> images;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the list of images associated with this HG-3 file.
		/// </summary>
		[JsonProperty("images")]
		public IReadOnlyList<Hg3Image> Images {
			get => images;
			private set {
				images = value;
				for (int i = 0; i < images.Count; i++) {
					images[i].Hg3 = this;
					images[i].ImageIndex = i;
				}
			}
		}
		/// <summary>
		///  Gets if this HG-3 has multiple frames. This also means the file name will have a +###[+###] at the end.
		/// </summary>
		[JsonIgnore]
		public bool IsAnimation => Images.Count != 1 || Images[0].FrameCount != 1;
		/// <summary>
		///  Gets the number of HG-3 images in <see cref="Images"/>.
		/// </summary>
		[JsonIgnore]
		public int Count => Images.Count;
		/// <summary>
		///  Gets the total number of frames in this HG-3.
		/// </summary>
		[JsonIgnore]
		public int FrameCount => Images.Sum(i => i.FrameCount);
		/// <summary>
		///  Gets the name of the file for loading the <see cref="Hg3"/> data.
		/// </summary>
		[JsonIgnore]
		public string JsonFileName => GetJsonFileName(FileName);

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned HG-3 for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public Hg3() { }
		/// <summary>
		///  Constructs an HG-3 with the specified file name, header, images, frame offsets, and expansion setting.
		/// </summary>
		/// <param name="fileName">The file name of the HG-3 with the .hg3 extension.</param>
		/// <param name="hdr">The HG3HDR containing extra information on the HG-3.</param>
		/// <param name="stdInfos">The HG3STDINFO struct array containing image dimension information.</param>
		/// <param name="frameOffsets">The frame offsets for each image frame entry in the HG-3 file.</param>
		/// <param name="expand">True if the images were extracted while <paramref name="expand"/> was true.</param>
		private Hg3(string fileName, HG3HDR hdr, HG3STDINFO[] stdInfos, long[][] frameOffsets, bool expand) {
			Version = CurrentVersion;
			FileName = Path.GetFileName(fileName);
			Unknown = hdr.Unknown;
			//Unknown3 = hdr.Unknown3;
			//EntryCount = hdr.EntryCount;
			Expanded = expand;
			Hg3Image[] images = new Hg3Image[stdInfos.Length];
			for (int i = 0; i < images.Length; i++)
				images[i] = new Hg3Image(i, stdInfos[i], frameOffsets[i], this);
			this.images = Array.AsReadOnly(images);
		}
		/// <summary>
		///  Constructs an HG-3 with the specified file name, header, images, frame offsets, and expansion setting.
		/// </summary>
		/// <param name="fileName">The file name of the HG-3 with the .hg3 extension.</param>
		/// <param name="hdr">The HG3HDR containing extra information on the HG-3.</param>
		/// <param name="stdInfos">The HG3STDINFO struct array containing image dimension information.</param>
		/// <param name="expand">True if the images were extracted while <paramref name="expand"/> was true.</param>
		private Hg3(string fileName, HG3HDR hdr, HG3STDINFO[] stdInfos, bool expand) {
			Version = CurrentVersion;
			FileName = Path.GetFileName(fileName);
			Unknown = hdr.Unknown;
			//Unknown3 = hdr.Unknown3;
			//EntryCount = hdr.EntryCount;
			Expanded = expand;
			Hg3Image[] images = new Hg3Image[stdInfos.Length];
			for (int i = 0; i < images.Length; i++)
				images[i] = new Hg3Image(i, stdInfos[i], this);
			this.images = Array.AsReadOnly(images);
		}

		#endregion

		#region Helpers

		/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="imgIndex">
		///  The first index, which is assocaited to an <see cref="Hg3.ImageIndex"/>.
		/// </param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="imgIndex"/> or <paramref name="frmIndex"/> are not a valid frame index.
		/// </exception>
		public string GetFrameFileName(int imgIndex, int frmIndex) {
			if (imgIndex < 0 || imgIndex >= Images.Count)
				throw new ArgumentOutOfRangeException(nameof(imgIndex));
			if (frmIndex < 0 || frmIndex >= Images[imgIndex].FrameCount)
				throw new ArgumentOutOfRangeException(nameof(frmIndex));
			string baseName = Path.GetFileNameWithoutExtension(FileName);
			if (IsAnimation) {
				if (Images[imgIndex].IsAnimation)
					baseName = $"{baseName}+{imgIndex:D3}+{frmIndex:D3}";
				else
					baseName = $"{baseName}+{imgIndex:D3}";
			}
			return Path.ChangeExtension(baseName, ".png");
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="Hg3"/> images.</param>
		/// <param name="imgIndex">
		///  The first index, which is assocaited to an <see cref="Hg3.ImageIndex"/>.
		/// </param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="imgIndex"/> or <paramref name="frmIndex"/> are not a valid frame index.
		/// </exception>
		public string GetFrameFilePath(string directory, int imgIndex, int frmIndex) {
			return Path.Combine(directory, GetFrameFileName(imgIndex, frmIndex));
		}
		/*/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="fileName">The base filename of the <see cref="Hg3"/>.</param>
		/// <param name="imgIndex">
		///  The first index, which is assocaited to an <see cref="Hg3Image.ImageIndex"/>.
		/// </param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file name of the HG-3 image frame.</returns>
		public static string GetFrameFileName(string fileName, int imgIndex, int frmIndex) {
			string baseName = $"{Path.GetFileNameWithoutExtension(fileName)}+{imgIndex:D3}+{frmIndex:D3}";
			return Path.ChangeExtension(baseName, ".png");
		}*/
		/// <summary>
		///  Gets the file name for the JSON HG-3 information.
		/// </summary>
		/// <param name="fileName">The base filename of the <see cref="Hg3"/>.</param>
		/// <returns>The file name of the JSON HG-3 information.</returns>
		public static string GetJsonFileName(string fileName) {
			return Path.ChangeExtension(Path.GetFileNameWithoutExtension(fileName) + "+hg3", ".json");
		}
		/// <summary>
		///  Gets the file path for the JSON HG-3 information.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="Hg3"/>.</param>
		/// <param name="fileName">The base file name of the <see cref="Hg3"/>.</param>
		/// <returns>The file path of the JSON HG-3 information.</returns>
		public static string GetJsonFilePath(string directory, string fileName) {
			return Path.Combine(directory, GetJsonFileName(fileName));
		}

		#endregion

		#region I/O

		/// <summary>
		///  Deserializes the HG-3 from a json file in the specified directory and file name.
		/// </summary>
		/// <param name="directory">
		///  The directory for the json file to load and deserialize with <paramref name="fileName"/>.
		/// </param>
		/// <returns>The deserialized HG-3.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static Hg3 FromJsonDirectory(string directory, string fileName) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			string jsonFile = Path.Combine(directory, GetJsonFileName(fileName));
			return JsonConvert.DeserializeObject<Hg3>(File.ReadAllText(jsonFile));
		}
		/// <summary>
		///  Serializes the HG-3 to a json file in the specified directory.
		/// </summary>
		/// <param name="directory">The directory to save the json file to using <see cref="JsonFileName"/>.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public void SaveJsonToDirectory(string directory) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			string jsonFile = Path.Combine(directory, JsonFileName);
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
		/// <summary>
		///  Opens the stream to the bitmap from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to load the bitmap from.</param>
		/// <returns>The file stream to the bitmap.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public FileStream OpenBitmapStream(string directory) {
			return OpenBitmapStream(directory, 0, 0);
		}
		/// <summary>
		///  Opens the stream to the bitmap from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to load the bitmap from using <see cref="GetFrameFileName"/>.</param>
		/// <param name="imgIndex">
		///  The first index, which is assocaited to an <see cref="Hg3.ImageIndex"/>.
		/// </param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image"/>.
		/// </param>
		/// <returns>The file stream to the bitmap.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="imgIndex"/> or <paramref name="frmIndex"/> are not a valid frame index.
		/// </exception>
		public FileStream OpenBitmapStream(string directory, int imgIndex, int frmIndex) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			return File.OpenRead(Path.Combine(directory, GetFrameFilePath(directory, imgIndex, frmIndex)));
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3.
		/// </summary>
		/// <returns>The string representation of the HG-3.</returns>
		public override string ToString() => $"Hg3: \"{FileName}\" Count={Count}";

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the HG-3's images.
		/// </summary>
		/// <returns>The HG-3 image enumerator.</returns>
		public IEnumerator<Hg3Image> GetEnumerator() => Images.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
