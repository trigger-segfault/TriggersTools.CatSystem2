using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.Enums;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class KifintArchive {
		#region LoadArchive

		/// <summary>
		///  Loads and decrypts the KIFINT archive entries using the V_CODE2.<para/>
		///  Using this will initialize with <see cref="KifintType.Unknown"/>.
		/// </summary>
		/// <param name="kifintPath">The path to the KIFINT archive to decrypt.</param>
		/// <param name="vcode2">The V_CODE2 key obtained from the exe, used to decrypt the file names.</param>
		/// <param name="callback">The optional callback for progress made during loading.</param>
		/// <returns>The <see cref="KifintArchive"/> with all of the loaded entries.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintPath"/> or <paramref name="vcode2"/> is null.
		/// </exception>
		public static KifintArchive LoadArchive(string kifintPath, string vcode2,
			KifintProgressCallback callback = null)
		{
			if (kifintPath == null)
				throw new ArgumentNullException(nameof(kifintPath));
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			KifintType type = KifintType.Unknown;
			KifintArchive archive;
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = 1,
			};
			
			progress.ArchiveName = Path.GetFileName(kifintPath);
			using (Stream stream = File.OpenRead(kifintPath))
				archive = LoadLookup(type, stream, kifintPath, vcode2, progress, callback);
			progress.ArchiveIndex++;

			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
			return archive;
		}
		/// <summary>
		///  Loads and decrypts the KIFINT archive entries using the V_CODE2.<para/>
		///  Using this will initialize with the specified KIFINT archive type.
		/// </summary>
		/// <param name="kifintPath">The path to the KIFINT archive to decrypt.</param>
		/// <param name="type">The type of archive to create.</param>
		/// <param name="vcode2">The V_CODE2 key obtained from the exe, used to decrypt the file names.</param>
		/// <param name="callback">The optional callback for progress made during loading.</param>
		/// <returns>The <see cref="KifintArchive"/> with all of the loaded entries.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintPath"/> or <paramref name="vcode2"/> is null.
		/// </exception>
		public static KifintArchive LoadArchive(string kifintPath, KifintType type, string vcode2,
			KifintProgressCallback callback = null) {
			if (kifintPath == null)
				throw new ArgumentNullException(nameof(kifintPath));
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			KifintArchive archive;
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = 1,
			};

			progress.ArchiveName = Path.GetFileName(kifintPath);
			using (Stream stream = File.OpenRead(kifintPath))
				archive = LoadLookup(type, stream, kifintPath, vcode2, progress, callback);
			progress.ArchiveIndex++;

			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
			return archive;
		}

		#endregion

		#region DecryptLookup

		/// <summary>
		///  Decrypts the KIFINT archives using the wildcard search, install directory, and name of executable with the
		///  V_CODE2 used to decrypt.<para/>
		///  Using this will initialize with <see cref="KifintType.Unknown"/>.
		/// </summary>
		/// <param name="wildcard">The wildcard name of the files to look for and merge.</param>
		/// <param name="installDir">The installation directory for both the archives and executable.</param>
		/// <param name="vcode2">The V_CODE2 key obtained from the exe, used to decrypt the file names.</param>
		/// <param name="callback">The optional callback for progress made during decryption.</param>
		/// <returns>The <see cref="KifintLookup"/> merged with all loaded archives.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="wildcard"/>, <paramref name="installDir"/>, or <paramref name="vcode2"/> is null.
		/// </exception>
		public static KifintLookup LoadLookup(string wildcard, string installDir, string vcode2,
			KifintProgressCallback callback = null)
		{
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			KifintType type = KifintType.Unknown;
			KifintLookup lookup = new KifintLookup(type);
			string[] files = Directory.GetFiles(installDir, wildcard);
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = files.Length,
			};
			foreach (string kifintPath in files) {
				progress.ArchiveName = Path.GetFileName(kifintPath);
				using (Stream stream = File.OpenRead(kifintPath))
					lookup.Merge(LoadLookup(type, stream, kifintPath, vcode2, progress, callback));
				progress.ArchiveIndex++;
			}
			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
			return lookup;
		}
		/// <summary>
		///  Decrypts the KIFINT archives using the known archive type, install directory, and name of executable with
		///  the V_CODE2 used to decrypt.
		/// </summary>
		/// <param name="type">The type of archive to look for and decrypt.</param>
		/// <param name="installDir">The installation directory for both the archives and executable.</param>
		/// <param name="vcode2">The V_CODE2 key obtained from the exe, used to decrypt the file names.</param>
		/// <param name="callback">The optional callback for progress made during decryption.</param>
		/// <returns>The <see cref="KifintLookup"/> merged with all loaded archives.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="installDir"/> or <paramref name="vcode2"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is <see cref="KifintType.Unknown"/>.
		/// </exception>
		public static KifintLookup LoadLookup(KifintType type, string installDir, string vcode2,
			KifintProgressCallback callback = null)
		{
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			if (type == KifintType.Unknown)
				throw new ArgumentException($"{nameof(type)} cannot be {nameof(KifintType.Unknown)}!", nameof(type));
			KifintLookup lookup = new KifintLookup(type);
			var files = Enumerable.Empty<string>();
			var wildcardAttr = EnumInfo.GetAttribute<KifintType, KifintWildcardAttribute>(type);
			foreach (string wildcard in wildcardAttr.Wildcards) {
				files = files.Concat(Directory.GetFiles(installDir, wildcard));
			}
			//string wildcard = EnumInfo.GetAttribute<KifintType, KifintWildcardAttribute>(type).Wildcard;
			//string[] files = Directory.GetFiles(installDir, wildcard);
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = files.Count(),
			};
			foreach (string kifintPath in files) {
				progress.ArchiveName = Path.GetFileName(kifintPath);
				using (Stream stream = File.OpenRead(kifintPath))
					lookup.Merge(LoadLookup(type, stream, kifintPath, vcode2, progress, callback));
				progress.ArchiveIndex++;
			}
			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
			return lookup;
		}

		#endregion

		#region DecryptLookups

		/*public static KifintLookupCollection DecryptLookups(KifintType type, string installDir, string vcode2,
			KifintProgressCallback callback = null)
		{

		}*/

		#endregion

		#region DecryptArchive

		public static void DecryptArchives(string wildcard, string installDir, string vcode2,
			KifintProgressCallback callback = null) {
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			KifintType type = KifintType.Unknown;
			string[] files = Directory.GetFiles(installDir, wildcard);
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = files.Length,
			};
			foreach (string kifintPath in files) {
				progress.ArchiveName = Path.GetFileName(kifintPath);
				string kifintBackup = Path.Combine(Path.GetDirectoryName(installDir) + "intbackups");
				using (Stream stream = File.OpenRead(kifintPath))
					DecryptArchive(stream, kifintPath, kifintBackup, vcode2, false, progress, callback);
				progress.ArchiveIndex++;
			}
			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
		}
		public static void DecryptArchives(KifintType type, string installDir, string vcode2,
			KifintProgressCallback callback = null)
		{
			if (vcode2 == null)
				throw new ArgumentNullException(nameof(vcode2));
			if (type == KifintType.Unknown)
				throw new ArgumentException($"{nameof(type)} cannot be {nameof(KifintType.Unknown)}!", nameof(type));
			var files = Enumerable.Empty<string>();
			var wildcardAttr = EnumInfo.GetAttribute<KifintType, KifintWildcardAttribute>(type);
			foreach (string wildcard in wildcardAttr.Wildcards) {
				files = files.Concat(Directory.GetFiles(installDir, wildcard));
			}
			//string wildcard = EnumInfo.GetAttribute<KifintType, KifintWildcardAttribute>(type).Wildcard;
			//string[] files = Directory.GetFiles(installDir, wildcard);
			KifintProgressArgs progress = new KifintProgressArgs {
				ArchiveType = type,
				ArchiveIndex = 0,
				ArchiveCount = files.Count(),
			};
			foreach (string kifintPath in files) {
				progress.ArchiveName = Path.GetFileName(kifintPath);
				string kifintBackup = Path.Combine(Path.GetDirectoryName(installDir) + "intbackups");
				using (Stream stream = File.OpenRead(kifintPath))
					DecryptArchive(stream, kifintPath, kifintBackup, vcode2, false, progress, callback);
				progress.ArchiveIndex++;
			}
			progress.EntryName = null;
			progress.EntryIndex = 0;
			progress.EntryCount = 0;
			callback?.Invoke(progress);
		}
		public static void RestoreEncryptedArchives(string wildcard, string installDir) {
			string backupDir = Path.Combine(installDir, "intbackup");
			if (!Directory.Exists(backupDir))
				return;
			string[] files = Directory.GetFiles(backupDir, wildcard);
			foreach (string kifintBackup in files) {
				string kifintPath = Path.Combine(installDir, Path.GetFileName(kifintBackup));
				if (File.Exists(kifintBackup) && File.Exists(kifintPath)) {
					File.Delete(kifintPath);
					File.Move(kifintBackup, kifintPath);
				}
			}
		}
		public static void RestoreEncryptedArchives(KifintType type, string installDir) {
			if (type == KifintType.Unknown)
				throw new ArgumentException($"{nameof(type)} cannot be {nameof(KifintType.Unknown)}!", nameof(type));
			string backupDir = Path.Combine(installDir, "intbackup");
			if (!Directory.Exists(backupDir))
				return;
			var files = Enumerable.Empty<string>();
			var wildcardAttr = EnumInfo.GetAttribute<KifintType, KifintWildcardAttribute>(type);
			foreach (string wildcard in wildcardAttr.Wildcards) {
				files = files.Concat(Directory.GetFiles(installDir, wildcard));
			}
			//string[] files = Directory.GetFiles(backupDir, wildcard);
			foreach (string kifintBackup in files) {
				string kifintPath = Path.Combine(installDir, Path.GetFileName(kifintBackup));
				if (File.Exists(kifintBackup) && File.Exists(kifintPath)) {
					File.Delete(kifintPath);
					File.Move(kifintBackup, kifintPath);
				}
			}
		}

		#endregion

		#region IdentifyFileTypes

		public static string[] IdentifyFileTypes(string kifintPath, string vcode2) {
			using (Stream stream = File.OpenRead(kifintPath))
				return IdentifyFileTypes(stream, kifintPath, vcode2);
		}
		private static string[] IdentifyFileTypes(Stream stream, string kifintPath, string vcode2) {
			BinaryReader reader = new BinaryReader(stream);
			KIFHDR hdr = reader.ReadUnmanaged<KIFHDR>();

			if (hdr.Signature != "KIF") // It's really a KIF INT file
				throw new UnexpectedFileTypeException(kifintPath, "KIF");

			KIFENTRY[] entries = reader.ReadUnmanagedArray<KIFENTRY>(hdr.EntryCount);

			uint tocSeed = GenerateTocSeed(vcode2);
			uint fileKey = 0;
			bool decrypt = false;

			// Obtain the decryption file key if one exists
			for (int i = 0; i < hdr.EntryCount; i++) {
				if (entries[i].FileName == KeyFileName) {
					fileKey = MersenneTwister.GenRand(entries[i].Length);
					decrypt = true;
					break;
				}
			}

			HashSet<string> extensions = new HashSet<string>();

			// Decrypt the KIFINT entries using the file key
			if (decrypt) {
				for (uint i = 0; i < hdr.EntryCount; i++) {
					if (entries[i].FileName == KeyFileName)
						continue;
					// Give the entry the correct name
					UnobfuscateFileName(entries[i].FileNameRaw, tocSeed + i);
				}
			}
			for (uint i = 0; i < hdr.EntryCount; i++) {
				string entryFileName = entries[i].FileName;
				if (entryFileName == KeyFileName)
					continue;
				extensions.Add(Path.GetExtension(entryFileName));
			}

			return extensions.ToArray();
		}

		#endregion
		
		#region Extract
		
		/// <summary>
		///  Extracts the KIFINT entry file from the the entry's KIFINT archive.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="entry">The KIFINT entry used to locate the file.</param>
		/// <returns>A byte array containing the extracted KIFINT entry's file data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="entry"/> is null.
		/// </exception>
		internal static byte[] ExtractToBytes(KifintStream kifintStream, KifintEntry entry) {
			if (kifintStream == null)
				throw new ArgumentNullException(nameof(kifintStream));
			if (entry == null)
				throw new ArgumentNullException(nameof(entry));
			var kifint = entry.Kifint;
			kifintStream.Open(kifint);
			BinaryReader reader = new BinaryReader(kifintStream);
			kifintStream.Position = entry.Offset;
			byte[] buffer = reader.ReadBytes(entry.Length);

			if (kifint.IsEncrypted) {
				kifint.Blowfish.Decrypt(buffer, entry.Length & ~7);
			}
			return buffer;
		}
		/// <summary>
		///  Extracts the KIFINT entry to a fixed stream of <paramref name="kifintStream"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="entry">The KIFINT entry used to locate the file.</param>
		/// <param name="leaveOpen">
		///  True if the KIFINT archive stream should be left open even after closing the returned stream.
		/// </param>
		/// <returns>
		///  A fixed stream containing the data of the decrypted entry. This stream must always be disposed of, because
		///  it's not guaranteed to be a fixed stream of <paramref name="kifintStream"/>. This is the case when the
		///  length is small enough for extracting bytes to be more efficient.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="entry"/> is null.
		/// </exception>
		internal static Stream ExtractToStream(KifintStream kifintStream, KifintEntry entry, bool leaveOpen) {
			if (kifintStream == null)
				throw new ArgumentNullException(nameof(kifintStream));
			if (entry == null)
				throw new ArgumentNullException(nameof(entry));
			var kifint = entry.Kifint;

			// Smaller streams will be faster as reading a big chunk of data all at once.
			if (kifint.IsEncrypted && entry.Length < 131072 * 2) { // Some arbitrary power of 2 cutoff length
				try {
					return new MemoryStream(ExtractToBytes(kifintStream, entry));
				} finally {
					// We need to make sure the the stream is closed by us if we're not
					// relying on the fixed streams to close the KIFINT archive stream.
					if (!leaveOpen)
						kifintStream.Close();
				}
			}

			kifintStream.Open(kifint);
			kifintStream.Position = entry.Offset;

			if (kifint.IsEncrypted) {
				return new BlowfishInputStream(kifint.Blowfish, kifintStream, entry.Length, leaveOpen);
			}
			return new FixedStream(kifintStream, entry.Length, leaveOpen);
		}

		#endregion
	}
}
