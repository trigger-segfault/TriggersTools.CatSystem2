using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	/// <summary>
	///  A manifest patch to introduce enabled visual styles for windows common controls.
	/// </summary>
	public sealed class EnableVisualStylesPatch : ManifestResourceFragmentPatch {
		private static readonly string EnableVisualStylesFragment =
			Embedded.ReadAllText(Embedded.Combine(Constants.ResourcesPath, "EnableVisualStylesFragment.xml"));

		public EnableVisualStylesPatch() : base(EnableVisualStylesFragment) { }
	}
}
