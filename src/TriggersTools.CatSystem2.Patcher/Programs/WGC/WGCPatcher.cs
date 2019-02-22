using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;
using TriggersTools.CatSystem2.Patcher.Programs.WGC;

namespace TriggersTools.CatSystem2.Patcher {
	public class WGCPatcher : ProgramPatcher {

		//internal const ushort StringTable7Name = 7;
		//internal static readonly string[] StringTable7Strings = ReadStrings($"string_{StringTable7Name}.txt");

		internal const ushort MenuName = 109;
		//internal const ushort ContextMenuName = 137;
		//internal static readonly string[] MenuStrings = ReadStrings($"menu_{MenuName}.txt");
		//internal static readonly string[] ContextMenuStrings = ReadStrings($"menu_{ContextMenuName}.txt");

		//internal const ushort ConvertingName = 132;
		//internal const ushort OutputDestinationName = 144;
		internal const ushort SettingsDialogName = 145;
		//internal static readonly string[] ConvertingStrings = ReadStrings($"dialog_{ConvertingName}.txt");
		//internal static readonly string[] OutputDestinationStrings = ReadStrings($"dialog_{OutputDestinationName}.txt");
		//internal static readonly string[] SettingsStrings = ReadStrings($"dialog_{SettingsDialogName}.txt");

		internal static string[] ReadStrings(string fileName) {
			string path = Embedded.Combine(Constants.ResourcesPath, "WGC", fileName);
			return Embedded.ReadAllLines(path);
		}

		public WGCPatcher() {
			Signature = Constants.Signature;
			AddEnableVisualStyles(Constants.TypeFace, Constants.PointSize);
			//AddNewLanguage(Constants.Language, Constants.CodePage);

			Add(new WGCMenuPatch(MenuName));
			Add(new WGCSettingsDialogPatch(SettingsDialogName));

			AddResourceStringsPatches("WGC");
			/*Add(new MenuStringsPatch(MenuStrings, MenuName));
			Add(new MenuStringsPatch(ContextMenuStrings, ContextMenuName));
			Add(new DialogStringsPatch(SettingsStrings, SettingsName));
			Add(new DialogStringsPatch(OutputDestinationStrings, OutputDestinationName));
			Add(new DialogStringsPatch(ConvertingStrings, ConvertingName));
			Add(new StringTableStringsPatch(StringTable7Strings, StringTable7Name));*/
		}
	}
}
