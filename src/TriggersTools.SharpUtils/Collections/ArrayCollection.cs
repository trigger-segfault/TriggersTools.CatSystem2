using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;

namespace TriggersTools.SharpUtils.Collections {
	[Serializable]
	[DebuggerDisplay("Length = {Count}")]
	public class ArrayCollection<T> : IList<T>, IList, IReadOnlyList<T> {
		#region Fields
		
		protected T[] Items { get; }

		#endregion

		#region Constructors

		public ArrayCollection(int length) {
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length));
			Items = new T[length];
		}
		public ArrayCollection(T[] array) {
			// Behave just like Collection<T> where we act as a wrapper for the collection.
			Items = array ?? throw new ArgumentNullException(nameof(array));
		}

		#endregion

		#region IList Properties

		/// <summary>
		///  Gets the number of elements in the array.
		/// </summary>
		public int Count => Items.Length;
		public bool IsSynchronized => false;
		public object SyncRoot => Items.SyncRoot;
		/// <summary>
		///  Gets if the array is read only, always false.
		/// </summary>
		public bool IsReadOnly => false;
		/// <summary>
		///  Gets if the array is of a fixed length, always true.
		/// </summary>
		public bool IsFixedSize => true;

		#endregion

		#region Virtual Methods

		protected virtual void SetItem(int index, T item) => Items[index] = item;

		#endregion

		#region Supported IList Implementation

		public T this[int index] {
			get => Items[index];
			set {
				if (index < 0 || index >= Items.Length)
					throw new ArgumentOutOfRangeException(nameof(index));
				SetItem(index, value);
			}
		}
		object IList.this[int index] {
			get => Items[index];
			set => Items[index] = (T) value;
		}

		public T[] ToArray() {
			T[] array = new T[Items.Length];
			Array.Copy(Items, array, Items.Length);
			return array;
		}
		public void CopyTo(T[] array, int index)   => Array.Copy(Items, 0, array, index, Items.Length);
		public void CopyTo(Array array, int index) => Array.Copy(Items, 0, array, index, Items.Length);
		public bool Contains(T item)               => Array.IndexOf(Items, item) != -1;
		public int IndexOf(T item)                 => Array.IndexOf(Items, item);
		bool IList.Contains(object value)          => (IsCompatibleObject(value) ? Contains((T) value) : false);
		int  IList.IndexOf(object value)           => (IsCompatibleObject(value) ?  IndexOf((T) value) : -1);
		
		public IEnumerator<T> GetEnumerator()      => Items.Cast<T>().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator()    => Items.GetEnumerator();

		#endregion

		#region Not Supported IList Implementation

		int  IList.Add(object value)                { ThrowNotSupportedFixedSize(); return 0; }
		void ICollection<T>.Add(T item)            => ThrowNotSupportedFixedSize();
		void IList.Clear()                         => ThrowNotSupportedFixedSize();
		void ICollection<T>.Clear()                => ThrowNotSupportedFixedSize();
		void IList.Insert(int index, object value) => ThrowNotSupportedFixedSize();
		void IList<T>.Insert(int index, T item)    => ThrowNotSupportedFixedSize();
		void IList.Remove(object value)            => ThrowNotSupportedFixedSize();
		bool ICollection<T>.Remove(T item)          { ThrowNotSupportedFixedSize(); return false; }
		void IList.RemoveAt(int index)             => ThrowNotSupportedFixedSize();
		void IList<T>.RemoveAt(int index)          => ThrowNotSupportedFixedSize();

		private static void ThrowNotSupportedFixedSize() {
			throw new NotSupportedException("Not supported in a fixed-size collection!");
		}

		#endregion

		#region Private Methods

		private static bool IsCompatibleObject(object value) {
			// Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
			// Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
			return ((value is T) || (value == null && default(T) == null));
		}

		#endregion
	}
}