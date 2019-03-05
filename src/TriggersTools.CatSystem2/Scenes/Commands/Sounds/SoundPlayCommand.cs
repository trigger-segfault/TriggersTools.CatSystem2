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
	public sealed class SoundPlayCommand : SoundCommand {
		#region Properties
		
		public string Sound => Parameters[(HasBank ? 2 : 1)];
		public string SoundFile => $"{Sound}.ogg";
		
		public int Pan {
			get {
				int offset = (HasBank ? 0 : 1);
				if (Count >= 4 - offset && int.TryParse(Parameters[3 - offset], out int pan))
					return pan;
				return 0;
			}
		}
		public int Delay {
			get {
				int offset = (HasBank ? 0 : 1);
				if (Count >= 5 - offset && int.TryParse(Parameters[4 - offset], out int pan))
					return pan;
				return 0;
			}
		}

		#endregion

		#region Constructors

		public SoundPlayCommand() { }

		public SoundPlayCommand(string content) : base(content) { }

		#endregion

		#region IsCommand

		protected override bool RequiresBankParameter => false;
		protected override bool HasFileParameter => true;
		protected override string CommandParameter => null;
		public override int IsCommandPriority => 0;

		#endregion
	}
}
