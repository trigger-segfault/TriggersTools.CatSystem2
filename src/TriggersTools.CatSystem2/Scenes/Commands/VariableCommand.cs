using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets a variable.
	/// </summary>
	public sealed class VariableCommand : SceneCommand {
		#region Fields
		
		#endregion

		#region Constructors
		
		public VariableCommand(SceneLine catString) : base(catString) { }

		#endregion
	}
}
