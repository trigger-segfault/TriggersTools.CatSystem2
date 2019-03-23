using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	public partial class ZtPackage {
		public static ZtPackage ExtractFiles(string ztFile, string outputDir) {
			if (ztFile == null)
				throw new ArgumentNullException(nameof(ztFile));
			using (Stream stream = File.OpenRead(ztFile))
				return ExtractFiles(stream, ztFile, outputDir);
		}
		public static ZtPackage ExtractFiles(Stream stream, string fileName, string outputDir) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			return ExtractInternal(stream, fileName, outputDir);
		}
		public static ZtPackage Extract(string ztFile) {
			if (ztFile == null)
				throw new ArgumentNullException(nameof(ztFile));
			using (Stream stream = File.OpenRead(ztFile))
				return ExtractInternal(stream, ztFile, null);
		}
		public static ZtPackage Extract(Stream stream, string fileName) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			return ExtractInternal(stream, fileName, null);
		}


		private static ZtPackage ExtractInternal(Stream stream, string fileName, string outputDir) {
			BinaryReader reader = new BinaryReader(stream);

			long startPosition = stream.Position;
			ZTENTRYHDR entryHdr = new ZTENTRYHDR();
			ZTENTRY entry;

			List<ZTENTRYHDR> fileHdrs = new List<ZTENTRYHDR>();
			List<ZTENTRY> fileEntries = new List<ZTENTRY>();

			do {
				stream.Position = startPosition + entryHdr.OffsetNext;
				startPosition = stream.Position;

				entryHdr = reader.ReadUnmanaged<ZTENTRYHDR>();
				entry = reader.ReadUnmanaged<ZTENTRY>();

				fileHdrs.Add(entryHdr);
				fileEntries.Add(entry);

				if (outputDir != null) {
					byte[] fileData = Zlib.Decompress(reader, entry.CompressedLength, entry.DecompressedLength);
					/*byte[] compressed = reader.ReadBytes(entry.CompressedLength);
					byte[] decompressed = new byte[entry.DecompressedLength];

					Zlib.Uncompress(decompressed, ref entry.DecompressedLength, compressed, entry.CompressedLength);*/
					File.WriteAllBytes(Path.Combine(outputDir, entry.FileName), fileData);
				}
			} while (entryHdr.OffsetNext != 0);

			return new ZtPackage(fileName, fileHdrs.ToArray(), fileEntries.ToArray());
		}

	}
	partial class KifintEntryExtensions {
		#region ExtractZt

		/// <summary>
		///  Extracts the ZT package from the entry and saves the contained files to the output directory.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The extracted ZT package.</returns>
		/// <param name="outputDir">The output directory to save the package files to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static ZtPackage ExtractZtAndFiles(this KifintEntry entry, string outputDir) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return ZtPackage.ExtractFiles(stream, entry.FileName, outputDir);
		}
		/// <summary>
		///  Extracts the ZT package from the open KIFINT archive stream and saves the contained files to the output
		///  directory.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outputDir">The output directory to save the package files to.</param>
		/// <returns>The extracted ZT package.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outputDir"/> is null.
		/// </exception>
		public static ZtPackage ExtractZtAndFiles(this KifintEntry entry, KifintStream kifintStream, string outputDir) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return ZtPackage.ExtractFiles(stream, entry.FileName, outputDir);
		}

		/// <summary>
		///  Extracts the ZT package from the entry.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The extracted ZT package.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static ZtPackage ExtractZt(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return ZtPackage.Extract(stream, entry.FileName);
		}
		/// <summary>
		///  Extracts the ZT package from the open KIFINT archive stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted ZT package.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static ZtPackage ExtractZt(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return ZtPackage.Extract(stream, entry.FileName);
		}

		#endregion
	}
}
