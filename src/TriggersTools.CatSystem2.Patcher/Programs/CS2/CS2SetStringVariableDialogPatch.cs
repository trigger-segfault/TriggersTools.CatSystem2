using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;
using TriggersTools.Resources.Dialog;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2SetStringVariableDialogPatch : DialogResourcePatch {
		public CS2SetStringVariableDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;
			
			var control = controls.Find(c => {
				return (c.Y == 34 && c.Width == 9);
			});
			control.Width = 26;

			return true;
		}

	}
}
