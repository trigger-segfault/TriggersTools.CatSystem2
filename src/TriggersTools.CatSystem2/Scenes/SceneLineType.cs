using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;

namespace TriggersTools.CatSystem2.Scenes {
	/// <summary>
	///  The purpose of the string entry for a <see cref="SceneLine"/>.
	/// </summary>
	public enum SceneLineType : ushort {
		/// <summary>
		///  Waits for user input to continue the scene. The string entry for this type is empty. (513)
		/// </summary>
		[CommandName("input")]
		Input = 0x0201,
		/// <summary>Page break in novel view. (769) Also acts as <see cref="Input"/>.</summary>
		[CommandName("page")]
		Page = 0x0301,
		/// <summary>Changes the current dialog. (8193)</summary>
		[CommandName("message")]
		Message = 0x2001,
		/// <summary>Changes the current character name. (8449)</summary>
		[CommandName("name")]
		Name = 0x2101,
		/// <summary>Runs a command to change the scene in some way. (12289)</summary>
		[CommandName("command")]
		Command = 0x3001,
	}
}
