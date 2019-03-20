using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TriggersTools.CatSystem2 {
	partial class AnmAnimation {
		#region Decompile (From File)

		/// <summary>
		///  Loads and decompiles the ANM animation script file and returns the script as a string.
		/// </summary>
		/// <param name="anmFile">The file path to the ANM animation script file to extract.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="anmFile"/> is null.
		/// </exception>
		public static string Decompile(string anmFile) {
			return Extract(anmFile).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script file and outputs it to the specified file.
		/// </summary>
		/// <param name="anmFile">The file path to the ANM animation script file to extract.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="anmFile"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileToFile(string anmFile, string outFile) {
			Extract(anmFile).DecompileToFile(outFile);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script file and outputs it to the specified stream.
		/// </summary>
		/// <param name="anmFile">The file path to the ANM animation script file to extract.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="anmFile"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileToStream(string anmFile, Stream outStream) {
			Extract(anmFile).DecompileToStream(outStream);
		}

		#endregion

		#region Decompile (From Stream)

		/// <summary>
		///  Loads and decompiles the ANM animation script stream and returns the script as a string.
		/// </summary>
		/// <param name="stream">The stream to extract the animation script from.</param>
		/// <param name="fileName">The path or name of the animation script file being extracted.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static string Decompile(Stream stream, string fileName) {
			return Extract(stream, fileName).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script stream and outputs it to the specified file.
		/// </summary>
		/// <param name="stream">The stream to extract the animation script from.</param>
		/// <param name="fileName">The path or name of the animation script file being extracted.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileToFile(Stream inStream, string fileName, string outFile) {
			Extract(inStream, fileName).DecompileToFile(outFile);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script stream and outputs it to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to extract the animation script from.</param>
		/// <param name="fileName">The path or name of the animation script file being extracted.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/>, <paramref name="fileName"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileToStream(Stream inStream, string fileName, Stream outStream) {
			Extract(inStream, fileName).DecompileToStream(outStream);
		}

		#endregion

		#region Decompile (Instance)

		/// <summary>
		///  Decompiles the ANM animation script and returns the script as a string.
		/// </summary>
		/// <returns>The decompiled script.</returns>
		public string Decompile() {
			using (StringWriter writer = new StringWriter()) {
				DecompileInternal(writer);
				return writer.ToString();
			}
		}
		/// <summary>
		///  Decompiles the ANM animation script and outputs it to the specified file.
		/// </summary>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outFile"/> is null.
		/// </exception>
		public void DecompileToFile(string outFile) {
			using (StreamWriter writer = new StreamWriter(outFile, false, Encoding.UTF8))
				DecompileInternal(writer);
		}
		/// <summary>
		///  Decompiles the ANM animation script and outputs it to the specified stream.
		/// </summary>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="outStream"/> is null.
		/// </exception>
		public void DecompileToStream(Stream outStream) {
			using (StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8))
				DecompileInternal(writer);
		}

		#endregion

		#region Decompile (Internal)

		/// <summary>
		///  Decompiles the ANM animation script and writes it to the text writer.
		/// </summary>
		/// <param name="writer">The text writer to write the decompiled script to.</param>
		private void DecompileInternal(TextWriter writer) {
			Dictionary<int, string> labelMap = GetLabelMap();

			for (int i = 0; i < Count; i++) {
				AnmLine frame = Lines[i];

				// Append a label name if there's a label at this index.
				if (labelMap.TryGetValue(i, out string labelName))
					writer.WriteLine($"#{labelName}");

				// Are we a type of jump that is able to display a label instead of the normal variable?
				bool isLabelJump = frame.Type.IsJump() && !frame.Parameters[frame.Count - 1].IsVariable;
				// Is the last parameter a duplicate range and unwanted?
				bool removeLastParam = frame.IsDuplicateRange;// || isLabelJump;
				var parameters = frame.Parameters.Take(frame.IsDuplicateRange ? frame.Count - 1 : frame.Count)
												 .Select((p, j) => (j == frame.Count - 1 && isLabelJump ?
																	labelMap[p.Value] : p.ToString()));

				string command = frame.Type.GetCommand();
				writer.WriteLine($"\t{(command != null ? $"{command} " : "")}{string.Join(" ", parameters)}");
			}
			writer.Flush();
		}

		#endregion

		#region Decompile (Helpers)

		/// <summary>
		///  Gets the map of goto indecies to label name.
		/// </summary>
		/// <param name="animation">The animation to get the labels for.</param>
		/// <returns>A dictionary of indecies and label names.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="animation"/> is null.
		/// </exception>
		private Dictionary<int, string> GetLabelMap() {
			Dictionary<int, string> labelMap = new Dictionary<int, string>();
			foreach (AnmLine frame in Lines) {
				if (frame.Type.IsJump()) {
					AnmParameter param = frame.Parameters[frame.Count - 1];
					if (!param.IsVariable)
						labelMap[param.Value] = null;
				}
			}
			int labelIndex = 0;
			foreach (int index in labelMap.Keys.OrderBy(i => i)) {
				labelMap[index] = FormatLabel(labelIndex++);
			}
			return labelMap;
		}
		/// <summary>
		///  Formats a label name based on the order of appearance of the label (or its index).
		/// </summary>
		/// <param name="labelIndex">The index of the label in the list of appearing labels.</param>
		/// <returns>The name of the label without a prefixed '#'.</returns>
		private static string FormatLabel(int labelIndex) {
			const char startChar = 'a';
			if (labelIndex < 26)
				return $"label_{(char) (startChar + labelIndex)}";
			string alpha = string.Empty;
			while (labelIndex != 0) {
				alpha = $"{(char) (startChar + labelIndex)}{alpha}";
				labelIndex /= 26;
			}
			return $"label_{alpha}";
		}
		
		#endregion
	}
	partial class KifintEntryExtensions {
		#region DecompileAnimation

		/// <summary>
		///  Loads and decompiles the ANM animation script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static string DecompileAnimation(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return AnmAnimation.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileAnimationToFile(this KifintEntry entry, string outFile) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				AnmAnimation.DecompileToFile(stream, entry.FileName, outFile);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileAnimationToStream(this KifintEntry entry, Stream outStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				AnmAnimation.DecompileToStream(stream, entry.FileName, outStream);
		}

		#endregion

		#region DecompileAnimation (KifintStream)

		/// <summary>
		///  Loads and decompiles the ANM animation script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static string DecompileAnimation(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return AnmAnimation.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileAnimationToFile(this KifintEntry entry, KifintStream kifintStream,
			string outFile)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				AnmAnimation.DecompileToFile(stream, entry.FileName, outFile);
		}
		/// <summary>
		///  Loads and decompiles the ANM animation script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileAnimationToStream(this KifintEntry entry, KifintStream kifintStream,
			Stream outStream)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				AnmAnimation.DecompileToStream(stream, entry.FileName, outStream);
		}

		#endregion
	}
}
