using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes;

namespace TriggersTools.CatSystem2 {
	partial class SceneScript {
		#region Constants

		private const string ChoicePattern = @"(?'index'\d+)[ \t]+(?'goto'\w+)[ \t]+(?'text'.*)";
		private static readonly Regex ChoiceRegex = new Regex(ChoicePattern);

		#endregion

		#region HumanReadable (From File)

		/// <summary>
		///  Loads and decompiles the CST scene script file and returns the human readable script as a string.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <returns>The human readable script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> is null.
		/// </exception>
		public static string HumanReadable(string cstFile) {
			return Extract(cstFile).HumanReadable();
		}
		/// <summary>
		///  Loads and decompiles the CST scene script file and outputs it to the specified human readable file.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <param name="outFile">The output file to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void HumanReadableToFile(string cstFile, string outFile) {
			Extract(cstFile).HumanReadableToFile(outFile);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script file and outputs it to the specified human readable stream.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <param name="outStream">The output stream to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void HumanReadableToStream(string cstFile, Stream outStream) {
			Extract(cstFile).HumanReadableToStream(outStream);
		}

		#endregion

		#region HumanReadable (From Stream)

		/// <summary>
		///  Loads and decompiles the CST scene script stream and returns the human readable script as a string.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
		/// <returns>The human readable script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static string HumanReadable(Stream stream, string fileName) {
			return Extract(stream, fileName).HumanReadable();
		}
		/// <summary>
		///  Loads and decompiles the CST scene script stream and outputs it to the specified human readable file.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
		/// <param name="outFile">The output file to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void HumanReadableToFile(Stream inStream, string fileName, string outFile) {
			Extract(inStream, fileName).HumanReadableToFile(outFile);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script stream and outputs it to the specified human readable stream.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
		/// <param name="outStream">The output stream to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void HumanReadableToStream(Stream inStream, string fileName, Stream outStream) {
			Extract(inStream, fileName).HumanReadableToStream(outStream);
		}

		#endregion

		#region HumanReadable (Instance)

		/// <summary>
		///  Decompiles the CST scene script and returns the human readable script as a string.
		/// </summary>
		/// <returns>The human readable script.</returns>
		public string HumanReadable() {
			using (StringWriter writer = new StringWriter()) {
				HumanReadableInternal(writer);
				return writer.ToString();
			}
		}
		/// <summary>
		///  Decompiles the CST scene script and outputs it to the specified human readable file.
		/// </summary>
		/// <param name="outFile">The output file to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outFile"/> is null.
		/// </exception>
		public void HumanReadableToFile(string outFile) {
			using (StreamWriter writer = File.CreateText(outFile))
				HumanReadableInternal(writer);
		}
		/// <summary>
		///  Decompiles the CST scene script and outputs it to the specified human readable stream.
		/// </summary>
		/// <param name="outStream">The output stream to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outStream"/> is null.
		/// </exception>
		public void HumanReadableToStream(Stream outStream) {
			using (StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8))
				HumanReadableInternal(writer);
		}

		#endregion

		#region HumanReadable (Internal)

		/// <summary>
		///  Decompiles the CST scene script and writes it to the human readable text writer.
		/// </summary>
		/// <param name="writer">The text writer to write the human readable script to.</param>
		private void HumanReadableInternal(TextWriter writer) {
			// Header must state the output .cst script file name
			writer.WriteLine($"#{Path.GetFileNameWithoutExtension(FileName)}");
			writer.WriteLine();

			bool isChoice = false;

			List<string> choices = new List<string>();

			StringBuilder name = new StringBuilder();
			StringBuilder message = new StringBuilder();

			bool firstInput = true;

			void FlushText() {
				if (name.Length != 0 || message.Length != 0 || isChoice) {
					if (firstInput)
						firstInput = false;
					else
						writer.WriteLine();

					// The first replace prevents duplicate carraige returns
					name.Replace("\n\r", "\n").Replace("\n", "\n\r");
					message.Replace("\n\r", "\n").Replace("\n", "\n\r");

					if (!isChoice && name.Length != 0)
						writer.WriteLine(name);
					if (message.Length != 0)
						writer.WriteLine(message);
					if (isChoice) {
						if (message.Length != 0)
							writer.WriteLine();
						writer.WriteLine("Choice:");
						foreach (string choice in choices) {
							writer.WriteLine($"• {choice}");
						}
					}
				}
				message.Clear();
				name.Clear();
				choices.Clear();
				isChoice = false;
			}

			for (int i = 0; i < Count; i++) {
				ISceneLine line = Lines[i];
				switch (line.Type) {
				case SceneLineType.Command:
					Match match;
					if (line.Content == "fselect") {
						FlushText();
						isChoice = true;
					}
					else if (isChoice && (match = ChoiceRegex.Match(line.Content)).Success) {
						//writer.WriteLine($"• {match.Groups["text"].Value}");
						choices.Add(CatUtils.UnescapeString(match.Groups["text"].Value));
					}
					break;
				case SceneLineType.Message:
					message.Append(CatUtils.UnescapeMessage(line.Content, false));
					break;
				case SceneLineType.Name:
					name.Append(CatUtils.UnescapeMessage(line.Content, true));
					break;
				case SceneLineType.Input:
					FlushText();
					break;
				case SceneLineType.Page:
					FlushText();
					writer.WriteLine();
					writer.WriteLine();
					break;
				}
			}

			FlushText();

			writer.Flush();
		}

		#endregion
	}
	partial class KifintEntryExtensions {
		#region HumanReadableScene

		/// <summary>
		///  Loads and decompiles the CST scene script entry and returns the human readable script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The human readable script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static string HumanReadableScene(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return SceneScript.HumanReadable(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified human readable file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outFile">The output file to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void HumanReadableSceneToFile(this KifintEntry entry, string outFile) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				SceneScript.HumanReadableToFile(stream, entry.FileName, outFile);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified human readable stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outStream">The output stream to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void HumanReadableSceneToStream(this KifintEntry entry, Stream outStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				SceneScript.HumanReadableToStream(stream, entry.FileName, outStream);
		}

		#endregion

		#region HumanReadableScene (KifintStream)

		/// <summary>
		///  Loads and decompiles the CST scene script entry and returns the human readable script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The human readable script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static string HumanReadableScene(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return SceneScript.HumanReadable(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified human readable file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outFile">The output file to write the human readable script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void HumanReadableSceneToFile(this KifintEntry entry, KifintStream kifintStream,
			string outFile)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				SceneScript.HumanReadableToFile(stream, entry.FileName, outFile);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified human readable stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outStream">The output stream to write the human readable script to.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void HumanReadableSceneToStream(this KifintEntry entry, KifintStream kifintStream,
			Stream outStream)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				SceneScript.HumanReadableToStream(stream, entry.FileName, outStream);
		}

		#endregion
	}
}
