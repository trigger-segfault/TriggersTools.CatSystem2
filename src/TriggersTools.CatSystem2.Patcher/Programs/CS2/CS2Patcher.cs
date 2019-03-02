using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.CatSystem2.Patcher.Programs.CS2;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Patcher {
	public sealed class CS2Patcher : ProgramPatcher {
		#region Constants
		
		private const ushort ChangeWallpaperName = 212;
		private const ushort DebugDialogName = 207;
		private const ushort DebugInfoName = 112;
		private const ushort DebugQuizName = 215;
		private const ushort SetStringVariableName = 210;

		#endregion
		

		public CS2Patcher() : base("CS2") {
			PatchTitle = "CatSystem2";
			Signature = Constants.Signature;
			AddEnableVisualStyles(Constants.TypeFace, Constants.PointSize);

			Add(new CS2ChangeWallpaperDialogPatch(ChangeWallpaperName));
			Add(new CS2DebugDialogPatch(DebugDialogName));
			Add(new CS2DebugInfoDialogPatch(DebugInfoName));
			Add(new CS2DebugQuizDialogPatch(DebugQuizName));
			Add(new CS2SetStringVariableDialogPatch(SetStringVariableName));

			AddResourceStringsPatches();
			AddBinaryStringsPatch();
			//AddBinaryStringsPatch(ResourceDir, 0x560000, 0x5D0000);
		}

		#region Virtual Methods

		/// <summary>
		///  Attempts to locate the executable when no <see cref="ExecutableFile"/> is specified.
		/// </summary>
		/// <returns>The located executable file if found, otherwise null.</returns>
		/// 
		/// <exception cref="NotSupportedException">
		///  This method is not overwritten, or <see cref="TargetFile"/> is not specified.
		/// </exception>
		protected override string LocateExecutable() {
			return CatUtils.FindCatExecutable(InstallDir);
		}

		protected override bool AdditionalPatch() {
			return true;
		}

		#endregion
	}
}
