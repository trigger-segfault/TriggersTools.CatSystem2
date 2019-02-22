using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2ChangeWallpaperDialogPatch : DialogResourcePatch {
		public CS2ChangeWallpaperDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;

			dialogEx.cx += 23;

			for (int i = 0; i < controls.Count; i++) {
				var control = controls[i];
				if (control is DialogExTemplateControl controlEx && controlEx.Id == 1)
					continue;
				control.cx = 175;
			}

			/*var control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1170);
			});
			control.cx = 120;
			control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1170);
			});
			control.cx = 120;*/

			return true;
		}

	}
}
