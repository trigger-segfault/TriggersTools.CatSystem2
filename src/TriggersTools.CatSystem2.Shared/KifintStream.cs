using System;
using System.IO;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A special type of wrapper for a stream used to keep track of the last-opened KIFINT archive.<para/>
	///  Use this while bulk-extracting KIFINT entries from archives to keep a direct stream open to the archive.
	///  Make sure to dispose of this class when you are done.
	/// </summary>
	public sealed class KifintStream : IDisposable {
		#region Fields

		/// <summary>
		///  Gets the KIFINT archive information for the currently open stream.
		/// </summary>
		public KifintArchiveInfo Kifint { get; private set; }
		/// <summary>
		///  Getts the stream for the currently open KIFINT archive.
		/// </summary>
		public FileStream Stream { get; private set; }

		#endregion

		#region Properties
		
		/// <summary>
		///  Gets if a KIFINT archive stream is currently open.
		/// </summary>
		public bool IsOpen => Stream != null && Stream.CanRead;
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
				if (!IsOpen)
					throw new ObjectDisposedException($"No KIFINT archive is currently open!");
				return Stream.Position;
			}
			set {
				if (!IsOpen)
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
				if (!IsOpen)
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
		public KifintStream(KifintArchive kifint) {
			Open(kifint);
		}

		#endregion

		#region Open/Close

		/// <summary>
		///  Checks to make sure a stream is open, not disposed of, and leads to the KIFINT archive's file path.
		/// </summary>
		/// <param name="kifint">The KIFINT archive information to check the stream for.</param>
		/// <returns>True if the stream is open, not closed, and targets <paramref name="kifint"/>'s file.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifint"/> is null.
		/// </exception>
		public bool IsKifintOpen(KifintArchiveInfo kifint) {
			if (kifint == null)
				throw new ArgumentNullException(nameof(kifint));
			// Make sure we have the same file stream open and it's not disposed of.
			// CanRead is used to determine if the stream has been disposed of.
			string fullPath = Path.GetFullPath(kifint.FilePath);
			return (Stream != null && Stream.CanRead && string.Compare(Stream.Name, fullPath, true) == 0);
		}
		/// <summary>
		///  Opens the file stream for the specified KIFINT archive and closes the existing stream if there is one.
		///  <para/>
		///  This method does not reopen the stream if the specified KIFINT archive is the currenly open archive.
		/// </summary>
		/// <param name="kifint">The KIFINT archive information to open the stream for.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifint"/> is null.
		/// </exception>
		public void Open(KifintArchiveInfo kifint) {
			if (IsKifintOpen(kifint))
				return; // We're already open.
			Close();
			Stream = File.OpenRead(kifint.FilePath);
			Kifint = kifint;
		}
		/// <summary>
		///  Closes the currently opened KIFINT archive file stream, if there is one open.
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
		///  Casts the KIFINT archive stream to the currently open file stream.
		/// </summary>
		/// <param name="kifintStream">The KIFINT archive stream to cast.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		///  <see cref="IsOpen"/> is false.
		/// </exception>
		public static implicit operator Stream(KifintStream kifintStream) {
			if (kifintStream == null)
				return null;
			if (!kifintStream.IsOpen) {
				throw new ObjectDisposedException($"The KIFINT archive stream cannot be " +
												  $"casted because it is not open!");
			}
			return kifintStream.Stream;
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
