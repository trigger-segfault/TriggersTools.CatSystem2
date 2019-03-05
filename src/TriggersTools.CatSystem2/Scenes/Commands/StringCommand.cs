using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the indexed string of the scene.
	/// </summary>
	public sealed class StringCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { "str" };

		#endregion

		#region Fields

		/// <summary>
		///  Gets the index of the string being set by the command.
		/// </summary>
		public int Index => int.Parse(Parameters[1]);
		/// <summary>
		///  Gets the PCM voice file to load with the extensions.
		/// </summary>
		public string String => CatUtils.UnescapeString(Parameters[2]);

		#endregion

		#region Constructors

		public StringCommand() : base(commandNames) { }
		public StringCommand(string content) : base(content, commandNames) { }

		#endregion
	}
}
