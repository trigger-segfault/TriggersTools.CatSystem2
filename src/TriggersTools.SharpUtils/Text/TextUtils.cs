using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Collections;

namespace TriggersTools.SharpUtils.Text {
	public static class TextUtils {

		private static readonly Dictionary<char, char> CharEscapes = new Dictionary<char, char> {
			{ '\0', '0' },
			{ '\a', 'a' },
			{ '\b', 'b' },
			{ '\f', 'f' },
			{ '\n', 'n' },
			{ '\r', 'r' },
			{ '\t', 't' },
			{ '\v', 'v' },
		};
		private static readonly Dictionary<char, char> ReverseCharEscapes = CharEscapes.ReverseKeyValues();
		private static readonly Dictionary<char, char> QuoteEscapes = new Dictionary<char, char> {
			{ '\'', '\'' },
			{ '"', '"' },
		};
		private static readonly Dictionary<char, char> ReverseQuoteEscapes = QuoteEscapes.ReverseKeyValues();
		private const string MessageNoEscapePattern = @"(?<!\\)(?:\\\\)*(?'char'[\[\]_])";
		private const string EscapePattern = @"\\.";
		private static readonly Regex MessageNoEscapeRegex =
			new Regex(MessageNoEscapePattern, RegexOptions.IgnoreCase);
		private static readonly Regex EscapeRegex = new Regex(EscapePattern);

		public static string EscapeNormal(string s, bool escapeQuotes) {
			StringBuilder str = new StringBuilder(s);
			str.Replace("\\", "\\\\");
			foreach (var pair in CharEscapes)
				str.Replace(pair.Key.ToString(), $"\\{pair.Value}");
			if (escapeQuotes) {
				foreach (var pair in QuoteEscapes)
					str.Replace(pair.Key.ToString(), $"\\{pair.Value}");
			}
			return str.ToString();
		}
		public static string UnescapeNormal(string s) {
			StringBuilder str = new StringBuilder(s);
			MatchCollection matches = EscapeRegex.Matches(s);
			for (int i = matches.Count - 1; i >= 0; i--) {
				Match match = matches[i];
				char c = match.Value[1];
				if (ReverseCharEscapes.TryGetValue(c, out char unescaped)) {
					str.Remove(match.Index, match.Length);
					str.Insert(match.Index, unescaped);
				}
				else {
					str.Remove(match.Index, 1); // Just remove the backslash
				}
			}
			return str.ToString();
		}
	}
}
