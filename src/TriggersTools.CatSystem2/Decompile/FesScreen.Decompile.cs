using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.CatSystem2 {
	partial class FesScreen {
		#region Constants

		/// <summary>
		///  Gets the header types that should always have a indent level of zero.
		/// </summary>
		private static readonly IReadOnlyList<string> ObjectHeaders = new string[] {
			"OBJECT",
			"KEYBLOCK",
		};
		private const string DefineHeader = "DEFINE";
		private const string MacroHeader = "MACRO";
		private static readonly string LinePattern =
			$@"^(?'hdr'#(" +
				$@"(?'object'(?:{string.Join("|", ObjectHeaders)})(?:[ \t]|$))" +
				$@"|" +
				$@"(?'define'{DefineHeader})" +
				$@"|" +
				$@"(?'macro'{MacroHeader})" +
			$@")?)" +
			//@"|(?'if'if[ \t]*\(.+?\)[ \t]*(?'inline'.+)?$)" +
			@"|(?'if'if[ \t]*\(.+\)[ \t]*$)" +
			@"|(?'else'else[ \t]*$)" +
			@"|(?'endif'endif[ \t]*$)";
		private static readonly Regex LineRegex = new Regex(LinePattern, RegexOptions.IgnoreCase);

		#endregion

		#region Decompile (From File)

		/// <summary>
		///  Loads and decompiles the FES script script file and returns the script as a string.
		/// </summary>
		/// <param name="fesFile">The file path to the FES script script file to extract.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fesFile"/> is null.
		/// </exception>
		public static string Decompile(string fesFile) {
			return Extract(fesFile).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the FES script script file and outputs it to the specified file.
		/// </summary>
		/// <param name="fesFile">The file path to the FES script script file to extract.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fesFile"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileToFile(string fesFile, string outFile, Encoding encoding = null) {
			Extract(fesFile).DecompileToFile(outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the FES script script file and outputs it to the specified stream.
		/// </summary>
		/// <param name="fesFile">The file path to the FES script script file to extract.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fesFile"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileToStream(string fesFile, Stream outStream, Encoding encoding = null) {
			Extract(fesFile).DecompileToStream(outStream, encoding);
		}

		#endregion

		#region Decompile (From Stream)

		/// <summary>
		///  Loads and decompiles the FES script script stream and returns the script as a string.
		/// </summary>
		/// <param name="stream">The stream to extract the script script from.</param>
		/// <param name="fileName">The path or name of the script script file being extracted.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static string Decompile(Stream stream, string fileName) {
			return Extract(stream, fileName).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the FES script script stream and outputs it to the specified file.
		/// </summary>
		/// <param name="stream">The stream to extract the script script from.</param>
		/// <param name="fileName">The path or name of the script script file being extracted.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileToFile(Stream inStream, string fileName, string outFile, Encoding encoding = null)
		{
			Extract(inStream, fileName).DecompileToFile(outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the FES script script stream and outputs it to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to extract the script script from.</param>
		/// <param name="fileName">The path or name of the script script file being extracted.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileToStream(Stream inStream, string fileName, Stream outStream,
			Encoding encoding = null)
		{
			Extract(inStream, fileName).DecompileToStream(outStream, encoding);
		}

		#endregion

		#region Decompile (Instance)

		/// <summary>
		///  Decompiles the FES script script and returns the script as a string.
		/// </summary>
		/// <returns>The decompiled script.</returns>
		public string Decompile() {
			using (StringWriter writer = new StringWriter()) {
				DecompileInternal(writer);
				return writer.ToString();
			}
		}
		/// <summary>
		///  Decompiles the FES script script and outputs it to the specified file.
		/// </summary>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outFile"/> is null.
		/// </exception>
		public void DecompileToFile(string outFile, Encoding encoding = null) {
			using (StreamWriter writer = new StreamWriter(outFile, false, encoding ?? CatUtils.ShiftJIS))
				DecompileInternal(writer);
		}
		/// <summary>
		///  Decompiles the FES script script and outputs it to the specified stream.
		/// </summary>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outStream"/> is null.
		/// </exception>
		public void DecompileToStream(Stream outStream, Encoding encoding = null) {
			using (StreamWriter writer = new StreamWriter(outStream, encoding ?? CatUtils.ShiftJIS))
				DecompileInternal(writer);
		}

		#endregion

		#region Decompile (Internal)

		private int[] ReadSpacing(int index, int maxParts) {
			List<int> objectSpacing = new List<int>();
			Match match;
			do {
				int[] spacing = ReadSpacing(Lines[index++], maxParts);
				for (int i = 0; i < spacing.Length; i++) {
					if (i == objectSpacing.Count)
						objectSpacing.Add(spacing[i]);
					else
						objectSpacing[i] = Math.Max(objectSpacing[i], spacing[i]);
				}
			} while (!(match = LineRegex.Match(Lines[index])).Groups["hdr"].Success);
			return objectSpacing.ToArray();
		}
		private int[] ReadSpacing(string line, int maxParts) {
			string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			int length = Math.Min(parts.Length, (maxParts != -1 ? maxParts : int.MaxValue));
			int[] spacing = new int[length];
			for (int i = 0; i < spacing.Length; i++)
				// Spacing should leave at least 1 space of room
				spacing[i] = MathUtils.Pad(parts[i].Length + 1, 4);
			return spacing;
		}
		private void WriteSpaced(string line, int[] spacing, TextWriter writer) {
			string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < parts.Length; i++) {
				string part = parts[i];
				bool lastPart = (i + 1 == parts.Length);
				if (i < spacing.Length && !lastPart) {
					int tabs = MathUtils.Pad(spacing[i] - part.Length, 4) / 4;
					writer.Write(part);
					writer.Write(new string('\t', tabs));
				}
				else {
					writer.Write(part);
					if (i + 1 < parts.Length)
						writer.Write(' ');
				}
			}
			writer.WriteLine();
		}

		/// <summary>
		///  Decompiles the FES screen script and writes it to the text writer.
		/// </summary>
		/// <param name="writer">The text writer to write the decompiled script to.</param>
		private void DecompileInternal(TextWriter writer) {
			bool isSpaced = false; // Forces the tag level to be one lower, (should really always be zero)
			bool isObject = false;
			int[] spacing = null;
			int level = 1; // The indent level of the text

			for (int i = 0; i < Count; i++) {
				string line = Lines[i];
				Match match = LineRegex.Match(line);
				if (match.Groups["hdr"].Success) {
					if (i != 0)
						writer.WriteLine();
					isSpaced = (match.Groups["object"].Success ||
								match.Groups["define"].Success ||
								match.Groups["macro"].Success);
					if (isSpaced) {
						isObject = match.Groups["object"].Success;
						int maxParts = (match.Groups["macro"].Success ? 1 : -1);
						spacing = ReadSpacing((isObject ? i : (i + 1)), maxParts);
					}
					else {
						isObject = false;
						spacing = null;
					}
					if (isObject)
						WriteSpaced(line, spacing, writer);
					else
						writer.WriteLine(line);

					level = 1;
				}
				else {
					if (match.Groups["endif"].Success || match.Groups["else"].Success)
						level = Math.Max(1, level - 1);
					
					writer.Write(new string('\t', level - (isObject ? 1 : 0)));

					// Try to inline if statements
					if (match.Groups["if"].Success && i + 2 < Count) {
						Match statement = LineRegex.Match(Lines[i + 1]);
						Match endif = LineRegex.Match(Lines[i + 2]);
						// Is the next line a statement and after that an endif?
						if (!statement.Groups["if"].Success && !statement.Groups["else"].Success &&
							!statement.Groups["endif"].Success && !statement.Groups["hdr"].Success &&
							endif.Groups["endif"].Success)
						{
							// Skip the endif and write the statement on the same line as the if
							writer.Write(line);
							writer.Write(' ');
							line = Lines[i + 1];
							if (isSpaced)
								WriteSpaced(line, spacing, writer);
							else
								writer.WriteLine(line);
							// Ignore the statement and endif lines now
							i += 2;
							// Continue here, this will also avoid rewriting
							// the line and incrementing the level
							continue;
						}
					}
					if (isSpaced)
						WriteSpaced(line, spacing, writer);
					else
						writer.WriteLine(line);

					if (match.Groups["if"].Success || match.Groups["else"].Success)
						level++;
				}
			}
			writer.Flush();
		}

		#endregion
	}
	partial class KifintEntryExtensions {
		#region DecompileScreen

		/// <summary>
		///  Loads and decompiles the FES screen script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static string DecompileScreen(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return FesScreen.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the FES screen script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileScreenToFile(this KifintEntry entry, string outFile, Encoding encoding = null) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				FesScreen.DecompileToFile(stream, entry.FileName, outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the FES screen script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileScreenToStream(this KifintEntry entry, Stream outStream, Encoding encoding = null)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				FesScreen.DecompileToStream(stream, entry.FileName, outStream, encoding);
		}

		#endregion

		#region DecompileScreen (KifintStream)

		/// <summary>
		///  Loads and decompiles the FES screen script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static string DecompileScreen(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return FesScreen.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the FES screen script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileScreenToFile(this KifintEntry entry, KifintStream kifintStream, string outFile,
			Encoding encoding = null)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				FesScreen.DecompileToFile(stream, entry.FileName, outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the FES screen script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileScreenToStream(this KifintEntry entry, KifintStream kifintStream,
			Stream outStream, Encoding encoding = null)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				FesScreen.DecompileToStream(stream, entry.FileName, outStream, encoding);
		}

		#endregion
	}
}
