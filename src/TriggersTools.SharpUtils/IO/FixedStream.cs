using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.IO {
	/// <summary>
	///  A stream that is fixed to the specified offset and length.
	/// </summary>
	public sealed class FixedStream : Stream {
		#region Fields
		
		private readonly Stream stream;
		private readonly int fixedLength;
		private readonly long zeroPosition;
		private readonly bool leaveOpen;

		#endregion

		public FixedStream(Stream stream, int length) : this(stream, length, false) { }
		public FixedStream(Stream stream, int length, bool leaveOpen) {
			this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
			if (length < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(length), length, 0, true);
			if (!stream.CanRead)
				throw new ArgumentException($"{nameof(FixedStream)} only supports streams that can read!");
			fixedLength = length;
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
		public override int Read(byte[] buffer, int offset, int count) {
			count = (int) Math.Min(count, fixedLength - Position);
			if (count >= 0)
				return stream.Read(buffer, offset, count);
			return 0;
		}

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotSupportedException($"{nameof(Write)} is not supported by {nameof(FixedStream)}!");
		}
		public override void SetLength(long value) {
			throw new NotSupportedException($"{nameof(SetLength)} is not supported by {nameof(FixedStream)}!");
		}

		public override bool CanRead => stream.CanRead;
		public override bool CanSeek => stream.CanSeek;
		public override bool CanWrite => false;
		public override long Length => fixedLength;
		public override long Position {
			get => stream.Position - zeroPosition;
			set {
				if (value < 0)
					throw ArgumentOutOfRangeUtils.OutsideMin(nameof(Position), value, 0, true);
				stream.Position = value + zeroPosition;
			}
		}

		#endregion
	}
}
