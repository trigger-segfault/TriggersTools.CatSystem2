using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	public class ZtEntry {
		#region Fields

		[JsonProperty("file_name")]
		public string FileName { get; }
		[JsonProperty("length")]
		public int Length { get; }

		#endregion

		#region Constructors

		public ZtEntry() { }
		internal ZtEntry(ZTENTRY entry) {
			FileName = entry.FileName;
			Length = entry.DecompressedLength;
		}

		#endregion

		#region ToString Override

		public override string ToString() => $"\"{FileName}\" Length={Length}";

		#endregion
	}
}
