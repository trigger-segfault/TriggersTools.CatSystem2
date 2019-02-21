using System;
using System.IO;
using System.Linq;
using System.Text;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryReaderExtensions {
		#region ReadFixedString

		/// <summary>
		///  Reads a string from the current stream. The string is of the specified fixed length and ends with the
		///  <paramref name="padding"/> character.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="length">The length of the string to read.</param>
		/// <param name="padding">The character to trim from the end of the string.</param>
		/// <returns>The string that was read from the stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="length"/> is negative.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  The number of decoded characters to read is greater than <paramref name="length"/>. This can happen if a
		///  Unicode decoder returns fallback characters or a surrogate pair.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occured.
		/// </exception>
		public static string ReadFixedString(this BinaryReader reader, int length, char padding) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);

			string fullString = new string(reader.ReadChars(length));
			int paddingIndex = fullString.IndexOf(padding);
			if (paddingIndex != -1)
				return fullString.Substring(paddingIndex);
			return fullString;
		}

		/// <summary>
		///  Reads a string from the current stream. The string is of the specified fixed length.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="length">The length of the string to read.</param>
		/// <returns>The string that was read from the stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="length"/> is negative.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  The number of decoded characters to read is greater than <paramref name="length"/>. This can happen if a
		///  Unicode decoder returns fallback characters or a surrogate pair.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occured.
		/// </exception>
		public static string ReadFixedString(this BinaryReader reader, int length) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);

			return new string(reader.ReadChars(length));
		}

		#endregion

		#region ReadTerminatedString

		/// <summary>
		///  Reads a string from the current stream. The string is terminated with the specified character so the
		///  length is variable.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="terminator">The character to terminate the string with.</param>
		/// <returns>The string that was read from the stream.</returns>
		/// 
		/// <exception cref="EndOfStreamException">
		///  The end of stream was reached before the terminated was encountered.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occured.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  A surrogate character was read.
		/// </exception>
		public static string ReadTerminatedString(this BinaryReader reader, char terminator = '\0') {
			StringBuilder str = new StringBuilder();
			char c = reader.ReadChar();
			while (c != terminator) {
				str.Append(c);
				c = reader.ReadChar();
			}
			return str.ToString();
		}

		/// <summary>
		/// Reads a string from the current stream. The string is terminated with any of the specified
		/// characters.
		/// </summary>
		/// 
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="terminators">The characters the string can be terminated with.</param>
		/// <returns>The string that was read from the stream.</returns>
		/// 
		/// <exception cref="EndOfStreamException">
		/// The end of stream was reached before the terminated was encountered.
		/// </exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="IOException">An I/O error occured.</exception>
		/// <exception cref="ArgumentException">A surrogate character was read.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="terminators"/> is null.</exception>
		public static string ReadTerminatedString(this BinaryReader reader, params char[] terminators) {
			return reader.ReadTerminatedString(out _, terminators);
		}

		/// <summary>
		/// Reads a string from the current stream. The string is terminated with any of the specified
		/// characters.
		/// </summary>
		/// 
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="terminator">The resulting terminator that was encountered.</param>
		/// <param name="terminators">The characters the string can be terminated with.</param>
		/// <returns>The string that was read from the stream.</returns>
		/// 
		/// <exception cref="EndOfStreamException">
		/// The end of stream was reached before the terminated was encountered.
		/// </exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="IOException">An I/O error occured.</exception>
		/// <exception cref="ArgumentException">A surrogate character was read.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="terminators"/> is null.</exception>
		public static string ReadTerminatedString(this BinaryReader reader, out char terminator,
			params char[] terminators)
		{
			StringBuilder str = new StringBuilder();
			char c = reader.ReadChar();
			while (terminators.Contains(c)) {
				str.Append(c);
				c = reader.ReadChar();
			}
			terminator = c;
			return str.ToString();
		}

		#endregion
	}
}
