using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	public partial class ZtPackage {
		#region Fields

		[JsonProperty("entries")]
		public IReadOnlyList<ZtEntry> Entries { get; }

		#endregion

		#region Properties

		[JsonIgnore]
		public int Count => Entries.Count;

		#endregion

		#region Constructors

		public ZtPackage() { }
		internal ZtPackage(ZTENTRY[] fileEntries) {
			var entries = fileEntries.Select(f => new ZtEntry(f));
			Entries = Array.AsReadOnly(entries.ToArray());
		}

		#endregion
	}
}
