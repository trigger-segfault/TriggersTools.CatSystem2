using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class ACPatcher : ProgramPatcher {
		
		internal static readonly string[] BinaryStrings = ReadStrings("binary.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "AC", fileName);
			return Embedded.ReadAllLines(path);
		}

		public ACPatcher() {
			//Add(new BinaryStringsPatch(BinaryStrings, 0x504EC, 0x50D3C));
			Add(new BinaryStringsPatch(BinaryStrings));
		}

	}
}
