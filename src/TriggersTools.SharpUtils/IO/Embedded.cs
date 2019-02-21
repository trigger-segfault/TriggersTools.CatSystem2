using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  A static helper class for loading embedded resource streams.
	/// </summary>
	public static class Embedded {
		#region GetResources

		/// <summary>
		///  Returns the names of all the resources in the calling assembly.
		/// </summary>
		/// <returns>An array that contains the names of all embedded resources.</returns>
		public static string[] GetResources() {
			return GetResources(Assembly.GetCallingAssembly());
		}
		/// <summary>
		///  Returns the names of all the resources in the calling assembly that start with the specified path.
		/// </summary>
		/// <param name="path">The path resources must start with.</param>
		/// <returns>An array that contains the names of all the matching embedded resources.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		public static string[] GetResources(string path) {
			return GetResources(Assembly.GetCallingAssembly(), path);
		}
		/// <summary>
		///  Returns the names of all the resources in this assembly.
		/// </summary>
		/// <param name="assembly">The assembly to get the resources of.</param>
		/// <returns>An array that contains the names of all embedded resources.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		public static string[] GetResources(Assembly assembly) {
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));
			return assembly.GetManifestResourceNames();
		}
		/// <summary>
		///  Returns the names of all the resources in this assembly that start with the specified path.
		/// </summary>
		/// <param name="assembly">The assembly to get the resources of.</param>
		/// <param name="path">The path resources must start with.</param>
		/// <returns>An array that contains the names of all the matching embedded resources.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="path"/> is null.
		/// </exception>
		public static string[] GetResources(Assembly assembly, string path) {
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			return GetResources(assembly).Where(s => s.StartsWith(path, StringComparison.InvariantCultureIgnoreCase)).ToArray();
		}
		/// <summary>
		///  Returns the names of all the resources in type's the assembly.
		/// </summary>
		/// <param name="type">The type whose assembly is used to get the resources of.</param>
		/// <returns>An array that contains the names of all embedded resources.</returns>
		public static string[] GetResources(Type type) {
			return GetResources(type.Assembly);
		}
		/// <summary>
		///  Returns the names of all the resources in the type's assembly that start with the specified path.
		/// </summary>
		/// <param name="type">The type whose assembly is used to get the resources of.</param>
		/// <param name="path">The path resources must start with.</param>
		/// <returns>An array that contains the names of all the matching embedded resources.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/> or <paramref name="path"/> is null.
		/// </exception>
		public static string[] GetResources(Type type, string path) {
			return GetResources(type.Assembly, path);
		}

		#endregion

		#region Open

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly.
		/// </summary>
		/// <param name="path">The paths to combine to create the resource path.</param>
		/// <returns>The <see cref="Stream"/> for the resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static Stream Open(string path) {
			return Open(Assembly.GetCallingAssembly(), path);
		}
		/*/// <summary>
		///  Loads the specified manifest resource from the calling assembly.
		/// </summary>
		/// <param name="paths">The paths to combine to create the resource path.</param>
		/// <returns>The <see cref="Stream"/> for the resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> is null.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static Stream Open(params string[] paths) {
			return Open(Assembly.GetCallingAssembly(), Combine(paths));
		}*/
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <returns>The <see cref="Stream"/> for the resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static Stream Open(Assembly assembly, string path) {
			Stream stream = assembly.GetManifestResourceStream(path);
			return stream ?? throw new ResourceNotFoundException(assembly, path);
		}
		/*/// <summary>
		///  Loads the specified manifest resource from the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="paths">The paths to combine to create the resource path.</param>
		/// <returns>The <see cref="Stream"/> for the resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="paths"/> is null.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static Stream GetStream(Assembly assembly, params string[] paths) {
			return GetStream(assembly, Combine(paths));
		}*/
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <returns>The <see cref="Stream"/> for the resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/> or <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static Stream Open(Type type, string name) {
			Stream stream = type.Assembly.GetManifestResourceStream(type, name);
			return stream ?? throw new ResourceNotFoundException(type, name);
		}

		#endregion

		#region ReadAllBytes

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly as a byte array.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <returns>The byte array of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static byte[] ReadAllBytes(string path) {
			return ReadAllBytes(Assembly.GetCallingAssembly(), path);
		}
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly as a byte array.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <returns>The byte array of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static byte[] ReadAllBytes(Assembly assembly, string path) {
			using (Stream stream = Open(assembly, path))
				return stream.ReadToEnd();
		}
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  as a byte array
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <returns>The byte array of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/> or <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static byte[] ReadAllBytes(Type type, string name) {
			using (Stream stream = Open(type, name))
				return stream.ReadToEnd();
		}

		#endregion

		#region ReadAllText

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly as a string.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(string path) {
			return ReadAllText(Assembly.GetCallingAssembly(), path, Encoding.UTF8);
		}
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly as a string.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(Assembly assembly, string path) {
			return ReadAllText(assembly, path, Encoding.UTF8);
		}
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  as a string.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/> or <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(Type type, string name) {
			return ReadAllText(type, name, Encoding.UTF8);
		}

		#endregion

		#region ReadAllText (Encoding)

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly as a string.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(string path, Encoding encoding) {
			return ReadAllText(Assembly.GetCallingAssembly(), path, encoding);
		}
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly as a string.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/>, <paramref name="path"/>, or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(Assembly assembly, string path, Encoding encoding) {
			using (Stream stream = Open(assembly, path))
			using (StreamReader reader = new StreamReader(stream, encoding))
				return reader.ReadToEnd();
		}
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  as a string.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/>, <paramref name="name"/>, or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string ReadAllText(Type type, string name, Encoding encoding) {
			using (Stream stream = Open(type, name))
			using (StreamReader reader = new StreamReader(stream, encoding))
				return reader.ReadToEnd();
		}

		#endregion

		#region ReadAllLines

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly as a string array of lines.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(string path) {
			return ReadAllLines(Assembly.GetCallingAssembly(), path, Encoding.UTF8);
		}
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly as a string array of lines.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> or <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(Assembly assembly, string path) {
			return ReadAllLines(assembly, path, Encoding.UTF8);
		}
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  as a string array of lines.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/> or <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(Type type, string name) {
			return ReadAllLines(type, name, Encoding.UTF8);
		}

		#endregion

		#region ReadAllLines (Encoding)

		/// <summary>
		///  Loads the specified manifest resource from the calling assembly as a string array of lines.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(string path, Encoding encoding) {
			return ReadAllLines(Assembly.GetCallingAssembly(), path, encoding);
		}
		/// <summary>
		///  Loads the specified manifest resource from the specified assembly as a string array of lines.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/>, <paramref name="path"/>, or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(Assembly assembly, string path, Encoding encoding) {
			using (Stream stream = Open(assembly, path))
			using (StreamReader reader = new StreamReader(stream, encoding)) {
				string line;
				List<string> lines = new List<string>();

				while ((line = reader.ReadLine()) != null)
					lines.Add(line);
				return lines.ToArray();
			}
		}
		/// <summary>
		///  Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  as a string array of lines.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <param name="encoding">The encoding applied to the contents of the resource.</param>
		/// <returns>The string array of lines of the resource's data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/>, <paramref name="name"/>, or <paramref name="encoding"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static string[] ReadAllLines(Type type, string name, Encoding encoding) {
			using (Stream stream = Open(type, name))
			using (StreamReader reader = new StreamReader(stream, encoding)) {
				string line;
				List<string> lines = new List<string>();

				while ((line = reader.ReadLine()) != null)
					lines.Add(line);
				return lines.ToArray();
			}
		}

		#endregion

		#region SaveToFile

		/// <summary>
		///  Saves the specified manifest resource from the calling assembly to the specified file.
		/// </summary>
		/// <param name="path">The resource path.</param>
		/// <param name="file">The path of the file to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> or <paramref name="file"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToFile(string path, string file) {
			SaveToFile(Assembly.GetCallingAssembly(), path, file);
		}
		/// <summary>
		/// Saves the specified manifest resource from the specified assembly to the specified file.
		/// </summary>
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <param name="file">The path of the file to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/>, <paramref name="path"/>, or <paramref name="file"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToFile(Assembly assembly, string path, string file) {
			using (Stream inStream = Open(assembly, path))
			using (FileStream outStream = File.Create(file))
				inStream.CopyTo(outStream);
		}
		/// <summary>
		///  Saves the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  to the specified file.
		/// </summary>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <param name="file">The path of the file to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/>, <paramref name="name"/>, or <paramref name="file"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToFile(Type type, string name, string file) {
			using (Stream inStream = Open(type, name))
			using (FileStream outStream = File.Create(file))
				inStream.CopyTo(outStream);
		}

		#endregion

		#region SaveToStream

		/// <summary>
		///  Saves the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  to the specified stream.
		/// </summary>
		/// 
		/// <param name="path">The resource path.</param>
		/// <param name="stream">The stream to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> or <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToStream(string path, Stream stream) {
			SaveToStream(Assembly.GetCallingAssembly(), path, stream);
		}

		/// <summary>
		///  Saves the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  to the specified stream.
		/// </summary>
		/// 
		/// <param name="assembly">The assembly to load the resource from.</param>
		/// <param name="path">The resource path.</param>
		/// <param name="stream">The stream to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/>, <paramref name="path"/>, or <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToStream(Assembly assembly, string path, Stream stream) {
			using (Stream inStream = Open(assembly, path))
				inStream.CopyTo(stream);
		}
		/// <summary>
		///  Saves the specified manifest resource, scoped by the namespace of the specified type, from this assembly
		///  to the specified stream.
		/// </summary>
		/// 
		/// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
		/// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
		/// <param name="stream">The stream to save the resource to.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="type"/>, <paramref name="name"/>, or <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="name"/> is an empty string.
		/// </exception>
		/// <exception cref="ResourceNotFoundException">
		///  The resource could not be located.
		/// </exception>
		public static void SaveToStream(Type type, string name, Stream stream) {
			using (Stream inStream = Open(type, name))
				inStream.CopyTo(stream);
		}

		#endregion

		#region Combine

		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s.
		/// </summary>
		/// 
		/// <param name="paths">The paths to combine.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> or any of its values are null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  One of <paramref name="paths"/>'s values is an empty string.
		/// </exception>
		public static string Combine(params string[] paths) {
			foreach (string path in paths)
				CombineCheckPath(path);
			return string.Join(".", paths);
			//return string.Join(".", paths.Select(p => CheckPath(p)));
		}
		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s.
		/// </summary>
		/// 
		/// <param name="paths">The paths to combine.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> or any of its values are null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  One of <paramref name="paths"/>'s values is an empty string.
		/// </exception>
		public static string Combine(IEnumerable<string> paths) {
			return string.Join(".", paths.Select(p => CombineCheckPath(p)));
		}
		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s.
		/// </summary>
		/// 
		/// <param name="startPath">The first path to add.</param>
		/// <param name="paths">The remaining paths to add.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="startPath"/> or <paramref name="paths"/> or any of its values are null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="startPath"/> or one of <paramref name="paths"/>'s values is an empty string.
		/// </exception>
		public static string Combine(string startPath, IEnumerable<string> paths) {
			paths = new[] { startPath }.Concat(paths);
			//paths = paths.Prepend(startPath);
			return string.Join(".", paths.Select(p => CombineCheckPath(p)));
		}

		#endregion

		#region CombineIgnoreNull

		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s. Ignores all null strings.
		/// </summary>
		/// 
		/// <param name="paths">The paths to combine.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> is null.
		/// </exception>
		public static string CombineIgnoreNull(params string[] paths) {
			return CombineIgnoreNull((IEnumerable<string>) paths);
		}

		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s. Ignores all null strings.
		/// </summary>
		/// 
		/// <param name="paths">The paths to combine.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> is null.
		/// </exception>
		public static string CombineIgnoreNull(IEnumerable<string> paths) {
			paths = paths.Where(p => p != null);
			return string.Join(".", paths.Select(p => CombineCheckPath(p)));
		}
		/// <summary>
		///  Combines the embedded paths with a '.' separating each part and trimming '.'s. Ignores all null strings.
		/// </summary>
		/// 
		/// <param name="startPath">The first path to add.</param>
		/// <param name="paths">The remaining paths to add.</param>
		/// <returns>The combined resource path.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="paths"/> is null.
		/// </exception>
		public static string CombineIgnoreNull(string startPath, IEnumerable<string> paths) {
			paths = new[] { startPath }.Concat(paths).Where(p => p != null);
			//paths = paths.Prepend(startPath).Where(p => p != null);
			return string.Join(".", paths.Select(p => CombineCheckPath(p)));
		}

		#endregion

		#region Private

		/// <summary>
		///  Performs a null or empty check of the path during <see cref="Combine"/> and
		///  <see cref="CombineIgnoreNull"/>.
		/// </summary>
		/// <param name="path">The path to check.</param>
		/// <returns>The same string that was passed in.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="path"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="path"/> is an empty string.
		/// </exception>
		private static string CombineCheckPath(string path) {
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException($"{nameof(path)} is an empty or whitespace string!");
			return path;
		}

		#endregion
	}
}
