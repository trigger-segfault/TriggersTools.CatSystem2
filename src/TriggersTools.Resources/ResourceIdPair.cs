using System;

namespace TriggersTools.Resources {
	/// <summary>
	///  A pair of Ids to identify a unique resource.
	/// </summary>
	public struct ResourceIdPair : IComparable, IComparable<ResourceIdPair>, IEquatable<ResourceIdPair> {
		#region Fields

		/// <summary>
		///  Gets or sets the resource type.
		/// </summary>
		public ResourceId Type { get; set; }
		/// <summary>
		///  Gets or sets the resource name.
		/// </summary>
		public ResourceId Name { get; set; }
		/// <summary>
		///  Gets or sets the resource language Id.
		/// </summary>
		public ushort Language { get; set; }
		/// <summary>
		///  Gets or sets if the resource language Id is ignored during comparison.
		/// </summary>
		public bool IgnoreLanguage { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a resource Id pair from the existing resource.
		/// </summary>
		/// <param name="resource">The existing resource to get the Ids from.</param>
		public ResourceIdPair(Resource resource) : this(resource, false) { }
		/// <summary>
		///  Constructs a resource Id pair from the existing resource.
		/// </summary>
		/// <param name="resource">The existing resource to get the Ids from.</param>
		/// <param name="ignoreLanguage">True if langauge should be ignored during comparison.</param>
		public ResourceIdPair(Resource resource, bool ignoreLanguage) {
			Type = resource.Type;
			Name = resource.Name;
			Language = resource.Language;
			IgnoreLanguage = ignoreLanguage;
		}
		/// <summary>
		///  Constructs a resource Id pair with just a type and name and ignores the language.
		/// </summary>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		public ResourceIdPair(ResourceId type, ResourceId name) {
			Type = type;
			Name = name;
			Language = 0;
			IgnoreLanguage = true;
		}
		/// <summary>
		///  Constructs a resource Id pair with just a type, name and language.
		/// </summary>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		public ResourceIdPair(ResourceId type, ResourceId name, ushort language) {
			Type = type;
			Name = name;
			Language = language;
			IgnoreLanguage = false;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets the string representation of Id pairs's the resource type.
		/// </summary>
		public string TypeName => Type.TypeName;

		#endregion

		#region Object Overrides
		
		/// <summary>
		///  Gets the string representation of the resource Id pair.
		/// </summary>
		/// <returns>The resource Id pair names.</returns>
		public override string ToString() {
			if (IgnoreLanguage)
				return $"{TypeName} : {Name} : <any>";
			else
				return $"{TypeName} : {Name} : {Language}";
		}

		/// <summary>
		///  Gets the hash code for the resource Id pair. Either returns <see cref="Value"/>.
		/// </summary>
		/// <returns>The resource Id pair hash code.</returns>
		public override int GetHashCode() {
			if (IgnoreLanguage)
				return Type.GetHashCode() ^ Name.GetHashCode();
			else
				return Type.GetHashCode() ^ Name.GetHashCode() ^ (Language << 16);
		}

		/// <summary>
		///  Checks if the other object is a resource Id pair and is equal to this one.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>True if both resource Id pairs represent the same resource.</returns>
		public override bool Equals(object obj) {
			return (obj is ResourceIdPair resIdPair && Equals(resIdPair));
		}
		/// <summary>
		///  Checks if the other resource Id pair is equal to this one.
		/// </summary>
		/// <param name="other">The other resource Id pair to compare.</param>
		/// <returns>True if both resource Id pairs represent the same resource.</returns>
		public bool Equals(ResourceIdPair other) {
			return	(Type == other.Type && other.Name == other.Name &&
					(IgnoreLanguage || other.IgnoreLanguage || Language == other.Language));
		}
		/// <summary>
		///  Checks if the other object is a resource Id pair and compares this resource Id pair to it.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of both resource Id pairs.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="obj"/> is not a <see cref="ResourceIdPair"/>.
		/// </exception>
		public int CompareTo(object obj) {
			if (obj is ResourceIdPair resIdPair)
				return CompareTo(resIdPair);
			throw new ArgumentException($"{nameof(obj)} is not of type {nameof(ResourceIdPair)}!");
		}
		/// <summary>
		///  Compares this resource Id pair the other.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of this resource Id pair to the other.</returns>
		public int CompareTo(ResourceIdPair other) {
			int comparison = Type.CompareTo(other.Type);
			if (comparison != 0)
				return comparison;
			comparison = Name.CompareTo(other.Name);
			if (comparison != 0)
				return comparison;
			if (IgnoreLanguage || other.IgnoreLanguage)
				return 0;
			return Language.CompareTo(other.Language);
		}

		#endregion

		#region Operators

		public static bool operator ==(ResourceIdPair a, ResourceIdPair b) => a.Equals(b);
		public static bool operator !=(ResourceIdPair a, ResourceIdPair b) => !a.Equals(b);

		#endregion
	}
}
