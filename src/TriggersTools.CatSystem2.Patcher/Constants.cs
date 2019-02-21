using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher {
	internal static class Constants {
		public const string Signature = "Triggers_CatSystem2_Patcher";
		public const string TypeFace = "Microsoft Sans Serif";
		public const ushort PointSize = 8;
		public const ushort Language = 0x0409; // U.S. English
		public const ushort CodePage = 1200; // Unicode
		public const string ResourcesPath = "TriggersTools.CatSystem2.Patcher.Resources";

		/// <summary>
		///  Gets the Japanese encoding used for the CatSystem2 files.
		/// </summary>
		public static Encoding ShiftJIS { get; } = Encoding.GetEncoding(932);

		public static readonly FieldInfo MenuExTemplateItem_header;
		public static readonly FieldInfo MenuTemplateItem_header;

		static Constants() {
			// Fix broken shit preparations
			MenuExTemplateItem_header = typeof(MenuExTemplateItem).GetField("_header", BindingFlags.Instance | BindingFlags.NonPublic);
			MenuTemplateItem_header = typeof(MenuTemplateItem).GetField("_header", BindingFlags.Instance | BindingFlags.NonPublic);
		}
	}
}
