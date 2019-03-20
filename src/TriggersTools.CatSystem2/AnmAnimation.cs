using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  An animation loaded from an ANM animation script.
	/// </summary>
	[JsonObject]
	public sealed partial class AnmAnimation : IReadOnlyCollection<AnmLine> {
		#region Fields

		/// <summary>
		///  Gets the file name for the ANM animation script.
		/// </summary>
		[JsonProperty("file_name")]
		public string FileName { get; private set; }
		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		[JsonProperty("unknown")]
		public int Unknown { get; private set; }
		/// <summary>
		///  Gets the whole list of lines for the animation script.
		/// </summary>
		[JsonProperty("lines")]
		public IReadOnlyList<AnmLine> Lines { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of script lines in the animation.
		/// </summary>
		[JsonIgnore]
		public int Count => Lines.Count;
		/// <summary>
		///  Gets the name of the file for loading the <see cref="AnmAnimation"/> data.
		/// </summary>
		[JsonIgnore]
		public string JsonFileName => GetJsonFileName(FileName);

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned ANM animation script for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		private AnmAnimation() { }
		/// <summary>
		///  Constructs an ANM animation script with the specified file name, header, and script lines.
		/// </summary>
		/// <param name="fileName">The file name of the ANM animation with the .anm extension.</param>
		/// <param name="hdr">The ANMHDR containing extra information on the ANM animation.</param>
		/// <param name="lines">The ANMFRM struct array containing script line information.</param>
		private AnmAnimation(string fileName, ANMHDR hdr, ANMLINE[] lines) {
			FileName = Path.GetFileName(fileName);
			Unknown = hdr.Unknown;
			AnmLine[] newLines = new AnmLine[lines.Length];
			for (int i = 0; i < lines.Length; i++)
				newLines[i] = new AnmLine(lines[i]);
			Lines = newLines.ToImmutableArray();
		}

		#endregion

		#region Helpers

		/// <summary>
		///  Gets the file name for the JSON ANM animation information.
		/// </summary>
		/// <param name="fileName">The base file name of the <see cref="AnmAnimation"/>.</param>
		/// <returns>The file name of the JSON ANM animation information.</returns>
		public static string GetJsonFileName(string fileName) {
			return Path.ChangeExtension(Path.GetFileNameWithoutExtension(fileName) + "+anm", ".json");
		}
		/// <summary>
		///  Gets the file path for the JSON ANM animation information.
		/// </summary>
		/// <param name="directory">The directory of the <see cref="AnmAnimation"/>.</param>
		/// <param name="fileName">The base file name of the <see cref="AnmAnimation"/>.</param>
		/// <returns>The file path of the JSON ANM animation information.</returns>
		public static string GetJsonFilePath(string directory, string fileName) {
			return Path.Combine(directory, GetJsonFileName(fileName));
		}

		#endregion

		#region I/O


		/// <summary>
		///  Deserializes the ANM animation from a json file in the specified directory and file name.
		/// </summary>
		/// <param name="directory">
		///  The directory for the json file to load and deserialize with <paramref name="fileName"/>.
		/// </param>
		/// <returns>The deserialized ANM animation.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static AnmAnimation FromJsonDirectory(string directory, string fileName) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			string jsonFile = Path.Combine(directory, GetJsonFileName(fileName));
			return JsonConvert.DeserializeObject<AnmAnimation>(File.ReadAllText(jsonFile));
		}
		/// <summary>
		///  Serializes the ANM animation to a json file in the specified directory.
		/// </summary>
		/// <param name="directory">The directory to save the json file to using <see cref="JsonFileName"/>.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="directory"/> is null.
		/// </exception>
		public void SaveJsonToDirectory(string directory) {
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));
			string jsonFile = Path.Combine(directory, JsonFileName);
			File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ANM animation.
		/// </summary>
		/// <returns>The ANM animation's string representation.</returns>
		public override string ToString() => $"Animation \"{FileName}\" Lines={Lines.Count}";

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the animation script's lines.
		/// </summary>
		/// <returns>The animation script's line enumerator.</returns>
		public IEnumerator<AnmLine> GetEnumerator() => Lines.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
