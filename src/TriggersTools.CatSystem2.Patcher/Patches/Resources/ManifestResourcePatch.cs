using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TriggersTools.Windows.Resources.Manifest;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public abstract class ManifestResourcePatch : ResourcePatch<ManifestResource> {

		public ManifestResourcePatch() { }
		public ManifestResourcePatch(string name) : base(name) { }
		public ManifestResourcePatch(ushort name) : base(name) { }

		protected abstract bool Patch(XmlDocument doc, XmlNamespaceManager ns, XmlElement assembly);
		
		public override sealed bool Patch(ManifestResource manifestRes) {
			XmlDocument doc = manifestRes.Docment;
			XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
			ns.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			//doc.LoadXml(manifestRes.Xml);

			XmlElement assembly = (XmlElement) doc.SelectSingleNode("//asm:assembly", ns);

			if (!Patch(doc, ns, assembly))
				return false;

			manifestRes.Docment = doc;
			/*using (var stringWriter = new StringWriter())
			using (var xmlTextWriter = XmlWriter.Create(stringWriter)) {
				doc.WriteTo(xmlTextWriter);
				xmlTextWriter.Flush();
				manifestRes.ManifestText = stringWriter.GetStringBuilder().ToString();
			}*/
			return true;
		}

	}
}
