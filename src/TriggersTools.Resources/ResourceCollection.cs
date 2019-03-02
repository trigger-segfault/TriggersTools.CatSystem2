using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.Resources {
	/// <summary>
	///  A collection of resources for a specific resource type.
	/// </summary>
	public sealed class ResourceCollection : IList<Resource>, IReadOnlyList<Resource> {
		#region Fields

		/// <summary>
		///  The list of resources in this collection.
		/// </summary>
		private readonly List<Resource> resources = new List<Resource>();
		/// <summary>
		///  Gets the resource type of the resource collection.
		/// </summary>
		public ResourceId Type { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the resource collection with the specified type Id.
		/// </summary>
		/// <param name="type">The resource type of this collection.</param>
		public ResourceCollection(ResourceId type) {
			Type = type;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets the string representation of collection's the resource type.
		/// </summary>
		public string TypeName => Type.TypeName;
		/// <summary>
		///  Gets the number of resources in the collection.
		/// </summary>
		public int Count => resources.Count;

		#endregion

		public Resource Find(Predicate<Resource> match) => resources.Find(match);
		public int FindIndex(Predicate<Resource> match) => resources.FindIndex(match);

		public int IndexOf(Resource resource) => resources.IndexOf(resource);

		public bool Insert(int index, Resource resource) {
			if (resource == null)
				throw new ArgumentNullException(nameof(resource));
			if (resource.Type != Type)
				throw new ArgumentException($"{nameof(resource)} {resource} is not of type {TypeName}!");
			if (!resources.Contains(resource)) {
				resources.Insert(index, resource);
				return true;
			}
			return false;
		}
		void IList<Resource>.Insert(int index, Resource resource) => Insert(index, resource);

		public void RemoveAt(int index) => resources.RemoveAt(index);

		public Resource[] ToArray() => resources.ToArray();

		public Resource this[int index] {
			get => resources[index];
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				if (value.Type != Type)
					throw new ArgumentException($"{nameof(value)} {value} is not of type {TypeName}!");
				resources[index] = value;
			}
		}

		public bool Add(Resource resource) => Insert(resources.Count, resource);
		void ICollection<Resource>.Add(Resource resource) => Add(resource);

		public void Clear() => resources.Clear();

		public bool Contains(Resource resource) => resources.Contains(resource);

		public void CopyTo(Resource[] array, int arrayIndex) => resources.CopyTo(array, arrayIndex);

		public bool Remove(Resource resource) => resources.Remove(resource);

		bool ICollection<Resource>.IsReadOnly => false;

		public IEnumerator<Resource> GetEnumerator() => resources.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
