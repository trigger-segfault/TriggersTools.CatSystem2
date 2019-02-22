using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;

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
			control.cx = 140;
			control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1189);
			});
			control.cx = 100;
			control = controls.Find(c => {
				return (c is DialogExTemplateControl cEx && cEx.Id == 1187);
			});
			control.cx = 60;
			control = controls.Find(c => {
				return (c.x == 19 && c.cx == 24);
			});
			control.cx = 39;
			

			return true;
		}

	}
}
