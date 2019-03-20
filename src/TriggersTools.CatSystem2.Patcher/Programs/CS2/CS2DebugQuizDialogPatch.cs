using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;
using TriggersTools.Windows.Resources.Dialog;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2DebugQuizDialogPatch : DialogResourcePatch {
		public CS2DebugQuizDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;
			
			var control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1186);
			});
			control.Width = 140;
			control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1189);
			});
			control.Width = 100;
			control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1187);
			});
			control.Width = 60;
			control = controls.Find(c => {
				return (c.X == 19 && c.Width == 24);
			});
			control.Width = 39;
			

			return true;
		}

	}
}
