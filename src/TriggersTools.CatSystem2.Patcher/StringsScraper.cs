using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClrPlus.Windows.Api.Structures;
using ClrPlus.Windows.PeBinary.ResourceLib;
using TriggersTools.SharpUtils.IO;
using TriggersTools.SharpUtils.Mathematics;
using TriggersTools.SharpUtils.Text;
using MenuTemplate = ClrPlus.Windows.PeBinary.ResourceLib.MenuTemplate;
using MenuExTemplate = ClrPlus.Windows.PeBinary.ResourceLib.MenuExTemplate;
using DialogTemplate = ClrPlus.Windows.PeBinary.ResourceLib.DialogTemplate;
using DialogExTemplate = ClrPlus.Windows.PeBinary.ResourceLib.DialogExTemplate;
using TriggersTools.CatSystem2.Patcher.Patches;

namespace TriggersTools.CatSystem2.Patcher {
	public enum StringScrapeType {
		None,
		Normal,
		Language,
	}
	public class StringsScraper {
		#region Constants

		public const string NotTranslated = "$nr$";
		public const string EmptyTranslation = "$empty$";
		/// <summary>
		///  The prefix for a commented line
		/// </summary>
		public const string Comment = "//";

		public const string BinaryFileName = "binary.txt";
		public const string BinaryLanguageFileName = "binary_language.txt";
		public const string ResourceLanguageFilename = "resource_language.txt";
		
		private static readonly Regex BinaryHeaderRegex =
			new Regex($@"^{Comment}Bytes=(?'reserved'\d+),Offset=0x(?'offset'[A-Fa-f0-9]+)$");
		private static readonly Regex TranslatableRegex =
			new Regex(@"^\$[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}\$");

		#endregion

		#region ResourceScrape

		public static void ResourceScrape(string fileName, string outputDir) {
			Directory.CreateDirectory(outputDir);
			StringBuilder language = new StringBuilder();
			using (ResourceInfo resourceInfo = new ResourceInfo()) {
				resourceInfo.Load(fileName);
				foreach (Resource resource in resourceInfo.Resources.Values.SelectMany(list => list)) {
					StringBuilder normal = new StringBuilder();
					string path = null;
					switch (resource) {
					case StringResource stringTable:
						path = $"string";
						ScrapeStringTable(language, normal, stringTable); break;
					case MenuResource menu:
						path = $"menu";
						ScrapeMenu(language, normal, menu); break;
					case DialogResource dialog:
						path = $"dialog";
						ScrapeDialog(language, normal, dialog); break;
					}
					if (normal.Length != 0) {
						path = Path.Combine(outputDir, $"{path}_{resource.Name.Name}.txt");
						File.WriteAllText(path, normal.ToString());
					}
				}
			}
			if (language.Length != 0) {
				string path = Path.Combine(outputDir, ResourceLanguageFilename);
				File.WriteAllText(path, language.ToString());
			}
		}

		#endregion

		#region BinarySearch

		public static void BinarySearch(string fileName, string value, long start = 0, long end = 0) {
			BinarySearch(fileName, value, new BinaryRange(start, end));
		}
		public static void BinarySearch(string fileName, string value, params BinaryRange[] ranges) {
			using (Stream stream = File.OpenRead(fileName)) {
				foreach (BinaryRange range in ranges) {
					BinaryReader reader = new BinaryReader(stream, Constants.ShiftJIS);

					long end = (range.End != 0 ? Math.Min(range.End, stream.Length) : stream.Length);
					stream.Position = range.Start;
					while (stream.Position < end) {
						long position = stream.Position;
						string line;
						try {
							line = reader.ReadTerminatedString();
						} catch { break; }
						long reserved = (stream.Position - position);
						long bytes = reserved;
						reserved = MathUtils.Pad(reserved, 4);
						stream.SkipPadding(4);

						if (line == value) {
							Console.WriteLine($"{position:X8} = {bytes} B, \"{TextUtils.EscapeNormal(line, false)}\" Length={line.Length}");
						}
					}
				}
			}
		}

		#endregion

		#region BinaryScrape


