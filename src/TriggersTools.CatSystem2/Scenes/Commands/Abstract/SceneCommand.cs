using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Script;

namespace TriggersTools.CatSystem2.Scenes.Commands.Abstract {
	/// <summary>
	///  The immutable class for a CatScene <see cref="SceneLineType.Command"/>.
	/// </summary>
	public abstract class SceneCommand : SceneCommandBase, ISceneCommand {
		#region Constants

		private static readonly IReadOnlyList<string> NoParameters = Array.Empty<string>();

		#endregion

		#region Fields
		
		/*/// <summary>
		///  The cat string entry for this scene command.
		/// </summary>
		private SceneLine catString;*/
		/// <summary>
		///  Gets the parameters for the command, including the comamnd name.
		/// </summary>
		public IReadOnlyList<string> Parameters { get; private set; }

		private readonly string[] commandNames;

		private string content;

		#endregion

		#region Properties

		/*/// <summary>
		///  Gets the cat string entry for this scene command.
		/// </summary>
		public SceneLine CatString {
			get => catString;
			internal set {
				if (value == null)
					throw new ArgumentNullException(nameof(CatString));
				if (value.Type != SceneLineType.Command)
					throw new ArgumentException($"{nameof(CatString)} is not of type {nameof(SceneLineType.Command)}!");
				catString = value;
				Parameters = value.Content.Split(' ').ToImmutableArray();;
			}
		}*/
		/// <summary>
		///  Gets the unparsed content of the string command.
		/// </summary>
		public override string Content {
			get => content;
			set {
				content = value ?? throw new ArgumentNullException(nameof(content));
				if (content.Length == 0)
					Parameters = NoParameters;
				else
					Parameters = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
										.ToImmutableArray();
			}
		}
		/// <summary>
		///  Gets the name of the command. Returns <see cref="string.Empty"/> when there is no command.
		/// </summary>
		public string CommandName => (Parameters.Count != 0 ? Parameters[0] : string.Empty);
		/// <summary>
		///  Gets the number of parameters in the command.
		/// </summary>
		public int Count => Parameters.Count;
		
		#endregion

		#region Constructor

		/// <summary>
		///  Constructs the CatScene command from an existing string.
		/// </summary>
		/// <param name="content">The content command.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="content"/> is null.
		/// </exception>
		public SceneCommand(string[] commandNames) : this(string.Empty, commandNames) { }
		/// <summary>
		///  Constructs the CatScene command from an existing string.
		/// </summary>
		/// <param name="content">The content command.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="content"/> is null.
		/// </exception>
		public SceneCommand(string content, string[] commandNames) {
			this.commandNames = commandNames;
			Content = content;
		}
		/*/// <summary>
		///  Constructs the CatScene command from an existing cat string entry.
		/// </summary>
		/// <param name="catString">The CatScene string entry to construct the command from.</param>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="catString"/> does not have a type of <see cref="SceneLineType.Command"/>.
		/// </exception>
		public SceneCommand(SceneLine catString) {
			if (catString.Type != SceneLineType.Command)
				throw new ArgumentException($"{nameof(catString)} is not of type {nameof(SceneLineType.Command)}!");
			Content = CatString.Content;
			CatString = catString;
			Parameters = catString.Content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToImmutableArray();
		}*/

		#endregion

		#region IsCommand

		public virtual int IsCommandPriority => 0;
		public virtual bool IsCommand(string name, string content, string[] parameters) {
			return Array.IndexOf(commandNames, name) != -1;
		}

		#endregion
	}
}
