using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands.Sounds {
	public sealed class SoundInitCommand : SoundCommand {
		
		#region Constructors
		
		public SoundInitCommand() { }

		public SoundInitCommand(string content) : base(content) { }

		#endregion

		#region IsCommand

		protected override bool RequiresBankParameter => false;
		protected override bool HasFileParameter => false;
		protected override string CommandParameter => null;
		public override int IsCommandPriority => 1;
		public override bool IsCommand(string name, string content, string[] parameters) {
			return base.IsCommand(name, content, parameters) &&
				parameters.Length == (HasBankParameter(parameters) ? 2 : 1);
		}

		#endregion
	}
}
