using System;
using System.Text;

namespace TriggersTools.SharpUtils.Text {
	partial class StringExtensions {
		#region ToNullTerminated

		/// <summary>
		///  Returns a new string that is ended at the first index of a null character.
		/// </summary>
		/// <param name="s">The string to null terminate.</param>
		/// <returns>The null terminated string.-or- <paramref name="s"/> if there was no null character.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string ToNullTerminated(this string s) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			int index = s.IndexOf('\0');
			return (index != -1 ? s.Substring(0, index) : s);
		}
		/// <summary>
		///  Returns a new string that is ended at the first index of a null character in the array.
		/// </summary>
		/// <param name="chars">The character array to null terminate.</param>
		/// <returns>
		///  The null terminated string.-or- the full character array as a string if there was no null character.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="chars"/> is null.
		/// </exception>
		public static string ToNullTerminatedString(this char[] chars) {
			return new string(chars, 0, chars.IndexOfNullTerminator());
		}
		/// <summary>
		///  Returns a new string that is ended at the first index of a 0 byte in the array.
		/// </summary>
		/// <param name="bytes">The byte array to encode and null terminate.</param>
		/// <param name="encoding">The encoding to use for the bytes to get the string.</param>
		/// <returns>
		///  The null terminated string.-or- the full encoded string if there was no null character.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="bytes"/> or <paramref name="encoding"/> is null.
		/// </exception>
		public static string ToNullTerminatedString(this byte[] bytes, Encoding encoding) {
			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));
			return encoding.GetString(bytes, 0, bytes.IndexOfNullTerminator());
		}

		#endregion

		#region IndexOfNullTerminator

		/// <summary>
		///  Gets the index of the null terminator character in the string.
		/// </summary>
		/// <param name="s">The string to find the null terminator in.</param>
		/// <returns>The index of the null terminator.-or- -1 if no null terminator was found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static int IndexOfNullTerminator(this string s) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			int index = s.IndexOf('\0');
			return (index != -1 ? index : s.Length);
		}
		/// <summary>
		///  Gets the index of the null terminator character in the character array.
		/// </summary>
		/// <param name="chars">The character array to find the null terminator in.</param>
		/// <returns>The index of the null terminator.-or- -1 if no null terminator was found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="chars"/> is null.
		/// </exception>
		public static int IndexOfNullTerminator(this char[] chars) {
			if (chars == null)
				throw new ArgumentNullException(nameof(chars));
			int index = Array.IndexOf(chars, '\0');
			return (index != -1 ? index : chars.Length);
		}
		/// <summary>
		///  Gets the index of the 0 byte in the byte array.
		/// </summary>
		/// <param name="bytes">The byte array to find the 0 byte in.</param>
		/// <returns>The index of the 0 byte.-or- -1 if no 0 byte was found.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="bytes"/> is null.
		/// </exception>
		public static int IndexOfNullTerminator(this byte[] bytes) {
			if (bytes == null)
				throw new ArgumentNullException(nameof(bytes));
			int index = Array.IndexOf(bytes, (byte) 0);
			return (index != -1 ? index : bytes.Length);
		}

		#endregion
	}
}
