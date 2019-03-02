using System;
using System.IO;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryReaderExtensions {
		#region Read Array Big Endian (Signed)
		
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
		public static short[] ReadInt16sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			short[] values = new short[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadInt16BE();
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
		public static int[] ReadInt32sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			int[] values = new int[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadInt32BE();
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
		public static long[] ReadInt64sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			long[] values = new long[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadInt64BE();
			return values;
		}

		#endregion

		#region Read Array Big Endian (Unsigned)

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
		public static ushort[] ReadUInt16sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			ushort[] values = new ushort[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadUInt16BE();
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
		public static uint[] ReadUInt32sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			uint[] values = new uint[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadUInt32BE();
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
		public static ulong[] ReadUInt64sBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			ulong[] values = new ulong[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadUInt64BE();
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
		public static float[] ReadSinglesBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			float[] values = new float[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadSingleBE();
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
		public static double[] ReadDoublesBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			double[] values = new double[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadDoubleBE();
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
		public static decimal[] ReadDecimalsBE(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			
			decimal[] values = new decimal[count];
			for (int i = 0; i < count; i++)
				values[i] = reader.ReadDecimalBE();
			return values;
		}

		#endregion
	}
}
