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
		public static ZtPackage ExtractAndSaveFiles(string ztFile, string outputDir) {
			using (Stream stream = File.OpenRead(ztFile))
				return ExtractAndSaveFiles(stream, outputDir);
		}
		public static ZtPackage ExtractAndSaveFiles(Stream stream, string outputDir) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			return Extract(stream, outputDir);
		}
		public static ZtPackage Extract(string ztFile) {
			using (Stream stream = File.OpenRead(ztFile))
				return Extract(stream, null);
		}
		public static ZtPackage Extract(Stream stream) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			return Extract(stream, null);
		}


		private static ZtPackage Extract(Stream stream, string outputDir) {
			BinaryReader reader = new BinaryReader(stream);

			long lastStartPosition = stream.Position;
			ZTENTRYHDR entryHdr = new ZTENTRYHDR();
			ZTENTRY entry;

			List<ZTENTRY> fileEntries = new List<ZTENTRY>();

			do {
				stream.Position = lastStartPosition + entryHdr.OffsetNext;
				lastStartPosition = stream.Position;

				entryHdr = reader.ReadUnmanaged<ZTENTRYHDR>();
				entry = reader.ReadUnmanaged<ZTENTRY>();

				fileEntries.Add(entry);

				if (outputDir != null) {
					byte[] fileData = Zlib.Decompress(reader, entry.CompressedLength, entry.DecompressedLength);
					/*byte[] compressed = reader.ReadBytes(entry.CompressedLength);
					byte[] decompressed = new byte[entry.DecompressedLength];

					Zlib.Uncompress(decompressed, ref entry.DecompressedLength, compressed, entry.CompressedLength);*/
					File.WriteAllBytes(Path.Combine(outputDir, entry.FileName), fileData);
				}
			} while (entryHdr.OffsetNext != 0);

			return new ZtPackage(fileEntries.ToArray());
		}

	}
}
