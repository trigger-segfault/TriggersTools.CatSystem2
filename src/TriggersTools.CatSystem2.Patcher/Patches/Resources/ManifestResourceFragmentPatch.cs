using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class ManifestResourceFragmentPatch : ManifestResourcePatch {

		public string XmlFragment { get; }

		public ManifestResourceFragmentPatch(string xmlFragment) {
			XmlFragment = xmlFragment ?? throw new ArgumentNullException(nameof(xmlFragment));
		}
		public ManifestResourceFragmentPatch(string xmlFragment, string name) : base(name) {
			XmlFragment = xmlFragment ?? throw new ArgumentNullException(nameof(xmlFragment));
		}
		public ManifestResourceFragmentPatch(string xmlFragment, ushort name) : base(name) {
			XmlFragment = xmlFragment ?? throw new ArgumentNullException(nameof(xmlFragment));
		}

		protected override bool Patch(XmlDocument doc, XmlNamespaceManager ns, XmlElement assembly) {
			XmlDocumentFragment frag = doc.CreateDocumentFragment();
			//frag.InnerXml = XmlFragment;

			assembly.InnerXml += XmlFragment;
			/*foreach (XmlNode node in frag.ChildNodes)
				assembly.AppendChild(node);*/

			return true;
		}

	}
}
