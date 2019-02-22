using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.CatSystem2.Patcher.Programs.CS2;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public class CS2Patcher : ProgramPatcher {

		internal static readonly string[] BinaryStrings = ReadStrings("binary.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "CS2", fileName);
			return Embedded.ReadAllLines(path);
		}

		private const ushort ChangeWallpaperName = 212;
		private const ushort DebugDialogName = 207;
		private const ushort DebugInfoName = 112;
		private const ushort DebugQuizName = 215;
		private const ushort SetStringVariableName = 210;

		public CS2Patcher() {
			Signature = Constants.Signature;
			AddEnableVisualStyles(Constants.TypeFace, Constants.PointSize);
			//Add(new BinaryStringsPatch(BinaryStrings, 0x560000, 0x5D0000));
			Add(new BinaryStringsPatch(BinaryStrings, 0, 0));

			Add(new CS2ChangeWallpaperDialogPatch(ChangeWallpaperName));
			Add(new CS2DebugDialogPatch(DebugDialogName));
			Add(new CS2DebugInfoDialogPatch(DebugInfoName));
			Add(new CS2DebugQuizDialogPatch(DebugQuizName));
			Add(new CS2SetStringVariableDialogPatch(SetStringVariableName));

			AddResourceStringsPatches("CS2");
		}

	}
}
