using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.Windows.Resources.Dialog;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2DebugDialogPatch : DialogResourcePatch {
		public CS2DebugDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;
			
			var control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1179);
			});
			control.Width = 32;
			control.Y++;
			control.Height -= 2;
			control = controls.Find(c => {
				return (c.X == 246 && c.Y == 53 && c.Width == 17);
			});
			control.Width = 30;
			

			return true;
		}

	}
}
