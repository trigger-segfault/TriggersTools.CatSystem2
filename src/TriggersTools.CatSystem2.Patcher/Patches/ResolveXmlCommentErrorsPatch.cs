using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class ResolveXmlCommentErrorsPatch {

		private static readonly Regex CommentRegex = new Regex(@"(?'open'<!--)|(?'close'-->)");

		public string XmlFile { get; }

		public ResolveXmlCommentErrorsPatch(string xmlFile) {
			XmlFile = xmlFile;
		}

		public bool Patch() {
			try {
				XmlDocument doc = new XmlDocument();
				doc.Load(XmlFile);
				return true;
			} catch (XmlException) { }

			string[] lines = File.ReadAllLines(XmlFile, Constants.ShiftJIS);
			bool commentOpened = false;
			for (int i = 0; i < lines.Length; i++) {
				string line = lines[i];
				MatchCollection matches = CommentRegex.Matches(line);
				int charOffset = 0;
				for (int j = 0; j < matches.Count; j++) {
					Match match = matches[j];
					if (match.Groups["open"].Success == commentOpened) {
						// This is an error, remove it.
						line = line.Remove(match.Index - charOffset, match.Length);
						charOffset += match.Length;
					}
					else {
						commentOpened = !commentOpened;
					}
				}
				lines[i] = line;
			}
			File.WriteAllLines(XmlFile, lines, Constants.ShiftJIS);
			
			return true;
		}
	}
}
