using System;

namespace TriggersTools.SharpUtils.Text {
	/// <summary>
	///  Extensions for <see cref="string"/> objects.
	/// </summary>
	public static partial class StringExtensions {
		#region Surround

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Surround(this string s, char openClose, bool ignoreCase = false, bool trim = false) {
			return s.Surround(openClose, out _, ignoreCase, trim);
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Surround(this string s, char open, char close, bool ignoreCase = false, bool trim = false)
		{
			return s.Surround(open, close, out _, ignoreCase, trim);
		}

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">
		/// If true, the string will be trimmed of whitespace before it is surrouned.
		/// </param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static string Surround(this string s, string openClose, bool ignoreCase = false, bool trim = false) {
			return s.Surround(openClose, out _, ignoreCase, trim);
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static string Surround(this string s, string open, string close, bool ignoreCase = false,
			bool trim = false)
		{
			return s.Surround(open, close, out _, ignoreCase, trim);
		}

		#endregion

		#region Surround (out)

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>. Outputs if the string was
		///  already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding already existed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Surround(this string s, char openClose, out bool surrounded, bool ignoreCase = false,
			bool trim = false)
		{
			surrounded = s.IsSurrounded(openClose, ignoreCase);
			if (surrounded)
				return s;
			return openClose + (trim ? s.Trim() : s) + openClose;
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		///  Outputs if the string was already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding already existed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Surround(this string s, char open, char close, out bool surrounded,
			bool ignoreCase = false, bool trim = false)
		{
			surrounded = s.IsSurrounded(open, close, ignoreCase);
			if (surrounded)
				return s;
			return open + (trim ? s.Trim() : s) + close;
		}

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>. Outputs if the string was
		///  already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding already existed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static string Surround(this string s, string openClose, out bool surrounded, bool ignoreCase = false,
			bool trim = false)
		{
			surrounded = s.IsSurrounded(openClose, ignoreCase);
			if (surrounded)
				return s;
			return openClose + (trim ? s.Trim() : s) + openClose;
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		///  Outputs if the string was already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding already existed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static string Surround(this string s, string open, string close, out bool surrounded,
			bool ignoreCase = false, bool trim = false)
		{
			surrounded = s.IsSurrounded(open, close, ignoreCase);
			if (surrounded)
				return s;
			return open + (trim ? s.Trim() : s) + close;
		}

		#endregion

