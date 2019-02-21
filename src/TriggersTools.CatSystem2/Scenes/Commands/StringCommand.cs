using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the indexed string of the scene.
	/// </summary>
	public sealed class StringCommand : SceneCommand {
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
		
		public StringCommand(SceneLine catString) : base(catString) { }

		#endregion
	}
}
