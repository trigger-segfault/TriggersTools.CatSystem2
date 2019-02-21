using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class DialogStringsPatch : DialogResourcePatch {
		
		public IReadOnlyDictionary<string, string> Translation { get; }

		public DialogStringsPatch(string[] lines, string name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}
		public DialogStringsPatch(string[] lines, ushort name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}

		public override bool Patch(DialogTemplate dialog) {
			if (StringsScraper.IsNormalString(dialog.Caption)) {
				string id = StringsScraper.GetResId(dialog);
				if (Translation.TryGetValue(id, out string translation))
					dialog.Caption = translation;
			}
			foreach (var control in dialog.Controls) {
				if (StringsScraper.IsNormalString(control.CaptionId?.Name)) {
					string id = StringsScraper.GetResId(control);
					if (Translation.TryGetValue(id, out string translation))
						control.CaptionId.Name = translation;
				}
			}
			return true;
		}
		public override bool Patch(DialogExTemplate dialogEx) {
			if (StringsScraper.IsNormalString(dialogEx.Caption)) {
				string id = StringsScraper.GetResId(dialogEx);
				if (Translation.TryGetValue(id, out string translation))
					dialogEx.Caption = translation;
			}
			foreach (var control in dialogEx.Controls) {
				if (StringsScraper.IsNormalString(control.CaptionId?.Name)) {
					string id = StringsScraper.GetResId(control);
					if (Translation.TryGetValue(id, out string translation))
						control.CaptionId.Name = translation;
				}
			}
			return true;
		}
	}
}