		#region SurroundForce

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>. Always surrounds the string
		///  even if it is already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string SurroundForce(this string s, char openClose, bool trim = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			return openClose + (trim ? s.Trim() : s) + openClose;
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>. Always
		///  surrounds the string even if it is already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string SurroundForce(this string s, char open, char close, bool trim = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			return open + (trim ? s.Trim() : s) + close;
		}

		/// <summary>
		///  Returns the string that starts and ends with <paramref name="openClose"/>. Always surrounds the string
		///  even if it is already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>The string that starts and ends with <paramref name="openClose"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static string SurroundForce(this string s, string openClose, bool trim = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (openClose == null)
				throw new ArgumentNullException(nameof(openClose));
			return openClose + (trim ? s.Trim() : s) + openClose;
		}

		/// <summary>
		///  Returns the string that starts with <paramref name="open"/> and ends with <paramref name="close"/>. Always
		///  surrounds the string even if it is already surrounded.
		/// </summary>
		/// <param name="s">The string to surround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="trim">If true, the string will be trimmed of whitespace before it is surrouned.</param>
		/// <returns>
		///  The string that starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static string SurroundForce(this string s, string open, string close, bool trim = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (open == null)
				throw new ArgumentNullException(nameof(open));
			if (close == null)
				throw new ArgumentNullException(nameof(close));
			return open + (trim ? s.Trim() : s) + close;
		}

		#endregion

		#region Unsurround

		/// <summary>
		///  Returns the string with the start and end of <paramref name="openClose"/> removed if they exist.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unsurround(this string s, char openClose, bool ignoreCase = false) {
			return s.Unsurround(openClose, out _, ignoreCase);
		}

		/// <summary>
		///  Returns the string with the start of <paramref name="open"/> and end of <paramref name="close"/> removed
		///  if they exist.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unsurround(this string s, char open, char close, bool ignoreCase = false) {
			return s.Unsurround(open, close, out _, ignoreCase);
		}

		/// <summary>
		///  Returns the string with the start and end of <paramref name="openClose"/> removed if they exist.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static string Unsurround(this string s, string openClose, bool ignoreCase = false) {
			return s.Unsurround(openClose, out _, ignoreCase);
		}

		/// <summary>
		///  Returns the string with the start of <paramref name="open"/> and end of <paramref name="close"/> removed
		///  if they exist.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static string Unsurround(this string s, string open, string close, bool ignoreCase = false) {
			return s.Unsurround(open, close, out _, ignoreCase);
		}

		#endregion

		#region Unsurround (out)

		/// <summary>
		///  Returns the string with the start and end of <paramref name="openClose"/> removed if they exist. Also
		///  outputs if the string was surrounded.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding was removed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unsurround(this string s, char openClose, out bool surrounded, bool ignoreCase = false)
		{
			surrounded = s.IsSurrounded(openClose, ignoreCase);
			if (surrounded)
				return s.Substring(1, s.Length - 2);
			return s;
		}

		/// <summary>
		///  Returns the string with the start of <paramref name="open"/> and end of <paramref name="close"/> removed
		///  if they exist. Also outputs if the string was surrounded.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding was removed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unsurround(this string s, char open, char close, out bool surrounded,
			bool ignoreCase = false)
		{
			surrounded = s.IsSurrounded(open, close, ignoreCase);
			if (surrounded)
				return s.Substring(1, s.Length - 2);
			return s;
		}

		/// <summary>
		///  Returns the string with the start and end of <paramref name="openClose"/> removed if they exist. Also
		///  outputs if the string was surrounded.
		/// </summary>
		/// <param name="s">The string to unsurround.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding was removed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static string Unsurround(this string s, string openClose, out bool surrounded, bool ignoreCase = false)
		{
			surrounded = s.IsSurrounded(openClose, ignoreCase);
			if (surrounded)
				return s.Substring(openClose.Length, s.Length - openClose.Length * 2);
			return s;
		}

		/// <summary>
		/// Returns the string with the start of <paramref name="open"/> and end of <paramref name="close"/>
		/// removed if they exist. Also outputs if the string was surrounded.
		/// </summary>
		/// 
		/// <param name="s">The string to unsurround.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="surrounded">Set to true if the surrounding was removed.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>The string with the surrounding removed if it existed.</returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static string Unsurround(this string s, string open, string close, out bool surrounded,
			bool ignoreCase = false)
		{
			surrounded = s.IsSurrounded(open, close, ignoreCase);
			if (surrounded)
				return s.Substring(open.Length, s.Length - open.Length - close.Length);
			return s;
		}

		#endregion

		#region IsSurrounded

		/// <summary>
		///  Returns true if the string starts and ends with the <paramref name="openClose"/> parts.
		/// </summary>
		/// <param name="s">The string to check for surrounding.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>True if the string starts and ends with the <paramref name="openClose"/> parts.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static bool IsSurrounded(this string s, char openClose, bool ignoreCase = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length < 2)
				return false;
			if (ignoreCase) {
				char openCloseLower = char.ToLower(openClose);
				return	char.ToLower(s[0]) == openCloseLower &&
						char.ToLower(s[s.Length - 1]) == openCloseLower;
			}
			return s[0] == openClose && s[s.Length - 1] == openClose;
		}

		/// <summary>
		///  Returns true if the string starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </summary>
		/// <param name="s">The string to check for surrounding.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>
		///  True if the string starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static bool IsSurrounded(this string s, char open, char close, bool ignoreCase = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (s.Length < 2)
				return false;
			if (ignoreCase) {
				return  char.ToLower(s[0]) == char.ToLower(open) &&
						char.ToLower(s[s.Length - 1]) == char.ToLower(close);
			}
			return s[0] == open && s[s.Length - 1] == close;
		}

		/// <summary>
		///  Returns true if the string starts and ends with the <paramref name="openClose"/> parts.
		/// </summary>
		/// <param name="s">The string to check for surrounding.</param>
		/// <param name="openClose">The left and right parts of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>True if the string starts and ends with the <paramref name="openClose"/> parts.</returns>
		/// 
		/// <remarks>
		///  The two <paramref name="openClose"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> or <paramref name="openClose"/> is null.
		/// </exception>
		public static bool IsSurrounded(this string s, string openClose, bool ignoreCase = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (openClose == null)
				throw new ArgumentNullException(nameof(openClose));
			if (s.Length < openClose.Length * 2)
				return false;
			if (ignoreCase) {
				return	s.StartsWith(openClose, StringComparison.OrdinalIgnoreCase) &&
						s.EndsWith(openClose, StringComparison.OrdinalIgnoreCase);
			}
			return s.StartsWith(openClose) && s.EndsWith(openClose);
		}

		/// <summary>
		///  Returns true if the string starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </summary>
		/// <param name="s">The string to check for surrounding.</param>
		/// <param name="open">The left part of the surrounding.</param>
		/// <param name="close">The right part of the surrounding.</param>
		/// <param name="ignoreCase">True if the comparison is case-insensitive.</param>
		/// <returns>
		///  True if the string starts with <paramref name="open"/> and ends with <paramref name="close"/>.
		/// </returns>
		/// 
		/// <remarks>
		///  The <paramref name="open"/> and <paramref name="close"/> parts must not overlap each other.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/>, <paramref name="open"/>, or <paramref name="close"/> is null.
		/// </exception>
		public static bool IsSurrounded(this string s, string open, string close, bool ignoreCase = false) {
			if (s == null)
				throw new ArgumentNullException(nameof(s));
			if (open == null)
				throw new ArgumentNullException(nameof(open));
			if (close == null)
				throw new ArgumentNullException(nameof(close));
			if (s.Length < open.Length + close.Length)
				return false;
			if (ignoreCase) {
				return	s.StartsWith(open, StringComparison.OrdinalIgnoreCase) &&
						s.EndsWith(close, StringComparison.OrdinalIgnoreCase);
			}
			return s.StartsWith(open) && s.EndsWith(close);
		}

		#endregion
	}
}
