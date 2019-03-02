using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  Extensions for the <see cref="BinaryReader"/> class.
	/// </summary>
	public static partial class BinaryReaderExtensions {
		#region ReadToEnd

		/// <summary>
		///  Reads the remaining bytes in the stream and advances the current position.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>A byte array with the remaining bytes.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
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
		public static byte[] ReadToEnd(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return reader.BaseStream.ReadToEnd();
		}

		#endregion

		#region Skip

		/// <summary>
		///  Skips the specified number of bytes in the stream.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of bytes to skip.</param>
		/// <returns>The number of bytes, actually skipped.</returns>
		/// 
		/// <remarks>
		///  This method utilizes <see cref="Stream.Seek(long, SeekOrigin)"/> if it's <see cref="Stream.CanSeek"/> is
		///  true. Otherwise it reads bytes to a buffer of size 4096 until <paramref name="count"/> is reached.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is negative.
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
		public static int Skip(this BinaryReader reader, int count) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return reader.BaseStream.Skip(count);
		}
		/// <summary>
		///  Skips the specified number of bytes in the stream.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="count">The number of bytes to skip.</param>
		/// <param name="bufferSize">
		///  The size of the buffer to read bytes into at a time when <see cref="Stream.CanSeek"/> is false.
		/// </param>
		/// <returns>The number of bytes, actually skipped.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is less than zero.-or- <paramref name="bufferSize"/> is less than one.
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
		public static int Skip(this BinaryReader reader, int count, int bufferSize) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return reader.BaseStream.Skip(count, bufferSize);
		}

		#endregion

		#region ReadIntPtr
		
		/// <summary>
		///  Reads a 4-byte signed integer pointer from the current stream and advances the current position of the
		///  stream by four bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>A 4-byte signed integer pointer read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static IntPtr ReadInt32Ptr(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new IntPtr(reader.ReadInt32());
		}
		/// <summary>
		///  Reads a 4-byte unsigned integer pointer from the current stream and advances the current position of the
		///  stream by four bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>A 4-byte unsigned integer pointer read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		public static UIntPtr ReadUInt32Ptr(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new UIntPtr(reader.ReadUInt32());
		}

		/// <summary>
		///  Reads a 8-byte signed integer pointer from the current stream and advances the current position of the
		///  stream by eight bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>A 8-byte signed integer pointer read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  On a 32-bit platform, value is too large or too small to represent as an <see cref="IntPtr"/>.
		/// </exception>
		public static IntPtr ReadInt64Ptr(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new IntPtr(reader.ReadInt64());
		}
		/// <summary>
		///  Reads a 8-byte unsigned integer pointer from the current stream and advances the current position of the
		///  stream by eight bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>A 8-byte unsigned integer pointer read from the current stream.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="EndOfStreamException">
		///  The end of the stream is reached.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="OverflowException">
		///  On a 32-bit platform, value is too large or too small to represent as an <see cref="IntPtr"/>.
		/// </exception>
		public static UIntPtr ReadUInt64Ptr(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new UIntPtr(reader.ReadUInt64());
		}

		#endregion

		#region ReadUnmanaged

		/// <summary>
		///  Reads an unmanaged struct from the stream and increments the position.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="type">The unmanaged type.</param>
		/// <returns>The unmanaged object as a managed object.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> or <paramref name="type"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is a reference type that is not a formatted class.
		/// </exception>
		public static object ReadUnmanaged(this BinaryReader reader, Type type) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			byte[] buffer = reader.ReadBytes(Marshal.SizeOf(type));
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				return Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
			}
			finally {
				handle.Free();
			}
		}
		/// <summary>
		///  Reads an unmanaged struct from the stream and increments the position.
		/// </summary>
		/// <typeparam name="T">The unmanaged type.</typeparam>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>The unmanaged object as a managed object.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <typeparamref name="T"/> is a reference type that is not a formatted class.
		/// </exception>
		public static T ReadUnmanaged<T>(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			byte[] buffer = reader.ReadBytes(Marshal.SizeOf<T>());
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
			}
			finally {
				handle.Free();
			}
		}

		/// <summary>
		///  Reads an array of unmanaged structs from the stream.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="type">The unmanaged type.</param>
		/// <param name="length">The length of the array to read.</param>
		/// <returns>The array of unmanaged objects as a managed objects.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> or <paramref name="type"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="length"/> is negative.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is a reference type that is not a formatted class.
		/// </exception>
		public static Array ReadUnmanagedArray(this BinaryReader reader, Type type, int length) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);

			Array result = Array.CreateInstance(type, length);
			if (length == 0)
				return result;
			int size = Marshal.SizeOf(type);
			byte[] buffer = reader.ReadBytes(size * length);

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr ptr = handle.AddrOfPinnedObject();

				for (int i = 0; i < length; i++) {
					result.SetValue(Marshal.PtrToStructure(ptr, type), i);
					ptr = new IntPtr(ptr.ToInt64() + size);
				}
				return result;
			}
			finally {
				handle.Free();
			}
		}
		/// <summary>
		///  Reads an array of unmanaged structs from the stream.
		/// </summary>
		/// <typeparam name="T">The unmanaged type.</typeparam>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="length">The length of the array.</param>
		/// <returns>The array of unmanaged objects as a managed objects.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="length"/> is negative.
		/// </exception>
		/// <exception cref="IOException">
		///  An I/O error occurred.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  The stream is closed.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <typeparamref name="T"/> is a reference type that is not a formatted class.
		/// </exception>
		public static T[] ReadUnmanagedArray<T>(this BinaryReader reader, int length) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);

			T[] result = new T[length];
			if (length == 0)
				return result;
			int size = Marshal.SizeOf<T>();
			byte[] buffer = reader.ReadBytes(size * length);

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr ptr = handle.AddrOfPinnedObject();

				for (int i = 0; i < length; i++) {
					result[i] = Marshal.PtrToStructure<T>(ptr);
					ptr = new IntPtr(ptr.ToInt64() + size);
				}
				return result;
			}
			finally {
				handle.Free();
			}
		}

		#endregion

		#region Read7BitEncoded

		/// <summary>
		///  Reads out a 4-bit signed integer, 7 bits at a time. The high bit of the byte when on means to continue
		///  reading more bytes. Increments the stream by up to 5 bytes.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <returns>The read signed 4-bit integer.</returns>
		/// 
		/// <remarks>
		///  7-bit encoded integers are used by some microsoft formats, such as XNB files. They are also used
		///  internally by <see cref="BinaryReader.ReadString"/>.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		///  The number is larger than 5 bytes.
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
		public static int Read7BitEncodedInt(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			int count = 0;
			int shift = 0;
			byte b;
			do {
				// Check for a corrupted stream.  Read a max of 5 bytes.
				// In a future version, add a DataFormatException.
				if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
					throw new FormatException("Bad 7-bit Int32 Format!");

				// ReadByte handles end of stream cases for us.
				b = reader.ReadByte();
				count |= (b & 0x7F) << shift;
				shift += 7;
			} while ((b & 0x80) != 0);
			return count;
		}
		// BinaryReader.ReadString() already does this
		/*public static string Read7BitEncodedString(this BinaryReader reader) {
			int length = reader.Read7BitEncodedInt();
			return Encoding.UTF8.GetString(reader.ReadBytes(length));
		}*/

		#endregion

		#region NewEncoding

		/// <summary>
		///  Returns a new <see cref="BinaryReader"/> with the modified encoding.
		/// </summary>
		/// <param name="reader">The origal reader to get the stream from.</param>
		/// <param name="encoding">The new encoding to use.</param>
		/// <returns>Returns the new <see cref="BinaryReader"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="encoding"/> is null.
		/// </exception>
		public static BinaryReader NewEncoding(this BinaryReader reader, Encoding encoding) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new BinaryReader(reader.BaseStream, encoding);
		}
		/// <summary>
		///  Returns a new <see cref="BinaryReader"/> with the modified encoding.
		/// </summary>
		/// <param name="reader">The origal reader to get the stream from.</param>
		/// <param name="encoding">The new encoding to use.</param>
		/// <param name="leaveOpen">
		///  True to leave the stream open after the <see cref="BinaryReader"/> object is disposed; otherwise, false.
		/// </param>
		/// <returns>Returns the new <see cref="BinaryReader"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="encoding"/> is null.
		/// </exception>
		public static BinaryReader NewEncoding(this BinaryReader reader, Encoding encoding, bool leaveOpen) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			return new BinaryReader(reader.BaseStream, encoding, leaveOpen);
		}

		#endregion
	}
}
