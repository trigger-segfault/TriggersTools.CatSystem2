using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.Collections {
	/// <summary>
	///  A collection of disposable objects that can be cleaned up in one using statement.
	/// </summary>
	/// <typeparam name="T">The type of disposable object contained in the collection.</typeparam>
	/// 
	/// <remarks>
	///  Calling <see cref="Collection{T}.Clear()"/> will dispose of all items in the list.<para/>
	///  Use <see cref="DisposableCollection{T}.Clear(bool)"/> to choose whether items are disposed of.
	/// </remarks>
	public class DisposableCollection<T> : Collection<T>, IDisposable where T : IDisposable {
		#region Constructors

		/// <summary>
		///  Constructs an empty collection of disposable type <typeparamref name="T"/>.
		/// </summary>
		public DisposableCollection() { }
		/// <summary>
		///  Constructs an wrapper for the list of disposable type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="list">The list to wrap the collection around.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="list"/> is null.
		/// </exception>
		public DisposableCollection(IList<T> list) : base(list) { }

		#endregion

		#region Collection Overrides

		/// <summary>
		///  Removes all elements from the <see cref="DisposableCollection{T}"/>.
		/// </summary>
		protected override void ClearItems() {
			Dispose();
			base.ClearItems();
		}

		#endregion

		#region Clear (Optional Dispose)

		/// <summary>
		///  Removes all elements from the <see cref="DisposableCollection{T}"/> and optionally disposes of them.
		/// </summary>
		/// <param name="dispose">True if the elements should be disposed of.</param>
		public void Clear(bool dispose) {
			if (dispose)
				Dispose();
			base.ClearItems();
		}

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
