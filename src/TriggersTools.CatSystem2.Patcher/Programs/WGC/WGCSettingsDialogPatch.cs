using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.Windows.Resources.Dialog;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.WGC {
	internal sealed class WGCSettingsDialogPatch : DialogResourcePatch {
		public WGCSettingsDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;

			// Rects
			controls[6].Width = 90;
			controls[7].X += 10;
			controls[7].Width = 90;

			controls[19].X += 10; // slider
			controls[20].Width += 10; // JPEG compression
			controls[21].X += 15; // num
			controls[21].Width = 15; // num
			controls[22].Width += 10; // Scaling factor

			return true;
		}

	}
}