		public static void BinaryScrape(string fileName, string outputDir, long start = 0, long end = 0) {
			BinaryScrape(fileName, outputDir, new BinaryRange(start, end));
		}
		public static void BinaryScrape(string fileName, string outputDir, params BinaryRange[] ranges) {
			Directory.CreateDirectory(outputDir);
			//List<string> language = new List<string>();
			//List<string> languageHeaders = new List<string>();
			StringBuilder language = new StringBuilder();
			StringBuilder normal = new StringBuilder();
			using (Stream stream = File.OpenRead(fileName)) {
				foreach (BinaryRange range in ranges) {
					//using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, BinaryFileName), false, Encoding.UTF8)) {
					BinaryReader reader = new BinaryReader(stream, Constants.ShiftJIS);

					long end = (range.End != 0 ? range.End : Math.Min(range.End, stream.Length));
					stream.Position = range.Start;

					while (stream.Position < end) {
						long position = stream.Position;
						string line;
						try {
							line = reader.ReadTerminatedString();
						} catch { break; }
						long reserved = (stream.Position - position);
						long bytes = reserved;
						stream.SkipPadding(4);
						reserved = MathUtils.Pad(reserved, 4);

						StringScrapeType type = GetStringType(line);

						string header = $"Bytes={reserved},Offset=0x{position:X8}";
						bool added = AddResString(header, language, normal, line, line);
						if (added) {
							Console.WriteLine($"{position:X8} = {bytes} B, \"{TextUtils.EscapeNormal(line, false)}\" Length={line.Length}");
						}

					}
				}
			}
			string normalPath = Path.Combine(outputDir, BinaryFileName);
			File.WriteAllText(normalPath, normal.ToString());
			if (language.Length != 0) {
				string path = Path.Combine(outputDir, BinaryLanguageFileName);
				File.WriteAllText(path, language.ToString());
			}
		}


		public static void BinaryValidate(IReadOnlyList<string> lines) {
			for (int i = 0; i < lines.Count; ) {
				string line = lines[i++];
				if (IsEmptyLine(line))
					continue;
				Match match = BinaryHeaderRegex.Match(line);
				if (!match.Success)
					throw new Exception($"Expected format \"{Comment}Bytes=#,Offset=0xXXXXXXXX\" at line {i-1}!");

				for (; i < lines.Count && IsCommentedLine(lines[i]); i++) ;

				int reserved = int.Parse(match.Groups["reserved"].Value);
				long offset = long.Parse(match.Groups["offset"].Value, NumberStyles.HexNumber);

				if (i == lines.Count)
					throw new Exception("Expected translation line!");

				string oldLine = TextUtils.UnescapeNormal(lines[i++]);
				if (i == lines.Count)
					throw new Exception("Expected translation line!");

				string newLine = TextUtils.UnescapeNormal(lines[i++]);

				if (newLine == NotTranslated)
					continue;
				if (newLine == EmptyTranslation)
					newLine = string.Empty;

				byte[] lineBytes = Constants.ShiftJIS.GetBytes(newLine);
				int bytesLength = lineBytes.Length + 1;
				if (bytesLength > reserved) {
					Console.WriteLine($"{bytesLength} > {reserved} | \"{lines[i-1]}\"");
				}
			}
		}

		#endregion

		#region Private Scrape Resources

