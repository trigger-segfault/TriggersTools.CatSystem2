using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Scenes.Commands;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Scenes;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A CatSystem2 engine scene script CST file.
	/// </summary>
	[JsonObject]
	public sealed partial class SceneScript : IReadOnlyCollection<ISceneLine> {
		#region Fields

		/// <summary>
		///  Gets the file name of the scene script.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  Gets the list of command lines in the scene script.
		/// </summary>
		[JsonIgnore]
		public IReadOnlyList<ISceneLine> Lines { get; private set; }

		[JsonProperty("lines")]
		private IReadOnlyList<SceneLine> IOLines {
			get => Array.AsReadOnly(Lines.Select(l => new SceneLine(l)).ToArray());
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(IOLines));
				ISceneLine[] newLines = new ISceneLine[value.Count];
				for (int i = 0; i < value.Count; i++)
					newLines[i] = SceneUtils.CreateLine((SceneLineType) value[i].Type, value[i].Content);
				Lines = Array.AsReadOnly(newLines);
			}
		}

		#endregion

		#region Properties
		
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
		internal SceneScript(string fileName, SCRIPTLINE[] lines) {
			FileName = Path.GetFileName(fileName);
			ISceneLine[] newLines = new ISceneLine[lines.Length];
			for (int i = 0; i < lines.Length; i++)
				newLines[i] = SceneUtils.CreateLine((SceneLineType) lines[i].Type, lines[i].Content);
			Lines = Array.AsReadOnly(newLines);
		}

		#endregion

		/*#region Public Helpers

		public string GetSceneTitle() {
			for (int i = 0; i < Lines.Count; i++) {
				SceneLine str = Lines[i];
				if (str.Type == SceneLineType.Command) {
					ISceneCommand cmd = str.Command;
					if (cmd is SceneTitleCommand titleCmd)
						return titleCmd.Title;
					if (cmd is StringCommand stringCmd && stringCmd.Index == 155)
						return stringCmd.String;
				}
			}
			return null;
		}

		#endregion*/

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
		public IEnumerator<ISceneLine> GetEnumerator() => Lines.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region Private Classes

		/// <summary>
		///  A string line in a <see cref="SceneScript"/> file.
		/// </summary>
		private sealed class SceneLine {
			#region Fields

			/// <summary>
			///  Gets the purpose of the scene line.
			/// </summary>
			[JsonProperty("type")]
			public SceneLineType Type { get; private set; }
			/// <summary>
			///  Gets the string content of the scene line.
			/// </summary>
			[JsonProperty("content")]
			public string Content { get; private set; }

			#endregion

			#region Constructors

			/// <summary>
			///  Constructs an unassigned cat string entry for use with loading via <see cref="Newtonsoft.Json"/>.
			/// </summary>
			public SceneLine() { }

			internal SceneLine(ISceneLine line) {
				Type = line.Type;
				Content = line.Content;
			}

			#endregion
		}

		#endregion
	}
}
