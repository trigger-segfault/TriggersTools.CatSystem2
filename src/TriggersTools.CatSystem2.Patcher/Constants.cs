using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
	}
}
