using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  A static class with helper methods for files.
	/// </summary>
	public static class FileUtils {
		#region Constants

		/// <summary>
		///  The buffer size used for <see cref="ReEncode"/>.
		/// </summary>
		private const int ReEncodeBufferSize = 128 * 1024;

		#endregion

		#region ReEncode

		/// <summary>
		///  Re-encodes the source file from <paramref name="dstEncoding"/> and outputs it to the destination file.
		/// </summary>
		/// <param name="srcPath">The source file to re-encode.</param>
		/// <param name="dstPath">The destination re-encoded file.</param>
		/// <param name="dstEncoding">The encoding of the destination file.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="srcPath"/>, <paramref name="dstPath"/>, or <paramref name="newEncoding"/> is null.
		/// </exception>
		public static void ReEncode(string srcPath, string dstPath, Encoding dstEncoding) {
			ReEncode(srcPath, dstPath, null, dstEncoding);
		}
		/// <summary>
		///  Re-encodes the source file from <paramref name="srcEncoding"/> to <paramref name="dstEncoding"/> and
		///  outputs it to the destination file.
		/// </summary>
		/// <param name="srcPath">The source file to re-encode.</param>
		/// <param name="dstPath">The destination re-encoded file.</param>
		/// <param name="srcEncoding">The encoding of the source file.</param>
		/// <param name="dstEncoding">The encoding of the destination file.</param>
		/// 
		/// <remarks>
		///  If <paramref name="srcEncoding"/> is null, the reader will attempt to guess the file's encoding.
		/// </remarks>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="srcPath"/>, <paramref name="dstPath"/>, or <paramref name="newEncoding"/> is null.
		/// </exception>
		public static void ReEncode(string srcPath, string dstPath, Encoding srcEncoding, Encoding dstEncoding) {
			if (srcPath == null)
				throw new ArgumentNullException(nameof(srcPath));
			if (dstPath == null)
				throw new ArgumentNullException(nameof(dstPath));
			if (dstEncoding == null)
				throw new ArgumentNullException(nameof(dstEncoding));
			using (StreamReader reader = new StreamReader(srcPath, srcEncoding ?? Encoding.UTF8, true))
			using (StreamWriter writer = new StreamWriter(dstPath, false, dstEncoding)) {
				int charsRead;
				char[] buffer = new char[ReEncodeBufferSize];
				while ((charsRead = reader.ReadBlock(buffer, 0, buffer.Length)) > 0) {
					writer.Write(buffer, 0, charsRead);
				}
			}
		}

		#endregion
	}
}
