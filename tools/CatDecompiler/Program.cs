using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TriggersTools.CatSystem2;

namespace CatDecompiler {
	static class Program {

		private static List<string> inputs = null;
		private static string output = null;
		private static Encoding encoding = null;
		private static string extension = null;
		private static Level? errorLevel = null;
		private static Level? logLevel = null;
		private static int count = 0;

		private static int? ParseArguments(string[] args) {
			if (args.Length == 0) {
				DisplayHelp();
				return 0;
			}
			switch (args[0]) {
			case "-?":
			case "-h":
			case "-help":
			case "--help":
				DisplayHelp();
				return args.Length <= 1 ? 0 : 1;
			}

			try {
				bool explicitInput = false;
				bool isInput = false;
				for (int i = 0; i < args.Length; i++) {
					bool isArgInput = false;
					string firstArg = args[i];
					string arg = firstArg;
					Level level;

					switch (arg) {
					case "-i":
						if (explicitInput)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						if (inputs != null)
							throw new CommandLineException($"Inputs have already been specified \"{arg}\"!");
						inputs = new List<string>();
						explicitInput = true;
						isArgInput = true;
						isInput = true;
						break;
					case "-o":
						if (output != null)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						output = arg;
						break;
					case "-ext":
						if (extension != null)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						if (i + 1 == args.Length)
							throw new CommandLineException($"Excepted extension after \"{arg}\"!");
						extension = args[++i];
						break;

					case "-x":
						if (errorLevel.HasValue)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						if (i + 1 == args.Length)
							throw new CommandLineException($"Excepted error level after \"{arg}\"!");
						if (!Enum.TryParse(arg = args[++i], out level) || level == Level.med)
							throw new CommandLineException($"Invalid error level \"{arg}\"!");
						errorLevel = level;
						break;
					/*case "-l":
						if (logLevel.HasValue)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						if (i + 1 == args.Length)
							throw new CommandLineException($"Excepted log level after \"{arg}\"!");
						if (!Enum.TryParse(arg = args[++i], out level))
							throw new CommandLineException($"Invalid log level \"{arg}\"!");
						logLevel = level;
						break;*/
					case "-utf8":
						if (encoding != null)
							throw new CommandLineException($"Already specified option \"{arg}\"!");
						encoding = Encoding.UTF8;
						break;
					default:
						isArgInput = true;
						if (i == 0) {
							isInput = true;
							inputs = new List<string> {
								arg,
							};
						}
						else if (isInput) {
							inputs.Add(arg);
						}
						else {
							throw new CommandLineException($"Unexpected argument \"{arg}\"!");
						}

						break;
					}
					if (!isArgInput && isInput) {
						if (inputs.Count == 0)
							throw new CommandLineException($"Expected at least one input after \"-i\", got \"{firstArg}\"!");
						isInput = false;
					}
				}

				if (inputs == null || inputs.Count == 0)
					throw new CommandLineException("No inputs specified!");

				if (!errorLevel.HasValue)
					errorLevel = Level.none;
				if (!logLevel.HasValue)
					logLevel = Level.high;
				if (extension == null)
					extension = ".txt";

				if (logLevel != Level.none)
					Console.WriteLine("CatSystem2 Script Decompiler");

				if (output == null)
					output = Directory.GetCurrentDirectory();

				if (encoding == null)
					encoding = CatUtils.ShiftJIS;
			} catch (CommandLineException ex) {
				Console.WriteLine("CatSystem2 Script Decompiler");
				PrintError(ex.Message);
				return 1;
			}

			return null;
		}

