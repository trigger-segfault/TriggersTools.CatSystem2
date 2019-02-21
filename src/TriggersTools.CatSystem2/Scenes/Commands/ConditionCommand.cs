using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the cposition of a graphic.
	/// </summary>
	public sealed class ConditionCommand : SceneCommand {
		#region Fields
		
		#endregion

		#region Constructors
		
		public ConditionCommand(SceneLine catString) : base(catString) { }

		#endregion
	}
}