		private static void ScrapeStringTable(StringBuilder language, StringBuilder normal, StringResource stringTable) {
			int previousLanguageLength = language.Length;
			
			foreach (var str in stringTable.Strings) {
				string id = GetResId(str);
				AddResString($"STRING {str.Key}", language, normal, str.Value, GetResId(str));
			}

			string headerComment = $"{Comment}STRINGTABLE : {stringTable.Name}{Environment.NewLine}{Environment.NewLine}";
			if (normal.Length != 0) {
				normal.Insert(0, headerComment);
			}
			if (language.Length != previousLanguageLength) {
				language.Insert(previousLanguageLength, headerComment);
				language.Insert(previousLanguageLength, Environment.NewLine);
			}
		}
		private static void ScrapeMenu(StringBuilder language, StringBuilder normal, MenuResource menuRes) {
			int previousLanguageLength = language.Length;

			string headerComment = Comment;
			switch (menuRes.Menu) {
			case MenuTemplate menu:
				ScrapeMenuItems(language, normal, menu.MenuItems, 0);
				headerComment += "MENU";
				break;
			case MenuExTemplate menuEx:
				ScrapeMenuItems(language, normal, menuEx.MenuItems, 0);
				headerComment += "MENUEX";
				break;
			}
			headerComment += $" : {menuRes.Name}{Environment.NewLine}{Environment.NewLine}";
			if (normal.Length != 0) {
				normal.Insert(0, headerComment);
			}
			if (language.Length != previousLanguageLength) {
				language.Insert(previousLanguageLength, headerComment);
				language.Insert(previousLanguageLength, Environment.NewLine);
			}
		}
		private static void ScrapeMenuItems(StringBuilder language, StringBuilder normal,
			MenuTemplateItemCollection menuItems, int level)
		{
			foreach (var menuItem in menuItems) {
				string commentLine = null;
				if (menuItem is MenuTemplateItemCommand command) {
					commentLine = "MENUITEM";
					if (command.IsSeparator)
						commentLine += " SEPARATOR";
					else if (command.MenuId != 0)
						commentLine += $" {command.MenuId}";
					commentLine += $" L{level}";
				}
				else if (menuItem is MenuTemplateItemPopup) {
					commentLine = $"POPUP L{level}";
				}
				AddResString(commentLine, language, normal, menuItem.MenuString, GetResId(menuItem));
				if (menuItem is MenuTemplateItemPopup popup)
					ScrapeMenuItems(language, normal, popup.SubMenuItems, level + 1);
			}
		}
		private static void ScrapeMenuItems(StringBuilder language, StringBuilder normal,
			MenuExTemplateItemCollection menuItems, int level) {
			foreach (var menuItem in menuItems) {
				string commentLine = null;
				if (menuItem is MenuExTemplateItemCommand command) {
					var header = (MenuExItemTemplate) Constants.MenuExTemplateItem_header.GetValue(command);
					commentLine = "MENUITEM";
					if (header.dwMenuId != 0)
						commentLine += $" {header.dwMenuId}";
					commentLine += $" L{level}";
				}
				else if (menuItem is MenuExTemplateItemPopup) {
					commentLine = $"POPUP L{level}";
				}
				AddResString(commentLine, language, normal, menuItem.MenuString, GetResId(menuItem));
				if (menuItem is MenuExTemplateItemPopup popupEx)
					ScrapeMenuItems(language, normal, popupEx.SubMenuItems, level + 1);
			}
		}

		private static void ScrapeDialog(StringBuilder language, StringBuilder normal, DialogResource dialogRes) {
			int previousLanguageLength = language.Length;

			string headerComment = Comment;
			switch (dialogRes.Template) {
			case DialogTemplate dialog:
				ScrapeDialogTemplate(language, normal, dialog);
				headerComment += "DIALOG";
				break;
			case DialogExTemplate dialogEx:
				ScrapeDialogTemplate(language, normal, dialogEx);
				headerComment += "DIALOGEX";
				break;
			}

			headerComment += $" : {dialogRes.Name}{Environment.NewLine}{Environment.NewLine}";
			if (normal.Length != 0) {
				normal.Insert(0, headerComment);
			}
			if (language.Length != previousLanguageLength) {
				language.Insert(previousLanguageLength, headerComment);
				language.Insert(previousLanguageLength, Environment.NewLine);
			}
		}

		private static void ScrapeDialogTemplate(StringBuilder language, StringBuilder normal, DialogTemplate dialog) {
			AddResString("CAPTION", language, normal, dialog.Caption, GetResId(dialog));
			foreach (var control in dialog.Controls) {
				string commentLine = "CONTROL";
				if (control is DialogTemplateControl idControl && idControl.Id != 0)
					commentLine += $" {idControl.Id}";
				AddResString(commentLine, language, normal, control.CaptionId?.Name, GetResId(control));
			}
		}
		private static void ScrapeDialogTemplate(StringBuilder language, StringBuilder normal, DialogExTemplate dialogEx) {
			AddResString("CAPTION", language, normal, dialogEx.Caption, GetResId(dialogEx));
			foreach (var control in dialogEx.Controls) {
				string commentLine = "CONTROL";
				if (control is DialogExTemplateControl idControl && idControl.Id != 0)
					commentLine += $" {idControl.Id}";
				AddResString(commentLine, language, normal, control.CaptionId?.Name, GetResId(control));
			}
		}

		#endregion

