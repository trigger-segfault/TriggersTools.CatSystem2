using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public class CS2Patcher : ProgramPatcher {

		internal static readonly string[] BinaryStrings = ReadStrings("binary.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "CS2", fileName);
			return Embedded.ReadAllLines(path);
		}

		public CS2Patcher() {
			Signature = Constants.Signature;
			AddEnableVisualStyles(Constants.TypeFace, Constants.PointSize);
			//Add(new BinaryStringsPatch(BinaryStrings, 0x1AEE8, 0x1B1F4));
			Add(new BinaryStringsPatch(BinaryStrings));
		}

	}
}
