using System;
using System.IO;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A special type of stream used to keep track of the last-opened KIFINT archive.
	/// </summary>
	public sealed class KifintStream : IDisposable {
		#region Fields

		/// <summary>
		///  Getts the KIFINT archive for the currently open stream.
		/// </summary>
		public Kifint Kifint { get; private set; }
		/// <summary>
		///  Getts the stream for the currently open KIFINT archive.
		/// </summary>
		public Stream Stream { get; private set; }

		#endregion

		#region Properties
		
		/// <summary>
		///  Gets if a KIFINT archive stream is currently open.
		/// </summary>
		public bool IsOpen => Stream != null;
		/// <summary>
		///  Gets or sets the position within the current KIFINT archive stream.
		/// </summary>
		/// 
		/// <exception cref="IOException">
		///  An I/O error occured.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public long Position {
			get {
				if (Stream == null)
					throw new ObjectDisposedException($"No KIFINT archive is currently open!");
				return Stream.Position;
			}
			set {
				if (Stream == null)
					throw new ObjectDisposedException($"No KIFINT archive is currently open!");
				Stream.Position = value;
			}
		}
		/// <summary>
		///  Gets the length in bytes of the KIFINT archive stream.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public long Length {
			get {
				if (Stream == null)
					throw new ObjectDisposedException($"No KIFINT archive is currently open!");
				return Stream.Length;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unopen KIFINT archive stream.
		/// </summary>
		public KifintStream() { }
		/// <summary>
		///  Constructs and opens the file stream for the specified KIFINT archive and closes the existing stream if
		///  there is one.
		/// </summary>
		/// <param name="kifint">The KIFINT archive to open the stream for.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifint"/> is null.
		/// </exception>
		public KifintStream(Kifint kifint) {
			Open(kifint);
		}

		#endregion

		#region Open/Close

		/// <summary>
		///  Opens the file stream for the specified KIFINT archive and closes the existing stream if there is one.
		///  <para/>
		///  This method does not reopen the stream if the specified KIFINT archive is the currenly open archive.
		/// </summary>
		/// <param name="kifint">The KIFINT archive to open the stream for.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifint"/> is null.
		/// </exception>
		public void Open(Kifint kifint) {
			if (kifint == null)
				throw new ArgumentNullException(nameof(kifint));
			if (kifint == Kifint)
				return; // We're already open.
			Close();
			Stream = File.OpenRead(kifint.FilePath);
			Kifint = kifint;
		}
		/// <summary>
		///  Closes the currently opened KIFINT archive file stream, if there is one open.<para/>
		///  Otherwise does nothing.
		/// </summary>
		public void Close() {
			if (Stream != null) {
				Stream.Dispose();
				Stream = null;
				Kifint = null;
			}
		}

		#endregion

		#region Casting

		/// <summary>
		///  Casts the KIFINT archive stream to a regular stream.
		/// </summary>
		/// <param name="kifintStream">The KIFINT archive stream to cast.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public static implicit operator Stream(KifintStream kifintStream) {
			return kifintStream.Stream ?? throw new ObjectDisposedException($"The KIFINT archive stream cannot be " +
																			$"casted because it is not open!");
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Disposes of the KIFINT archive stream and closes the currently open stream.
		/// </summary>
		public void Dispose() {
			Close();
		}

		#endregion
	}
}
