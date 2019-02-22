using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
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
			control.cx = 32;
			control.y++;
			control.cy -= 2;
			control = controls.Find(c => {
				return (c.x == 246 && c.y == 53 && c.cx == 17);
			});
			control.cx = 30;
			

			return true;
		}

	}
}
