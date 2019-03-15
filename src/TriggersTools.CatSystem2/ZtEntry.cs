using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A ZT package file entry.
	/// </summary>
	public sealed class ZtEntry {
		#region Fields

		/// <summary>
		///  Gets the file name of this ZT package entry.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  Gets the file size of this ZT package entry.
		/// </summary>
		[JsonProperty("length")]
		public int Length { get; private set; }
		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		[JsonProperty("unknown1")]
		public int Unknown1 { get; private set; }
		/// <summary>
		///  Unknown 4-byte value 2.
		/// </summary>
		[JsonProperty("unknown2")]
		public int Unknown2 { get; private set; }
		/// <summary>
		///  Unknown 4-byte value 3. Likely reserved.
		/// </summary>
		[JsonProperty("reserved")]
		public int Reserved { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned ZT package file entry for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public ZtEntry() { }
		internal ZtEntry(ZTENTRYHDR hdr, ZTENTRY entry) {
			FileName = entry.FileName;
			Length = entry.DecompressedLength;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ZT package file entry.
		/// </summary>
		/// <returns>The string representation of the ZT package file entry.</returns>
		public override string ToString() => $"\"{FileName}\" Length={Length}";

		#endregion
	}
}
