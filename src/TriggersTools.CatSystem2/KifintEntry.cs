using System;
using System.IO;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Extensions methods for extracting from <see cref="KifintEntry"/>s.
	/// </summary>
	public static partial class KifintEntryExtensions {

	}
	/// <summary>
	///  An entry for a file in a KIF INT archive.
	/// </summary>
	public sealed partial class KifintEntry {
		#region Fields
		
		/// <summary>
		///  Gets the KIFINT file used to extract this entry from.
		/// </summary>
		public KifintArchiveInfo Kifint { get; private set; }
		/// <summary>
		///  Gets the name of the file with the extension.
		/// </summary>
		public string FileName { get; private set; }
		/// <summary>
		///  Gets the offset of the entry in the KIFINT file.
		/// </summary>
		public uint Offset { get; private set; }
		/// <summary>
		///  Gets the length of the entry into the KIFINT file.
		/// </summary>
		public int Length { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the name of the file without the extension.
		/// </summary>
		public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FileName);
		/// <summary>
		///  Gets the extension of the file.
		/// </summary>
		public string Extension => Path.GetExtension(FileName).ToLower();
		/// <summary>
		///  Gets the weak reference to the KIFINT archive for this KIFINT archive information.
		///  Returns null if the KIFINT archive has been collected.
		/// </summary>
		public KifintArchive Archive => Kifint.Archive;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned KIFINT entry for use with <see cref="Read"/>.
		/// </summary>
		private KifintEntry() { }
		/// <summary>
		///  Constructs a KIFINT entry with the specified entry data, parent KIFINT archive.
		/// </summary>
		/// <param name="kifEntry">The decrypted data for the entry.</param>
		/// <param name="kifint">The parent KIFINT arhive.</param>
		internal KifintEntry(KIFENTRY kifEntry, KifintArchive kifint) {
			Kifint = kifint;
			FileName = kifEntry.FileName;
			Offset = kifEntry.Offset;
			Length = kifEntry.Length;
		}
		/// <summary>
		///  Constructs a KIFINT entry with the specified file name, entry data, parent KIFINT archive.
		/// </summary>
		/// <param name="fileName">
		///  The cached name of the file. Calling <see cref="KifintArchive.KIFENTRY.FileName"/> is wasteful.
		/// </param>
		/// <param name="kifEntry">The decrypted data for the entry.</param>
		/// <param name="kifint">The parent KIFINT arhive.</param>
		internal KifintEntry(string fileName, KIFENTRY kifEntry, KifintArchive kifint) {
			Kifint = kifint;
			FileName = fileName;
			Offset = kifEntry.Offset;
			Length = kifEntry.Length;
		}

		#endregion

		#region I/O

		/// <summary>
		///  Writes the decrypted KIFINT entry to the stream. For use with <see cref="KifintLookup"/>.
		/// </summary>
		/// <param name="writer">The writer for the current stream.</param>
		internal void Write(BinaryWriter writer) {
			writer.Write(FileName);
			writer.Write(Offset);
			writer.Write(Length);
		}
		/// <summary>
		///  Reads the KIFINT archive from the stream. For use with <see cref="KifintLookup"/>.
		/// </summary>
		/// <param name="reader">The reader for the current stream.</param>
		/// <param name="version">The current version being read.</param>
		/// <param name="kifint">The KIFINT archive containing this entry.</param>
		/// <returns>The loaded cached KIFINT.</returns>
		internal static KifintEntry Read(BinaryReader reader, int version, KifintArchive kifint) {
			return new KifintEntry {
				Kifint = kifint,
				FileName = reader.ReadString(),
				Offset = reader.ReadUInt32(),
				Length = reader.ReadInt32(),
			};
		}

		#endregion

		#region Extract

		/// <summary>
		///  Extracts the KIFINT entry to a <see cref="byte[]"/>.
		/// </summary>
		/// <returns>A byte array containing the data of the decrypted entry.</returns>
		public byte[] ExtractToBytes() {
			using (KifintStream kifintStream = new KifintStream())
				return KifintArchive.ExtractToBytes(kifintStream, this);
		}
		/// <summary>
		///  Extracts the KIFINT entry to a <see cref="byte[]"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>A byte array containing the data of the decrypted entry.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public byte[] ExtractToBytes(KifintStream kifintStream) {
			return KifintArchive.ExtractToBytes(kifintStream, this);
		}
		/// <summary>
		///  Extracts the KIFINT entry to a fixed stream.
		/// </summary>
		/// <returns>A fixed stream containing the data of the decrypted entry.</returns>
		public Stream ExtractToStream() {
			if (CatDebug.StreamExtract) // Do not leave the stream open, it's not used anywhere else.
				return ExtractToStream(new KifintStream(), false);
			else
				return new MemoryStream(ExtractToBytes());
		}
		/// <summary>
		///  Extracts the KIFINT entry to a fixed stream of <paramref name="kifintStream"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>
		///  A fixed stream containing the data of the decrypted entry. This stream must always be disposed of, because
		///  it's not guaranteed to be a fixed stream of <paramref name="kifintStream"/>. This is the case when the
		///  length is small enough for extracting bytes to be more efficient.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public Stream ExtractToStream(KifintStream kifintStream) {
			if (CatDebug.StreamExtract) // Leave stream open by default, it may be used again.
				return ExtractToStream(kifintStream, true);
			else
				return new MemoryStream(ExtractToBytes(kifintStream));
		}
		/// <summary>
		///  Extracts the KIFINT entry to a fixed stream of <paramref name="kifintStream"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
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
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public Stream ExtractToStream(KifintStream kifintStream, bool leaveOpen) {
			if (CatDebug.StreamExtract)
				return KifintArchive.ExtractToStream(kifintStream, this, leaveOpen);
			else
				return new MemoryStream(ExtractToBytes(kifintStream));
		}
		/// <summary>
		///  Extracts the KIFINT entry and saves it to <paramref name="filePath"/>.
		/// </summary>
		/// <param name="filePath">The file path to save the decrypted entry to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="filePath"/> is null.
		/// </exception>
		public void ExtractToFile(string filePath) {
			if (CatDebug.StreamExtract) {
				using (var output = File.Create(filePath))
				using (var input = ExtractToStream())
					input.CopyTo(output);
			}
			else {
				File.WriteAllBytes(filePath, ExtractToBytes());
			}
		}
		/// <summary>
		///  Extracts the KIFINT entry and saves it to <paramref name="filePath"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="filePath">The file path to save the decrypted entry to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="filePath"/> is null.
		/// </exception>
		public void ExtractToFile(KifintStream kifintStream, string filePath) {
			if (CatDebug.StreamExtract) {
				using (var output = File.Create(filePath))
				using (var input = ExtractToStream(kifintStream, true))
					input.CopyTo(output);
			}
			else {
				File.WriteAllBytes(filePath, ExtractToBytes());
			}
			File.WriteAllBytes(filePath, ExtractToBytes(kifintStream));
		}
		/// <summary>
		///  Extracts the KIFINT entry and saves it to <paramref name="directory"/>/<see cref="FileName"/>.
		/// </summary>
		/// <param name="directory">The directory to save the decrypted entry to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public void ExtractToDirectory(string directory) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			ExtractToFile(Path.Combine(directory, FileName));
		}
		/// <summary>
		///  Extracts the KIFINT entry and saves it to <paramref name="directory"/>/<see cref="FileName"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="directory">The directory to save the decrypted entry to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> or <paramref name="directory"/> is null.
		/// </exception>
		public void ExtractToDirectory(KifintStream kifintStream, string directory) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			ExtractToFile(kifintStream, Path.Combine(directory, FileName));
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the KIFINT entry.
		/// </summary>
		/// <returns>The string representation of the KIFINT entry.</returns>
		public override string ToString() => $"\"{FileName}\" Length={Length}";

		#endregion
	}
}
