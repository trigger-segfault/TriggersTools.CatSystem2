using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using TriggersTools.SharpUtils.IO;
using TriggersTools.SharpUtils.Mathematics;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public struct BinaryRange {
		public long Start { get; }
		public long End { get; }

		public BinaryRange(long start) : this(start, 0) { }
		public BinaryRange(long start, long end) {
			Start = start;
			End = end;
		}
	}
	public class BinaryStringsPatch {
		
		public IReadOnlyDictionary<string, string> Translations { get; }
		
		public IReadOnlyList<BinaryRange> Ranges { get; }

		public BinaryStringsPatch(string[] lines, long start = 0, long end = 0)
			: this(lines, new BinaryRange(start, end))
		{
		}
		public BinaryStringsPatch(string[] lines, params BinaryRange[] ranges) {
			if (ranges.Length == 0)
				Ranges = Array.AsReadOnly(new BinaryRange[1]);
			else
				Ranges = Array.AsReadOnly(ranges);
			Translations = StringsScraper.BuildTranslation(lines);
		}

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
								writer.Write(new byte[reserved - required]);
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
	}
}
