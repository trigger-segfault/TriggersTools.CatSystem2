using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class MCPatcher : ProgramPatcher {
		
		internal static readonly string[] BinaryStrings = ReadStrings("binary.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "MC", fileName);
			return Embedded.ReadAllLines(path);
		}

		public MCPatcher() {
			//Add(new BinaryStringsPatch(BinaryStrings, 0x1AEE8, 0x1B1F4));
			Add(new BinaryStringsPatch(BinaryStrings));
		}

	}
}
