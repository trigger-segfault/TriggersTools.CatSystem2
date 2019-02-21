using System;
using System.IO;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryReaderExtensions {
		#region Read Array (Signed)

		/// <summary>
		///  Reads an array of 1-byte signed integers from the current stream and advances the current position of the
		///  stream by <paramref name="count"/> bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 1-byte signed integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static sbyte[] ReadSBytes(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			sbyte[] values = new sbyte[count];
			byte[] buffer = reader.ReadBytes(count);
			Buffer.BlockCopy(buffer, 0, values, 0, count);
			return values;
		}
		/// <summary>
		///  Reads an array of 2-byte signed integers from the current stream and advances the current position of the
		///  stream by two bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 2-byte signed integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static short[] ReadInt16s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(short);
			short[] values = new short[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of 4-byte signed integers from the current stream and advances the current position of the
		///  stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 4-byte signed integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int[] ReadInt32s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(int);
			int[] values = new int[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of 8-byte signed integers from the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 8-byte signed integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static long[] ReadInt64s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(long);
			long[] values = new long[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}

		#endregion

		#region Read Array (Unsigned)

		/// <summary>
		///  Reads an array of 2-byte unsigned integers from the current stream and advances the current position of
		///  the stream by two bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 2-byte unsigned integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static ushort[] ReadUInt16s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(ushort);
			ushort[] values = new ushort[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of 4-byte unsigned integers from the current stream and advances the current position of
		///  the stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 4-byte unsigned integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static uint[] ReadUInt32s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(uint);
			uint[] values = new uint[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of 8-byte unsigned integers from the current stream and advances the current position of
		///  the stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of 8-byte unsigned integers read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static ulong[] ReadUInt64s(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(ulong);
			ulong[] values = new ulong[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}

		#endregion

		#region Read Array (Floating)

		/// <summary>
		///  Reads an array of single-floating points from the current stream and advances the current position of the
		///  stream by four bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of single-floating points read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static float[] ReadSingles(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(float);
			float[] values = new float[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of double-floating points from the current stream and advances the current position of the
		///  stream by eight bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of double-floating points read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static double[] ReadDoubles(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(double);
			double[] values = new double[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}
		/// <summary>
		///  Reads an array of decimal-floating points from the current stream and advances the current position of the
		///  stream by sixteen bytes times <paramref name="count"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of values to read.</param>
		/// <returns>An array of decimal-floating points read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  The number of bytes to read is greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static decimal[] ReadDecimals(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);

			int bufferSize = count * sizeof(decimal);
			decimal[] values = new decimal[count];
			byte[] buffer = reader.ReadBytes(bufferSize);
			Buffer.BlockCopy(buffer, 0, values, 0, bufferSize);
			return values;
		}

		#endregion
	}
}
