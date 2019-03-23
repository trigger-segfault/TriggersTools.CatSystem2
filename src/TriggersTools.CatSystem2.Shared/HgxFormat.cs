using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  The HG-X format of an <see cref="HgxImage"/>. Either HG-2 or HG3.
	/// </summary>
	public enum HgxFormat : byte {
		/// <summary>No HG-X format. This is invalid.</summary>
		[Description("HG-X")]
		None = 0,
		/// <summary>This is an HG-2 image.</summary>
		[Description("HG-2")]
		[JsonProperty("hg2")]
		Hg2 = 1,
		/// <summary>This is an HG-3 image.</summary>
		[Description("HG-3")]
		[JsonProperty("hg3")]
		Hg3 = 2,
	}
}
