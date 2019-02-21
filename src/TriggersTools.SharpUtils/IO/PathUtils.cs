using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>A helper with extra methods for paths, files, and directories.</summary>
	public static class PathUtils {

		//-----------------------------------------------------------------------------
		// Constants
		//-----------------------------------------------------------------------------

		/// <summary>The stored executable path for the entry assembly.</summary>
		public static readonly string ExePath =
			Assembly.GetEntryAssembly().Location;

		/// <summary>The directory of the entry executable.</summary>
		public static readonly string ExeDirectory =
			Path.GetDirectoryName(ExePath);

		/// <summary>Gets the file name of the entry executable.</summary>
		public static readonly string ExeFile =
			Path.GetFileName(ExePath);

		/// <summary>Gets the file name of the entry executable without its extension.</summary>
		public static readonly string ExeName =
			Path.GetFileNameWithoutExtension(ExePath);

		/// <summary>Provides a platform-specific character used to separate directory
		/// levels in a path string that reflects a hierarchical file system
		/// organization.</summary>
		public static readonly char[] DirectorySeparators = new char[] {
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar,
		};

		//-----------------------------------------------------------------------------
		// Methods
		//-----------------------------------------------------------------------------

		/// <summary>Returns true if the fileName has valid characters for search
		/// patterns.</summary>
		public static bool IsValidNamePattern(string name) {
			name = name.Replace("*", "").Replace("?", "");
			return IsValidName(name);
		}

		/// <summary>Returns true if the filePath has valid characters for search
		/// patterns.</summary>
		public static bool IsValidPathPattern(string path) {
			path = path.Replace("*", "").Replace("?", "");
			return IsValidPath(path);
			//return path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
		}

		/// <summary>Returns true if the fileName has valid characters.</summary>
		public static bool IsValidName(string name) {
			return name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
		}

		/// <summary>Returns true if the filePath has valid characters.</summary>
		public static bool IsValidPath(string path) {
			try {
				Path.GetFullPath(path);
				return true;
			}
			catch {
				return false;
			}
			//return path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
		}

		/// <summary>Returns true if the filePath has valid characters and does not
		/// lead to a directory.</summary>
		public static bool IsValidFile(string path) {
			return IsValidPath(path) &&
				!Directory.Exists(path);
		}

		/// <summary>Returns true if the filePath has valid characters and does not
		/// lead to a file.</summary>
		public static bool IsValidDirectory(string path) {
			return IsValidPath(path) &&
				!File.Exists(path);
		}

		/// <summary>Returns true if the filePath has valid characters and is not
		/// rooted.</summary>
		public static bool IsValidRelativePath(string path) {
			if (Path.IsPathRooted(path))
				return false;
			return IsValidPath(path);
		}

		/// <summary>Returns a path that can be compared with another normalized path.</summary>
		public static string NormalizePath(string path) {
			return Path.GetFullPath(path)
					   .TrimEnd(DirectorySeparators)
					   .ToUpperInvariant();
		}

		/// <summary>Returns true if the two paths lead to the same location.</summary>
		public static bool IsPathTheSame(string path1, string path2) {
			return string.Compare(
				NormalizePath(path1), NormalizePath(path2), true) == 0;
		}

		/// <summary>Combines the specified paths with the executable directory.</summary>
		public static string CombineExecutable(string path1) {
			return Path.Combine(ExeDirectory, path1);
		}

		/// <summary>Combines the specified paths with the executable directory.</summary>
		public static string CombineExecutable(string path1, string path2) {
			return Path.Combine(ExeDirectory, path1, path2);
		}

		/// <summary>Combines the specified paths with the executable directory.</summary>
		public static string CombineExecutable(string path1, string path2,
			string path3) {
			return Path.Combine(ExeDirectory, path1, path2, path3);
		}

		/// <summary>Combines the specified paths with the executable directory.</summary>
		public static string CombineExecutable(params string[] paths) {
			return Path.Combine(ExeDirectory, Path.Combine(paths));
		}

		/// <summary>Gets the proper capitalization of a path so it looks nice.</summary>
		public static string GetProperDirectoryCapitalization(string dir) {
			string path = GetProperDirectoryCapitalization(new DirectoryInfo(dir));
			if (path.Length >= 2 && char.IsLetter(path[0]) &&
				path[1] == ':' && path[2] == '\\')
			{
				path = char.ToUpper(path[0]) + path.Substring(1);
			}
			return path;
		}

		/// <summary>Gets the proper capitalization of a path so it looks nice.</summary>
		private static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo) {
			DirectoryInfo parentDirInfo = dirInfo.Parent;
			if (null == parentDirInfo)
				return dirInfo.Name;
			return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
								parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
		}

		/// <summary>Returns a collection of all files and subfiles in the directory.</summary>
		public static List<string> GetAllFiles(string directory) {
			List<string> files = new List<string>();
			AddAllFiles(files, directory);
			return files;
		}

		/// <summary>Returns a collection of all files and subfiles in the directory.</summary>
		public static IEnumerable<string> EnumerateAllFiles(string directory, string pattern) {
			foreach (string file in Directory.EnumerateFiles(directory, pattern)) {
				yield return file;
			}
			foreach (string dir in Directory.EnumerateDirectories(directory)) {
				foreach (string file in EnumerateAllFiles(dir, pattern))
					yield return file;
			}
		}

		public static bool IsDirectoryEmpty(string directory) {
			return !Directory.EnumerateFileSystemEntries(directory).Any();
		}

		/// <summary>Returns a collection of all files and subfiles in the directory.</summary>
		public static void DeleteAllEmptyDirectories(string directory) {
			foreach (string dir in Directory.GetDirectories(directory)) {
				if (IsDirectoryEmpty(dir)) {
					Directory.Delete(dir);
				}
				else {
					DeleteAllEmptyDirectories(dir);
					if (IsDirectoryEmpty(dir))
						Directory.Delete(dir);
				}
			}
		}
		

		//-----------------------------------------------------------------------------
		// Internal Methods
		//-----------------------------------------------------------------------------

		/// <summary>Adds all of the files and subfiles in the directory to the list.</summary>
		private static void AddAllFiles(List<string> files, string directory) {
			foreach (string file in Directory.GetFiles(directory)) {
				files.Add(file);
			}
			foreach (string dir in Directory.GetDirectories(directory)) {
				AddAllFiles(files, dir);
			}
		}
	}
}
