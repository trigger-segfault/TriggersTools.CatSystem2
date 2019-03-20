using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  An input stream for decrypting blowfish data as it's read.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	sealed class BlowfishInputStream : Stream {
		#region Constants

		private const int DefaultBufferSize = 8;//4096;//81920;

		#endregion

		#region Fields

		public Blowfish Blowfish { get; }
		private readonly Stream stream;
		private readonly byte[] decryptedBuffer;
		//private readonly int readAhead;
		private int decryptedLength;
		private int decryptedPosition;
		private int virtualPosition;
		private readonly int virtualLength;
		private readonly int cutoffLength;
		private readonly long zeroPosition;
		private readonly bool leaveOpen;

		#endregion

		public BlowfishInputStream(Blowfish blowfish, Stream stream, int length)
			: this(blowfish, stream, length, DefaultBufferSize, false)
		{
		}
		public BlowfishInputStream(Blowfish blowfish, Stream stream, int length, bool leaveOpen)
			: this(blowfish, stream, length, DefaultBufferSize, leaveOpen)
		{
		}
		public BlowfishInputStream(Blowfish blowfish, Stream stream, int length, int bufferSize)
			: this(blowfish, stream, length, bufferSize, false)
		{
		}
		public BlowfishInputStream(Blowfish blowfish, Stream stream, int length, int bufferSize, bool leaveOpen) {
			Blowfish = blowfish ?? throw new ArgumentNullException(nameof(blowfish));
			this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);
			if (bufferSize < 8)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(bufferSize), bufferSize, 8, true);
			if (bufferSize % 8 != 0)
				throw new ArgumentException($"{nameof(bufferSize)} must be a multiple of 8!");
			if (!stream.CanRead)
				throw new ArgumentException($"{nameof(BlowfishInputStream)} only supports streams that can read!");
			decryptedBuffer = new byte[bufferSize];
			decryptedLength = 0;
			decryptedPosition = 0;
			virtualPosition = 0;
			virtualLength = length;
			cutoffLength = length - length % 8;
			zeroPosition = stream.Position;
			this.leaveOpen = leaveOpen;
		}

		#region Stream Overrides

		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (!leaveOpen)
					stream.Close();
			}
		}
		public override void Flush() {
			stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			switch (origin) {
			case SeekOrigin.Begin:
				return Position = offset;
			case SeekOrigin.Current:
				return Position += offset;
			case SeekOrigin.End:
				return Position = Length + offset;
			}
			return Position;
		}

		public override void SetLength(long value) {
			throw new NotSupportedException();
		}
		
		public void GetReadBlocks(int count, out int leftoverCount, out int newCount, out int edgeCount, out int outsideCount) {
			int virtualPosition = this.virtualPosition;

			// Leftover Buffer
			int leftover = ((decryptedPosition + decryptedLength) - virtualPosition);
			if (leftover > 0 && virtualPosition > decryptedPosition) {
				leftoverCount = Math.Min(leftover, count);
				count -= leftoverCount;
				virtualPosition += leftoverCount;
				if (count == 0) {
					newCount = 0;
					edgeCount = 0;
					outsideCount = 0;
					return;
				}
			}
			else {
				leftoverCount = 0;
			}

			// New Uncharted
			int @new = (cutoffLength - virtualPosition);
			if (@new > 0) {
				newCount = Math.Min(@new, count - count % 8);
				count -= newCount;
				virtualPosition += newCount;
				if (count == 0) {
					edgeCount = 0;
					outsideCount = 0;
					return;
				}
			}
			else {
				newCount = 0;
			}

			int edge = cutoffLength - virtualPosition;
			if (count < edge) {
				edgeCount = count;
				outsideCount = 0;
				return;
			}
			else {
				edgeCount = 0;
			}

			int outside = virtualLength - virtualPosition;
			if (outside > 0) {
				outsideCount = Math.Min(outside, count);
			}
			else {
				outsideCount = 0;
			}
		}
		
		public override int Read(byte[] buffer, int offset, int count) {
			if (count == 0)
				return 0;

			int startVirtualPosition = virtualPosition;

			int countdown = count;
			GetReadBlocks(count, out int leftoverCount, out int newCount, out int edgeCount, out int outsideCount);

			if (leftoverCount != 0) {
				int decryptedIndex = (virtualPosition - decryptedPosition);
				Array.Copy(decryptedBuffer, decryptedIndex, buffer, offset, leftoverCount);
				offset += leftoverCount;
				virtualPosition += leftoverCount;
			}

			if (newCount != 0) {
				stream.Read(buffer, offset, newCount);
				Blowfish.Decrypt(buffer, offset, newCount);
				offset += newCount;
				virtualPosition += newCount;
			}

			if (edgeCount != 0) {
				ReadAndDecryptToBuffer(virtualPosition);
				Array.Copy(decryptedBuffer, 0, buffer, offset, edgeCount);
				//offset += edgeCount;
				virtualPosition += edgeCount;
			}
			else if (outsideCount != 0) {
				// Read unencrypted
				stream.Read(buffer, offset, outsideCount);
				virtualPosition += outsideCount;
			}

			return virtualPosition - startVirtualPosition;
		}

		private void ReadAndDecryptToBuffer(int decryptedPosition) {
			this.decryptedPosition = decryptedPosition;
			decryptedLength = Math.Min(cutoffLength - decryptedPosition, decryptedBuffer.Length);
			stream.Read(decryptedBuffer, 0, decryptedLength);
			Blowfish.Decrypt(decryptedBuffer, 0, decryptedLength);
		}

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotSupportedException();
		}

		public override bool CanRead => stream.CanRead;
		public override bool CanSeek => stream.CanSeek;
		public override bool CanWrite => false;
		public override long Length => virtualLength;
		public override long Position {
			get => virtualPosition;
			set {
				virtualPosition = (int) value;

				// Are we outside the leftover buffer?
				if (value < decryptedPosition || value > decryptedPosition + decryptedLength) {
					if (value >= cutoffLength) {
						stream.Position = value + zeroPosition;
					}
					else {
						int mod = virtualPosition % 8;
						int newPosition = virtualPosition - mod;
						stream.Position = zeroPosition + newPosition;
						if (mod != 0) {
							// Decrypt the next bufferSize bytes because we'll need them when we start reading
							ReadAndDecryptToBuffer(newPosition);
						}
					}
				}
				else {
					// Place the stream position at the end of the read decrypted data
					stream.Position = decryptedPosition + decryptedLength + zeroPosition;
				}
			}
		}
		public long BasePosition {
			get => stream.Position - zeroPosition;
			set => stream.Position = value + zeroPosition;
		}

		#endregion
	}
}
