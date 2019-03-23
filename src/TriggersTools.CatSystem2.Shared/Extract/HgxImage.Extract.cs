using System;
using System.Drawing;
using System.IO;

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region Extract Public

		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="hg3File">The file path to the HG-3 file.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static HgxImage ExtractImages(string hg3File, string outputDir, HgxOptions options) {
			using (var stream = File.OpenRead(hg3File))
				return ExtractInternal(stream, hg3File, outputDir, options);
		}
		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="stream">The open stream to the HG-3.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outputDir"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static HgxImage ExtractImages(Stream stream, string fileName, string outputDir, HgxOptions options) {
			return ExtractInternal(stream, fileName, outputDir, options);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the file.
		/// </summary>
		/// <param name="hg3File">The HG-3 file to extract and decrypt.</param>
		/// <returns>The <see cref="HgxImage"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> is null.
		/// </exception>
		public static HgxImage Extract(string hg3File) {
			using (var stream = File.OpenRead(hg3File))
				return ExtractInternal(stream, hg3File, null, HgxOptions.None);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the stream.
		/// </summary>
		/// <param name="stream">The stream to the HG-3.</param>
		/// <param name="fileName">The filename used to identify the HG-3.</param>
		/// <returns>The <see cref="HgxImage"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static HgxImage Extract(Stream stream, string fileName) {
			return ExtractInternal(stream, fileName, null, HgxOptions.None);
		}

		#endregion

		internal static void ExtractImage(Stream stream, HgxFrame frame, string pngFile, HgxOptions options) {
			switch (frame.HgxType) {
			case HgxFormat.Hg2: ExtractHg2ImageFromFrame(stream, frame, pngFile, options); break;
			case HgxFormat.Hg3: ExtractHg3ImageFromFrame(stream, frame, pngFile, options); break;
			default: throw new InvalidOperationException($"{nameof(HgxFrame.HgxType)} is not valid!");
			}
		}
	}
	partial class KifintEntryExtensions {
		#region ExtractHgx

		/// <summary>
		///  Extracts the HG-X image information from the KIFINT entry and saves all images to the output
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static HgxImage ExtractHgxAndImages(this KifintEntry entry, string outputDir, HgxOptions options) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return HgxImage.ExtractImages(stream, entry.FileName, outputDir, options);
		}
		/// <summary>
		///  Extracts the HG-X image information from the KIFINT entry's open KIFINT archive stream and saves all
		///  images to the output <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outputDir">The output directory to save the images to.</param>
		/// <param name="options">The options for manipulating the image during extraction.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outputDir"/> is null.
		/// </exception>
		public static HgxImage ExtractHgxAndImages(this KifintEntry entry, KifintStream kifintStream, string outputDir, HgxOptions options) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return HgxImage.ExtractImages(stream, entry.FileName, outputDir, options);
		}
		/// <summary>
		///  Extracts the HG-X image information ONLY and does not extract the actual images.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static HgxImage ExtractHgx(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return HgxImage.Extract(stream, entry.FileName);
		}
		/// <summary>
		///  Extracts the HG-X image information ONLY from open KIFINT archive stream and does not extract the actual
		///  images.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted <see cref="HgxImage"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static HgxImage ExtractHgx(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return HgxImage.Extract(stream, entry.FileName);
		}

		#endregion
	}
}
