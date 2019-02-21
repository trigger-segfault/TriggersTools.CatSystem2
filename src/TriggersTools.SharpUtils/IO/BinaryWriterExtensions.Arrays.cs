using System;
using System.IO;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryWriterExtensions {
		#region Write Array (Signed)

		/// <summary>
		///  Writes an array of 1-byte signed integers to the current stream and advances the current position of the
		///  stream by <paramref name="count"/> bytes.
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
		public static void Write(this BinaryWriter writer, sbyte[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length;
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
		}
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, short[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(short);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, int[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(int);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, long[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(long);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
		}

		#endregion

		#region Write Array (Unsigned)

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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, ushort[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(ushort);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, uint[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(int);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, ulong[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(long);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, float[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(float);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, double[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(double);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
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
		/// <exception cref="OverflowException">
		///  The number of bytes to write is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Write(this BinaryWriter writer, decimal[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int bufferSize = values.Length * sizeof(decimal);
			byte[] buffer = new byte[bufferSize];
			Buffer.BlockCopy(values, 0, buffer, 0, bufferSize);
			writer.Write(buffer);
		}

		#endregion
	}
}
