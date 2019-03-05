using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands.Sounds {
	/// <summary>
	///  A CatScene command that sets the character voice to play.
	/// </summary>
	public sealed class SoundMoveCommand : SoundCommand {
		#region Properties
		
		
		#endregion

		#region Constructors
		
		public SoundMoveCommand() { }

		public SoundMoveCommand(string content) : base(content) { }

		#endregion

		#region IsCommand
		
		protected override bool RequiresBankParameter => false;
		protected override bool HasFileParameter => false;
		protected override string CommandParameter => "move";
		
		#endregion
	}
}
