using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A loaded and cached KIFINT archive.
	/// </summary>
	public sealed partial class Kifint : IReadOnlyCollection<KifintEntry> {
		#region Fields

		/// <summary>
		///  Gets the file path to the KIFINT archive.
		/// </summary>
		public string FilePath { get; private set; }
		/// <summary>
		///  Gets the file key used for decryption. Null if there is no encryption.
		/// </summary>
		public uint FileKey { get; private set; }
		/// <summary>
		///  Gets if the KIFINT archive requires decryption when accessing a file.
		/// </summary>
		public bool IsEncrypted { get; private set; }
		/// <summary>
		///  Gets the list of entries in the KIFINT archive.
		/// </summary>
		public IReadOnlyDictionary<string, KifintEntry> Entries { get; private set; }
		/// <summary>
		///  Gets the type associated with this archive.
		/// </summary>
		public KifintType ArchiveType { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the file name of the KIFINT archive.
		/// </summary>
		public string FileName => Path.GetFileName(FilePath);
		/// <summary>
		///  Gets the file name of the KIFINT archive without the extension.
		/// </summary>
		public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FilePath);
		/// <summary>
		///  Gets the number of cached entries in the KIFINT archive.
		/// </summary>
		public int Count => Entries.Count;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned KIFINT archive for use with <see cref="Read"/>.
		/// </summary>
		private Kifint() { }
		/// <summary>
		///  Constructs a cached KIFINT archive with the specified path, entries, and file key.
		/// </summary>
		/// <param name="kifintPath">The absolute path to the KIFINT archive.</param>
		/// <param name="kifEntries">The array of unobfuscated KIFENTRIES inside the KIFINT.</param>
		/// <param name="decrypt">True if the file key is required.</param>
		/// <param name="fileKey">The file key when <paramref name="decrypt"/> is true.</param>
		private Kifint(string kifintPath, KIFENTRY[] kifEntries, bool decrypt, uint fileKey, KifintType type) {
			FilePath = kifintPath;
			IsEncrypted = decrypt;
			if (IsEncrypted)
				FileKey = fileKey;
			ArchiveType = type;
			Dictionary<string, KifintEntry> entries = new Dictionary<string, KifintEntry>(kifEntries.Length);
			foreach (var kifEntry in kifEntries) {
				string fileName = kifEntry.FileName;
				if (fileName != KeyFileName) {
					entries.Add(fileName, new KifintEntry(fileName, kifEntry, this));
				}
			}
			Entries = new ReadOnlyDictionary<string, KifintEntry>(entries);
		}

		#endregion

		#region Accessors
		
		/// <summary>
		///  Gets the KIFINT entry with the specified file name key.
		/// </summary>
		/// <param name="key">The file name of the entry to get.</param>
		/// <returns>The KIFINT entry with the specified file name key.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="key"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  The entry with the <paramref name="key"/> was not found.
		/// </exception>
		public KifintEntry this[string key] => Entries[key];
		/// <summary>
		///  Tries to get the KIFINT entry with the specified key and returns true on success.
		/// </summary>
		/// <param name="key">The key of the entry to get.</param>
		/// <param name="value">The output entry if the key was found.</param>
		/// <returns>True if the key was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="key"/> is null.
		/// </exception>
		public bool TryGetValue(string key, out KifintEntry value) => Entries.TryGetValue(key, out value);
		/// <summary>
		///  Returns true if a KIFINT entry with the specified key exists.
		/// </summary>
		/// <param name="key">The key to look for.</param>
		/// <returns>True if the key was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="key"/> is null.
		/// </exception>
		public bool ContainsKey(string key) => Entries.ContainsKey(key);

		#endregion

		#region SaveList

		/// <summary>
		///  Writes the list of entry names to the specified file in the specified format.
		/// </summary>
		/// <param name="path">The file path to the entries to write to.</param>
		/// <param name="format">The format to write the entries in.</param>
		public void SaveList(string path, KifintListFormat format) {
			using (StreamWriter writer = new StreamWriter(path))
				SaveList(writer, format);
		}
		/// <summary>
		///  Writes the list of entry names to the specified writer in the specified format.
		/// </summary>
		/// <param name="writer">The stream writer to the entries to write to.</param>
		/// <param name="format">The format to write the entries in.</param>
		public void SaveList(StreamWriter writer, KifintListFormat format) {
			var list = new KifintList(this);
			var ordered = Entries.Values.OrderBy(e => e.FileName);
			switch (format) {
			case KifintListFormat.Text:
				writer.Write(list.FilePath);
				for (int i = 0; i < list.Entries.Count; i++)
					writer.WriteLine(list.Entries[i]);
				break;
			case KifintListFormat.Csv:
				writer.Write(list.FilePath);
				for (int i = 0; i < list.Entries.Count; i++) {
					writer.Write(',');
					writer.Write(list.Entries[i]);
				}
				break;
			case KifintListFormat.Json:
				writer.Write(JsonConvert.SerializeObject(list));
				break;
			default:
				throw new ArgumentException($"{nameof(format)} is undefined!");
			}
		}

		#endregion

		#region I/O

		/// <summary>
		///  Writes the cached KIFINT archive to the stream. For use with <see cref="KifintLookup"/>.
		/// </summary>
		/// <param name="writer">The writer for the current stream.</param>
		internal void Write(BinaryWriter writer) {
			writer.Write(Path.GetFileName(FilePath));

			writer.Write(IsEncrypted);
			writer.Write(FileKey);

			writer.Write(Entries.Count);
			foreach (KifintEntry entry in Entries.Values) {
				entry.Write(writer);
			}
		}
		/// <summary>
		///  Reads the KIFINT archive from the stream. For use with <see cref="KifintLookup"/>.
		/// </summary>
		/// <param name="reader">The reader for the current stream.</param>
		/// <param name="version">The current version being read.</param>
		/// <param name="installDir">The installation directory where the archive is located.</param>
		/// <returns>The loaded cached KIFINT archive.</returns>
		internal static Kifint Read(BinaryReader reader, int version, string installDir, KifintType type) {
			Kifint kifint = new Kifint {
				FilePath = Path.Combine(installDir, reader.ReadString()),
				ArchiveType = type,
			};
			kifint.IsEncrypted = reader.ReadBoolean();
			kifint.FileKey = reader.ReadUInt32();
			if (!kifint.IsEncrypted)
				kifint.FileKey = 0;

			int count = reader.ReadInt32();
			Dictionary<string, KifintEntry> entries = new Dictionary<string, KifintEntry>(count);
			for (int i = 0; i < count; i++) {
				KifintEntry entry = KifintEntry.Read(reader, version, kifint);
				entries.Add(entry.FileName, entry);
			}
			kifint.Entries = new ReadOnlyDictionary<string, KifintEntry>(entries);
			return kifint;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the KIFINT archive.
		/// </summary>
		/// <returns>The string representation of the KIFINT archive.</returns>
		public override string ToString() => $"Kifint: \"{FileName}\" Type={ArchiveType} Count={Count}";

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the cached KIFINT archive's entries.
		/// </summary>
		/// <returns>The KIFINT archive's entry enumerator.</returns>
		public IEnumerator<KifintEntry> GetEnumerator() => Entries.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		#endregion
	}
}
