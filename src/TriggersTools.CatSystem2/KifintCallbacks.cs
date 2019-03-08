
namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  The callback args used with <see cref="KifintProgressCallback"/>.
	/// </summary>
	public struct KifintProgressArgs {
		/// <summary>
		///  Gets the type of KIFINT archive currently being decrypted.
		/// </summary>
		public KifintType ArchiveType { get; internal set; }
		/// <summary>
		///  Gets the file name of the current KIFINT archive being decrypted.
		/// </summary>
		public string ArchiveName { get; internal set; }
		/// <summary>
		///  Gets the index of the KIFINT archive currently being decrypted.
		/// </summary>
		public int ArchiveIndex { get; internal set; }
		/// <summary>
		///  Gets the total number of KIFINT archives to decrypt.
		/// </summary>
		public int ArchiveCount { get; internal set; }

		/// <summary>
		///  Gets the name of the entry being decrpted in the current KIFINT archive.
		/// </summary>
		public string EntryName { get; internal set; }
		/// <summary>
		///  Gets the index of the entry being decrypted in the current KIFINT archive.
		/// </summary>
		public int EntryIndex { get; internal set; }
		/// <summary>
		///  Gets the total number of KIFINT entries in the current KIFINT archive.
		/// </summary>
		public int EntryCount { get; internal set; }

		/// <summary>
		///  Gets the progress made on the current set of KIFINT archive files.
		/// </summary>
		public double Progress {
			get {
				if (ArchiveIndex == ArchiveCount)
					return 1d;
				if (EntryIndex == EntryCount)
					return (double) (ArchiveIndex + 1) / ArchiveCount;
				return (ArchiveIndex + (double) EntryIndex / EntryCount) / ArchiveCount;
			}
		}
		/// <summary>
		///  Gets if the progress is completely finished.
		/// </summary>
		public bool IsDone => ArchiveIndex == ArchiveCount;
	}
	/// <summary>
	///  A callback for progress made during <see cref="KifintArchive.DecryptLookup"/>.
	/// </summary>
	/// <param name="e">The progress callback args.</param>
	public delegate void KifintProgressCallback(KifintProgressArgs e);
}
