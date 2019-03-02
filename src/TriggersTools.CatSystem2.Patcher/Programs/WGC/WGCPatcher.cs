using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;
using TriggersTools.CatSystem2.Patcher.Programs.WGC;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class WGCPatcher : ProgramPatcher {
		#region Constants
		
		private const ushort MenuName = 109;
		private const ushort SettingsDialogName = 145;

		#endregion

		public WGCPatcher() : base("WGC", "WGC.exe") {
			Signature = Constants.Signature;
			AddEnableVisualStyles(Constants.TypeFace, Constants.PointSize);

			Add(new WGCMenuPatch(MenuName));
			Add(new WGCSettingsDialogPatch(SettingsDialogName));

			AddResourceStringsPatches();
			AddBinaryStringsPatch();
		}
	}
}
