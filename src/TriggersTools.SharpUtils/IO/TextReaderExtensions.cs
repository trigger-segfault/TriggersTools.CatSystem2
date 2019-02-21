using System;
using System.Collections.Generic;
using System.IO;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  Extension methods for the <see cref="StreamReader"/> class.
	/// </summary>
	public static class TextReaderExtensions {
		#region ReadLines

		/// <summary>
		///  Reads all remaining lines of characters from the current stream and returns the data as a string array.
		/// </summary>
		/// <param name="reader">The stream reader to read with.</param>
		/// <returns>
		///  The remaining lines from the input stream, or an empty array if the end of the input stream is reached.
		/// </returns>
		/// 
		/// <exception cref="OutOfMemoryException">
		///  There is insufficient memory to allocate a buffer for the returned string.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occured.
		/// </exception>
		public static string[] ReadLinesToEnd(this TextReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			List<string> lines = new List<string>();
			string line;
			while ((line = reader.ReadLine()) != null) {
				lines.Add(line);
			}
			return lines.ToArray();
		}

		#endregion
	}
}