		public static string GetResId(KeyValuePair<ushort, string> stringEntry) {
			return $"${stringEntry.Key}{stringEntry.Value}";
		}
		public static string GetResId(DialogTemplate dialog) {
			return $"$CAPTION${dialog.Caption}";
		}
		public static string GetResId(DialogExTemplate dialogEx) {
			return $"$CAPTION${dialogEx.Caption}";
		}
		public static string GetResId(DialogTemplateControlBase control) {
			if (control is DialogTemplateControl normalControl) {
				//if (normalControl.Id != 0)
					return $"${normalControl.Id}${normalControl.CaptionId}";
				//return normalControl.CaptionId.ToString();
			}
			else if (control is DialogExTemplateControl exControl) {
				//if (exControl.Id != 0)
					return $"${exControl.Id}${exControl.CaptionId}";
				//return exControl.CaptionId.ToString();
			}
			return null;
		}
		public static string GetResId(MenuTemplateItem menuItem) {
			if (menuItem is MenuTemplateItemCommand command) {
				//if (command.MenuId != 0)
					return $"${command.MenuId}${menuItem.MenuString}";
				//return menuItem.MenuString;
			}
			else {
				return $"$POPUP${menuItem.MenuString}";
			}
		}
		public static string GetResId(MenuExTemplateItem menuItem) {
			if (menuItem is MenuExTemplateItemCommand command) {
				var header = (MenuExItemTemplate) Constants.MenuExTemplateItem_header.GetValue(command);
				//if (header.dwMenuId != 0)
					return $"${header.dwMenuId}${menuItem.MenuString}";
				//return menuItem.MenuString;
			}
			else {
				return $"$POPUP${menuItem.MenuString}";
			}
		}

		private static bool AddResString(string commentLine, StringBuilder language, StringBuilder normal, string s,
			string resId)
		{
			StringScrapeType type = GetStringType(s);
			switch (type) {
			case StringScrapeType.Normal:
				if (commentLine != null)
					normal.AppendLine($"{Comment}{commentLine}");
				normal.AppendLine($"{Comment}{TextUtils.EscapeNormal(s, false)}");
				normal.AppendLine(TextUtils.EscapeNormal(resId, false));
				normal.AppendLine(NotTranslated);
				normal.AppendLine();
				return true;
			case StringScrapeType.Language:
				string id = TranslatableRegex.Match(s).Value;
				string value = s.Substring(id.Length);

				if (commentLine != null)
					language.AppendLine($"{Comment}{commentLine}");
				language.AppendLine($"{Comment}{TextUtils.EscapeNormal(value, false)}");
				language.AppendLine(id);
				language.AppendLine(NotTranslated);
				language.AppendLine();
				return true;
			default:
				return false;
			}
		}

		public static StringScrapeType GetStringType(string s) {
			if (string.IsNullOrWhiteSpace(s))
				return StringScrapeType.None;
			if (TranslatableRegex.IsMatch(s))
				return StringScrapeType.Language;
			return StringScrapeType.Normal;
		}
		public static bool IsNormalString(string s) => GetStringType(s) == StringScrapeType.Normal;



		public static bool IsCommentedLine(string line) => line.StartsWith(Comment);
		public static bool IsEmptyLine(string line) => line.Length == 0;
		public static string CommentLine(string line) => (!IsCommentedLine(line) ? $"{Comment}{line}" : line);

		public static IReadOnlyDictionary<string, string> BuildTranslation(string[] lines) {
			Dictionary<string, string> translations = new Dictionary<string, string>();
			for (int i = 0; i < lines.Length; i++) {
				string output, input = lines[i];
				if (IsEmptyLine(input) || IsCommentedLine(input))
					continue; // Skip empty lines

				// Get input translation
				switch (input) {
				case NotTranslated:
				case EmptyTranslation:
					throw new Exception($"Expected translation input line, not \"{input}\" on line {i}!");
				}
				if (i + 1 == lines.Length)
					throw new Exception($"Unexpected end of file, expected translation after input \"{input}\" on line {i}!");
				output = lines[++i];

				if (IsEmptyLine(output))
					throw new Exception($"Unexpected empty line, expected translation after input \"{input}\" on line {i}!");
				else if (IsCommentedLine(output))
					throw new Exception($"Unexpected commented line, expected translation after input \"{input}\" on line {i}!");

				input = TextUtils.UnescapeNormal(input);
				output = TextUtils.UnescapeNormal(output);

				switch (output) {
				case NotTranslated:
					// No translation, don't add anything
					break;
				case EmptyTranslation:
					if (!translations.ContainsKey(input))
						translations.Add(input, string.Empty);
					break;
				default:
					if (!translations.ContainsKey(input))
						translations.Add(input, output);
					break;
				}
			}
			return new ReadOnlyDictionary<string, string>(translations);
		}
	}
}
