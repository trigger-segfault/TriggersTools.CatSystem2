using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	public sealed class NovelCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { "novel" };

		#endregion

		#region Fields
		
		/// <summary>
		///  Gets the state of the novel mode being set, on or off.
		/// </summary>
		public bool State {
			get {
				string state = Parameters[1].ToLower();
				if (int.TryParse(state, out int boolean)) {
					switch (boolean) {
					case 0: return false;
					case 1: return true;
					default: throw new Exception();
					}
				}
				else {
					switch (state) {
					case "off": return false;
					case "on": return true;
					default: throw new Exception();
					}
				}
			}
		}

		#endregion

		#region Constructors

		public NovelCommand() : base(commandNames) { }
		public NovelCommand(string content) : base(content, commandNames) { }

		#endregion
	}
}