		static int Main(string[] args) {
			int? result = ParseArguments(args);
			if (result.HasValue)
				return result.Value;

			foreach (string input in inputs) {
				string fileName = Path.GetFileName(input);
				string directory = Path.GetDirectoryName(input);
				if (string.IsNullOrEmpty(directory))
					directory = Directory.GetCurrentDirectory();
				else if (!Path.IsPathRooted(directory))
					directory = Path.GetFullPath(directory);
				foreach (string file in Directory.GetFiles(directory, fileName)) {
					if (!Decompile(file)) {
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Stopping due to error level...");
						Console.ResetColor();
						return -1;
					}
				}
			}
			Console.WriteLine($"Decompiled {count} scripts");

			#region Unused
			/*if (args.Length == 0 || args[0] == "-h" || args[0] == "--help") {
				DisplayHelp();
				return (args.Length <= 1 ? 0 : 1);
			}
			//string inputPattern = null;
			//string[] inputPatterns = null;
			List<string> inputs = null;
			//string outputFile = null;
			string output = null;
			Encoding encoding = null;
			bool explicitInput = false;
			bool continueOnError = true;
			string extension = null;
			Level? errorLevel = null;
			Level? logLevel = null;
			//bool isInputPattern = false;

			try {
				bool isInput = false;
				for (int i = 0; i < args.Length; i++) {
					bool isThisArgInput = false;
					string arg = args[i];
					switch (arg) {
					case "-i":
					//case "--input":
						if (explicitInput)
							throw new CommandLineException($"Argument already specified \"{arg}\"!");
						if (inputs != null)
							throw new CommandLineException($"Inputs have already been specified \"{arg}\"!");
						inputs = new List<string>();
						explicitInput = true;
						isInput = true;
						break;
					case "-o":
					//case "--output":
						if (isInput && inputs.Count == 0)
							throw new CommandLineException($"Expected at least one input!");
						if (output != null)
							throw new CommandLineException($"Argument already specified \"{arg}\"!");
						output = arg;
						isInput = false;

						break;
					case "-ext":
					//case "--extension":
						if (isInput && inputs.Count == 0)
							throw new CommandLineException($"Expected at least one input!");
						if (extension != null)
							throw new CommandLineException($"Argument already specified \"{arg}\"!");
						if (i + 1 == args.Length)
							throw new CommandLineException($"Excepted extension after \"{arg}\"!");
						extension = args[++i];
						isInput = false;
						break;

					case "-x":
					//case "--error":
						if (errorLevel.HasValue)
							throw new CommandLineException($"Argument already specified \"{arg}\"!");
						if (i + 1 == args.Length)
							throw new CommandLineException($"Excepted error level after \"{arg}\"!");
						continueOnError = true;
						break;
					case "-utf8":
					//case "--utf8":
						if (isInput && inputs.Count == 0)
							throw new CommandLineException($"Expected at least one input!");
						if (encoding != null)
							throw new CommandLineException($"Argument already specified \"{arg}\"!");
						encoding = Encoding.UTF8;
						isInput = false;
						break;
					default:
						isThisArgInput = true;
						if (i == 0) {
							isInput = true;
							inputs = new List<string> {
								arg,
							};
						}
						else if (isInput) {
							inputs.Add(arg);
						}
						else {
							throw new CommandLineException($"Unexpected argument \"{arg}\"!");
						}

						break;
					}
					if (!isThisArgInput && isInput) {

					}
				}

				if (inputs == null || inputs.Count == 0)
					throw new CommandLineException("No inputs specified!");
			} catch (CommandLineException ex) {
				Console.WriteLine("CatSystem2 Script Decompiler");
				PrintError(ex.Message);
				return 1;
			}

			if (!errorLevel.HasValue)
				errorLevel = Level.none;
			if (!logLevel.HasValue)
				logLevel = Level.high;

			if (logLevel != Level.none)
				Console.WriteLine("CatSystem2 Script Decompiler");

			if (output == null)
				output = Directory.GetCurrentDirectory();

			if (encoding == null)
				encoding = CatUtils.ShiftJIS;

			foreach (string input in inputs) {
				string fileName = Path.GetFileName(input);
				string directory = Path.GetDirectoryName(input);
				if (string.IsNullOrEmpty(directory))
					directory = Directory.GetCurrentDirectory();
				else if (!Path.IsPathRooted(directory))
					directory = Path.GetFullPath(directory);
				foreach (string file in Directory.GetFiles(directory, fileName)) {
					Decompile(file, output, encoding, errorLevel, logLevel);
				}
			}*/
			#endregion

			return 0;
		}

