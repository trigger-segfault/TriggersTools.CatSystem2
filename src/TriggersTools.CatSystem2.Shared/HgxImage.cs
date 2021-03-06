﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Collections;
using TriggersTools.SharpUtils.Enums;
using TriggersTools.CatSystem2.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Dimensions and other information extracted from an HG-2 or HG-3 image.
	/// </summary>
	[JsonObject]
	public sealed partial class HgxImage : IEnumerable<HgxFrame> {
		#region Constants

		/// <summary>
		///  The current file version for the HG-X json format.
		/// </summary>
		public const int CurrentVersion = 5;

		#endregion

		#region Fields

		/// <summary>
		///  Gets the file version of the loaded HG-X image.
		/// </summary>
		[JsonProperty("version")]
		public int Version { get; private set; }

		/// <summary>
		///  Gets the file name of the HG-X file with the .hg2 or .hg3 extension.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  Unknown 4-byte value. Always seems to be 0x20 or 0x25 for HG-2, and 0x300 for HG-3.
		/// </summary>
		[JsonProperty("type")]
		public int Type { get; private set; }
		/// <summary>
		///  Gets if the HG-X frames were saved with <see cref="HgxOptions.Expand"/>.
		/// </summary>
		[JsonProperty("expanded")]
		public bool Expanded { get; private set; }
		/// <summary>
		///  Gets if the HG-X frames were saved with <see cref="HgxOptions.Expand"/>.
		/// </summary>
		[JsonProperty("flipped")]
		public bool Flipped { get; private set; }
		/// <summary>
		///  Gets the HG-X type of the image.
		/// </summary>
		[JsonProperty("format")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public HgxFormat Format { get; private set; }
		/// <summary>
		///  Gets the list of frames in the HG-X image.
		/// </summary>
		[JsonIgnore]
		private IReadOnlyList<HgxFrame> frames;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the list of frames associated with this HG-X file.
		/// </summary>
		[JsonProperty("frames")]
		public IReadOnlyList<HgxFrame> Frames {
			get => frames;
			private set {
				frames = value;
				for (int i = 0; i < frames.Count; i++)
					frames[i].HgxImage = this;
			}
		}
		/// <summary>
		///  Gets if this HG-X has multiple frames. This also means the file name will have a +#### at the end.
		/// </summary>
		[JsonIgnore]
		public bool IsAnimation => Frames.Count != 1;
		/// <summary>
		///  Gets the number of HG-X frames in <see cref="Frames"/>.
		/// </summary>
		[JsonIgnore]
		public int Count => Frames.Count;
		/// <summary>
		///  Gets the name of the file for loading the <see cref="HgxImage"/> data.
		/// </summary>
		[JsonIgnore]
		public string JsonFileName => HgxImage.GetJsonFileName(FileName);

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned HG-X image for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		private HgxImage() { }
		/// <summary>
		///  Constructs an HG-3 image with the specified file name, image index, <see cref="HG3STDINFO"/>, and
		///  bitmap frames.
		/// </summary>
		/// <param name="fileName">The file name of the HG-3 image with the .hg3 extension.</param>
		/// <param name="hdr">The HGXHDR struct containing extra information on the HG-3.</param>
		/// <param name="frameInfos">The HG-3 frame information classes to construct the frames from.</param>
		/// <param name="options">The options used when extracting the HG-3 image.</param>
		internal HgxImage(string fileName, HGXHDR hdr, Hg3FrameInfo[] frameInfos, HgxOptions options) {
			Version = CurrentVersion;
			FileName = fileName;
			Type = hdr.Type;
			Format = HgxFormat.Hg3;

			Expanded = options.HasFlag(HgxOptions.Expand);
			Flipped = options.HasFlag(HgxOptions.Flip);
			HgxFrame[] frames = new HgxFrame[frameInfos.Length];
			for (int i = 0; i < frames.Length; i++)
				frames[i] = new HgxFrame(frameInfos[i], this);
			this.frames = frames.AsReadOnly();
		}
		/// <summary>
		///  Constructs an HG-2 image with the specified file name, image index, <see cref="HG3STDINFO"/>, and
		///  bitmap frames.
		/// </summary>
		/// <param name="fileName">The file name of the HG-2 image with the .hg2 extension.</param>
		/// <param name="hdr">The HGXHDR struct containing extra information on the HG-2.</param>
		/// <param name="frameInfos">The HG-2 frame information classes to construct the frames from.</param>
		/// <param name="options">The options used when extracting the HG-2 image.</param>
		internal HgxImage(string fileName, HGXHDR hdr, Hg2FrameInfo[] frameInfos, HgxOptions options) {
			Version = CurrentVersion;
			FileName = fileName;
			Type = hdr.Type;
			Format = HgxFormat.Hg2;

			Expanded = options.HasFlag(HgxOptions.Expand);
			Flipped = options.HasFlag(HgxOptions.Flip);
			HgxFrame[] frames = new HgxFrame[frameInfos.Length];
			for (int i = 0; i < frames.Length; i++)
				frames[i] = new HgxFrame(frameInfos[i], this);
			this.frames = frames.AsReadOnly();
		}

		#endregion

		#region Accessors

		/// <summary>
		///  Finds the frame with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the frame to find.</param>
		/// <returns>The located HG-X frame.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  No HG-X frame with the <paramref name="id"/> was found.
		/// </exception>
		public HgxFrame GetFrameById(int id) {
			HgxFrame frame = Frames.FirstOrDefault(f => f.Id == id);
			if (frame == null)
				throw new KeyNotFoundException($"Could not find the Id \"{id}\"!");
			return frame;
		}
		/// <summary>
		///  Finds the frame with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the frame to find.</param>
		/// <returns>The located HG-X frame.-or- null if the Id was not found.</returns>
		public HgxFrame FindFrameById(int id) {
			return Frames.FirstOrDefault(f => f.Id == id);
		}
		/// <summary>
		///  Tries to get the frame with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the frame to find.</param>
		/// <param name="frame">The output located HG-X frame.-or- null if the Id was not found.</param>
		/// <returns>True if the frame was found, otherwise false.</returns>
		public bool TryGetFrameById(int id, out HgxFrame frame) {
			frame = FindFrameById(id);
			return frame != null;
		}
		/// <summary>
		///  Gets the index of the frame with the specified Id.
		/// </summary>
		/// <param name="id">The Id of the frame to find.</param>
		/// <returns>The index of the located HG-X frame.-or- -1 if the Id was not found.</returns>
		public int IndexOfFrameById(int id) {
			return Frames.IndexOf(f => f.Id == id);
		}
		/// <summary>
		///  Gets the index of the frame.
		/// </summary>
		/// <param name="frame">The HG-X frame to get the index of.</param>
		/// <returns>The index of the located HG-X frame.-or- -1 if the frame was not found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="frame"/> is null.
		/// </exception>
		public int IndexOfFrame(HgxFrame frame) {
			if (frame == null)
				throw new ArgumentNullException(nameof(frame));
			return Frames.IndexOf(frame);
		}

		#endregion

		#region ExtractImages

		/// <summary>
		///  Extracts the HG-X image's frames to the specified output directory as a PNG file.
		/// </summary>
		/// <param name="stream">The stream to the HG-X file.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The dictionary of Id/file path key value pairs to the extracted PNGs.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  <paramref name="stream"/> is closed.
		/// </exception>
		public Dictionary<int, string> ExtractImages(Stream stream, string outputDir, HgxOptions options) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			Dictionary<int, string> pngFiles = new Dictionary<int, string>();
			foreach (HgxFrame frame in frames) {
				// Perform assignment instead of add because HG-X
				// images can incorrectly contain duplicate IDs.
				pngFiles[frame.Id] = frame.ExtractImageToDirectory(stream, outputDir, options);
			}
			return pngFiles;
		}

		#endregion

		#region Helpers

		/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="id">The id, which is assocaited to the <see cref="HgxFrame.Id"/>.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="IsAnimation"/> is false.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		public string GetFrameFileName(int id, bool forcePostfix) {
			string baseName = Path.GetFileNameWithoutExtension(FileName);
			if (!forcePostfix && !IsAnimation)
				return Path.ChangeExtension(baseName, ".png");
			return Path.ChangeExtension($"{baseName}+{id:D4}", ".png");
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="HgxImage"/> images.</param>
		/// <param name="id">The id, which is assocaited to the <see cref="HgxFrame.Id"/>.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="IsAnimation"/> is false.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="imgIndex"/> or <paramref name="frmIndex"/> are not a valid frame index.
		/// </exception>
		public string GetFrameFilePath(string directory, int id, bool forcePostfix) {
			return Path.Combine(directory, GetFrameFileName(id, forcePostfix));
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
		/// <returns>The file name of the HG-X image frame.</returns>
		public static string GetFrameFileName(string fileName, int imgIndex, int frmIndex) {
			string baseName = $"{Path.GetFileNameWithoutExtension(fileName)}+{imgIndex:D3}+{frmIndex:D3}";
			return Path.ChangeExtension(baseName, ".png");
		}*/
		/// <summary>
		///  Gets the file name for the JSON HG-X information.
		/// </summary>
		/// <param name="fileName">The base filename of the <see cref="Hg3"/>.</param>
		/// <returns>The file name of the JSON HG-X information.</returns>
		public static string GetJsonFileName(string fileName) {
			return Path.ChangeExtension(Path.GetFileNameWithoutExtension(fileName) + "+hgx", ".json");
		}
		/// <summary>
		///  Gets the file path for the JSON HG-X information.
		/// </summary>
		/// <param name="filePath">The file path of the <see cref="HgxImage"/>.</param>
		/// <returns>The file path of the JSON HG-X information.</returns>
		public static string GetJsonFilePath(string filePath) {
			return Path.Combine(Path.GetDirectoryName(filePath), GetJsonFileName(filePath));
		}
		/// <summary>
		///  Gets the file path for the JSON HG-X information.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="HgxImage"/>.</param>
		/// <param name="fileName">The base file name of the <see cref="HgxImage"/>.</param>
		/// <returns>The file path of the JSON HG-X information.</returns>
		public static string GetJsonFilePath(string directory, string fileName) {
			return Path.Combine(directory, GetJsonFileName(fileName));
		}
		
		#endregion

		#region I/O

#if JSON
		/// <summary>
		///  Deserializes the HG-X image from a json file in the specified directory and file name.
		/// </summary>
		/// <param name="directory">
		///  The directory for the json file to load and deserialize with <paramref name="fileName"/>.
		/// </param>
		/// <returns>The deserialized HG-X image.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static HgxImage FromJsonDirectory(string directory, string fileName) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			string jsonFile = GetJsonFilePath(directory, fileName);
			return JsonConvert.DeserializeObject<HgxImage>(File.ReadAllText(jsonFile));
		}
		/// <summary>
		///  Deserializes the HG-X image from a json file path.
		/// </summary>
		/// <param name="filePath">
		///  The file path to the json file to load and deserialize.
		/// </param>
		/// <returns>The deserialized HG-X image.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> is null.
		/// </exception>
		public static HgxImage FromJsonFile(string filePath) {
			if (filePath == null)
				throw new ArgumentNullException(nameof(filePath));
			string jsonFile = GetJsonFilePath(filePath);
			return JsonConvert.DeserializeObject<HgxImage>(File.ReadAllText(filePath));
		}
		/// <summary>
		///  Serializes the HG-X image to a json file in the specified directory.
		/// </summary>
		/// <param name="directory">The directory to save the json file to using <see cref="JsonFileName"/>.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public void SaveJsonToDirectory(string directory) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			string jsonFile = GetJsonFilePath(directory, FileName);
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
		/// <summary>
		///  Serializes the HG-X image to a json file in the specified directory.
		/// </summary>
		/// <param name="filePath">The file path to save the json file to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> is null.
		/// </exception>
		public void SaveJsonToFile(string filePath) {
			if (filePath == null)
				throw new ArgumentNullException(nameof(filePath));
			string jsonFile = GetJsonFilePath(filePath);
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
		/// <summary>
		///  Opens the stream to the bitmap from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to load the bitmap from.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="IsAnimation"/> is false.
		/// </param>
		/// <returns>The file stream to the bitmap.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public FileStream OpenBitmapStream(string directory, bool forcePostfix) {
			return OpenBitmapStream(directory, 0, forcePostfix);
		}
		/// <summary>
		///  Opens the stream to the bitmap from the specified directory.
		/// </summary>
		/// <param name="directory">The directory to load the bitmap from using <see cref="GetFrameFileName"/>.</param>
		/// <param name="id">The id, which is assocaited to the <see cref="HgxFrame.Id"/>.</param>
		/// <param name="forcePostfix">
		///  True if the animation postfix will always be displayed even when <see cref="IsAnimation"/> is false.
		/// </param>
		/// <returns>The file stream to the bitmap.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public FileStream OpenBitmapStream(string directory, int id, bool forcePostfix) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			return File.OpenRead(GetFrameFilePath(directory, id, forcePostfix));
		}
#endif

		#endregion

		#region Helpers

		/*/// <summary>
		///  Gets the file name for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image2"/>.
		/// </param>
		/// <returns>The file name of the frame.</returns>
		public string GetFrameFileName(int frmIndex) {
			return Hg3.GetFrameFileName(0, frmIndex);
		}
		/// <summary>
		///  Gets the file path for the PNG image with the specified image and frame indecies.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="Hg3"/> images.</param>
		/// <param name="frmIndex">
		///  The second index, which is associated to a frame inside an <see cref="Hg3Image2"/>.
		/// </param>
		/// <returns>The file path of the frame.</returns>
		public string GetFrameFilePath(string directory, int frmIndex) {
			return Hg3.GetFrameFilePath(directory, 0, frmIndex);
		}*/

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the HG-X images's frames.
		/// </summary>
		/// <returns>The HG-X frame enumerator.</returns>
		public IEnumerator<HgxFrame> GetEnumerator() => frames.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-X image.
		/// </summary>
		/// <returns>The string representation of the HG-X image.</returns>
		public override string ToString() => $"{Format.ToDescription() ?? "HG-X"} Image \"{FileName}\" Frames={Count}";
		
		#endregion
	}
}
