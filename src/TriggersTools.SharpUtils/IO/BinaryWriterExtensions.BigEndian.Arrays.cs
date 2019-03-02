using System;
using System.IO;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryWriterExtensions {
		#region Write Array Big Endian (Signed)
		
		/// <summary>
		///  Writes an array of 2-byte signed integers to the current stream and advances the current position of the
		///  stream by two bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, short[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of 4-byte signed integers to the current stream and advances the current position of the
		///  stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, int[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of 8-byte signed integers to the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, long[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}

		#endregion

		#region Write Array Big Endian (Unsigned)

		/// <summary>
		///  Writes an array of 2-byte unsigned integers to the current stream and advances the current position of the
		///  stream by two bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, ushort[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of 4-byte unsigned integers to the current stream and advances the current position of the
		///  stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, uint[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of 8-byte unsigned integers to the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, ulong[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}

		#endregion

		#region Write Array (Floating)

		/// <summary>
		///  Writes an array of single-floating points to the current stream and advances the current position of the
		///  stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, float[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of double-floating points to the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, double[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}
		/// <summary>
		///  Writes an array of decimal-floating points to the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="buffer">The values to write.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void WriteBE(this BinaryWriter writer, decimal[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			for (int i = 0; i < values.Length; i++)
				writer.WriteBE(values[i]);
		}

		#endregion
	}
}
