using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the name of the scene.
	/// </summary>
	public sealed class SceneTitleCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { "scene" };

		#endregion

		#region Fields

		/// <summary>
		///  Gets the PCM voice file to load with the extensions.
		/// </summary>
		public string Title => CatUtils.UnescapeString(Parameters[1]);

		#endregion

		#region Constructors

		public SceneTitleCommand() : base(commandNames) { }
		public SceneTitleCommand(string content) : base(content, commandNames) { }

		#endregion
	}
}
