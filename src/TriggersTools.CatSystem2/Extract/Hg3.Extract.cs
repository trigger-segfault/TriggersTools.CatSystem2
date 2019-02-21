using System;
using System.IO;

namespace TriggersTools.CatSystem2 {
	partial class Hg3 {
		#region Extract Public

		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="directory"/>.
		/// </summary>
		/// <param name="hg3File">The file path to the HG-3 file.</param>
		/// <param name="directory">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> or <paramref name="directory"/> is null.
		/// </exception>
		public static Hg3 ExtractImages(string hg3File, string directory, bool expand) {
			using (var stream = File.OpenRead(hg3File))
				return Extract(stream, hg3File, directory, true, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information and images contained within HG-3 file to the output
		///  <paramref name="directory"/>.
		/// </summary>
		/// <param name="stream">The open stream to the HG-3.</param>
		/// <param name="directory">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="directory"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static Hg3 ExtractImages(Stream stream, string fileName, string directory, bool expand) {
			return Extract(stream, fileName, directory, true, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the file.
		/// </summary>
		/// <param name="hg3File">The HG-3 file to extract and decrypt.</param>
		/// <returns>The <see cref="Hg3"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hg3File"/> is null.
		/// </exception>
		public static Hg3 Extract(string hg3File) {
			using (var stream = File.OpenRead(hg3File))
				return Extract(stream, hg3File, null, false, false);
		}
		/// <summary>
		///  Extracts the HG-3 image info ONLY from the stream.
		/// </summary>
		/// <param name="stream">The stream to the HG-3.</param>
		/// <param name="fileName">The filename used to identify the HG-3.</param>
		/// <returns>The <see cref="Hg3"/> file information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static Hg3 Extract(Stream stream, string fileName) {
			return Extract(stream, fileName, null, false, false);
		}

		#endregion
	}
	partial class KifintEntry {
		#region ExtractHg3

		/// <summary>
		///  Extracts the HG-3 image information from the KIFINT entry and saves all images to the output
		///  <paramref name="directory"/>.
		/// </summary>
		/// <param name="directory">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public Hg3 ExtractHg3AndImages(string directory, bool expand) {
			using (MemoryStream ms = ExtractToStream())
				return Hg3.ExtractImages(ms, FileName, directory, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information from the KIFINT entry's open KIFINT archive stream and saves all
		///  images to the output <paramref name="directory"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="directory">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="directory"/> is null.
		/// </exception>
		public Hg3 ExtractHg3AndImages(KifintStream kifintStream, string directory, bool expand) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return Hg3.ExtractImages(ms, FileName, directory, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information ONLY and does not extract the actual images.
		/// </summary>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		public Hg3 ExtractHg3() {
			using (MemoryStream ms = ExtractToStream())
				return Hg3.Extract(ms, FileName);
		}
		/// <summary>
		///  Extracts the HG-3 image information ONLY from open KIFINT archive stream and does not extract the actual
		///  images.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public Hg3 ExtractHg3(KifintStream kifintStream) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return Hg3.Extract(ms, FileName);
		}

		#endregion
	}
}
