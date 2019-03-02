using System;
using System.IO;

namespace TriggersTools.CatSystem2 {
	partial class Hg3Image {
		#region Extract Public

		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="hg3File">The file path to the HG-3 file.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static Hg3Image ExtractImages(string hg3File, string outputDir, bool expand) {
			using (var stream = File.OpenRead(hg3File))
				return ExtractInternal(stream, hg3File, outputDir, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="stream">The open stream to the HG-3.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outputDir"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static Hg3Image ExtractImages(Stream stream, string fileName, string outputDir, bool expand) {
			return ExtractInternal(stream, fileName, outputDir, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the file.
		/// </summary>
		/// <param name="hg3File">The HG-3 file to extract and decrypt.</param>
		/// <returns>The <see cref="Hg3Image"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> is null.
		/// </exception>
		public static Hg3Image Extract(string hg3File) {
			using (var stream = File.OpenRead(hg3File))
				return ExtractInternal(stream, hg3File, null, false);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the stream.
		/// </summary>
		/// <param name="stream">The stream to the HG-3.</param>
		/// <param name="fileName">The filename used to identify the HG-3.</param>
		/// <returns>The <see cref="Hg3Image"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static Hg3Image Extract(Stream stream, string fileName) {
			return ExtractInternal(stream, fileName, null, false);
		}

		#endregion
	}
	partial class KifintEntry {
		#region ExtractHg3

		/// <summary>
		///  Extracts the HG-3 image information from the KIFINT entry and saves all images to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outputDir"/> is null.
		/// </exception>
		public Hg3Image ExtractHg3AndImages(string outputDir, bool expand) {
			using (MemoryStream ms = ExtractToStream())
				return Hg3Image.ExtractImages(ms, FileName, outputDir, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information from the KIFINT entry's open KIFINT archive stream and saves all
		///  images to the output <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public Hg3Image ExtractHg3AndImages(KifintStream kifintStream, string outputDir, bool expand) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return Hg3Image.ExtractImages(ms, FileName, outputDir, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information ONLY and does not extract the actual images.
		/// </summary>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		public Hg3Image ExtractHg3() {
			using (MemoryStream ms = ExtractToStream())
				return Hg3Image.Extract(ms, FileName);
		}
		/// <summary>
		///  Extracts the HG-3 image information ONLY from open KIFINT archive stream and does not extract the actual
		///  images.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted <see cref="Hg3Image"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public Hg3Image ExtractHg3(KifintStream kifintStream) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return Hg3Image.Extract(ms, FileName);
		}

		#endregion
	}
}
