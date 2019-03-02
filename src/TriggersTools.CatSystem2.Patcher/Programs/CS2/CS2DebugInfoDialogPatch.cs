using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;
using TriggersTools.Resources.Dialog;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2DebugInfoDialogPatch : DialogResourcePatch {
		public CS2DebugInfoDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;

			const int increment = 7;

			dialogEx.Width += increment;

			for (int i = 0; i < controls.Count; i++) {
				var control = controls[i]; // Label
				if (control.X != 7)
					continue;
				control.Width += increment;
				control = controls[++i]; // ":"
				control.X += increment;
				control = controls[++i]; // Value
				control.X += increment;
				control = controls[++i]; // Unit
				control.X += increment;
			}

			return true;
		}

	}
}
