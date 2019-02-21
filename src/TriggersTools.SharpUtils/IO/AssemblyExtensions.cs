using System;
using System.Reflection;

namespace TriggersTools.SharpUtils.IO {
	public static class AssemblyExtensions {
		/// <summary>
		///  Gets the local build time from an embedded resource file storing the time as a string.
		/// </summary>
		/// <param name="assembly">The assembly to get the build time from.</param>
		/// <param name="path">The path to the embedded resource file.</param>
		/// <returns>The date time stored in the embedded resource file.</returns>
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
		public static DateTime GetEmbeddedBuildTime(this Assembly assembly, string path) {
			return DateTime.Parse(Embedded.ReadAllText(assembly, path).Trim()).ToLocalTime();
		}
		/// <summary>
		///  Gets the UTC build time from an embedded resource file storing the time as a string.
		/// </summary>
		/// <param name="assembly">The assembly to get the build time from.</param>
		/// <param name="path">The path to the embedded resource file.</param>
		/// <returns>The date time stored in the embedded resource file.</returns>
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
		public static DateTime GetEmbeddedBuildTimeUtc(this Assembly assembly, string path) {
			return DateTime.Parse(Embedded.ReadAllText(assembly, path).Trim()).ToUniversalTime();
		}
	}
}
