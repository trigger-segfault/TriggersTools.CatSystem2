using System;
using System.Collections.Generic;
using System.Linq;
using TriggersTools.SharpUtils.Collections;

namespace TriggersTools.SharpUtils.IO {
	/// <summary>
	///  Extension methods for the <see cref="IDisposable"/> interface.
	/// </summary>
	public static class IDisposableExtensions {
		#region ToDisposableCollection

		/// <summary>
		///  Takes the source collection and adds it to a disposable collection.
		/// </summary>
		/// <typeparam name="T">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A new disposable collection containing the elements from the source collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="source"/> is null.
		/// </exception>
		public static DisposableCollection<T> ToDisposableCollection<T>(this IEnumerable<T> source)
			where T : IDisposable
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			return new DisposableCollection<T>(source.ToList());
		}

		#endregion

		#region ToDisposableArray

		/// <summary>
		///  Takes the source collection and adds it to a disposable array.
		/// </summary>
		/// <typeparam name="T">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A new disposable array containing the elements from the source collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="source"/> is null.
		/// </exception>
		public static DisposableArray<T> ToDisposableArray<T>(this IEnumerable<T> source)
			where T : IDisposable
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			return new DisposableArray<T>(source.ToArray());
		}
		/// <summary>
		///  Takes the source collection and adds it to a disposable array.
		/// </summary>
		/// <typeparam name="T">The element type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A new disposable array containing the elements from the source collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="source"/> is null.
		/// </exception>
		public static DisposableArray<T> ToDisposableArray<T>(this ICollection<T> source)
			where T : IDisposable
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			T[] newSource = new T[source.Count];
			source.CopyTo(newSource, 0);
			return new DisposableArray<T>(newSource);
		}
		/// <summary>
		///  Takes the source collection and adds it to a disposable array.
		/// </summary>
		/// <typeparam name="T">The element type of the array.</typeparam>
		/// <param name="source">The source array.</param>
		/// <returns>A new disposable array containing the elements from the source array.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="source"/> is null.
		/// </exception>
		public static DisposableArray<T> ToDisposableArray<T>(this T[] source)
			where T : IDisposable
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			T[] newSource = new T[source.Length];
			Array.Copy(source, newSource, source.Length);
			return new DisposableArray<T>(newSource);
		}
		
		#endregion
	}
}
