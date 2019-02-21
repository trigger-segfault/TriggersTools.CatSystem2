using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Scenes.Commands;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A CatSystem2 engine scene script CST file.
	/// </summary>
	[JsonObject]
	public sealed partial class SceneScript : IReadOnlyCollection<SceneLine> {
		#region Fields

		/// <summary>
		///  Gets the file name of the scene script.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  The list of command lines in the scene script.
		/// </summary>
		[JsonIgnore]
		private IReadOnlyList<SceneLine> lines;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the list of command lines in the scene script.
		/// </summary>
		[JsonProperty("lines")]
		public IReadOnlyList<SceneLine> Lines {
			get => lines;
			private set {
				lines = value;
				for (int i = 0; i < lines.Count; i++) {
					lines[i].Scene = this;
				}
			}
		}
		/// <summary>
		///  Gets the number of command lines in the scene script.
		/// </summary>
		[JsonIgnore]
		public int Count => Lines.Count;

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned scene script for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public SceneScript() { }
		/// <summary>
		///  Constructs the CST scene script from the specified file name and lines.
		/// </summary>
		/// <param name="fileName">The file name of the CST scene script with the .cst extension.</param>
		/// <param name="lines">The SCENELINE struct array containing the scene line commands.</param>
		internal SceneScript(string fileName, SCENELINE[] lines) {
			FileName = Path.GetFileName(fileName);
			SceneLine[] newStrings = new SceneLine[lines.Length];
			for (int i = 0; i < lines.Length; i++) {
				newStrings[i] = new SceneLine(lines[i], this);
			}
			this.lines = Array.AsReadOnly(newStrings);
		}

		#endregion

		#region Public Helpers

		public string GetSceneTitle() {
			for (int i = 0; i < lines.Count; i++) {
				SceneLine str = lines[i];
				if (str.Type == SceneLineType.Command) {
					string commandName = str.CommandName;
					if (SceneCommands.SceneTitle.IsCommand(commandName))
						return ((SceneTitleCommand) str.Command).Title;
					if (SceneCommands.String.IsCommand(commandName)) {
						StringCommand command = (StringCommand) str.Command;
						if (command.Index == 155)
							return command.String;
					}
				}
			}
			return null;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the scene script.
		/// </summary>
		/// <returns>The string representation of the scene script.</returns>
		public override string ToString() => $"Scene: \"{FileName}\" Lines={Count}";

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the scene script's command lines.
		/// </summary>
		/// <returns>The scene script's command lines enumerator.</returns>
		public IEnumerator<SceneLine> GetEnumerator() => Lines.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
