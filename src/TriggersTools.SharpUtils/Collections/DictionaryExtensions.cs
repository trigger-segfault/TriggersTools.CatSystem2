using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.Collections {
	public static class DictionaryExtensions {

		/// <summary>
		///  Creates a copy of the dictionary and reverses the key value pairs in a dictionary with the same key and
		///  value type.
		/// </summary>
		/// <typeparam name="T">The key and value type of the dictionary.</typeparam>
		/// <param name="dictionary">The dictionary to reverse.</param>
		/// <returns>The reversed key value dictionary copy.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="dictionary"/> is null.
		/// </exception>
		public static Dictionary<T, T> ReverseKeyValues<T>(this IReadOnlyDictionary<T, T> dictionary) {
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));
			Dictionary<T, T> reversed = new Dictionary<T, T>();
			foreach (var pair in dictionary) {
				reversed.Add(pair.Value, pair.Key);
			}
			return reversed;
		}
	}
}
