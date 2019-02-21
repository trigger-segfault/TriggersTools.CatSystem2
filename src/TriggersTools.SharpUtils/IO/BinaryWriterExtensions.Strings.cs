using System;
using System.IO;
using System.Text;
using TriggersTools.SharpUtils.Collections;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryWriterExtensions {
		#region WriteFixed (string)

		/// <summary>
		///  Writes a fixed-length string without any length information to this stream in the current encoding of the
		///  <see cref="BinaryWriter"/>, and advances the current position of the stream in accordance with the
		///  encoding used and the specific characters being written to the stream.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="value">The string to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="value"/> is null.
		/// </exception>
		public static void WriteFixed(this BinaryWriter writer, string value) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			writer.Write(value.ToCharArray());
		}
		/// <summary>
		///  Writes a fixed-length string to this stream in the current encoding of the <see cref="BinaryWriter"/>, and
		///  advances the current position of the stream in accordance with the encoding used and the specific
		///  characters being written to the stream.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="value">The string to write.</param>
		/// <param name="length">The fixed length of the string.</param>
		/// <param name="padding">The character to pad the string with when it is less than the fixed length.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="value"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="length"/> is less than zero.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> has a length longer than <paramref name="length"/>.
		/// </exception>
		public static void WriteFixed(this BinaryWriter writer, string value, int length, char padding = '\0') {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);
			if (length < value.Length)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, $"{nameof(value)}.Length",
					value.Length, false);
			if (value.IndexOf(padding) != -1)
				throw new ArgumentException($"{nameof(value)} contains padding character '{padding}'!");

			writer.Write(value.PadRight(length, padding).ToCharArray());
		}

		#endregion

		#region WriteTerminated (string)

		/// <summary>
		///  Writes a string with a terminator at the end to this stream in the current encoding of the
		///  <see cref="BinaryWriter"/>, and advances the current position of the stream in accordance with the
		///  encoding used and the specific characters being written to the stream.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="value">The string to write.</param>
		/// <param name="terminator">The character to terminate the string with.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="value"/> is null.
		/// </exception>
		public static void WriteTerminated(this BinaryWriter writer, string value, char terminator = '\0') {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (value.IndexOf(terminator) != -1)
				throw new ArgumentException($"{nameof(value)} contains terminator character '{terminator}'!");

			writer.Write(value.ToCharArray());
			writer.Write(terminator);
		}

		#endregion
	}
}
