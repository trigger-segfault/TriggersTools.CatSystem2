using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Windows.Resources;
using TriggersTools.Windows.Resources.Dialog;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class DialogStringsPatch : DialogResourcePatch {
		
		public IReadOnlyDictionary<string, string> Translation { get; }
		
		public DialogStringsPatch(IReadOnlyDictionary<string, string> translations, ResourceId name) : base(name) {
			Translation = translations;
		}

		private bool PatchInternal(IDialogBaseTemplate dialog) {
			if (StringsScraper.IsNormalString(dialog.Caption)) {
				string id = StringsScraper.GetResId(dialog);
				if (Translation.TryGetValue(id, out string translation))
					dialog.Caption = translation;
			}
			foreach (var control in dialog.Controls) {
				if (StringsScraper.IsNormalString(control.CaptionId.Name)) {
					string id = StringsScraper.GetResId(control);
					if (Translation.TryGetValue(id, out string translation))
						control.CaptionId = translation;
				}
			}
			return true;
		}
		public override bool Patch(DialogTemplate dialog) {
			return PatchInternal(dialog);
			/*if (StringsScraper.IsNormalString(dialog.Caption)) {
				string id = StringsScraper.GetResId(dialog);
				if (Translation.TryGetValue(id, out string translation))
					dialog.Caption = translation;
			}
			foreach (var control in dialog.Controls) {
				if (StringsScraper.IsNormalString(control.CaptionId.Name)) {
					string id = StringsScraper.GetResId(control);
					if (Translation.TryGetValue(id, out string translation))
						control.CaptionId = translation;
				}
			}
			return true;*/
		}
		public override bool Patch(DialogExTemplate dialogEx) {
			return PatchInternal(dialogEx);
			/*if (StringsScraper.IsNormalString(dialogEx.Caption)) {
				string id = StringsScraper.GetResId(dialogEx);
				if (Translation.TryGetValue(id, out string translation))
					dialogEx.Caption = translation;
			}
			foreach (var control in dialogEx.Controls) {
				if (StringsScraper.IsNormalString(control.CaptionId.Name)) {
					string id = StringsScraper.GetResId(control);
					if (Translation.TryGetValue(id, out string translation))
						control.CaptionId = translation;
				}
			}
			return true;*/
		}
	}
}
