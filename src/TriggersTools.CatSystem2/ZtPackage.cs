using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
		internal ZtPackage(ZTENTRY[] entries) {
			ZtEntry[] newEntries = new ZtEntry[entries.Length];
			for (int i = 0; i < entries.Length; i++)
				newEntries[i] = new ZtEntry(entries[i]);
			Entries = newEntries.ToImmutableArray();
		}

		#endregion
	}
}
