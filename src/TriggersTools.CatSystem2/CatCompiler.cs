using System;
using System.Diagnostics;
using System.IO;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A class for storing the locations of compiler executables and comiling script files.
	/// </summary>
	public class CatCompiler {
		#region Private Enums

		/// <summary>
		///  A quick-access enum to define what type of compiler to run.
		/// </summary>
		private enum CompileType {
			Animation,
			ScreenScript,
			SceneScript,
		}

		#endregion

		#region Constants

		/// <summary>
		///  Gets the directory used as a temporary compiling working directory.
		/// </summary>
		public static string CompileDir { get; } = Path.Combine(CatUtils.TempDir, "Compile");

		#endregion

		#region Fields

		/// <summary>
		///  Gets or sets the path to ac.exe, which is used to compile <see cref="AnmAnimation"/>'s.
		/// </summary>
		public string AcPath { get; set; } = "ac.exe";
		/// <summary>
		///  Gets or sets the path to mc.exe, which is used to compile <see cref="CstScene"/>'s.
		/// </summary>
		public string McPath { get; set; } = "mc.exe";
		/// <summary>
		///  Gets or sets the path to fes.exe, which is used to compile <see cref="FesScreen"/>'s.
		/// </summary>
		public string FesPath { get; set; } = "fes.exe";

		#endregion

		#region Animation

		/// <summary>
		///  Compiles the animation script files and outputs them to the same directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public int CompileAnimationFiles(string patternOrFile) {
			if (patternOrFile == null)
				throw new ArgumentNullException(nameof(patternOrFile));
			string patternDir = Path.GetDirectoryName(patternOrFile);
			if (patternDir.Length == 0)
				patternDir = Directory.GetCurrentDirectory();
			return CompileAnimationFiles(patternOrFile, patternDir);
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
		public int CompileAnimationFiles(string patternOrFile, string outputDir) {
			return CompileFiles(patternOrFile, outputDir, CompileType.Animation);
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
		public int CompileAnimationScript(string script, string outputFile) {
			return CompileScriptToFile(script, outputFile, CompileType.Animation);
		}

		#endregion

		#region Screen

		/// <summary>
		///  Compiles the screen script files and outputs them to the same directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public int CompileScreenFiles(string patternOrFile) {
			if (patternOrFile == null)
				throw new ArgumentNullException(nameof(patternOrFile));
			string patternDir = Path.GetDirectoryName(patternOrFile);
			if (patternDir.Length == 0)
				patternDir = Directory.GetCurrentDirectory();
			return CompileScreenFiles(patternOrFile, patternDir);
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
		public int CompileScreenFiles(string patternOrFile, string outputDir) {
			return CompileFiles(patternOrFile, outputDir, CompileType.ScreenScript);
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
		public int CompileScreenScript(string script, string outputFile) {
			return CompileScriptToFile(script, outputFile, CompileType.ScreenScript);
		}

		#endregion

		#region Scene

		/// <summary>
		///  Compiles the scene script files and outputs them to the specified directory.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> is null.
		/// </exception>
		public int CompileSceneFiles(string patternOrFile) {
			if (patternOrFile == null)
				throw new ArgumentNullException(nameof(patternOrFile));
			string patternDir = Path.GetDirectoryName(patternOrFile);
			if (patternDir.Length == 0)
				patternDir = Directory.GetCurrentDirectory();
			return CompileSceneFiles(patternOrFile, patternDir);
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
		public int CompileSceneFiles(string patternOrFile, string outputDir) {
			return CompileFiles(patternOrFile, outputDir, CompileType.SceneScript);
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
		public int CompileSceneScript(string script, string outputDir) {
			return CompileScriptToDirectory(script, outputDir, CompileType.SceneScript);
		}

		#endregion

		#region Compile (Private)

		/// <summary>
		///  Called for scripts that output one file that relies on the file name.
		/// </summary>
		/// <param name="script">The script text to compile.</param>
		/// <param name="outputFile">The output file to compile to.</param>
		/// <param name="type">The type of script to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="script"/> or <paramref name="outputFile"/> is null.
		/// </exception>
		private int CompileScriptToFile(string script, string outputFile, CompileType type) {
			if (script == null)
				throw new ArgumentNullException(nameof(script));
			if (outputFile == null)
				throw new ArgumentNullException(nameof(outputFile));
			// Generate a new temporary compile directory
			// Delete the temporary compile directory when done
			using (var tmp = CreateCompileTempDir()) {
				string tmpFile = Path.Combine(tmp, Path.ChangeExtension(Path.GetFileName(outputFile), ".txt"));
				string outputDir = Path.GetDirectoryName(outputFile);
				if (outputDir.Length == 0)
					outputDir = Directory.GetCurrentDirectory();

				// Write the script to the file
				File.WriteAllText(tmpFile, script, CatUtils.ShiftJIS);

				// Run the compiler and move the output files
				return RunCompiler(type, Path.GetFileName(tmpFile), tmp, outputDir);
			}
		}

		/// <summary>
		///  Called for scripts that have the ability to output more than one file, and do not rely on the file name.
		/// </summary>
		/// <param name="script">The script text to compile.</param>
		/// <param name="outputFile">The output directory to compile to.</param>
		/// <param name="type">The type of script to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="script"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		private int CompileScriptToDirectory(string script, string outputDir, CompileType type) {
			if (script == null)
				throw new ArgumentNullException(nameof(script));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			// Generate a new temporary compile directory
			// Delete the temporary compile directory when done
			using (var tmp = CreateCompileTempDir()) {
				//string tmpFile = Path.Combine(tmp, Path.ChangeExtension(Path.GetTempFileName(), ".txt"));
				string tmpFile = Path.Combine(tmp, Path.ChangeExtension(Guid.NewGuid().ToString(), ".txt"));

				// Write the script to the file
				File.WriteAllText(tmpFile, script, CatUtils.ShiftJIS);

				// Run the compiler and move the output files
				return RunCompiler(type, Path.GetFileName(tmpFile), tmp, outputDir);
			}
		}
		/// <summary>
		///  Called to compile a pattern of script files or a single script file.<para/>
		///  This method compies the files to a temporary directory so they can be encoded in
		///  <see cref="CatUtils.ShiftJIS"/>.
		/// </summary>
		/// <param name="patternOrFile">The wildcard pattern or file path to compile.</param>
		/// <param name="outputDir">The output directory for the files.</param>
		/// <param name="type">The type of script to compile.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="patternOrFile"/> or <paramref name="outputDir"/> is null.
		/// </exception>
		private int CompileFiles(string patternOrFile, string outputDir, CompileType type) {
			if (patternOrFile == null)
				throw new ArgumentNullException(nameof(patternOrFile));
			if (outputDir == null)
				throw new ArgumentNullException(nameof(outputDir));
			string patternName = Path.GetFileName(patternOrFile);
			string patternDir = Path.GetDirectoryName(patternOrFile);
			if (patternDir.Length == 0)
				patternDir = ".";
			if (!Path.IsPathRooted(patternDir))
				patternDir = Path.GetFullPath(patternDir);

			// Generate a new temporary compile directory
			// Delete the temporary compile directory when done
			using (var tmp = CreateCompileTempDir()) {
				// Copy and re-encode the files to the tmp directory with ShiftJIS encoding.
				string[] txtFiles = Directory.GetFiles(patternDir, patternName);
				for (int i = 0; i < txtFiles.Length; i++) {
					string file = txtFiles[i];
					string tmpFile = Path.Combine(tmp, Path.GetFileName(file));
					CatUtils.ReEncodeToShiftJIS(file, tmpFile);
				}

				// Run the compiler and move the output files
				return RunCompiler(type, patternName, tmp, outputDir);
			}
		}
		/// <summary>
		///  Creates a disposable temporary compilation directory.
		/// </summary>
		/// <returns>The temporary directory that can be disposed of.</returns>
		private static TempDirectory CreateCompileTempDir() {
			string tmp = Path.Combine(CompileDir, Guid.NewGuid().ToString());
			Directory.CreateDirectory(tmp);
			return new TempDirectory(tmp);
		}
		/// <summary>
		///  Runs the compiled by getting the exe type, running the exe, then copying the generated files to
		///  <paramref name="outputDir"/>.
		/// </summary>
		/// <param name="type">The type of script to compile.</param>
		/// <param name="patternOrFile">The wildcard pattern or file name to compile.</param>
		/// <param name="tmp">The temporary directory path being worked in.</param>
		/// <param name="outputDir">The output directory for the compiled files.</param>
		/// <returns>The number of successfully compiled scripts.</returns>
		private int RunCompiler(CompileType type, string patternOrFile, string tmp, string outputDir) {
			// Get the executable path and extension
			string exe;
			switch (type) {
			case CompileType.Animation: exe = AcPath; break;
			case CompileType.ScreenScript: exe = FesPath; break;
			case CompileType.SceneScript: exe = McPath; break;
			default: throw new ArgumentException(nameof(type));
			}
				

			if (exe == null) {
				throw new InvalidOperationException($"Cannot compile {type} when path to compiler has " +
													$"not been specified!");
			}

			// Run the compiler
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = exe,
				Arguments = $"\"{patternOrFile}\"",
				UseShellExecute = false,
				WorkingDirectory = tmp,
				RedirectStandardOutput = false,
			};
			using (Process p = Process.Start(startInfo))
				p.WaitForExit();

			// Move the compiled output files to the output directory
			int count = 0;
			string[] tmpFiles = Directory.GetFiles(tmp);
			for (int i = 0; i < tmpFiles.Length; i++) {
				string tmpFile = tmpFiles[i];
				string file = Path.Combine(outputDir, Path.GetFileName(tmpFile));
				// Make sure the file exists (was successfully compiled)
				if (File.Exists(tmpFile)) {
					if (File.Exists(file))
						File.Delete(file);
					File.Move(tmpFile, file);
					count++;
				}
			}

			return count;
		}

		#endregion
	}
}
