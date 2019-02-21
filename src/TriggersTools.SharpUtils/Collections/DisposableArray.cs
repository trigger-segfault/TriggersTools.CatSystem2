using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.Collections {
	/// <summary>
	///  An array of disposable objects that can be cleaned up in one using statement.
	/// </summary>
	/// <typeparam name="T">The type of disposable object contained in the collection.</typeparam>
	public class DisposableArray<T> : ArrayCollection<T>, IDisposable where T : IDisposable {
		#region Constructors

		/// <summary>
		///  Constructs an array of disposable type <typeparamref name="T"/> and the specified length.
		/// </summary>
		/// <param name="length">The length of the array.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="length"/> is less than zero.
		/// </exception>
		public DisposableArray(int length) : base(length) { }
		/// <summary>
		///  Constructs an wrapper for the array of disposable type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="array">The list to wrap the array around.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="array"/> is null.
		/// </exception>
		public DisposableArray(T[] array) : base(array) { }

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Disposes of all the items in the collection.
		/// </summary>
		public void Dispose() {
			Dispose(true);
		}
		/// <summary>
		///  An overrideable method to change how items in the collection are disposed of.
		/// </summary>
		/// <param name="disposing">True if disposing is going on.</param>
		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				foreach (T item in Items) {
					item?.Dispose();
				}
			}
		}

		#endregion
	}
}
