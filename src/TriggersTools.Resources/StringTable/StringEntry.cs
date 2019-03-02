using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.Resources.StringTable {
	/// <summary>
	///  A <see cref="StringResource"/> entry with a string Id and text.
	/// </summary>
	public struct StringEntry {
		#region Fields

		/// <summary>
		///  Gets the string Id of the string entry.
		/// </summary>
		public ushort Id { get; }
		/// <summary>
		///  Gets the text of the string entry.
		/// </summary>
		public string String { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the entry with a string Id and text.
		/// </summary>
		/// <param name="stringId">The string Id of the text.</param>
		/// <param name="s">The entry's text.</param>
		public StringEntry(ushort stringId, string s) {
			Id = stringId;
			String = s;
		}

		#endregion
	}
}
