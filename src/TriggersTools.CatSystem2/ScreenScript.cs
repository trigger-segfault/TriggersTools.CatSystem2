using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A CatSystem2 engine screen script FES file.
	/// </summary>
	[JsonObject]
	public partial class ScreenScript : IReadOnlyCollection<string> {
		#region Fields

		/// <summary>
		///  Gets the file name of the screen script.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; }
		/// <summary>
		///  Gets the list of command lines in the screen script.
		/// </summary>
		[JsonProperty("lines")]
		public IReadOnlyList<string> Lines { get; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of command lines in the screen script.
		/// </summary>
		[JsonIgnore]
		public int Count => Lines.Count;

		#endregion

		#region Constructor

		/// <summary>
		///  Constructs an unassigned screen script for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public ScreenScript() { }
		/// <summary>
		///  Constructs the FES screen script from the specified file name and lines.
		/// </summary>
		/// <param name="fileName">The file name of the FES screen script with the .fes extension.</param>
		/// <param name="lines">The string array containing the screen line commands.</param>
		internal ScreenScript(string fileName, string[] lines) {
			FileName = Path.GetFileName(fileName);
			Lines = Array.AsReadOnly(lines);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the screen script.
		/// </summary>
		/// <returns>The string representation of the screen script.</returns>
		public override string ToString() => $"Screen: \"{FileName}\" Lines={Count}";

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the screen script's command lines.
		/// </summary>
		/// <returns>The screen script's command lines enumerator.</returns>
		public IEnumerator<string> GetEnumerator() => Lines.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
