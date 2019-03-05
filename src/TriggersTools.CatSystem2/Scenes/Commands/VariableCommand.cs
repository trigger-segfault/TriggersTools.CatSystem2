using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets a variable.
	/// </summary>
	public sealed class VariableCommand : SceneCommandBase, ISceneCommand {
		#region Constants

		private static readonly Regex OperatorRegex = new Regex(@"");

		#endregion

		#region Fields

		private string content;
		
		/// <summary>
		///  Gets the parameters for the command, including the comamnd name.
		/// </summary>
		public IReadOnlyList<string> Parameters { get; private set; }

		public string Command => "if";

		public int Count => 3;

		public override string Content {
			get => content;
			set {
				content = value ?? throw new ArgumentNullException(nameof(Content));
				
				int parenLevel = 0;
				int parenStart = 0;
				string[] parameters = new string[3] { "if", string.Empty, string.Empty };
				for (int i = 2; i < value.Length; i++) {
					char c = value[i];
					if (c == '(') {
						parenLevel++;
						parenStart = i;
					}
					else if (c == ')') {
						parenLevel--;
						if (parenLevel == 0) {
							parameters[1] = value.Substring(parenStart, i - parenStart + 1);
							parameters[2] = value.Substring(i + 1).TrimStart(' ');
							break;
						}
						else if (parenLevel < 0)
							throw new ArgumentException(nameof(Content));
					}
				}
				//IfCommand = SceneCommands.CreateCommand(parameters[2]);
				Parameters = Array.AsReadOnly(parameters);
			}
		}

		#endregion

		#region Constructors

		public VariableCommand() : this(string.Empty) { }
		public VariableCommand(string content) {
			Content = content;
		}

		#endregion

		#region IsCommand

		public int IsCommandPriority => 0;
		public bool IsCommand(string name, string content, string[] parameters) {
			return content.StartsWith("#");
		}

		#endregion
	}
}
