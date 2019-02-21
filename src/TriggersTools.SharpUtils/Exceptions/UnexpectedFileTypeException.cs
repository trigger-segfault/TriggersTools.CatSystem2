using System;
using System.IO;

namespace TriggersTools.SharpUtils.Exceptions {
	/// <summary>
	///  An exception thrown when the file is not of the valid type.
	/// </summary>
	public class UnexpectedFileTypeException : Exception {
		#region Fields

		/// <summary>
		///  Gets the name of the invalid file.
		/// </summary>
		public string FileName { get; }
		/// <summary>
		///  Gets the type that the file was expected to be.
		/// </summary>
		public string ExpectedType { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the exception and creates a message based on the expected type.
		/// </summary>
		/// <param name="expectedType">The excepted type that the file was supposed to be.</param>
		public UnexpectedFileTypeException(string expectedType)
			: base($"File is not a valid {expectedType} file!")
		{
			ExpectedType = expectedType;
		}
		/// <summary>
		///  Constructs the exception and creates a message based on the file name and expected type.
		/// </summary>
		/// <param name="fileName">The name or path to the file that was invalid.</param>
		/// <param name="expectedType">The excepted type that the file was supposed to be.</param>
		public UnexpectedFileTypeException(string fileName, string expectedType)
			: base($"'{Path.GetFileName(fileName)}' is not a valid {expectedType} file!")
		{
			FileName = Path.GetFileName(fileName);
			ExpectedType = expectedType;
		}

		#endregion

		#region Public Static Helpers

		/// <summary>
		///  Throws an unexpected file type exception when <paramref name="type"/> does not match
		///  <paramref name="expectedType"/>.
		/// </summary>
		/// <param name="type">The type of the file.</param>
		/// <param name="expectedType">The expected type of the file.</param>
		/// 
		/// <exception cref="UnexpectedFileTypeException">
		///  <paramref name="type"/> does not match <paramref name="expectedType"/>.
		/// </exception>
		public static void ThrowIfInvalid(string type, string expectedType) {
			if (type != expectedType)
				throw new UnexpectedFileTypeException(expectedType);
		}
		/// <summary>
		///  Throws an unexpected file type exception when <paramref name="type"/> does not match
		///  <paramref name="expectedType"/>.
		/// </summary>
		/// <param name="fileName">The name or path to the file that was invalid.</param>
		/// <param name="type">The type of the file.</param>
		/// <param name="expectedType">The expected type of the file.</param>
		/// 
		/// <exception cref="UnexpectedFileTypeException">
		///  <paramref name="type"/> does not match <paramref name="expectedType"/>.
		/// </exception>
		public static void ThrowIfInvalid(string fileName, string type, string expectedType) {
			if (type != expectedType)
				throw new UnexpectedFileTypeException(fileName, expectedType);
		}

		#endregion
	}
}
