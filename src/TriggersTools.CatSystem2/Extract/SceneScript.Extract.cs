using System;
using System.IO;
using System.Text;
using TriggersTools.CatSystem2.Native;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class SceneScript {
		#region Extract

		/// <summary>
		///  Extracts the scene script information.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <returns>The extracted <see cref="SceneScript"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> is null.
		/// </exception>
		public static SceneScript Extract(string cstFile) {
			using (var stream = File.OpenRead(cstFile))
				return Extract(stream, cstFile);
		}
		/// <summary>
		///  Extracts the scene script information.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
		/// <returns>The extracted <see cref="SceneScript"/> information.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static SceneScript Extract(Stream stream, string fileName) {
			BinaryReader reader = new BinaryReader(stream);
			CATSCENEHDR hdr = reader.ReadUnmanaged<CATSCENEHDR>();

			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, CATSCENEHDR.ExpectedSignature);

			byte[] compressed = reader.ReadBytes(hdr.CompressedSize);
			byte[] decompressed = new byte[hdr.DecompressedSize];
			int decompressedLength = hdr.DecompressedSize;
			ZLib1.Uncompress(decompressed, ref decompressedLength, compressed, hdr.CompressedSize);

			SCENELINE[] strings;

			using (MemoryStream decompressedStream = new MemoryStream(decompressed))
				strings = ReadScript(decompressedStream, decompressedLength);

			return new SceneScript(fileName, strings);
		}

		#endregion

		#region ReadScript

		/// <summary>
		///  Reads the string entries in the script.
		/// </summary>
		/// <param name="stream">The stream to read the script from.</param>
		/// <param name="length">The actual length of the script file.</param>
		/// <returns>The cat string entries.</returns>
		private static SCENELINE[] ReadScript(Stream stream, int length) {
			BinaryReader reader = new BinaryReader(stream, CatUtils.ShiftJIS);
			SCRIPTHDR hdr = reader.ReadUnmanaged<SCRIPTHDR>();

			if (hdr.ScriptLength + SCRIPTHDR.CbSize != length)
				throw new Exception("Corrupted Script!");
			
			int entryCount = hdr.EntryCount;

			stream.Position = hdr.OffsetTable + SCRIPTHDR.CbSize;

			int[] offsets = reader.ReadInt32s(entryCount);
			SCENELINE[] entries = new SCENELINE[entryCount];

			for (int i = 0; i < entryCount; i++) {
				stream.Position = offsets[i] + hdr.StringTable + SCRIPTHDR.CbSize;
				entries[i] = new SCENELINE {
					Type = reader.ReadUInt16(),
					Content = reader.ReadTerminatedString(),
				};
			}

			return entries;
		}

		#endregion
	}
	partial class KifintEntry {
		#region ExtractScene

		/// <summary>
		///  Extracts the CST scene script from the entry.
		/// </summary>
		/// <returns>The extracted scene script.</returns>
		public SceneScript ExtractScene(KifintEntry entry) {
			using (MemoryStream ms = ExtractToStream())
				return SceneScript.Extract(ms, FileName);
		}
		/// <summary>
		///  Extracts the CST scene script from the open KIFINT archive stream.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted scene script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public SceneScript ExtractScene(KifintStream kifintStream) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return SceneScript.Extract(ms, FileName);
		}

		#endregion
	}
}
