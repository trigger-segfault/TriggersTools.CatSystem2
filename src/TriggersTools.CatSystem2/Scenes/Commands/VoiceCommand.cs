using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  A CatScene command that sets the character voice to play.
	/// </summary>
	public sealed class VoiceCommand : SceneCommand {
		#region Fields

		/// <summary>
		///  Gets the PCM voice file to load with the extensions.
		/// </summary>
		public string VoiceFile => $"{Parameters.Skip(1).First(p => char.IsUpper(p[0]))}.ogg";
		/// <summary>
		///  Gets if a PCM voice is actually present.
		/// </summary>
		public bool HasVoice => Parameters.Skip(1).Any(p => char.IsUpper(p[0]));

		#endregion

		#region Constructors
		
		public VoiceCommand(SceneLine catString) : base(catString) { }

		#endregion
	}
}
