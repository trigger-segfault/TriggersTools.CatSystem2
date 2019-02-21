using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the name of the scene.
	/// </summary>
	public sealed class SceneTitleCommand : SceneCommand {
		#region Fields

		/// <summary>
		///  Gets the PCM voice file to load with the extensions.
		/// </summary>
		public string Title => CatUtils.UnescapeString(Parameters[1]);

		#endregion

		#region Constructors
		
		public SceneTitleCommand(SceneLine catString) : base(catString) { }

		#endregion
	}
}
