using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A ZT package which contains extractable files.
	/// </summary>
	public sealed partial class ZtPackage {
		#region Fields

		/// <summary>
		///  Gets the file name for the ZT package.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  Gets the list of file entries in the ZT package.
		/// </summary>
		[JsonProperty("entries")]
		public IReadOnlyList<ZtEntry> Entries { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of file entries in the ZT package.
		/// </summary>
		[JsonIgnore]
		public int Count => Entries.Count;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned ZT package for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		private ZtPackage() { }
		/// <summary>
		///  Constructs an ZT package with the specified file name, header, and script lines.
		/// </summary>
		/// <param name="fileName">The file name of the ZT package with the .zt extension.</param>
		/// <param name="hdrs">The ZTENTRY struct array containing entry header information.</param>
		/// <param name="entries">The ZTENTRY struct array containing entry information.</param>
		internal ZtPackage(string fileName, ZTENTRYHDR[] hdrs, ZTENTRY[] entries) {
			FileName = Path.GetFileName(fileName);
			ZtEntry[] newEntries = new ZtEntry[entries.Length];
			for (int i = 0; i < entries.Length; i++)
				newEntries[i] = new ZtEntry(hdrs[i], entries[i]);
			Entries = newEntries.ToImmutableArray();
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ZT package.
		/// </summary>
		/// <returns>The string representation of the ZT package.</returns>
		public override string ToString() => $"ZtPackage \"{FileName}\" Entries={Count}";

		#endregion
	}
}
