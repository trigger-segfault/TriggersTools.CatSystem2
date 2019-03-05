using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands.Sounds {
	public sealed class SoundReplayCommand : SoundCommand {
		#region Constructors
		
		public SoundReplayCommand() { }

		public SoundReplayCommand(string content) : base(content) { }

		#endregion

		#region IsCommand

		protected override bool RequiresBankParameter => false;
		protected override bool HasFileParameter => false;
		protected override string CommandParameter => "replay";

		#endregion
	}
}
