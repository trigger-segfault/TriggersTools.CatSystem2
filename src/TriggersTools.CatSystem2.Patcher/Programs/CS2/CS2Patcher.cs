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

		private static readonly byte[] Validation1 = new byte[] { 0x75, 0x75, 0x8D, 0x4C };
		private static readonly byte[] Validation2 = new byte[] { 0x74, 0x2D, 0x6A, 0x00, 0x68 };
		private const byte ValidationReplace = 0xEB;
		private const string Cs2DebugKey = "cs2_debug_key.dat";
		private const string Languages = "language.txt";
		private const string Startup = @"config\startup.xml";

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

		#region Properties

		private string StartupPath => Path.Combine(InstallDir, Startup);
		//private string StartupBak => $"{StartupPath}.bak";

		private string Cs2DebugKeyRes => Embedded.Combine(ResourcePath, Cs2DebugKey);
		private string Cs2DebugKeyPath => Path.Combine(InstallDir, Cs2DebugKey);

		private string LanguagesRes => Embedded.Combine(ResourcePath, Languages);
		private string LanguagesPath => Path.Combine(InstallDir, Languages);
		//private string LanguagesBak => $"{LanguagesPath}.bak";

		#endregion

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
		
		protected override bool PostPatch() {
			if (!StartupXmlPatch())
				return false;
			if (!LanguagesPatch())
				return false;
			if (!DebugPatch())
				return false;
			return true;
		}
		protected override void Restore() {
			RestoreFile(StartupPath);
			RestoreFile(LanguagesPath);
		}

		#endregion

		#region StartupXmlPatch

		private bool StartupXmlPatch() {
			if (!BackupFile(StartupPath, true))
				return false;

			var patch1 = new ResolveXmlCommentErrorsPatch(StartupPath);
			var patch2 = new CS2XmlDebugPatch(StartupPath, true, ResourcePath);
			if (!patch1.Patch())
				return false;
			if (!patch2.Patch())
				return false;
			return true;
		}

		#endregion

		#region LanguagesPatch

		private bool LanguagesPatch() {
			if (!BackupFile(LanguagesPath, false))
				return false;
			if (!FileRetry(() => Embedded.SaveToFile(LanguagesRes, LanguagesPath)))
				return false;
			return true;
		}

		#endregion

		#region DebugPatch

		private bool DebugPatch() {
			// Patch in validation
			byte[] binary = File.ReadAllBytes(Executable);
			if (!PatchValidation(binary, Validation1))
				return false;
			if (!PatchValidation(binary, Validation2))
				return false;
			if (!FileRetry(() => File.WriteAllBytes(Executable, binary)))
				return false;

			// Save debug key
			if (!FileRetry(() => Embedded.SaveToFile(Cs2DebugKeyRes, Cs2DebugKeyPath)))
				return false;

			return true;
		}
		private bool PatchValidation(byte[] binary, byte[] validation) {
			for (int i = 0; i < binary.Length; i++) {
				if (SequenceEqualsAt(binary, i, validation)) {
					binary[i] = ValidationReplace;
					return true;
				}
			}
			return false;
		}
		private static bool SequenceEqualsAt(byte[] binary, int start, byte[] sequence) {
			if (sequence.Length + start > binary.Length)
				return false;
			for (int i = 0; i < sequence.Length; i++) {
				if (sequence[i] != binary[start + i])
					return false;
			}
			return true;
		}

		#endregion
	}
}
