using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;
using TriggersTools.CatSystem2.Scenes.Commands.Sounds;

namespace TriggersTools.CatSystem2.Scenes.Commands.Abstract {
	public abstract class SoundCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { "se", "bgm", "pcm" };

		#endregion

		#region Propreties

		public SoundType SoundType {
			get => (Count != 0 ? CommandNameAttribute.Parse<SoundType>(Parameters[0]) : Sounds.SoundType.None);
		}
		public int Bank {
			get {
				if (Count >= 2 && int.TryParse(Parameters[1], out int bank))
					return bank;
				return 0;
			}
		}
		public bool HasBank => (Count >= 2 && int.TryParse(Parameters[1], out _));

		#endregion

		#region Constructors

		public SoundCommand() : base(commandNames) { }
		public SoundCommand(string[] commandNames) : base(commandNames) { }

		public SoundCommand(string content) : base(content, commandNames) { }
		public SoundCommand(string content, string[] commandNames) : base(content, commandNames) { }

		#endregion

		#region IsBankParameter

		private static readonly string[] Unimplemented = {
			/*"loop", "pause", "replay",*/ "vol", "fade", "end", "base", "pos", "rot", /*"move",*/ "arc", "quake", "shake", "mrot",
		};

		protected static bool HasBankParameter(string[] parameters) {
			return (parameters.Length >= 2 && int.TryParse(parameters[1], out _));
		}

		#endregion

		#region IsCommand

		protected abstract bool RequiresBankParameter { get; }
		protected abstract bool HasFileParameter { get; }
		protected abstract string CommandParameter { get; }
		public override int IsCommandPriority => (CommandParameter != null ? 2 : 0);
		public override bool IsCommand(string name, string content, string[] parameters) {
			if (!base.IsCommand(name, content, parameters))
				return false;
			bool hasCommand = CommandParameter != null;
			bool hasBank = HasBankParameter(parameters);
			if (!hasBank && RequiresBankParameter)
				return false;
			int offset = (hasBank ? 0 : 1);
			if (hasCommand && (parameters.Length < 3 - offset ||
				!parameters[2 - offset].Equals(CommandParameter, StringComparison.InvariantCultureIgnoreCase)))
				return false;
			if (parameters.Length >= 3 - offset) {
				string cmd = parameters[2 - offset];
				if (Unimplemented.Any(c => cmd.Equals(c, StringComparison.InvariantCultureIgnoreCase)))
					return false;
			}
			return true;
		}

		#endregion
	}
}
