using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class LanguagePatch : ResourcePatch<Resource> {

		public ushort Language { get; }
		public ushort CodePage { get; }

		public LanguagePatch(ushort language, ushort codePage) {
			Language = language;
			CodePage = codePage;
		}

		public override bool Patch(Resource resource) {
			if (resource is VersionResource version) {
				if (version.Resources.TryGetValue("VarFileInfo", out var header)) {
					VarFileInfo varFileInfo = (VarFileInfo) header;
					if (varFileInfo.Vars.TryGetValue("Translation", out VarTable varTable)) {
						varTable.Languages.Clear();
						varTable.Languages.Add(Language, CodePage);
					}
				}
			}
			resource.Language = Language;
			return true;
		}
	}
}
