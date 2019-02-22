using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.CS2 {
	internal sealed class CS2SetStringVariableDialogPatch : DialogResourcePatch {
		public CS2SetStringVariableDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;
			
			var control = controls.Find(c => {
				return (c.y == 34 && c.cx == 9);
			});
			control.cx = 26;

			return true;
		}

	}
}
