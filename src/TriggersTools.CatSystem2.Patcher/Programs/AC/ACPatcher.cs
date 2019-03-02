using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class ACPatcher : ProgramPatcher {
		public ACPatcher() : base("AC", "ac.exe") {
			//AddBinaryStringsPatch(0x504EC, 0x50D3C);
			AddBinaryStringsPatch();
		}
	}
}
