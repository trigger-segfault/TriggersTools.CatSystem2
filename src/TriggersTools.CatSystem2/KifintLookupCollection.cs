using System;
using System.Collections;
using System.Collections.Generic;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A readonly collection for loaded and cached KIFINT lookups.
	/// </summary>
	public interface IKifintLookupCollection : IReadOnlyList<KifintLookup> {
		#region Properties

		/*/// <summary>
		///  Gets the number of loaded lookups.
		/// </summary>
		int Count { get; }*/
		/// <summary>
		///  Gets the optionally loaded <see cref="KifintType.Update"/> lookup associated with this collection.
		/// </summary>
		KifintLookup Update { get; }
		/// <summary>
		///  Gets the optionally loaded <see cref="KifintType.Image"/> lookup associated with this collection.
		/// </summary>
		KifintLookup Image { get; }

		#endregion

		#region Accessors/Mutators
		
		/*/// <summary>
		///  Gets the KIFINT lookup at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup at the specified index in the collection.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="index"/> is outside the bounds of the collection.
		/// </exception>
		KifintLookup this[int index] { get; }*/
		/// <summary>
		///  Gets the KIFINT lookup with the specified known type in the collection.
		/// </summary>
		/// <param name="type">The type of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup with the specified type in the collection.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  A known KIFINT lookup of <paramref name="type"/> is not present in the collection.
		/// </exception>
		KifintLookup this[KifintType type] { get; }
		/// <summary>
		///  Gets the KIFINT lookup with the specified unknown type name in the collection.
		/// </summary>
		/// <param name="unknownName">The type of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup with the specified type in the collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <see cref="KifintLookup.ArchiveType"/> is not <see cref="KifintType.Unknown"/>.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  An unknown KIFINT lookup of <paramref name="unknownName"/> is not present in the collection.
		/// </exception>
		KifintLookup this[string unknownName] { get; }
		/// <summary>
		///  Tries to get the KIFINT lookup with the specified known type in the collection.
		/// </summary>
		/// <param name="type">The type of the KIFINT lookup to get.</param>
		/// <param name="lookup">The output KIFINT lookup with the specified known type.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		bool TryGetValue(KifintType type, out KifintLookup lookup);
		/// <summary>
		///  Tries to get the KIFINT lookup with the specified unknown type name in the collection.
		/// </summary>
		/// <param name="unknownName">The type of the KIFINT lookup to get.</param>
		/// <param name="lookup">The output KIFINT lookup with the specified unknown type.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		bool TryGetValue(string unknownName, out KifintLookup lookup);
		/// <summary>
		///  Checks if a KIFINT lookup with the specified known type exists.
		/// </summary>
		/// <param name="type">The known type of the lookup to check for.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		bool ContainsKey(KifintType type);
		/// <summary>
		///  Checks if a KIFINT lookup with the specified unknown type name exists.
		/// </summary>
		/// <param name="type">The unknown type name of the lookup to check for.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		bool ContainsKey(string unknownName);

		#endregion
	}
	/// <summary>
	///  A mutable collection for loaded and cached KIFINT lookups.
	/// </summary>
	internal sealed class KifintLookupCollection : IKifintLookupCollection {
		#region Fields

		private readonly List<KifintLookup> lookups = new List<KifintLookup>();
		private readonly Dictionary<KifintType, KifintLookup> knownLookups =
			new Dictionary<KifintType, KifintLookup>();
		private readonly Dictionary<string, KifintLookup> unknownLookups =
			new Dictionary<string, KifintLookup>();

		/// <summary>
		///  Gets the optionally loaded <see cref="KifintType.Update"/> lookup associated with this collection.
		/// </summary>
		public KifintLookup Update { get; private set; } = null;
		/// <summary>
		///  Gets the optionally loaded <see cref="KifintType.Image"/> lookup associated with this collection.
		/// </summary>
		public KifintLookup Image { get; private set; } = null;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of loaded lookups.
		/// </summary>
		public int Count => lookups.Count;
		public IReadOnlyCollection<KifintLookup> TypeValues => knownLookups.Values;
		public IReadOnlyCollection<KifintType> TypeKeys => knownLookups.Keys;
		public IReadOnlyCollection<KifintLookup> UnknownNameValues => unknownLookups.Values;
		/// <summary>
		///  Gets the collection of keys for all unknown name lookups.
		/// </summary>
		public IReadOnlyCollection<string> UnknownNameKeys => unknownLookups.Keys;

		#endregion

		#region Accessors

		/// <summary>
		///  Gets the KIFINT lookup at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup at the specified index in the collection.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="index"/> is outside the bounds of the collection.
		/// </exception>
		public KifintLookup this[int index] => lookups[index];
		/// <summary>
		///  Gets the KIFINT lookup with the specified known type in the collection.
		/// </summary>
		/// <param name="type">The type of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup with the specified type in the collection.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  A known KIFINT lookup of <paramref name="type"/> is not present in the collection.
		/// </exception>
		public KifintLookup this[KifintType type] {
			get {
				ThrowIfUndefinedOrUnknown(type);
				return knownLookups[type];
			}
			set {
				ThrowIfUndefinedOrUnknown(type);

				if (knownLookups.TryGetValue(type, out KifintLookup lookup)) {
					lookups.Remove(lookup);
				}
				if (value == null) {
					knownLookups.Remove(type);
				}
				else {
					value.Update = Update;
					lookups.Add(value);
					knownLookups[type] = value;
				}
				if (type == KifintType.Update)
					AssignUpdateLookup(value);
				else if (type == KifintType.Image)
					Image = value;
			}
		}
		/// <summary>
		///  Gets the KIFINT lookup with the specified unknown type name in the collection.
		/// </summary>
		/// <param name="unknownName">The type of the KIFINT lookup to get.</param>
		/// <returns>The KIFINT lookup with the specified type in the collection.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <see cref="KifintLookup.ArchiveType"/> is not <see cref="KifintType.Unknown"/>.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  An unknown KIFINT lookup of <paramref name="unknownName"/> is not present in the collection.
		/// </exception>
		public KifintLookup this[string unknownName] {
			get => unknownLookups[unknownName];
			set {
				if (unknownName == null)
					throw new ArgumentNullException(nameof(unknownName));
				if (value != null)
					ThrowIfNotUnknown(value.ArchiveType);

				if (unknownLookups.TryGetValue(unknownName, out KifintLookup lookup)) {
					lookups.Remove(lookup);
				}
				if (value == null) {
					unknownLookups.Remove(unknownName);
				}
				else {
					value.Update = Update;
					lookups.Add(value);
					unknownLookups[unknownName] = value;
				}
			}
		}
		/// <summary>
		///  Tries to get the KIFINT lookup with the specified known type in the collection.
		/// </summary>
		/// <param name="type">The type of the KIFINT lookup to get.</param>
		/// <param name="lookup">The output KIFINT lookup with the specified known type.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		public bool TryGetValue(KifintType type, out KifintLookup lookup) {
			ThrowIfUndefinedOrUnknown(type);
			return knownLookups.TryGetValue(type, out lookup);
		}
		/// <summary>
		///  Tries to get the KIFINT lookup with the specified unknown type name in the collection.
		/// </summary>
		/// <param name="unknownName">The type of the KIFINT lookup to get.</param>
		/// <param name="lookup">The output KIFINT lookup with the specified unknown type.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		public bool TryGetValue(string unknownName, out KifintLookup lookup) {
			return unknownLookups.TryGetValue(unknownName, out lookup);
		}
		/// <summary>
		///  Checks if a KIFINT lookup with the specified known type exists.
		/// </summary>
		/// <param name="type">The known type of the lookup to check for.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="type"/> is not defined or is <see cref="KifintType.Unknown"/>.
		/// </exception>
		public bool ContainsKey(KifintType type) {
			ThrowIfUndefinedOrUnknown(type);
			return knownLookups.ContainsKey(type);
		}
		/// <summary>
		///  Checks if a KIFINT lookup with the specified unknown type name exists.
		/// </summary>
		/// <param name="type">The unknown type name of the lookup to check for.</param>
		/// <returns>True if the KIFINT lookup was found, otherwise false.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="unknownName"/> is null.
		/// </exception>
		public bool ContainsKey(string unknownName) => unknownLookups.ContainsKey(unknownName);

		#endregion

		#region Mutators
		
		/// <summary>
		///  Clears all lookups from the collection.
		/// </summary>
		public void Clear() {
			lookups.Clear();
			knownLookups.Clear();
			unknownLookups.Clear();
			Update = null;
			Image = null;
		}
		public void Add(KifintLookup lookup) {
			if (lookup == null)
				throw new ArgumentNullException(nameof(lookup));
			ThrowIfUndefinedOrUnknown(lookup.ArchiveType);
			knownLookups.Add(lookup.ArchiveType, lookup);
			lookups.Add(lookup);
		}
		public void Add(string unknownName, KifintLookup lookup) {
			if (unknownName == null)
				throw new ArgumentNullException(nameof(unknownName));
			if (lookup == null)
				throw new ArgumentNullException(nameof(lookup));
			unknownLookups.Add(unknownName, lookup);
			lookups.Add(lookup);
		}
		public bool Remove(KifintType type) {
			ThrowIfUndefinedOrUnknown(type);
			if (knownLookups.TryGetValue(type, out KifintLookup lookup)) {
				lookups.Remove(lookup);
				knownLookups.Remove(type);
				return true;
			}
			return false;
		}
		public bool Remove(string unknownName) {
			if (unknownName == null)
				throw new ArgumentNullException(nameof(unknownName));
			if (unknownLookups.TryGetValue(unknownName, out KifintLookup lookup)) {
				lookups.Remove(lookup);
				unknownLookups.Remove(unknownName);
				return true;
			}
			return false;
		}

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for all cached KIFINT lookups in each KIFINT lookups collection.
		/// </summary>
		/// <returns>The enumerator for all lookups.</returns>
		public IEnumerator<KifintLookup> GetEnumerator() => lookups.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		private static void ThrowIfUndefinedOrUnknown(KifintType type) {
			if (!Enum.IsDefined(typeof(KifintType), type))
				throw new ArgumentException($"{nameof(KifintType)} {type} is not defined!");
			if (type == KifintType.Unknown)
				throw new ArgumentException($"{nameof(KifintType)} cannot be {KifintType.Unknown}!");
		}
		private static void ThrowIfNotUnknown(KifintType type) {
			if (type != KifintType.Unknown)
				throw new ArgumentException($"{nameof(KifintType)} must be {KifintType.Unknown}!");
		}
		private void AssignUpdateLookup(KifintLookup updateLookup) {
			Update = updateLookup;
			foreach (KifintLookup lookup in lookups) {
				if (lookup.ArchiveType != KifintType.Update)
					lookup.Update = updateLookup;
			}
		}
	}
}
