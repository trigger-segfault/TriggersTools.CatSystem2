using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class KifintArchive {
		#region Constants

		/// <summary>
		///  The file name of the KIFINT entry used as a blowfish decryption key.
		/// </summary>
		public const string KeyFileName = "__key__.dat";

		#endregion

		#region Private DecryptLookup

		/// <summary>
		///  Decrypts the KIFINT archives using the known archive type, install directory, and name of executable with
		///  the V_CODE2 used to decrypt.
		/// </summary>
		/// <param name="type">The type of archive to look for and decrypt.</param>
		/// <param name="stream">The stream to the open KIFINT archive.</param>
		/// <param name="installDir">The installation directory for both the archives and executable.</param>
		/// <param name="exePath">The path to the executable to extract the V_CODE2 key from.</param>
		/// <returns>The <see cref="KifintLookup"/> merged with all loaded archives.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="kifintPath"/>, or <paramref name="exePath"/> is null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		private static KifintArchive LoadLookup(KifintType type, Stream stream, string kifintPath, string vcode2,
			KifintProgressArgs progress, KifintProgressCallback callback)
		{
			if (kifintPath == null)
				throw new ArgumentNullException(nameof(kifintPath));
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));

			BinaryReader reader = new BinaryReader(stream);
			KIFHDR hdr = reader.ReadUnmanaged<KIFHDR>();
			
			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, KIFHDR.ExpectedSignature);

			KIFENTRY[] entries = reader.ReadUnmanagedArray<KIFENTRY>(hdr.EntryCount);

			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = entries.Length;

			// Table of contents seed
			uint tocSeed = GenerateTocSeed(vcode2);
			uint fileKey = 0;
			int fileKeyIndex = -1;
			Blowfish blowfish = null;

			// Obtain the decryption file key if one exists
			for (int i = 0; i < hdr.EntryCount; i++) {
				if (entries[i].FileName == KeyFileName) {
					fileKey = MersenneTwister.GenRand(entries[i].Length);
					if (!CatDebug.NativeBlowfish)
						blowfish = new BlowfishManaged(fileKey);
					else
						blowfish = new BlowfishNative(fileKey);
					fileKeyIndex = i;
					break;
				}
			}
			
			const int ProgressThreshold = 500;
			
			// Decrypt the KIFINT entries using the file key
			if (fileKeyIndex != -1) {
				for (uint i = 0; i < hdr.EntryCount; i++, progress.EntryIndex++) {
					if (unchecked((int) i) == fileKeyIndex)
						continue;

					// Give the entry the correct name
					UnobfuscateFileName(entries[i].FileNameRaw, unchecked(tocSeed + i));
					// Apply the extra offset before decryption
					entries[i].Offset += i;
					// Decrypt the entry's offset and length
					blowfish.Decrypt(ref entries[i].Info);

					progress.EntryName = entries[i].FileName;
					if (i % ProgressThreshold == 0 || i + 1 == hdr.EntryCount)
						callback?.Invoke(progress);
				}
			}

			return new KifintArchive(kifintPath, entries, fileKeyIndex != -1, fileKey, type, blowfish);
		}

		#endregion
		
		#region Private DecryptLookup
		
		private static bool DecryptArchive(Stream inStream, string kifintPath, string kifintBackup, string vcode2, bool isBackup,
			KifintProgressArgs progress, KifintProgressCallback callback)
		{
			if (kifintPath == null)
				throw new ArgumentNullException(nameof(kifintPath));
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));

			/*const string BackupName = "intbackup";
			string dir = Path.GetDirectoryName(kifintPath);
			string name = Path.GetFileName(kifintPath);
			string kifintBackup = Path.Combine(dir, BackupName, name);
			bool isBackup = Path.GetFileName(dir).ToLower() == BackupName;
			if (isBackup) {
				dir = Path.GetDirectoryName(dir);
				kifintPath = Path.Combine(dir, name);
			}
			else {
				
			}*/
			Directory.CreateDirectory(Path.GetDirectoryName(kifintBackup));

			BinaryReader reader = new BinaryReader(inStream);
			KIFHDR hdr = reader.ReadUnmanaged<KIFHDR>();

			try {
				UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, KIFHDR.ExpectedSignature);
			} catch (UnexpectedFileTypeException) {
				if (!isBackup && File.Exists(kifintBackup)) {
					inStream.Close();
					// We must have stopped while decrypting an archive. Let's restart from the already-made backup
					using (inStream = File.OpenRead(kifintBackup))
						DecryptArchive(inStream, kifintPath, kifintBackup, vcode2, true, progress, callback);
					return true;
				}
				throw;
			}
			
			List<KIFENTRY> entries = new List<KIFENTRY>(hdr.EntryCount);
			entries.AddRange(reader.ReadUnmanagedArray<KIFENTRY>(hdr.EntryCount));

			// Table of contents seed
			uint tocSeed = GenerateTocSeed(vcode2);
			uint fileKey = 0;
			bool decrypt = false;
			Blowfish blowfish = null;
			int keyIndex = -1;

			// Obtain the decryption file key if one exists
			for (int i = 0; i < hdr.EntryCount; i++) {
				if (entries[i].FileName == KeyFileName) {
					fileKey = MersenneTwister.GenRand(entries[i].Length);
					decrypt = true;
					if (!CatDebug.NativeBlowfish)
						blowfish = new BlowfishManaged(fileKey);
					else
						blowfish = new BlowfishNative(fileKey);
					keyIndex = i;
					break;
				}
			}

			// This archive is already decrypted, return and let the calling method know
			if (!decrypt)
				return false;
			
			if (isBackup) {
				using (Stream outStream = File.Create(kifintPath))
					DecryptArchiveSave(hdr, entries, tocSeed, fileKey, blowfish, keyIndex, inStream, outStream,
						progress, callback);
			}
			else {
				if (File.Exists(kifintBackup)) {
					File.Delete(kifintBackup);
				}
				inStream.Close();
				File.Move(kifintPath, kifintBackup);
				using (inStream = File.OpenRead(kifintBackup))
				using (Stream outStream = File.Create(kifintPath))
					DecryptArchiveSave(hdr, entries, tocSeed, fileKey, blowfish, keyIndex, inStream, outStream,
						progress, callback);
			}
			
			return true;
		}

		private static void DecryptArchiveSave(KIFHDR hdr, List<KIFENTRY> entries, uint tocSeed, uint fileKey,
			Blowfish blowfish, int keyIndex, Stream inStream, Stream outStream, KifintProgressArgs progress,
			KifintProgressCallback callback)
		{
			BinaryReader reader = new BinaryReader(inStream);
			BinaryWriter writer = new BinaryWriter(outStream);

			const int ProgressThreshold = 1;

			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = entries.Count;
			
			// Decrypt the KIFINT entries using the file key
			for (uint i = 0; i < hdr.EntryCount; i++, progress.EntryIndex++) {
				if (unchecked((int) i) == keyIndex)
					continue;

				KIFENTRY entry = entries[unchecked((int) i)];
				
				// Give the entry the correct name
				UnobfuscateFileName(entry.FileNameRaw, unchecked(tocSeed + i));
				// Apply the extra offset to be decrypted
				entry.Offset += i;
				// Decrypt the entry's length and offset
				blowfish.Decrypt(ref entry.Info);

				progress.EntryName = entry.FileName;
				if (i % ProgressThreshold == 0 || i + 1 == hdr.EntryCount)
					callback?.Invoke(progress);

				// Goto the entry's decrypted offset, read the buffer, then decrypt it
				inStream.Position = entry.Offset;
				byte[] buffer = reader.ReadBytes(entry.Length);
				blowfish.Decrypt(buffer, entry.Length & ~7);

				// Move to the entry's offset in the output stream and write the buffer
				outStream.Position = entry.Offset;
				writer.Write(buffer);

				// Make sure to reassign the entry to the list, because
				// it's not an array and does not return struct references.
				entries[unchecked((int) i)] = entry;
			}

			entries.RemoveAt(keyIndex);
			hdr.EntryCount--;

			outStream.Position = 0;
			writer.WriteUnmanaged(hdr);
			writer.WriteUnmanagedArray(entries);
		}

		#endregion

		#region UnobfuscateFileName

		/// <summary>
		///  Unobfuscates the <see cref="KIFENTRY.FileNameRaw"/> field using the specified seed.
		/// </summary>
		/// <param name="fileName">The raw file name in bytes.</param>
		/// <param name="seed">
		///  The seed used to generate the unobfuscation key.
		///  This is the value generated by <see cref="GenerateTocSeed"/> + entryIndex.
		/// </param>
		private static void UnobfuscateFileName(byte[] fileName, uint seed) {
			const int Length = 52;
			const string FWD = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			const string REV = "zyxwvutsrqponmlkjihgfedcbaZYXWVUTSRQPONMLKJIHGFEDCBA";

			uint key = MersenneTwister.GenRand(seed);
			int shift = (byte) ((key >> 24) + (key >> 16) + (key >> 8) + key);

			for (int i = 0; i < fileName.Length; i++, shift++) {
				byte c = fileName[i];

				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) {
					int index = 0;
					int index2 = shift;

					while (REV[index2 % Length] != c) {
						if (REV[(shift + index + 1) % Length] == c) {
							index += 1;
							break;
						}

						if (REV[(shift + index + 2) % Length] == c) {
							index += 2;
							break;
						}

						if (REV[(shift + index + 3) % Length] == c) {
							index += 3;
							break;
						}

						index += 4;
						index2 += 4;

						if (index >= Length) // We're outside the array, no need to continue
							break;
					}

					if (index < Length) // Only assign if we're inside the array
						fileName[i] = (byte) FWD[index];
				}
			}
		}
		/// <summary>
		///  Generates the table of contents seed used during <see cref="UnobfuscateFileName"/>.
		/// </summary>
		/// <param name="vcode2">The decrypted V_CODE2 extracted from the game resource.</param>
		/// <returns>The generated seed.</returns>
		private static uint GenerateTocSeed(string vcode2) {
			const uint magic = 0x04C11DB7;
			uint seed = uint.MaxValue;

			for (int i = 0; i < vcode2.Length; i++) {
				seed ^= ((uint) vcode2[i]) << 24;

				for (int j = 0; j < 8; j++) {
					if ((seed & 0x80000000) != 0) {
						seed *= 2;
						seed ^= magic;
					}
					else {
						seed *= 2;
					}
				}

				seed = ~seed;
			}

			return seed;
		}

		#endregion
	}
}
