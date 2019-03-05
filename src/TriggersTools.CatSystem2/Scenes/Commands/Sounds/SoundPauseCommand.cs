using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands.Sounds {
	public sealed class SoundPauseCommand : SoundCommand {
		#region Constructors
		
		public SoundPauseCommand() { }

		public SoundPauseCommand(string content) : base(content) { }

		#endregion

		#region IsCommand

		protected override bool RequiresBankParameter => true;
		protected override bool HasFileParameter => false;
		protected override string CommandParameter => "pause";

		#endregion
	}
}
