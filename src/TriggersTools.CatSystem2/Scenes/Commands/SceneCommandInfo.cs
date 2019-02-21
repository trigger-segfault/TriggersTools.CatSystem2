using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	public delegate ISceneCommand CreateCatCommand(SceneLine catString);
	public delegate bool IsCatCommand(string commandName);
	public sealed class SceneCommandInfo {
		#region Properties

		/// <summary>
		///  Gets the name of the command.
		/// </summary>
		public string Id { get; internal set; }
		/// <summary>
		///  Gets the constructor for the CatScene command.
		/// </summary>
		internal CreateCatCommand Create { get; set; }
		/// <summary>
		///  Gets the delegate for matching of a CatScene command.
		/// </summary>
		internal IsCatCommand IsCatCommand { get; set; }
		/// <summary>
		///  Gets if this command requires the use of <see cref="IsCommand"/> to match the command.
		/// </summary>
		public bool IsSpecialCommand { get; internal set; }

		#endregion

		#region Constructors

		public SceneCommandInfo() {
			IsCatCommand = IsCatCommandDefault;
		}

		#endregion

		#region IsCommand
		
		/// <summary>
		///  Gets if the specified command name matches this command type.
		/// </summary>
		/// <param name="command">The command name to check with.</param>
		/// <returns>True if the command is of this type.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="command"/> is null.
		/// </exception>
		public bool IsCommand(string command) {
			if (command == null)
				throw new ArgumentNullException(nameof(command));
			return IsCatCommand(command);
		}
		private bool IsCatCommandDefault(string commandName) {
			return commandName == Id;
		}

		#endregion
	}
}
