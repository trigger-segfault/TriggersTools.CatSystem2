using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class ZTPatcher : ProgramPatcher {
		public ZTPatcher() : base("ZT", "ztpack.exe") {
			AddBinaryStringsPatch();
		}
	}
}
