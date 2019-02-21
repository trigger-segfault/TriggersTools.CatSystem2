using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.Collections {
	public static class EnumerableExtensions {
		public static int IndexOf<T>(this IEnumerable<T> source, T value) {
			int index = 0;
			var comparer = EqualityComparer<T>.Default; // or pass in as a parameter
			foreach (T item in source) {
				if (comparer.Equals(item, value)) return index;
				index++;
			}
			return -1;
		}
	}
}
