using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class FesScreen {
		#region Extract

		/// <summary>
		///  Extracts the scene script information.
		/// </summary>
		/// <param name="fesFile">The file path to the FES screen script file.</param>
		/// <returns>The extracted <see cref="FesScreen"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fesFile"/> is null.
		/// </exception>
		public static FesScreen Extract(string fesFile) {
			using (var stream = File.OpenRead(fesFile))
				return Extract(stream, fesFile);
		}
		/// <summary>
		///  Extracts the screen script information.
		/// </summary>
		/// <param name="stream">The stream to extract the screen script from.</param>
		/// <param name="fileName">The path or name of the screen script file being extracted.</param>
		/// <returns>The extracted <see cref="FesScreen"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static FesScreen Extract(Stream stream, string fileName) {
			BinaryReader reader = new BinaryReader(stream);
			FESHDR hdr = reader.ReadUnmanaged<FESHDR>();

			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, FESHDR.ExpectedSignature);

			byte[] scriptData = Zlib.Decompress(reader, hdr.CompressedLength, hdr.DecompressedLength);
			/*byte[] compressed = reader.ReadBytes(hdr.CompressedLength);
			byte[] decompressed = new byte[hdr.DecompressedLength];
			int decompressedLength = hdr.DecompressedLength;
			Zlib.Uncompress(decompressed, ref decompressedLength, compressed, hdr.CompressedLength);*/
			
			string[] lines;
			using (MemoryStream ms = new MemoryStream(scriptData))
			using (StreamReader sr = new StreamReader(ms, CatUtils.ShiftJIS))
				lines = sr.ReadLinesToEnd();
			
			return new FesScreen(fileName, lines);
		}

		#endregion
	}
	partial class KifintEntryExtensions {
		#region ExtractScreen

		/// <summary>
		///  Extracts the FES screen script from the entry.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The extracted screen script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static FesScreen ExtractScreen(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return FesScreen.Extract(stream, entry.FileName);
		}
		/// <summary>
		///  Extracts the FES screen script from the open KIFINT archive stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted screen script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static FesScreen ExtractScreen(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return FesScreen.Extract(stream, entry.FileName);
		}

		#endregion
	}
}
