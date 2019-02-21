using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class StringTableStringsPatch : ResourcePatch<StringResource> {

		public IReadOnlyDictionary<string, string> Translation { get; }

		public StringTableStringsPatch(string[] lines, string name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}
		public StringTableStringsPatch(string[] lines, ushort name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}

		public override bool Patch(StringResource stringTable) {
			var table = stringTable.Strings.ToArray();
			foreach (var str in table) {
				ushort key = str.Key;
				string value = str.Value;
				if (StringsScraper.IsNormalString(value)) {
					string id = StringsScraper.GetResId(str);
					if (Translation.TryGetValue(id, out string translation))
						stringTable[key] = translation;
				}
			}
			return true;
		}
	}
}
