using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	public sealed class ChoiceCommand : SceneCommandBase, ISceneCommand {
		#region Fields

		private string content;
		
		public ISceneCommand Command { get; private set; }
		
		/// <summary>
		///  Gets the parameters for the command, including the comamnd name.
		/// </summary>
		public IReadOnlyList<string> Parameters { get; private set; }
		
		public int ChoiceId => int.Parse(Parameters[0]);
		public string ChoiceJump => Parameters[1];
		public string ChoiceContent => Parameters[2];
		public string ChoiceText => CatUtils.UnescapeMessage(Parameters[2], true);

		public string CommandName => "choice";


		public int Count => 3;

		public override string Content {
			get => content;
			set {
				content = value ?? throw new ArgumentNullException(nameof(Content));
				if (content.Length == 0)
					return;
				
				string[] parameters = content.Split(new[] { ' ', '\t' }, 3, StringSplitOptions.RemoveEmptyEntries);
				if (parameters.Length != 3)
					throw new ArgumentException(nameof(Content));
				Parameters = parameters.ToImmutableArray();
			}
		}

		#endregion

		#region Constructors

		public ChoiceCommand() : this(string.Empty) { }
		public ChoiceCommand(string content) {
			Content = content;
		}
		public ChoiceCommand(int choiceId, string choiceJump, string choiceContent) {
			Content = $"{choiceId} {choiceJump} {choiceContent}";
		}

		#endregion

		#region IsCommand

		public int IsCommandPriority => 0;
		public bool IsCommand(string name, string content, string[] parameters) {
			return int.TryParse(name, out _);
		}

		#endregion
	}
}
