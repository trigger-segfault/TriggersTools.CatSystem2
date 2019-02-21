using System;
using System.IO;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  A disposable directory that functions in using statements.
	/// </summary>
	public class TempDirectory : IDisposable, IComparable, IComparable<string>, IComparable<TempDirectory>,
		IEquatable<string>, IEquatable<TempDirectory>
	{
		#region Fields

		/// <summary>
		///  Gets the path to the temporary directory.
		/// </summary>
		public string Path { get; }
		/// <summary>
		///  Gets the full path to the temporary directory.
		/// </summary>
		public string FullPath { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the temporary directory with the specified path and then creates the directory.
		/// </summary>
		/// <param name="path">The path of the directory.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		public TempDirectory(string path) : this(path, true) { }
		/// <summary>
		///  Constructs the temporary directory with specified path and optionally creates the directory.
		/// </summary>
		/// <param name="path">The path to the directory.</param>
		/// <param name="createDir">True if the directory should be created.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		public TempDirectory(string path, bool createDir) {
			Path = path ?? throw new ArgumentNullException(nameof(path));
			FullPath = System.IO.Path.GetFullPath(path);
			if (createDir)
				Directory.CreateDirectory(FullPath);
		}

		#endregion

		#region Object Overrides

		/// <summary>
		///  Gets the string representation of the directory by its path.
		/// </summary>
		/// <returns>The path of the directory.</returns>
		public override string ToString() => Path;
		
		public override bool Equals(object obj) {
			if (obj is TempDirectory tempDir)
				return Equals(tempDir.Path);
			if (obj is string path)
				return Equals(path);
			return false;
		}
		public bool Equals(string other) => Path == other;
		public bool Equals(TempDirectory other) => Path == other?.Path;

		public int CompareTo(object obj) {
			if (obj is TempDirectory tempDir)
				return CompareTo(tempDir);
			if (obj is string path)
				return CompareTo(path);
			throw new ArgumentException(nameof(obj));
		}
		public int CompareTo(string other) => Path.CompareTo(other);
		public int CompareTo(TempDirectory other) => Path.CompareTo(other?.Path);

		public override int GetHashCode() => Path.GetHashCode();

		#endregion

		#region Operators

		public static bool operator ==(TempDirectory a, TempDirectory b) => a?.Path == b?.Path;
		public static bool operator ==(TempDirectory a,        string b) => a?.Path == b;
		public static bool operator ==(       string a, TempDirectory b) => a == b?.Path;
		public static bool operator !=(TempDirectory a, TempDirectory b) => a?.Path != b?.Path;
		public static bool operator !=(TempDirectory a,        string b) => a?.Path != b;
		public static bool operator !=(       string a, TempDirectory b) => a != b?.Path;

		#endregion

		#region Casting

		/// <summary>
		///  Implicitly casts the temporary directory to its path.
		/// </summary>
		/// <param name="tempDir">The temporary directory to cast.</param>
		public static implicit operator string(TempDirectory tempDir) {
			return tempDir?.Path;
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Disposes of the temporary directory by deleting it from the file system.
		/// </summary>
		public void Dispose() {
			if (Directory.Exists(FullPath))
				Directory.Delete(FullPath, true);
		}

		#endregion
	}
}
