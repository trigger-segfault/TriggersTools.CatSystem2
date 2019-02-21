using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Compiler {
	class Program {
		enum CompileType {
			Anm,
			Cst,
			Fes,
		}
		static int Main(string[] args) {
			Console.WriteLine("CatSystem2 UTF-8 Compiler");
			if (args.Length == 0 || (args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))) {
				Console.WriteLine("usage: catcompiler <anm|cst|fes> <input> [output]");
				return 1;
			}
			else if (args.Length < 2) {
				Console.WriteLine("Not enough arguments!");
				return 1;
			}
			else if (args.Length > 3) {
				Console.WriteLine("Too many arguments!");
				return 1;
			}
			if (!Enum.TryParse(args[0], true, out CompileType type)) {
				Console.WriteLine("Argument 1, expected anm, cst, or fes!");
				return 1;
			}
			try {
				string input = args[1];
				string output;
				if (args.Length == 3) {
					output = args[2];
				}
				else {
					output = Path.GetDirectoryName(input);
					if (output.Length == 0)
						output = Directory.GetCurrentDirectory();
				}
				switch (type) {
				case CompileType.Anm:
					CatUtils.CompileAnimationFiles(input, output);
					break;
				case CompileType.Cst:
					CatUtils.CompileSceneFiles(input, output);
					break;
				case CompileType.Fes:
					CatUtils.CompileScreenFiles(input, output);
					break;
				}
				return 0;
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return 1;
			}
			
		}
	}
}
