using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TriggersTools.SharpUtils.Collections {
	public class ObservableArray<T> : ArrayCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged {
		#region Constants

		private const string CountString = "Count";
		/// <summary>
		///  This must agree with Binding.IndexerName.  It is declared separately here so as to avoid a dependency on
		///  PresentationFramework.dll.
		/// </summary>
		private const string IndexerName = "Item[]";

		#endregion

		#region Fields

		private SimpleMonitor _monitor = new SimpleMonitor();

		#endregion

		#region Constructors

		public ObservableArray(int length) : base(length) { }
		public ObservableArray(T[] array) : base(CopyFrom(array)) { }
		public ObservableArray(ICollection<T> collection) : base(CopyFrom(collection)) { }
		public ObservableArray(IEnumerable<T> collection) : base(CopyFrom(collection)) { }

		private static T[] CopyFrom(T[] array) {
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			T[] newArray = new T[array.Length];
			Array.Copy(array, newArray, array.Length);
			return newArray;
		}
		private static T[] CopyFrom(ICollection<T> collection) {
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			T[] newArray = new T[collection.Count];
			collection.CopyTo(newArray, 0);
			return newArray;
		}
		private static T[] CopyFrom(IEnumerable<T> collection) {
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			return collection.ToArray();
		}

		#endregion

		#region INotifyPropertyChanged Implementation

		/// <summary>
		///  PropertyChanged event (per <see cref="INotifyPropertyChanged"/>).
		/// </summary>
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
			add => PropertyChanged += value;
			remove => PropertyChanged -= value;
		}

		#endregion

		#region Events

		/// <summary>
		///  Occurs when the collection changes, either by adding or removing an item.
		/// </summary>
		/// <remarks>
		///  See <seealso cref="INotifyCollectionChanged"/>
		/// </remarks>
		[field: NonSerialized]
		public virtual event NotifyCollectionChangedEventHandler CollectionChanged;
		/// <summary>
		///  PropertyChanged event (per <see cref="INotifyPropertyChanged"/>).
		/// </summary>
		[field: NonSerialized]
		protected virtual event PropertyChangedEventHandler PropertyChanged;
		
		#endregion
		
		#region Protected Methods

		/// <summary>
		///  Called by base class <see cref="ArrayCollection{T}"/> when an item is set in list; raises a
		///  <see cref="CollectionChanged"/> event to any listeners.
		/// </summary>
		protected override void SetItem(int index, T item) {
			CheckReentrancy();
			T originalItem = this[index];
			base.SetItem(index, item);

			OnPropertyChanged(IndexerName);
			OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
		}
		
		/// <summary>
		///  Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
		/// </summary>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
			PropertyChanged?.Invoke(this, e);
		}
		/// <summary>
		///  Raise <see cref="CollectionChanged"/> event to any listeners. Properties/methods modifying this
		///  <see cref="ObservableArray{T}"/> will raise a collection changed event through this virtual method.
		/// </summary>
		/// <remarks>
		///  When overriding this method, either call its base implementation or call <see cref="BlockReentrancy"/> to
		///  guard against reentrant collection changes.
		/// </remarks>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
			if (CollectionChanged != null) {
				using (BlockReentrancy())
					CollectionChanged(this, e);
			}
		}
		/// <summary>
		///  Disallow reentrant attempts to change this collection. E.g. a event handler of the
		///  <see cref="CollectionChanged"/> event is not allowed to make changes to this collection.
		/// </summary>
		/// <remarks>
		///  Typical usage is to wrap e.g. a <see cref="OnCollectionChanged"/> call with a using() scope:
		///  <code>
		///          using (BlockReentrancy()) {
		///              CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
		///          }
		///  </code>
		/// </remarks>
		protected IDisposable BlockReentrancy() {
			_monitor.Enter();
			return _monitor;
		}
		/// <summary>
		///  Check and assert for reentrant attempts to change this collection.
		/// </summary>
		/// 
		/// <exception cref="InvalidOperationException">
		///  Raised when changing the collection while another collection change is still being notified to other
		///  listeners.
		/// </exception>
		protected void CheckReentrancy() {
			if (_monitor.Busy) {
				// we can allow changes if there's only one listener - the problem
				// only arises if reentrant changes make the original event args
				// invalid for later listeners.  This keeps existing code working
				// (e.g. Selector.SelectedItems).
				if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
					throw new InvalidOperationException("ObservableArray Reentrancy Not Allowed!");
			}
		}

		#endregion
		
		#region Private Methods

		/// <summary>
		///  Helper to raise a PropertyChanged event  />).
		/// </summary>
		private void OnPropertyChanged(string propertyName) {
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		/// <summary>
		///  Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged(NotifyCollectionChangedAction action) {
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
		}
		/// <summary>
		///  Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index) {
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}
		/// <summary>
		///  Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex) {
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
		}
		/// <summary>
		///  Helper to raise CollectionChanged event to any listeners
		/// </summary>
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index) {
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
		}
		/// <summary>
		///  Helper to raise CollectionChanged event with action == Reset to any listeners
		/// </summary>
		private void OnCollectionReset() {
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		#endregion

		#region Private Types
		
		/// <summary>
		///  This class helps prevent reentrant calls.
		/// </summary>
		[Serializable]
		private class SimpleMonitor : IDisposable {
			public void Enter() => ++_busyCount;
			public void Dispose() => --_busyCount;

			public bool Busy => _busyCount > 0;

			private int _busyCount;
		}

		#endregion
	}
}
