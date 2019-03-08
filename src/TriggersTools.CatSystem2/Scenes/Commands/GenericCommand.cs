using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;
using TriggersTools.CatSystem2.Scenes.Script;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  The immutable class for a CatScene <see cref="SceneLineType.Command"/>.
	/// </summary>
	public sealed class GenericCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { };

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
		public GenericCommand() : base(commandNames) { }
		/// <summary>
		///  Constructs the CatScene command from an existing string.
		/// </summary>
		/// <param name="content">The content command.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="content"/> is null.
		/// </exception>
		public GenericCommand(string content) : base(content, commandNames) { }
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

		public override int IsCommandPriority => int.MinValue;
		public override bool IsCommand(string name, string content, string[] parameters) {
			return true;
		}

		#endregion
	}
}
