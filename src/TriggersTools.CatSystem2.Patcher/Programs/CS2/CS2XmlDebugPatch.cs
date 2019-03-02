using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TriggersTools.SharpUtils.Collections;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	public class CS2XmlDebugPatch {
		public string XmlFile { get; }
		public string DebugKeysXmlFragment { get; }
		public bool DebugSaveFolder { get; }

		public CS2XmlDebugPatch(string xmlFile, bool debugSaveFolder, string resourcePath) {
			string debugKeysXmlPath = Embedded.Combine(resourcePath, "debug_keys.xml");
			DebugKeysXmlFragment = Embedded.ReadAllText(debugKeysXmlPath);
			XmlFile = xmlFile;
			DebugSaveFolder = debugSaveFolder;
		}

		public bool Patch() {
			XmlDocument doc = new XmlDocument {
				PreserveWhitespace = true,
			};
			doc.Load(XmlFile);
			
			if (doc.SelectSingleNode("//document/APP") is XmlElement app) {
				// Comment out v_code to enable debug mode
				if (app.SelectSingleNode("//v_code") is XmlElement v_code) {
					XmlComment v_codeComment = doc.CreateComment(v_code.OuterXml);
					//int index = app.ChildNodes.Cast<XmlNode>().IndexOf(v_code);
					app.ReplaceChild(v_codeComment, v_code);
				}
				else {

				}

				// Enable Debug window menu
				if (!(app.SelectSingleNode("//wndmenu") is XmlElement wndmenu)) {
					wndmenu = doc.CreateElement("wndmenu");
					app.AppendChild(wndmenu);
				}
				wndmenu.InnerText = "1";
			}
			else {
				return false;
			}

			// Move the save folder to the Debug subdirectory
			if (DebugSaveFolder && doc.SelectSingleNode("//document/FOLDER") is XmlElement folder) {
				if (folder.SelectSingleNode("//save") is XmlElement save) {
					if (!save.InnerText.ToLower().EndsWith("\\debug") &&
						!save.InnerText.ToLower().EndsWith("/debug")) {
						save.InnerText = Path.Combine(save.InnerText, "Debug");
					}
				}
				else {

				}
			}
			else {
				Console.Write("");
			}

			// Add debug keys back to game, (translated)
			XmlDocumentFragment debugKeysFrag = doc.CreateDocumentFragment();
			debugKeysFrag.InnerXml = DebugKeysXmlFragment;
			if (doc.SelectSingleNode("//document/KEYCUSTOMIZE") is XmlElement keyCustomize) {
				for (int i = 0; i < debugKeysFrag.ChildNodes.Count; i++) {
					XmlNode node = debugKeysFrag.ChildNodes[i];
					if (node is XmlElement && keyCustomize.SelectSingleNode($"//{node.Name}") is XmlElement existing) {
						XmlAttribute type = node.Attributes["type"];
						if (type.InnerText == "DEBUG") {
							keyCustomize.ReplaceChild(existing, node);
							i--;
						}
					}
					else {
						keyCustomize.AppendChild(node);
						i--;
					}
				}
			}
			else {

			}

			doc.Save(XmlFile);
			return true;
		}
	}
}
