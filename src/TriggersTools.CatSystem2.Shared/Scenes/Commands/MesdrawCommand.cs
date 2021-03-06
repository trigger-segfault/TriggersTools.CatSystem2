﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands.Abstract;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	public sealed class MesdrawCommand : SceneCommand {
		#region Constants

		private static readonly string[] commandNames = { "mesdraw" };

		#endregion

		#region Fields
		
		/// <summary>
		///  Gets the state of the novel mode being set, on or off.
		/// </summary>
		public bool State {
			get {
				string state = Parameters[1];
				switch (int.Parse(state)) {
				case 0: return false;
				case 1: return true;
				default: throw new Exception();
				}
			}
		}

		#endregion

		#region Constructors

		public MesdrawCommand() : base(commandNames) { }
		public MesdrawCommand(string content) : base(content, commandNames) { }

		#endregion
	}
}