		private static bool Decompile(string file) {
			try {
				AnmAnimation anm = AnmAnimation.Extract(file);
				try {
					string outFile = Path.Combine(output, Path.GetFileNameWithoutExtension(file) + extension);
					anm.DecompileToFile(outFile, encoding);
					Console.WriteLine($"  ANM {Path.GetFileName(file)} -> {Path.GetFileName(outFile)}");
					count++;
					return true;
				} catch (Exception ex) {
					PrintError($"ANM {Path.GetFileName(file)}, {ex.Message}");
					if (errorLevel.Value >= Level.low)
						return false;
				}
			} catch { }
			try {
				CstScene cst = CstScene.Extract(file);
				try {
					string outFile = Path.Combine(output, Path.GetFileNameWithoutExtension(file) + extension);
					cst.DecompileToFile(outFile, encoding);
					Console.WriteLine($"  CST {Path.GetFileName(file)} -> {Path.GetFileName(outFile)}");
					count++;
					return true;
				} catch (Exception ex) {
					PrintError($"CST {Path.GetFileName(file)}, {ex.Message}");
					if (errorLevel.Value >= Level.low)
						return false;
				}
			} catch { }
			try {
				FesScreen fes = FesScreen.Extract(file);
				try {
					string outFile = Path.Combine(output, Path.GetFileNameWithoutExtension(file) + extension);
					fes.DecompileToFile(outFile, encoding);
					Console.WriteLine($"  FES {Path.GetFileName(file)} -> {Path.GetFileName(outFile)}");
					count++;
					return true;
				} catch (Exception ex) {
					PrintError($"FES {Path.GetFileName(file)}, {ex.Message}");
					if (errorLevel.Value >= Level.low)
						return false;
				}
			} catch { }
			PrintWarning($"{Path.GetFileName(file)} is not a CaySystem2 script file!");
			return !(errorLevel.Value >= Level.high);
		}
		private enum Level {
			none = 0,
			low = 1,
			med = 2,
			high = 3,
		}
		private static void PrintError(string message) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"  Error: {message}");
			Console.ResetColor();
		}
		private static void PrintWarning(string message) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"  Warning: {message}");
			Console.ResetColor();
		}

		class CommandLineException : Exception {

			public CommandLineException(string message) : base(message) { }
		}

		static void PrintOption(string option, string description) {
			Console.WriteLine($" {option:18} {description}");
		}

		static void DisplayHelp() {
			string exeName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
			//Console.WriteLine($"Usage: {exeName} [-i] <inputs...> [-o <output_dir>] [-e utf8|jis]");
			Console.WriteLine($"usage: {exeName} [infiles...] [options]");
			Console.WriteLine();
			Console.WriteLine($"Options:");
			PrintOption("-h", "Show help");
			PrintOption("-i <infiles...>", "Input files to decompile (supports wildcards)");
			PrintOption("-o <outdir>", "Output directory for decompiled files");
			PrintOption("-ext <.ext>", "Output extension for decompiled files, default is \".txt\"");
			PrintOption("-utf8", "Decompiled files will be encoded in UTF-8, default is Shift JIS");
			PrintOption("-x <errlevel>", "Errors to stop at: high=warnings, low=errors (default), none");
			//PrintOption("-l <loglevel>", "What to log: high=output, med=any error, low=program error (default), none");
			/*Console.WriteLine($"  -o <outdir>  Output directory for decompiled files");
			//Console.WriteLine($"  -e|--encoding utf8|jis  Output file encoding, Shift JIS by default");
			Console.WriteLine($"  -utf8  Output file is UTF-8, otherwise Shift JIS");
			Console.WriteLine($"  -ext  Set the output extension");
			Console.WriteLine($"  -x  Stop on error");*/
		}
	}
}
