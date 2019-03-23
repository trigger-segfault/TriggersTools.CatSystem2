using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils;
using TriggersTools.SharpUtils.IO;
using TriggersTools.Windows.Resources;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A static class containing helper functions and constants for use with CatSystem2, or CS2 for short.
	/// </summary>
	public static class CatUtils {
		#region Constants

		/// <summary>
		///  Gets the Japanese encoding used for the CatSystem2 files (Shift JIS, codepage 932).
		/// </summary>
		public static Encoding ShiftJIS { get; } = Encoding.GetEncoding(932);
		/// <summary>
		///  The language identifier for Japanese Windows resources.
		/// </summary>
		public const ushort LanguageIdentifier = 0x0411;

		/// <summary>
		///  Gets the temporary directory for TriggersTools.CatSystem2 file operations.
		/// </summary>
		public static string TempDir { get; } = TriggerTemp.Combine("CatSystem2");

		/// <summary>
		///  The list of escapes that should be removed from <see cref="SceneLineType.Message"/> commands.
		/// </summary>
		private static readonly IReadOnlyList<string> MessageEscapesToRemove = new string[] {
			@"c[rs]?0x[A-Fa-f0-9]{8};?", // Change font color
			@"fr?[+\-]?\d+;?", // Change font size (input)
			@"fr?(?:n|s|ss|l|ll)", // Change font size (enum)
			@"p[lcr]", // Change display position
			@"wf?\d+;?", // Change display timing
			@"@", // Block script
		};
		private static readonly string MessageEscapesToRemovePattern =
			$@"\\(?:{string.Join("|", MessageEscapesToRemove)})";
		private static readonly Regex MessageEscapesToRemoveRegex =
			new Regex(MessageEscapesToRemovePattern, RegexOptions.IgnoreCase);

		private const string MessageNoEscapePattern = @"(?<!\\)(?:\\\\)*(?'char'[\[\]_])";
		private static readonly Regex MessageNoEscapeRegex =
			new Regex(MessageNoEscapePattern, RegexOptions.IgnoreCase);
		private const string EscapePattern = @"\\.";
		private static readonly Regex EscapeRegex = new Regex(EscapePattern);

		#endregion

		#region Static Constructors

		static CatUtils() {
			Directory.CreateDirectory(TempDir);
		}

		#endregion

		#region Fields

		/// <summary>
		///  Gets the static instance of a compiler that can be used with <see cref="CatUtils"/> compile methods.
		/// </summary>
		public static CatCompiler CompilerInstance { get; } = new CatCompiler();
		/// <summary>
		///  The path to extract all native Dlls to, because they're a pain with NuGet packages.
		/// </summary>
		private static string nativeDllExtractPath = TempDir;//Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the path to extract all native Dlls to, because they're a pain with NuGet packages.
		/// </summary>
		public static string NativeDllExtractPath {
			get => nativeDllExtractPath;
			set => nativeDllExtractPath = value ?? throw new ArgumentNullException(nameof(NativeDllExtractPath));
		}
		/// <summary>
		///  Gets or sets the path to ac.exe, which is used to compile <see cref="AnmAnimation"/>'s.
		/// </summary>
		public static string AcPath {
			get => CompilerInstance.AcPath;
			set => CompilerInstance.AcPath = value;
		}
		/// <summary>
		///  Gets or sets the path to mc.exe, which is used to compile <see cref="CstScene"/>'s.
		/// </summary>
		public static string McPath {
			get => CompilerInstance.McPath;
			set => CompilerInstance.McPath = value;
		}
		/// <summary>
		///  Gets or sets the path to fes.exe, which is used to compile <see cref="FesScreen"/>'s.
		/// </summary>
		public static string FesPath {
			get => CompilerInstance.FesPath;
			set => CompilerInstance.FesPath = value;
		}

		#endregion

		#region CompileAnimation

		/// <summary>
		///  Compiles the animation script files and outputs them to the same directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public static int CompileAnimationFiles(string patternOrFile) {
			return CompilerInstance.CompileAnimationFiles(patternOrFile);
		}
		/// <summary>
		///  Compiles the animation script files and outputs them to the specified directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <param name="outputDir">The output directory for the files.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static int CompileAnimationFiles(string patternOrFile, string outputDir) {
			return CompilerInstance.CompileAnimationFiles(patternOrFile, outputDir);
		}
		/// <summary>
		///  Compiles the animation script and outputs it to the specified file.
		/// </summary>
		/// <param name="script">The script text to compile.</param>
		/// <param name="outputFile">The output file to compile to.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="script"/> or <paramref name="outputFile"/> is null.
		/// </exception>
		public static int CompileAnimationScript(string script, string outputFile) {
			return CompilerInstance.CompileAnimationScript(script, outputFile);
		}

		#endregion

		#region CompileScreen

		/// <summary>
		///  Compiles the screen script files and outputs them to the same directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public static int CompileScreenFiles(string patternOrFile) {
			return CompilerInstance.CompileScreenFiles(patternOrFile);
		}
		/// <summary>
		///  Compiles the screen script files and outputs them to the specified directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <param name="outputDir">The output directory for the files.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static int CompileScreenFiles(string patternOrFile, string outputDir) {
			return CompilerInstance.CompileScreenFiles(patternOrFile, outputDir);
		}
		/// <summary>
		///  Compiles the screen script and outputs it to the specified file.
		/// </summary>
		/// <param name="script">The script text to compile.</param>
		/// <param name="outputFile">The output file to compile to.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="script"/> or <paramref name="outputFile"/> is null.
		/// </exception>
		public static int CompileScreenScript(string script, string outputFile) {
			return CompilerInstance.CompileScreenScript(script, outputFile);
		}

		#endregion

		#region CompileScene

		/// <summary>
		///  Compiles the scene script files and outputs them to the specified directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public static int CompileSceneFiles(string patternOrFile) {
			return CompilerInstance.CompileSceneFiles(patternOrFile);
		}
		/// <summary>
		///  Compiles the scene script files and outputs them to the specified directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <param name="outputDir">The output directory for the files.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static int CompileSceneFiles(string patternOrFile, string outputDir) {
			return CompilerInstance.CompileSceneFiles(patternOrFile, outputDir);
		}
		/// <summary>
		///  Compiles the scene script and outputs it to the specified file.
		/// </summary>
		/// <param name="script">The script text to compile.</param>
		/// <param name="outputFile">The output file to compile to.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="script"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		public static int CompileSceneScript(string script, string outputDir) {
			return CompilerInstance.CompileSceneScript(script, outputDir);
		}

		#endregion

		#region Message/Name

		/// <summary>
		///  Unescapes a scene script's message to display in a readable manner.
		/// </summary>
		/// <param name="content">The content of the message line.</param>
		/// <returns>The unescaped message.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="content"/> is null.
		/// </exception>
		public static string UnescapeMessage(string content, bool name) {
			if (content == null)
				throw new ArgumentNullException(nameof(content));
			StringBuilder str = new StringBuilder(content);
			MatchCollection matches = MessageEscapesToRemoveRegex.Matches(content);
			for (int i = matches.Count - 1; i >= 0; i--) {
				Match match = matches[i];
				str.Remove(match.Index, match.Length);
			}

			matches = MessageNoEscapeRegex.Matches(str.ToString());
			for (int i = matches.Count - 1; i >= 0; i--) {
				Group group = matches[i].Groups["char"];
				char c = group.Value[0];
				switch (c) {
				case '_':
					break;
				case '[':
				case ']':
					str.Remove(group.Index, group.Length);
					break;
				}
			}

			matches = EscapeRegex.Matches(str.ToString());
			for (int i = matches.Count - 1; i >= 0; i--) {
				Match match = matches[i];
				char c = match.Value[1];
				switch (c) {
				case '_':
					str.Remove(match.Index, match.Length);
					str.Insert(match.Index, ' ');
					break;
				case 'n':
					str.Remove(match.Index, match.Length);
					if (!name)
						str.Insert(match.Index, "\n");
					break;
				case 'r':
					str.Remove(match.Index, match.Length);
					break;
				default:
					str.Remove(match.Index, 1); // Remove only the backslash
					break;
				}
			}

			return str.ToString();
		}
		/// <summary>
		///  Unescapes a scene script's string to display in a readable manner.
		/// </summary>
		/// <param name="content">The content of the name line or string command.</param>
		/// <returns>The unescaped string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="content"/> is null.
		/// </exception>
		public static string UnescapeString(string content) {
			if (content == null)
				throw new ArgumentNullException(nameof(content));
			StringBuilder str = new StringBuilder(content);
			MatchCollection matches = MessageNoEscapeRegex.Matches(str.ToString());
			for (int i = matches.Count - 1; i >= 0; i--) {
				Group group = matches[i].Groups["char"];
				char c = group.Value[0];
				switch (c) {
				case '_':
					str.Remove(group.Index, group.Length);
					str.Insert(group.Index, ' ');
					break;
				case '[':
				case ']':
					str.Remove(group.Index, group.Length);
					break;
				}
			}

			matches = EscapeRegex.Matches(str.ToString());
			for (int i = matches.Count - 1; i >= 0; i--) {
				Match match = matches[i];
				str.Remove(match.Index, 1);
				char c = match.Value[1];
				switch (c) {
				case 'n':
				case 'r':
					str.Remove(match.Index, match.Length);
					break;
				default:
					str.Remove(match.Index, 1); // Remove only the backslash
					break;
				}
			}

			return str.ToString();
		}

		#endregion

		#region IsCatExecutable

		/// <summary>
		///  Checks if <paramref name="exeFile"/> is a CatSystem2 executable file with V_CODEs.
		/// </summary>
		/// <param name="exeFile">The executable file to check.</param>
		/// <returns>True if <paramref name="exeFile"/> has V_CODEs.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="exeFile"/> is null.
		/// </exception>
		public static bool IsCatExecutable(string exeFile) {
			if (exeFile == null)
				throw new ArgumentNullException(nameof(exeFile));
			try {
				VCodes.Load(exeFile);
				return true;
			} catch { return false; }
		}
		/// <summary>
		///  Attempts to locate the first executable with V_CODEs in the CatSytem2 game install directory.
		/// </summary>
		/// <param name="installDir">The installation directory to check the files of.</param>
		/// <returns>The file path of an executable that contains V_CODEs, or null if none was found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="installDir"/> is null.
		/// </exception>
		public static string FindCatExecutable(string installDir) {
			return FindCatExecutables(installDir).FirstOrDefault();
		}
		/// <summary>
		///  Attempts to locate all executables with V_CODEs in the CatSytem2 game install directory.
		/// </summary>
		/// <param name="installDir">The installation directory to check the files of.</param>
		/// <returns>A collection of executable paths that contain V_CODEs.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="installDir"/> is null.
		/// </exception>
		public static IEnumerable<string> FindCatExecutables(string installDir) {
			foreach (string file in Directory.EnumerateFiles(installDir)) {
				string ext = Path.GetExtension(file).ToLower();
				if (ext != ".bak" && IsCatExecutable(file))
					yield return installDir;
			}
		}

		#endregion

		/*public static int ParseInt(string s) {
			NumberStyles style = NumberStyles.Integer;
			if (s.StartsWith("$")) {
				s = s.Substring(1).Trim();
				style = NumberStyles.HexNumber;
			}
			else if (s.StartsWith("0x") || s.StartsWith("0X")) {
				s = s.Substring(2).Trim();
				style = NumberStyles.HexNumber;
			}
			return int.Parse(s, style);
		}
		public static float ParseFloat(string s) {

		}*/

		#region ReEncode

		/// <summary>
		///  Re-encodes the source file to <see cref="ShiftJIS"/> and outputs it to the destination file.
		/// </summary>
		/// <param name="srcPath">The source file to re-encode.</param>
		/// <param name="dstPath">The destination re-encoded file.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="srcPath"/> or <paramref name="dstPath"/> is null.
		/// </exception>
		public static void ReEncodeToShiftJIS(string srcPath, string dstPath) {
			FileUtils.ReEncode(srcPath, dstPath, Encoding.UTF8, ShiftJIS);
		}
		/// <summary>
		///  Re-encodes the source file to <see cref="Encoding.UTF8"/> and outputs it to the destination file.
		/// </summary>
		/// <param name="srcPath">The source file to re-encode.</param>
		/// <param name="dstPath">The destination re-encoded file.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="srcPath"/> or <paramref name="dstPath"/> is null.
		/// </exception>
		public static void ReEncodeToUTF8(string srcPath, string dstPath) {
			FileUtils.ReEncode(srcPath, dstPath, ShiftJIS, Encoding.UTF8);
		}

		#endregion
	}
}
