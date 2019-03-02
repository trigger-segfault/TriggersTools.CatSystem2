using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Native;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class ScreenScript {
		#region Extract

		/// <summary>
		///  Extracts the scene script information.
		/// </summary>
		/// <param name="fesFile">The file path to the FES screen script file.</param>
		/// <returns>The extracted <see cref="ScreenScript"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fesFile"/> is null.
		/// </exception>
		public static ScreenScript Extract(string fesFile) {
			using (var stream = File.OpenRead(fesFile))
				return Extract(stream, fesFile);
		}
		/// <summary>
		///  Extracts the screen script information.
		/// </summary>
		/// <param name="stream">The stream to extract the screen script from.</param>
		/// <param name="fileName">The path or name of the screen script file being extracted.</param>
		/// <returns>The extracted <see cref="ScreenScript"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static ScreenScript Extract(Stream stream, string fileName) {
			BinaryReader reader = new BinaryReader(stream);
			FESHDR hdr = reader.ReadUnmanaged<FESHDR>();

			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, FESHDR.ExpectedSignature);

			byte[] compressed = reader.ReadBytes(hdr.CompressedSize);
			byte[] decompressed = new byte[hdr.DecompressedSize];
			int decompressedLength = hdr.DecompressedSize;
			Zlib.Uncompress(decompressed, ref decompressedLength, compressed, hdr.CompressedSize);
			
			string[] lines;
			using (MemoryStream memoryStream = new MemoryStream(decompressed, 0, decompressedLength))
			using (StreamReader memoryReader = new StreamReader(memoryStream, CatUtils.ShiftJIS))
				lines = memoryReader.ReadLinesToEnd();
			
			return new ScreenScript(fileName, lines);
		}

		#endregion
	}
	partial class KifintEntry {
		#region ExtractScreen

		/// <summary>
		///  Extracts the FES screen script from the entry.
		/// </summary>
		/// <returns>The extracted screen script.</returns>
		public ScreenScript ExtractScreen(KifintEntry entry) {
			using (MemoryStream ms = ExtractToStream())
				return ScreenScript.Extract(ms, FileName);
		}
		/// <summary>
		///  Extracts the FES screen script from the open KIFINT archive stream.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted screen script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public ScreenScript ExtractScreen(KifintStream kifintStream) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return ScreenScript.Extract(ms, FileName);
		}

		#endregion
	}
}
