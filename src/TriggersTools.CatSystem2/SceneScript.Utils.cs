using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2 {
	partial class SceneScript {
		#region Constants

		private static readonly IReadOnlyList<string> EscapesToRemove = new string[] {
			@"c[rs]?0x[A-Fa-f0-9]{8};?", // Change font color
			@"fr?[+\-]?\d+;?", // Change font size (input)
			@"fr?(?:n|s|ss|l|ll)", // Change font size (enum)
			@"p[lcr]", // Change display position
			@"wf?\d+;?", // Change display timing
			@"@", // Block script
		};

		private static readonly string[] DialogPrefixes = new string[] {
			"n",
			"@",
			"r",
			"pc",
			"pl",
			"pr",
			"wf",
			"w",
			"fr",
			"fnl",
			"fss",
			"fnn",
			"fll",
			//"fn",
			"ti",
			//"f",
			@"f\d+"
		};
		//P\\fX|
		//private static readonly string DialogPrefixPattern = $@" \[P\\fnX\]|\\(?:{string.Join("|", DialogPrefixes)})";
		private static readonly string DialogPrefixPattern = $@"^(?:\\(?:{string.Join("|", DialogPrefixes)}))+|(?:\\(?:{string.Join("|", DialogPrefixes)}))+$";
		private static readonly Regex DialogPrefixRegex = new Regex(DialogPrefixPattern, RegexOptions.IgnoreCase);
		private static readonly string EscapesToRemovePattern = $@"\\(?:{string.Join("|", EscapesToRemove)})";
		private static readonly Regex EscapesToRemoveRegex = new Regex(DialogPrefixPattern, RegexOptions.IgnoreCase);
		
		#endregion

		public static string UnescapeMessage(string content) {
			StringBuilder str = new StringBuilder(content);
			MatchCollection matches = EscapesToRemoveRegex.Matches(content);
			for (int i = matches.Count - 1; i >= 0; i--) {
				Match match = matches[i];
				str.Remove(match.Index, match.Length);
			}

			str.Replace("\\'",  "'");
			str.Replace("\\\"", "\"");
			str.Replace("\\n",  "\n");
			string toStr = str.ToString();
			if (toStr.Contains("_")) {
				Console.Write("");
			}
			if (toStr.Contains(@"\fnX")) {
				Console.Write("");
			}
			foreach (string prefix in DialogPrefixes) {
				if (toStr.Contains($@"\{prefix}")) {
					Console.Write("");
				}
			}
			str.Replace("\\_",  " ");
			str.Replace("[", "");
			str.Replace("]", "");
			
			return str.ToString();
		}

		public static string UnescapeSpaces(string content) {
			return content.Replace("_", " ");
		}
	}
}
