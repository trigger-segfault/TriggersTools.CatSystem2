using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Script;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  The immutable class for a CatScene <see cref="SceneLineType.Command"/>.
	/// </summary>
	public class SceneCommand : ISceneCommand {
		#region Fields

		/// <summary>
		///  The cat string entry for this scene command.
		/// </summary>
		private SceneLine catString;
		/// <summary>
		///  Gets the parameters for the command, including the comamnd name..
		/// </summary>
		public IReadOnlyList<string> Parameters { get; private set; }

		#endregion

		#region Properties

		/// <summary>
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
				Parameters = Array.AsReadOnly(value.Content.Split(' '));
			}
		}
		/// <summary>
		///  Gets the unparsed content of the string command.
		/// </summary>
		public string Content => CatString.Content;
		/// <summary>
		///  Gets the name of the command. Returns <see cref="string.Empty"/> when there is no command.
		/// </summary>
		public string Command => (Parameters.Count != 0 ? Parameters[0] : string.Empty);
		/// <summary>
		///  Gets the number of parameters in the command.
		/// </summary>
		public int Count => Parameters.Count;

		#endregion

		#region Constructor
		
		/// <summary>
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
			CatString = catString;
			Parameters = Array.AsReadOnly(catString.Content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		}

		#endregion

		#region Execute

		public void Execute(SceneInstance scene) {

		}

		#endregion
	}
}
