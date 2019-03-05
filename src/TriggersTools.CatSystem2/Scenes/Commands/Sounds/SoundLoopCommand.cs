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
	public sealed class SoundLoopCommand : SoundCommand {
		#region Constants
		
		private static readonly string[] commandNames = { "se" };

		#endregion

		#region Fields

		public string Sound => Parameters[3];
		public string SoundFile => $"{Sound}.ogg";

		public int X => (Count >= 5 && int.TryParse(Parameters[4], out int x) ? x : 0);
		public int Y => (Count >= 6 && int.TryParse(Parameters[5], out int y) ? y : 0);
		public int Z => (Count >= 7 && int.TryParse(Parameters[6], out int z) ? z : 0);

		#endregion

		#region Constructors

		public SoundLoopCommand() : base(commandNames) { }

		public SoundLoopCommand(string content) : base(content, commandNames) { }

		#endregion

		#region IsCommand

		protected override bool RequiresBankParameter => true;
		protected override bool HasFileParameter => true;
		protected override string CommandParameter => "loop";

		#endregion
	}
}
