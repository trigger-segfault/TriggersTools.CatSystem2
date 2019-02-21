using System;

namespace TriggersTools.SharpUtils.Text {
	/// <summary>
	///  Extensions for surrounding <see cref="string"/>s with single or double quotes.
	/// </summary>
	public static partial class StringExtensions {
		#region GetQuoteType

		/// <summary>
		///  Gets the type of quote the string is surrounded with.
		/// </summary>
		/// <param name="s">The string to get the quote type of.</param>
		/// <returns>The quote type the string was surrounded with. Null character if none.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static char GetQuoteType(this string s) {
			if (s.IsSurrounded('"'))
				return '"';
			if (s.IsSurrounded('\''))
				return '\'';
			return '\0';
		}

		#endregion

		#region IsQuoted

		/// <summary>
		///  Checks if the string is surrounded with the specified quote type.
		/// </summary>
		/// <param name="s">The string to check.</param>
		/// <param name="quoteType">The quote type to check for.</param>
		/// <returns>True if the string was surrounded by the quote type.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static bool IsQuoted(this string s, char quoteType) {
			return s.IsSurrounded(quoteType);
		}
		/// <summary>
		///  Checks if the string is surrounded with single or double quotes.
		/// </summary>
		/// <param name="s">The string to check.</param>
		/// <returns>True if the string was surrounded by any quote type.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static bool IsQuotedAny(this string s) {
			return s.IsSurrounded('"') || s.IsSurrounded('\'');
		}
		/// <summary>
		///  Checks if the string is surrounded with single or double quotes. Outputs the quote type the string was
		///  surrounded with.
		/// </summary>
		/// <param name="s">The string to check.</param>
		/// <param name="quoteType">The quote type the string was surrounded with. Null character if none.</param>
		/// <returns>True if the string was surrounded by any quote type.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static bool IsQuotedAny(this string s, out char quoteType) {
			quoteType = '\0';
			if (s.IsSurrounded('"'))
				quoteType = '"';
			else if (s.IsSurrounded('\''))
				quoteType = '\'';
			return quoteType != '\0';
		}

		#endregion

		#region Quote

		/// <summary>
		///  Surrounds the string with <paramref name="quoteType"/> if it is not already surrounded.
		/// </summary>
		/// <param name="s">The string to quote.</param>
		/// <param name="quoteType">The quote type to use.</param>
		/// <returns>Returns the string surrounded with <paramref name="quoteType"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Quote(this string s, char quoteType) {
			return s.Quote(quoteType, out _);
		}
		/// <summary>
		///  Surrounds the string with <paramref name="quoteType"/> if it is not already surrounded. Outputs if the
		///  string was already surrounded.
		/// </summary>
		/// <param name="s">The string to quote.</param>
		/// <param name="quoteType">The quote type to use.</param>
		/// <param name="quoted">True if the string was already quoted.</param>
		/// <returns>Returns the string surrounded with <paramref name="quoteType"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Quote(this string s, char quoteType, out bool quoted) {
			return s.Surround(quoteType, out quoted);
		}
		/// <summary>
		///  Surrounds the string with <paramref name="quoteType"/> if it is not already surrounded with either single
		///  or double quotes.
		/// </summary>
		/// <param name="s">The string to quote.</param>
		/// <param name="quoteType">The quote type to use.</param>
		/// <returns>
		///  Returns the string surrounded with <paramref name="quoteType"/> or single or double quotes.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string QuoteAny(this string s, char quoteType) {
			return s.QuoteAny(quoteType, out _);
		}
		/// <summary>
		///  Surrounds the string with <paramref name="quoteType"/> if it is not already surrounded with either single
		///  or double quotes. Outputs if the string was already surrounded.
		/// </summary>
		/// <param name="s">The string to quote.</param>
		/// <param name="quoteType">The quote type to use.</param>
		/// <param name="quoted">True if the string was already quoted.</param>
		/// <returns>
		///  Returns the string surrounded with <paramref name="quoteType"/> or single or double quotes.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string QuoteAny(this string s, char quoteType, out bool quoted) {
			quoted = s.IsQuotedAny();
			if (quoted)
				return s;
			return s.SurroundForce(quoteType);
		}

		/// <summary>
		///  Forcefully quotes the string with <paramref name="quoteType"/>, even if it is already quoted.
		/// </summary>
		/// <param name="s">The string to quote.</param>
		/// <param name="quoteType">The quote type to force-surround the string with.</param>
		/// <returns>The string with the added quotes.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string QuoteForce(this string s, char quoteType) {
			return s.SurroundForce(quoteType);
		}

		#endregion

		#region Unquote

		/// <summary>
		///  Unsurrounds the string with <paramref name="quoteType"/> if it is surrounded.
		/// </summary>
		/// <param name="s">The string to unquote.</param>
		/// <param name="quoteType">The quote type to remove.</param>
		/// <returns>The unquoted string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unquote(this string s, char quoteType) {
			return s.Unquote(quoteType, out _);
		}
		/// <summary>
		///  Unsurrounds the string with <paramref name="quoteType"/> if it is surrounded. Outputs if the string was
		///  previously quoted.
		/// </summary>
		/// <param name="s">The string to unquote.</param>
		/// <param name="quoteType">The quote type to remove.</param>
		/// <param name="quoted">True if the string was quoted.</param>
		/// <returns>The unquoted string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string Unquote(this string s, char quoteType, out bool quoted) {
			return s.Unsurround(quoteType, out quoted);
		}
		/// <summary>
		///  Unsurrounds the string with single or double quotes if it is surrounded.
		/// </summary>
		/// <param name="s">The string to unquote.</param>
		/// <returns>The unquoted string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string UnquoteAny(this string s) {
			return s.UnquoteAny(out _);
		}
		/// <summary>
		///  Unsurrounds the string with single or double quotes if it is surrounded. Outputs if the string was
		///  previously quoted.
		/// </summary>
		/// <param name="s">The string to unquote.</param>
		/// <param name="quoted">True if the string was quoted.</param>
		/// <returns>The unquoted string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="s"/> is null.
		/// </exception>
		public static string UnquoteAny(this string s, out bool quoted) {
			char quoteType = s.GetQuoteType();
			quoted = quoteType != '\0';
			if (quoted)
				return s.Unsurround(quoteType, out _);
			return s;
		}

		#endregion
	}
}
