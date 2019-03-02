using System;
using System.Collections.Generic;
using System.IO;
using TriggersTools.SharpUtils.IO;
using TriggersTools.SharpUtils.Mathematics;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	/// <summary>
	///  A patch that replaces null-terminated strings in a binary file.
	/// </summary>
	public class BinaryStringsPatch {
		#region Fields

		/// <summary>
		///  The translations to replace the located strings with.
		/// </summary>
		public IReadOnlyDictionary<string, string> Translations { get; }
		/// <summary>
		///  The ranges to look in, in the binary file.
		/// </summary>
		public IReadOnlyList<BinaryRange> Ranges { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the binary strings patch with a full range.
		/// </summary>
		/// <param name="translations">The translations to replace the located strings with.</param>
		public BinaryStringsPatch(IReadOnlyDictionary<string, string> translations)
			: this(translations, new BinaryRange(0, 0)) {
		}
		/// <summary>
		///  Constructs the binary strings patch with the specified range.
		/// </summary>
		/// <param name="translations">The translations to replace the located strings with.</param>
		/// <param name="start">The start position.</param>
		/// <param name="end">The end position.</param>
		public BinaryStringsPatch(IReadOnlyDictionary<string, string> translations, long start, long end)
			: this(translations, new BinaryRange(start, end))
		{
		}
		/// <summary>
		///  Constructs the binary strings patch with the specified ranges.
		/// </summary>
		/// <param name="translations">The translations to replace the located strings with.</param>
		/// <param name="ranges">The range positions.</param>
		public BinaryStringsPatch(IReadOnlyDictionary<string, string> translations, params BinaryRange[] ranges) {
			if (ranges.Length == 0)
				Ranges = Array.AsReadOnly(new BinaryRange[1]);
			else
				Ranges = Array.AsReadOnly(ranges);
			Translations = translations;
		}

		#endregion

		#region Patch

		/// <summary>
		///  Patches the file by reading the input stream, and writing to the output stream.
		/// </summary>
		/// <param name="inStream">The input stream to read the translations from.</param>
		/// <param name="outStream">The output stream to write the translations to.</param>
		/// <returns>True if the patch was successful.</returns>
		public bool Patch(Stream inStream, Stream outStream) {
			BinaryReader reader = new BinaryReader(inStream, Constants.ShiftJIS);
			BinaryWriter writer = new BinaryWriter(outStream, Constants.ShiftJIS);
			//long start = StartRange;
			//long end = (EndRange != 0 ? EndRange : inStream.Length);
			int count = 0;
			foreach (BinaryRange range in Ranges) {
				long start = range.Start;
				long end = (range.End != 0 ? Math.Min(range.End, inStream.Length) : inStream.Length);
				inStream.Position = start;
				outStream.Position = start;
				while (inStream.Position < end) {
					try {
						long position = inStream.Position;
						string id = reader.ReadTerminatedString();
						int bytes = (int) (inStream.Position - position);
						int reserved = MathUtils.Pad(bytes, 4);

						inStream.SkipPadding(4);

						if (Translations.TryGetValue(id, out string translation)) {
							byte[] buffer = Constants.ShiftJIS.GetBytes(translation);
							int required = buffer.Length + 1;
							if (required > reserved) {
								Console.WriteLine("FAILED!");
								return false;
							}
							outStream.Position = position;
							writer.WriteTerminated(translation);
							if (required != reserved)
								writer.WriteZeroBytes(reserved - required);
							Console.WriteLine($"0x{position:X5} = {TextUtils.EscapeNormal(translation, false)}");
							count++;
						}

					} catch (EndOfStreamException) {
						break;
					}
				}
			}
			Console.WriteLine(count);
			return true;
		}

		#endregion
	}
}
