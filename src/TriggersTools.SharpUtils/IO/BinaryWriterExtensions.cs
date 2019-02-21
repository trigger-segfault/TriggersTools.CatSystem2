using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  Extensions for the <see cref="BinaryWriter"/> class.
	/// </summary>
	public static partial class BinaryWriterExtensions {
		#region WriteUnmanaged

		/// <summary>
		///  Writes an unmanaged object to the stream and increments the position.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="value">The value to write to the stream.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="value"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="value"/> is a reference type that is not a formatted class.
		/// </exception>
		public static void WriteUnmanaged(this BinaryWriter writer, object value) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			byte[] buffer = new byte[Marshal.SizeOf(value)];
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), true);
				writer.Write(buffer);
			} finally {
				handle.Free();
			}
		}
		/// <summary>
		///  Writes an unmanaged type to the stream and increments the position.
		/// </summary>
		/// <typeparam name="T">The unmanaged type.</typeparam>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="value">The value to write to the stream.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="value"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="value"/> is a reference type that is not a formatted class.
		/// </exception>
		public static void WriteUnmanaged<T>(this BinaryWriter writer, T value) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			byte[] buffer = new byte[Marshal.SizeOf<T>()];
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), true);
				writer.Write(buffer);
			} finally {
				handle.Free();
			}
		}

		/// <summary>
		///  Writes an array of unmanaged types to the stream and increments the position.
		/// </summary>
		/// <typeparam name="T">The unmanaged type.</typeparam>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="values">The array of values to write to the stream.</param>
		/// 
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> or one of its elements is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  An element in <paramref name="values"/> is a reference type that is not a formatted class.
		/// </exception>
		public static void WriteUnmanagedArray<T>(this BinaryWriter writer, T[] values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int length = values.Length;
			int size = Marshal.SizeOf<T>();
			byte[] buffer = new byte[size * length];

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr ptr = handle.AddrOfPinnedObject();
				for (int i = 0; i < length; i++, ptr += size) {
					T value = values[i];
					if (value == null)
						throw new ArgumentNullException($"{nameof(values)}[{i}]");
					Marshal.StructureToPtr(value, ptr, true);
				}
				writer.Write(buffer);
			} finally {
				handle.Free();
			}
		}
		/// <summary>
		///  Writes an array of unmanaged types to the stream and increments the position.
		/// </summary>
		/// <typeparam name="T">The unmanaged type.</typeparam>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="values">The list of values to write to the stream.</param>
		/// 
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="values"/> or one of its elements is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  An element in <paramref name="values"/> is a reference type that is not a formatted class.
		/// </exception>
		public static void WriteUnmanagedArray<T>(this BinaryWriter writer, IReadOnlyList<T> values) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			int length = values.Count;
			int size = Marshal.SizeOf<T>();
			byte[] buffer = new byte[size * length];

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr ptr = handle.AddrOfPinnedObject();
				for (int i = 0; i < length; i++, ptr += size) {
					T value = values[i];
					if (value == null)
						throw new ArgumentNullException($"{nameof(values)}[{i}]");
					Marshal.StructureToPtr(value, ptr, true);
				}
				writer.Write(buffer);
			} finally {
				handle.Free();
			}
		}

		#endregion

		#region Write7BitEncoded

		/// <summary>
		///  Writes out a 4-bit signed integer, 7 bits at a time. The high bit of the byte when on means to continue
		///  reading more bytes. Increments the stream by up to 5 bytes.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <returns>The read signed 4-bit integer.</returns>
		/// 
		/// <remarks>
		///  7-bit encoded integers are used by some microsoft formats, such as XNB files. They are also used
		///  internally by <see cref="BinaryWriter.Write(string)"/>.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static void Write7BitEncodedInt(this BinaryWriter writer, int value) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			uint v = unchecked((uint) value);   // support negative numbers
			while (v >= 0x80) {
				writer.Write((byte) (v | 0x80));
				v >>= 7;
			}
			writer.Write((byte) v);
		}
		// BinaryWriter.Write(string) already does this
		/*public static void Write7BitEncodedString(this BinaryWriter writer, string value) {
			writer.Write7BitEncodedInt(value.Length);
			writer.Write(Encoding.UTF8.GetBytes(value));
		}*/

		#endregion

		#region NewEncoding

		/// <summary>
		///  Returns a new <see cref="BinaryWriter"/> with the modified encoding.
		/// </summary>
		/// <param name="encoding">The new encoding to use.</param>
		/// <returns>Returns the new <see cref="BinaryWriter"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="encoding"/> is null.
		/// </exception>
		public static BinaryWriter NewEncoding(this BinaryWriter writer, Encoding encoding) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			return new BinaryWriter(writer.BaseStream, encoding, true);
		}
		/// <summary>
		///  Returns a new <see cref="BinaryWriter"/> with the modified encoding.
		/// </summary>
		/// <param name="encoding">The new encoding to use.</param>
		/// <param name="leaveOpen">
		///  True to leave the stream open after the <see cref="BinaryWriter"/> object is disposed; otherwise, false.
		/// </param>
		/// <returns>Returns the new <see cref="BinaryWriter"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="encoding"/> is null.
		/// </exception>
		public static BinaryWriter NewEncoding(this BinaryWriter writer, Encoding encoding, bool leaveOpen) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			return new BinaryWriter(writer.BaseStream, encoding, leaveOpen);
		}

		#endregion

		#region Pad

		/// <summary>
		///  Writes 0 bytes to the stream and advances the position to conform to a padding amount.
		/// </summary>
		/// <param name="stream">The stream to advance.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The amount of bytes written.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="padding"/> is less than or equal to zero..
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support seeking or reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static int Pad(this BinaryWriter writer, int padding) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			return writer.Pad(padding, 0);
		}
		/// <summary>
		///  Writes 0 bytes to the stream and advances the position conform to a padding amount with the supplied
		///  additional offset.
		/// </summary>
		/// <param name="stream">The stream to advance.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The amount of bytes written.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="padding"/> is less than or equal to zero.-or- <paramref name="offset"/> is less than zero.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///  The stream does not support seeking or reading.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static int Pad(this BinaryWriter writer, int padding, int offset) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (padding <= 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(padding), padding, 1, false);
			if (offset < 0)
				throw ArgumentOutOfRangeUtils.OutsideRange(nameof(offset), padding, 0, nameof(padding), padding, true,
					false);
			byte[] bytes = new byte[(int) MathUtils.Padding(writer.BaseStream.Position, padding, offset)];
			writer.Write(bytes);
			return bytes.Length;
		}

		#endregion
	}
}
