using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes;

namespace TriggersTools.CatSystem2 {
	partial class CstScene {
		#region Decompile (From File)

		/// <summary>
		///  Loads and decompiles the CST scene script file and returns the script as a string.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> is null.
		/// </exception>
		public static string Decompile(string cstFile) {
			return Extract(cstFile).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the CST scene script file and outputs it to the specified file.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileToFile(string cstFile, string outFile, Encoding encoding = null) {
			Extract(cstFile).DecompileToFile(outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script file and outputs it to the specified stream.
		/// </summary>
		/// <param name="cstFile">The file path to the CST scene script file to extract.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="cstFile"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileToStream(string cstFile, Stream outStream, Encoding encoding = null) {
			Extract(cstFile).DecompileToStream(outStream, encoding);
		}

		#endregion

		#region Decompile (From Stream)

		/// <summary>
		///  Loads and decompiles the CST scene script stream and returns the script as a string.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		public static string Decompile(Stream stream, string fileName) {
			return Extract(stream, fileName).Decompile();
		}
		/// <summary>
		///  Loads and decompiles the CST scene script stream and outputs it to the specified file.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
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
		///  Loads and decompiles the CST scene script stream and outputs it to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to extract the scene script from.</param>
		/// <param name="fileName">The path or name of the scene script file being extracted.</param>
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
		///  Decompiles the CST scene script and returns the script as a string.
		/// </summary>
		/// <returns>The decompiled script.</returns>
		public string Decompile() {
			using (StringWriter writer = new StringWriter()) {
				DecompileInternal(writer);
				return writer.ToString();
			}
		}
		/// <summary>
		///  Decompiles the CST scene script and outputs it to the specified file.
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
		///  Decompiles the CST scene script and outputs it to the specified stream.
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

		/// <summary>
		///  Decompiles the CST scene script and writes it to the text writer.
		/// </summary>
		/// <param name="writer">The text writer to write the decompiled script to.</param>
		private void DecompileInternal(TextWriter writer) {
			// Header must state the output .cst script file name
			writer.WriteLine($"#{Path.GetFileNameWithoutExtension(FileName)}");
			writer.WriteLine();

			for (int i = 0; i < Count; i++) {
				ISceneLine line = Lines[i];
				switch (line.Type) {
				case SceneLineType.Command:
					writer.WriteLine($"\t{line.Content}");
					break;
				case SceneLineType.Message:
					// When a scene file has a message written on a new line after another message,
					// a '\n' is automatically prefixed. We need to remove that.
					if (line.Content.StartsWith("\\n"))
						writer.WriteLine($"\t{line.Content.Substring(2)}");
					// Empty messages are the result of '\n' as the line input
					else if (line.Content.Length == 0)
						writer.WriteLine("\t\\n");
					else
						writer.WriteLine($"\t{line.Content}");
					break;
				case SceneLineType.Name:
					// Write the name without tab first
					writer.Write(line.Content);
					// See if we can continue by writing the preceding message on the same line.
					if (i + 1 < Count && Lines[i + 1].Type == SceneLineType.Message) {
						writer.WriteLine($"\t{Lines[i + 1].Content}");
						i++; // Skip the next entry, we just wrote it
					}
					break;
				case SceneLineType.Input:
					// Input is specified by an empty line, whitespace is allowed
					writer.WriteLine();
					break;
				case SceneLineType.Page:
					// Page break in novel view, denoted with \p
					writer.WriteLine();
					writer.WriteLine("\t\\p");
					writer.WriteLine();
					break;
				}

			}
			writer.Flush();
		}

		#endregion
	}
	partial class KifintEntryExtensions {
		#region DecompileScene

		/// <summary>
		///  Loads and decompiles the CST scene script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static string DecompileScene(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return CstScene.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileSceneToFile(this KifintEntry entry, string outFile, Encoding encoding = null) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				CstScene.DecompileToFile(stream, entry.FileName, outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileSceneToStream(this KifintEntry entry, Stream outStream, Encoding encoding = null) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				CstScene.DecompileToStream(stream, entry.FileName, outStream, encoding);
		}

		#endregion

		#region DecompileScene (KifintStream)

		/// <summary>
		///  Loads and decompiles the CST scene script entry and returns the script as a string.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The decompiled script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static string DecompileScene(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return CstScene.Decompile(stream, entry.FileName);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified file.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outFile">The output file to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outFile"/> is null.
		/// </exception>
		public static void DecompileSceneToFile(this KifintEntry entry, KifintStream kifintStream, string outFile,
			Encoding encoding = null)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				CstScene.DecompileToFile(stream, entry.FileName, outFile, encoding);
		}
		/// <summary>
		///  Loads and decompiles the CST scene script entry and outputs it to the specified stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <param name="outStream">The output stream to write the decompiled script to.</param>
		/// <param name="encoding">The output encoding, <see cref="CatUtils.ShiftJIS"/> if null.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/>, <paramref name="kifintStream"/>, or <paramref name="outStream"/> is null.
		/// </exception>
		public static void DecompileSceneToStream(this KifintEntry entry, KifintStream kifintStream,
			Stream outStream, Encoding encoding = null)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				CstScene.DecompileToStream(stream, entry.FileName, outStream, encoding);
		}

		#endregion
	}
}
