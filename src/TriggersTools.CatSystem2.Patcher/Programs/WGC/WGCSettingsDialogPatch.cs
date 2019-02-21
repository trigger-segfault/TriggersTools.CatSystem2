using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher.Programs.WGC {
	internal sealed class WGCSettingsDialogPatch : DialogResourcePatch {
		public WGCSettingsDialogPatch(ushort name) : base(name) {
			AllowNormal = false;
		}

		public override bool Patch(DialogExTemplate dialogEx) {
			var controls = dialogEx.Controls;

			// Rects
			controls[6].cx = 90;
			controls[7].x += 10;
			controls[7].cx = 90;

			controls[19].x += 10; // slider
			controls[20].cx += 10; // JPEG compression
			controls[21].x += 15; // num
			controls[21].cx = 15; // num
			controls[22].cx += 10; // Scaling factor

			return true;
		}

	}
}
