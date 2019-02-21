using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class FESPatcher : ProgramPatcher {
		
		internal static readonly string[] BinaryStrings = ReadStrings("binary.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "FES", fileName);
			return Embedded.ReadAllLines(path);
		}

		public FESPatcher() {
			//Add(new BinaryStringsPatch(BinaryStrings, 0x3D828, 0x3DC68));
			Add(new BinaryStringsPatch(BinaryStrings));
		}

	}
}
