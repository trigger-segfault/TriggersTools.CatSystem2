using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Resources;
using TriggersTools.Resources.StringTable;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class StringTableStringsPatch : ResourcePatch<StringResource> {

		public IReadOnlyDictionary<string, string> Translation { get; }
		
		public StringTableStringsPatch(IReadOnlyDictionary<string, string> translations, ResourceId name) : base(name) {
			Translation = translations;
		}

		public override bool Patch(StringResource stringTable) {
			var table = stringTable.ToArray();
			foreach (var entry in stringTable) {
				ushort stringId = entry.Id;
				string s = entry.String;
				if (StringsScraper.IsNormalString(s)) {
					string id = StringsScraper.GetResId(entry);
					if (Translation.TryGetValue(id, out string translation))
						stringTable[stringId] = translation;
				}
			}
			return true;
		}
	}
}
