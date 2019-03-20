using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
//using ClrPlus.Windows.PeBinary.Utility;
using TriggersTools.SharpUtils.IO;
using System.Text.RegularExpressions;
using TriggersTools.Windows.Resources.Manifest;
using TriggersTools.Windows.Resources;
using System.ComponentModel;

namespace TriggersTools.CatSystem2.Patcher {
	/*/// <summary>
	///  How <see cref="ProgramPatcher"/> patches an executable.
	/// </summary>
	public enum PatchMode {
		/// <summary>
		///  The current executable will be backed up then overwritten.
		/// </summary>
		Backup,
		/// <summary>
		///  The any existing backup will be restored then the current executable will be backed up then overwritten.
		/// </summary>
		RestoreBackup,
		/// <summary>
		///  A new executable will be copied then overwritten.
		/// </summary>
		CopyNew,
	}*/
	/// <summary>
	///  The base class for patching a program.
	/// </summary>
	public abstract class ProgramPatcher {
		#region Constants
		
		private static readonly Regex NumberRegex = new Regex(@"\d+$");

		#endregion

		#region Fields

		private PatchSignaturePatch signaturePatcher;

		protected string ResourcePath { get; }
		protected string TargetFile { get; set; }
		protected string PatchTitle { get; set; }
		protected List<IResourcePatch> ResourcePatches { get; } = new List<IResourcePatch>();
		protected List<BinaryStringsPatch> BinaryPatches { get; } = new List<BinaryStringsPatch>();
		//private PatchMode patchMode;
		//private string installDir;
		/// <summary>
		///  Gets or sets the installation directory used to locate <see cref="Executable"/>.
		///  This value is required if <see cref="Executable"/> is not specified.
		/// </summary>
		public string InstallDir { get; set; }
		/// <summary>
		///   Gets or sets the executable to patch.
		///  This value is required if <see cref="InstallDir"/> is not specified.
		/// </summary>
		public string Executable { get; set; }
		/// <summary>
		///   Gets or sets if the patcher should restore the executable from the backup first.
		/// </summary>
		public bool RestoreBackupFirst { get; set; } = true;
		/// <summary>
		///  Gets or sets if the patcher produces verbose output.
		/// </summary>
		public bool Verbose { get; set; }
		/// <summary>
		///  Gets or sets the 
		/// </summary>
		public TextWriter Logger { get; set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the path to the backup executable file path.
		/// </summary>
		public string ExecutableBackup => (Executable != null ? $"{Executable}.bak" : null);

		#endregion

		#region Constructors

		public ProgramPatcher(string resourcePath, string targetFile = null) {
			ResourcePath = Embedded.Combine(Constants.ResourcesPath, resourcePath);
			TargetFile = targetFile;
		}

		#endregion

		//public PatchMarker PatchMarker { get; set; }


		//public string MD5 { get; set; }

		protected string Signature {
			get => signaturePatcher?.Signature;
			set => signaturePatcher = (value == null ? null : new PatchSignaturePatch(value));
		}


		protected void Add(IResourcePatch resourcePatch) => ResourcePatches.Add(resourcePatch);
		protected void Add(BinaryStringsPatch binaryPatch) => BinaryPatches.Add(binaryPatch);

		protected void AddEnableVisualStyles() {
			Add(new EnableVisualStylesPatch());
		}
		protected void AddEnableVisualStyles(string typeFace, ushort pointSize) {
			Add(new EnableVisualStylesPatch());
			Add(new TypeFacePatch(typeFace, pointSize));
		}
		/*public void AddNewLanguage(ushort language, ushort codePage) {
			Add(new LanguagePatch(language, codePage));
		}*/



		protected void AddResourceStringsPatches() {
			string[] paths = Embedded.GetResources(ResourcePath);
			foreach (string path in paths) {
				string name = Embedded.GetFileNameWithoutExtension(path, true);
				Match match = NumberRegex.Match(name);
				ushort resourceName = (match.Success ? ushort.Parse(match.Value) : (ushort) 0);
				if (name.StartsWith("menu_")) {
					string[] lines = Embedded.ReadAllLines(path);
					Add(new MenuStringsPatch(StringsScraper.BuildTranslation(lines), resourceName));
				}
				else if (name.StartsWith("dialog_")) {
					string[] lines = Embedded.ReadAllLines(path);
					Add(new DialogStringsPatch(StringsScraper.BuildTranslation(lines), resourceName));
				}
				else if (name.StartsWith("string_")) {
					string[] lines = Embedded.ReadAllLines(path);
					Add(new StringTableStringsPatch(StringsScraper.BuildTranslation(lines), resourceName));
				}
			}
		}
		protected void AddBinaryStringsPatch(params BinaryRange[] ranges) {
			string path = Embedded.Combine(ResourcePath, "binary.txt");
			var translations = StringsScraper.BuildTranslation(Embedded.ReadAllLines(path));
			Add(new BinaryStringsPatch(translations, ranges));
		}

		public bool Patch() {
			if (Executable == null) {
				if (InstallDir == null)
					throw new InvalidOperationException($"{nameof(InstallDir)} or {nameof(Executable)} must " +
														$"be specified!");
				try {
					Executable = LocateExecutable();
				} catch (NotSupportedException) {
					throw new InvalidOperationException($"{nameof(InstallDir)} is not supported by this patcher!");
				}
			}
			else if (InstallDir == null) {
				InstallDir = Path.GetDirectoryName(Executable);
			}

			bool bakExists = File.Exists(ExecutableBackup);
			//bool exeExists = File.Exists(Executable);

			if (bakExists) {
				if (RestoreBackupFirst) {
					// Restore the backup executable first as requested
					File.Copy(ExecutableBackup, Executable, true);
				}
			}
			else {
				// Backup the executable
				File.Copy(Executable, ExecutableBackup);
			}
			
			// Attempt to patch the executable
			bool result = PatchInternal();

			if (!result) {
				// Restore the backup executable and overwrite the botched patch
				File.Copy(ExecutableBackup, Executable, true);
			}

			return result;
		}

		private bool PatchInternal() {
			/*if (MD5 != null) {
				if (PEInfo.Scan(srcFileName).MD5 != MD5)
					return false;
			}*/
			if (BinaryPatches.Count != 0) {
				Thread.Sleep(300);
				using (Stream inStream = File.OpenRead(ExecutableBackup))
				using (Stream outStream = File.OpenWrite(Executable)) {
					foreach (BinaryStringsPatch patch in BinaryPatches) {
						if (!FileRetry(() => patch.Patch(inStream, outStream)))
							return false;
						//if (!patch.Patch(inStream, outStream))
						//	return false;
					}
				}
			}
			if (signaturePatcher != null || ResourcePatches.Count != 0) {
				ResourceInfo resourceInfo = new ResourceInfo(Executable);

				HashSet<Resource> patchedResources = new HashSet<Resource>();

				ManifestResource manifest = (ManifestResource) resourceInfo[ResourceTypes.Manifest].FirstOrDefault();
				if (signaturePatcher != null) {
					if (manifest == null)
						return false;
					if (!signaturePatcher.Patch(manifest))
						return false;
					patchedResources.Add(manifest);
				}
				//foreach (Resource resource in resourceInfo.Resources.Values.SelectMany(list => list)) {
				foreach (Resource resource in resourceInfo) {
						foreach (IResourcePatch patch in ResourcePatches) {
						if (patch.IsPatchable(resource)) {
							if (!patch.Patch(resource))
								return false;
							patchedResources.Add(resource);
						}
					}
				}
				
				if (!FileRetry(() => Resource.SaveTo(Executable, patchedResources)))
					return false;
			}

			if (!AdditionalPatch())
				return false;
			

			return true;
		}

		protected static bool FileRetry(Func<bool> action, int retries = 5) {
			for (int i = 0; i < retries; i++) {
				try {
					return action();
				} catch (IOException ex) {
					Console.Write(ex);
					if (i + 1 == retries)
						throw;
				} catch (Win32Exception ex) {
					Console.Write(ex);
					if (i + 1 == retries)
						throw;
				}
			}
			return false;
		}
		protected static bool FileRetry(Action action, int retries = 5) {
			return FileRetry(() => {
				action();
				return true;
			}, retries);
		}

		protected void LogLine() {
			if (Verbose) {
				if (Logger != null)
					Logger.WriteLine();
				else
					Console.WriteLine();
			}
		}
		protected void LogLine(string line) {
			if (Verbose) {
				if (Logger != null)
					Logger.WriteLine(line);
				else
					Console.WriteLine(line);
			}
		}

		#region Virtual Methods

		/// <summary>
		///  Attempts to locate the executable when no <see cref="Executable"/> is specified.
		/// </summary>
		/// <returns>The located executable file if found, otherwise null.</returns>
		/// 
		/// <exception cref="NotSupportedException">
		///  This method is not overwritten, or <see cref="TargetFile"/> is not specified.
		/// </exception>
		protected virtual string LocateExecutable() {
			if (TargetFile == null)
				throw new NotSupportedException($"{nameof(LocateExecutable)} is not supported!");
			foreach (string file in Directory.EnumerateFiles(InstallDir)) {
				string name = Path.GetFileName(file);
				if (name.Equals(TargetFile, StringComparison.InvariantCultureIgnoreCase))
					return file;
			}
			return null;
		}

		protected virtual bool AdditionalPatch() => true;

		#endregion
	}
}
