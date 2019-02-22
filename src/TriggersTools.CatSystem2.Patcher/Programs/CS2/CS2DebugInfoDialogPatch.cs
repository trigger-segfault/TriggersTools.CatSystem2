using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2DebugInfoDialogPatch : DialogResourcePatch {
		public CS2DebugInfoDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;

			const int increment = 7;

			dialogEx.cx += increment;

			for (int i = 0; i < controls.Count; i++) {
				var control = controls[i]; // Label
				if (control.x != 7)
					continue;
				control.cx += increment;
				control = controls[++i]; // ":"
				control.x += increment;
				control = controls[++i]; // Value
				control.x += increment;
				control = controls[++i]; // Unit
				control.x += increment;
			}

			return true;
		}

	}
}
