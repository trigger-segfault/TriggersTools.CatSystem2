using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class FESPatcher : ProgramPatcher {
		public FESPatcher() : base("FES", "fes.exe") {
			//AddBinaryStringsPatch(0x3D828, 0x3DC68);
			AddBinaryStringsPatch();
		}

	}
}
