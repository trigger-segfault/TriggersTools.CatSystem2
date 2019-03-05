using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOPath = System.IO.Path;

namespace TriggersTools.SharpUtils {
	/// <summary>
	///  A class solely used for Trigger's Tools & Games' temporary directory.
	/// </summary>
	public static class TriggerTemp {
		#region Constants

		/// <summary>
		///  The name of the temporary subdirectory for Trigger's Tools & Games.
		/// </summary>
		public const string Directory = "TriggersToolsGames";
		/// <summary>
		///  The path to the temporary directory for Trigger's Tools & Games.
		/// </summary>
		public static string Path { get; } = IOPath.Combine(IOPath.GetTempPath(), Directory);

		#endregion

		#region Combine

		/// <summary>
		///  Appends the path to the temporary <see cref="Path"/>.
		/// </summary>
		/// <param name="path">The path to append.</param>
		/// <returns>The combined file path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		public static string Combine(string path) {
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			return IOPath.Combine(Path, path);
		}
		/// <summary>
		///  Appends the path to the temporary <see cref="Path"/>.
		/// </summary>
		/// <param name="path1">The first path to append.</param>
		/// <param name="path2">The second path to append.</param>
		/// <returns>The combined file path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path1"/> or <paramref name="path2"/> is null.
		/// </exception>
		public static string Combine(string path1, string path2) {
			if (path1 == null)
				throw new ArgumentNullException(nameof(path1));
			if (path2 == null)
				throw new ArgumentNullException(nameof(path2));
			return IOPath.Combine(Path, path1, path2);
		}
		/// <summary>
		///  Appends the paths to the temporary <see cref="Path"/>.
		/// </summary>
		/// <param name="paths">The paths to append.</param>
		/// <returns>The combined file path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> or one of its elements is null.
		/// </exception>
		public static string Combine(params string[] paths) {
			if (paths == null)
				throw new ArgumentNullException(nameof(paths));
			string fullPath = Path;
			for (int i = 0; i < paths.Length; i++) {
				string path = paths[i];
				if (path == null)
					throw new ArgumentNullException($"{nameof(paths)}[{i}]");
				fullPath = IOPath.Combine(fullPath, path);
			}
			return fullPath;
		}

		#endregion
	}
}
