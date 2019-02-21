using System;
using System.IO;

namespace TriggersTools.CatSystem2.Attributes {
	/// <summary>
	///  An attribute used to assocaite a <see cref="KifintType"/> with a wildcard lookup for file names.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class KifintWildcardAttribute : Attribute {
		/// <summary>
		///  Gets the wildcard string used to match the assocaited files when searching.
		/// </summary>
		public string Wildcard { get; }

		/// <summary>
		///  Constructs the KIFINT wildcard attribute and automatically adds a .int extension if no extrension is
		///  present.
		/// </summary>
		/// <param name="wildcard">The wildcard file name to used to match the files.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="wildcard"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="wildcard"/> is an empty string or whitespace.
		/// </exception>
		public KifintWildcardAttribute(string wildcard) {
			if (wildcard == null)
				throw new ArgumentNullException(nameof(wildcard));
			if (string.IsNullOrWhiteSpace(wildcard))
				throw new ArgumentException($"{nameof(wildcard)} cannot be empty or whitespace!", nameof(wildcard));
			Wildcard = wildcard;
			if (!Path.HasExtension(Wildcard))
				Wildcard += ".int";
		}
	}
	/// <summary>
	///  An attribute used to identify the types of files are contained by a <see cref="KifintType"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	internal class KifintFileTypesAttribute : Attribute {
		/// <summary>
		///  Gets the file types that exist in the <see cref="KifintType"/> for this <see cref="KnownType"/>.
		/// </summary>
		public string[] Extensions { get; }
		/// <summary>
		///  Gets how these file types are handled for the <see cref="KifintType"/>.
		/// </summary>
		public KifintKnownType KnownType { get; }

		/// <summary>
		///  Constructs the KIFINT archive attribute that list the file types and how they are handled.
		/// </summary>
		/// <param name="knownType">How the file types are handled.</param>
		/// <param name="exts">The file types for this <paramref name="knownType"/>.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="exts"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="exts"/> has a length of zero.-or- one of its elements is null, an empty string, or
		///  whitespace.
		/// </exception>
		public KifintFileTypesAttribute(KifintKnownType knownType, params string[] exts) {
			if (exts == null)
				throw new ArgumentNullException(nameof(exts));
			if (exts.Length == 0)
				throw new ArgumentException($"{nameof(exts)} cannot have a length of zero!", nameof(exts));
			for (int i = 0; i < exts.Length; i++) {
				if (exts[i] == null)
					throw new ArgumentException($"{nameof(exts)}[{i}] is null!", nameof(exts));
				if (string.IsNullOrWhiteSpace(exts[i]))
					throw new ArgumentException($"{nameof(exts)}[{i}] cannot have an empty or whitespace!",
						nameof(exts));
				if (exts[i][0] != '.')
					throw new ArgumentException($"{nameof(exts)}[{i}] must start with a '.'!", nameof(exts));
			}
			Extensions = exts;
			KnownType = knownType;
		}
	}
}
