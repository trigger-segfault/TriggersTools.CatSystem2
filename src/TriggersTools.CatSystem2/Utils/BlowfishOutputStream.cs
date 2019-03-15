using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  An output stream for encrypting blowfish data as it's written. This does NOT support seeking.
	/// </summary>
#if DEBUG_LIBRARY
		public
#else
	internal
#endif
	sealed class BlowfishOutputStream : Stream {
		#region Constants

		/// <summary>
		///  The default size of the buffer used to decrypt data before it's written.
		/// </summary>
		private const int DefaultBufferSize = short.MaxValue;//81920;

		#endregion

		#region Fields

		public Blowfish Blowfish { get; }
		private readonly Stream stream;
		private readonly byte[] decryptedBuffer;
		//private int decryptedLength;
		//private int decryptedPosition;
		private int virtualPosition;
		private readonly bool leaveOpen;

		#endregion

		public BlowfishOutputStream(Blowfish blowfish, Stream stream)
			: this(blowfish, stream, DefaultBufferSize, false)
		{
		}
		public BlowfishOutputStream(Blowfish blowfish, Stream stream, bool leaveOpen)
			: this(blowfish, stream, DefaultBufferSize, leaveOpen)
		{
		}
		public BlowfishOutputStream(Blowfish blowfish, Stream stream, int bufferSize)
			: this(blowfish, stream, bufferSize, false)
		{
		}
		public BlowfishOutputStream(Blowfish blowfish, Stream stream, int bufferSize, bool leaveOpen) {
			Blowfish = blowfish ?? throw new ArgumentNullException(nameof(blowfish));
			this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
			if (bufferSize < 8)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(bufferSize), bufferSize, 8, true);
			if (bufferSize % 8 != 0)
				throw new ArgumentException($"{nameof(bufferSize)} must be a multiple of 8!");
			if (!stream.CanWrite)
				throw new ArgumentException($"{nameof(BlowfishOutputStream)} only supports streams that can write!");

			decryptedBuffer = new byte[bufferSize];
			virtualPosition = 0;
			this.leaveOpen = leaveOpen;
		}

		#region Stream Overrides

		public override void Flush() {
			// Write remaining unaligned buffer as unencrypted data
			int mod = virtualPosition % 8;
			if (mod != 0)
				stream.Write(decryptedBuffer, 0, mod);
			stream.Flush();
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				Flush();
				if (!leaveOpen)
					stream.Close();
			}
		}

		public override long Seek(long offset, SeekOrigin origin) {
			throw new NotSupportedException();
			/*switch (origin) {
			case SeekOrigin.Begin:
				return Position = offset;
			case SeekOrigin.Current:
				return Position += offset;
			case SeekOrigin.End:
				return Position = Length + offset;
			}
			return Position;*/
		}

		public override void SetLength(long value) {
			if (value != 0)
				throw new NotSupportedException($"{nameof(BlowfishOutputStream)} only supports setting length to 0!");
			stream.SetLength(0);
		}
		
		public void GetWriteBlocks(int count, out int flushCount, out bool flushEncrypt, out int newCount, out int edgeCount) {
			int virtualPosition = this.virtualPosition;

			// Leftover Buffer
			int flush = MathUtils.Padding(virtualPosition, 8);
			if (flush > 0) {
				flushCount = Math.Min(flush, count);
				count -= flushCount;
				virtualPosition += flushCount;
				flushEncrypt = (flushCount == flush);
				if (count == 0) {
					newCount = 0;
					edgeCount = 0;
					return;
				}
			}
			else {
				flushEncrypt = false;
				flushCount = 0;
			}

			// New Uncharted
			int @new = count - count % 8;
			if (@new > 0) {
				newCount = @new;
				count -= newCount;
				virtualPosition += newCount;
				if (count == 0) {
					edgeCount = 0;
					return;
				}
			}
			else {
				newCount = 0;
			}

			int edge = count;
			if (edge > 0) {
				edgeCount = count;
				return;
			}
			else {
				edgeCount = 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Floor8(int x) => x - x % 8;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long Floor8(long x) => x - x % 8;

		public override int Read(byte[] buffer, int offset, int count) {
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count) {
			if (count == 0)
				return;
			
			GetWriteBlocks(count, out int flushCount, out bool flushEncrypt, out int newCount, out int edgeCount);

			if (flushCount != 0) {
				Buffer.BlockCopy(buffer, offset, decryptedBuffer, virtualPosition % 8, flushCount);
				Blowfish.Encrypt(decryptedBuffer, 8);
				stream.Write(decryptedBuffer, 0, 8);
				virtualPosition += flushCount;
				if (!flushEncrypt)
					return;
				offset += flushCount;
				stream.Write(decryptedBuffer, 0, 8);
			}

			if (newCount != 0) {
				Buffer.BlockCopy(buffer, offset, decryptedBuffer, 0, newCount);
				Blowfish.Encrypt(decryptedBuffer, newCount);
				stream.Write(decryptedBuffer, 0, newCount);
				virtualPosition += newCount;
				offset += newCount;
			}

			if (edgeCount != 0) {
				Buffer.BlockCopy(buffer, offset, decryptedBuffer, 0, edgeCount);
				virtualPosition += edgeCount;
				//offset += edgeCount;
			}
		}

		public override bool CanRead => false;
		public override bool CanSeek => false;
		public override bool CanWrite => stream.CanWrite;
		public override long Length => stream.Length;
		public override long Position {
			get => virtualPosition;
			set => throw new NotSupportedException();
		}

		#endregion
	}
}
