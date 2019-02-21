using System;
using System.IO;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  An entry for a file in a KIF INT archive.
	/// </summary>
	public sealed partial class KifintEntry {
		#region Fields
		
		/// <summary>
		///  Gets the KIFINT file used to extract this entry from.
		/// </summary>
		public Kifint Kifint { get; private set; }
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

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned KIFINT entry for use with <see cref="Read"/>.
		/// </summary>
		private KifintEntry() { }
		/// <summary>
		///  Constructs a KIFINT entry with the specified file name, entry data, parent KIFINT archive.
		/// </summary>
		/// <param name="fileName">
		///  The cached name of the file. Calling <see cref="Kifint.KIFENTRY.FileName"/> is wasteful.
		/// </param>
		/// <param name="kifEntry">The decrypted data for the entry.</param>
		/// <param name="kifint">The parent KIFINT arhive.</param>
		internal KifintEntry(string fileName, KIFENTRY kifEntry, Kifint kifint) {
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
		/// <param name="writer"></param>
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
		internal static KifintEntry Read(BinaryReader reader, int version, Kifint kifint) {
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
		public byte[] Extract() {
			return Kifint.Extract(this);
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
		public byte[] Extract(KifintStream kifintStream) {
			return Kifint.Extract(kifintStream, this);
		}
		/// <summary>
		///  Extracts the KIFINT entry to a <see cref="MemoryStream"/>.
		/// </summary>
		/// <returns>A memory stream containing the data of the decrypted entry.</returns>
		public MemoryStream ExtractToStream() {
			return new MemoryStream(Extract());
		}
		/// <summary>
		///  Extracts the KIFINT entry to a <see cref="MemoryStream"/>.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>A memory stream containing the data of the decrypted entry.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public MemoryStream ExtractToStream(KifintStream kifintStream) {
			return new MemoryStream(Extract(kifintStream));
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
			File.WriteAllBytes(filePath, Extract());
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
			File.WriteAllBytes(filePath, Extract(kifintStream));
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
		/*/// <summary>
		///  Extracts the HG-3 file and the images contained within to the output <paramref name="directory"/>.
		/// </summary>
		/// <param name="directory">The output directory to save the images to.</param>
		/// <param name="expand">True if the images are expanded to their full size when saving.</param>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		public Hg3 ExtractHg3AndImages(string directory, bool expand) {
			return Kifint.ExtractHg3AndImages(this, directory, expand);
		}
		/// <summary>
		///  Extracts the HG-3 image information ONLY and does not extract the actual images.
		/// </summary>
		/// <returns>The extracted <see cref="Hg3"/> information.</returns>
		public Hg3 ExtractHg3() {
			return Kifint.ExtractHg3(this);
		}
		/// <summary>
		///  Extracts the ANM animation information from the entry.
		/// </summary>
		/// <returns>The extracted <see cref="Animation"/> animation information.</returns>
		public Animation ExtractAnm() {
			return Kifint.ExtractAnimation(this);
		}*/

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the KIFINT entry.
		/// </summary>
		/// <returns>The string representation of the KIFINT entry.</returns>
		public override string ToString() => $"KifintEntry: \"{FileName}\"";

		#endregion
	}
}
