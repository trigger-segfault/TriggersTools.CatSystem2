using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class PatchSignaturePatch : ManifestResourcePatch {

		public string Signature { get; protected set; }

		public PatchSignaturePatch(string signature) {
			Signature = signature;
		}

		protected override bool Patch(XmlDocument doc, XmlNamespaceManager ns, XmlElement assembly) {
			foreach (XmlNode node in assembly) {
				if (node is XmlComment comment && comment.InnerText.Trim() == Signature)
					return false; // We're already patched, not allowed
			}
			assembly.InnerXml += $"{Environment.NewLine}  <!-- {Signature} -->";
			//assembly.AppendChild(doc.CreateComment($" {Signature} "));
			return true;
		}

	}
}
