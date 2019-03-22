using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatDecompiler {
	public class ArgumentParser {


		public Argument this[string id] => Arguments.Find(a => a.Id == id);

		public Argument HelpArgument { get; set; }

		public ArgumentParser() {
			HelpArgument = new Argument {
				Id = "help",
				Description = "Display help",
				Switches = new[] { "-h", "--help" },
				Flags = ArgumentFlags.None,
				ParameterCount = 0,
				Usage = "",
				ParameterNames = Array.Empty<string>(),
				Execute = DisplayHelp,
				RequiredIndex = -1,
			};
			Arguments.Add(HelpArgument);
		}

		public void DisplayHelp() {
			DisplayHelp(Array.Empty<string>(), 0);
		}
		private bool DisplayHelp(string[] args, int index) {

			return true;
		}

		public string Title { get; set; } = Path.GetFileName(Assembly.GetEntryAssembly().Location);
		public string Executable { get; set; } = Path.GetFileName(Assembly.GetEntryAssembly().Location);

		public string Usage { get; set; }
		
		public string Description { get; set; }

		public List<Argument> Arguments { get; }

		public bool Execute() {
			string[] cmdLineArgs = Environment.GetCommandLineArgs();
			string[] args = new string[cmdLineArgs.Length - 1];
			Array.Copy(cmdLineArgs, 1, args, 0, args.Length);
			return Execute(args);
		}
		public bool Execute(string[] args) {
			List<Argument> executed = new List<Argument>();
			if (args.Length == 0)
				DisplayHelp();
			return true;
		}
	}
	public delegate bool ExecuteArgument(string[] args, int index);
	public enum ArgumentParserFlags {

	}
	public enum ArgumentFlags {
		None = 0,
		/// <summary>
		///  The argument must be the first in the list.
		/// </summary>
		First = (1 << 0),
		/// <summary>
		///  The argument must be the first in the list if it does not have a switch.
		/// </summary>
		FirstIfNoSwitch = (1 << 1),
		/// <summary>
		///  The argument must be the last in the list.
		/// </summary>
		Last = (1 << 2),
		/// <summary>
		///  The argument must be the last in the list if it does not have a switch.
		/// </summary>
		LastIfNoSwitch = (1 << 3),
		/// <summary>
		///  The argument can be used multiple times.
		/// </summary>
		Multiple = (1 << 4),
		/// <summary>
		///  The argument is required to be present.
		/// </summary>
		Required = (1 << 5),
		/// <summary>
		///  The switch to this argument is optional.
		/// </summary>
		OptionalSwitch = (1 << 6),
	}
	public class Argument {
		public string Id { get; set; }
		public string[] RequiredArgumentIds { get; set; } = Array.Empty<string>();
		public string[] Switches { get; set; }

		/// <summary>
		///  The usage display arguments for the switch.
		/// </summary>
		public string Usage { get; set; }
		/// <summary>
		///  The description for the switch.
		/// </summary>
		public string Description { get; set; }
		
		public ArgumentFlags Flags { get; set; }
		/// <summary>
		///  The required index of the argument if not -1.
		/// </summary>
		public int RequiredIndex { get; set; }

		/// <summary>
		///  The number of required arguments, does not count the switch itself.
		/// </summary>
		public int ParameterCount { get; set; }

		public string[] ParameterNames { get; set; }

		public ExecuteArgument Execute { get; set; }
	}
}
