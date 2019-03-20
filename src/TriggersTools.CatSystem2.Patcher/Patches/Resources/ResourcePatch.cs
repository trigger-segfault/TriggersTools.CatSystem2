using System;
using TriggersTools.Windows.Resources;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	/// <summary>
	///  The non-generic interface for a resource patch.
	/// </summary>
	public interface IResourcePatch {
		/// <summary>
		///  Patches the resource.
		/// </summary>
		/// <param name="resource">The resource to patch.</param>
		/// <returns>True if the patch was successful.</returns>
		bool Patch(Resource resource);
		/// <summary>
		///  Checks if this resource can be patched by this patch.
		/// </summary>
		/// <param name="resource">The resource to patch.</param>
		/// <returns>True if this patch can be used on this resource.</returns>
		bool IsPatchable(Resource resource);
	}
	/// <summary>
	///  The generic abstract class for a resource patch of a specified resource type.
	/// </summary>
	/// <typeparam name="T">The type of the resource.</typeparam>
	public abstract class ResourcePatch<T> : IResourcePatch where T : Resource {
		#region Properties

		/// <summary>
		///  Gets the name of the resource to be patched.
		/// </summary>
		public ResourceId Name { get; }
		/// <summary>
		///  Gets the optional language of the resource to be patched.
		/// </summary>
		public ushort Language { get; }
		/// <summary>
		///  True if the language is not compared when patching.
		/// </summary>
		public bool IgnoreLanguage { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a resource patch with no name or language.
		/// </summary>
		public ResourcePatch() { }
		/// <summary>
		///  Constructs a resource patch with a name and an optional language.
		/// </summary>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The optional resource langauge.</param>
		public ResourcePatch(ResourceId name, ushort? language = null) {
			Name = name.ToString();
			if (language.HasValue)
				Language = language.Value;
			IgnoreLanguage = !language.HasValue;
		}

		#endregion

		#region IResourcePatch Implementation

		/// <summary>
		///  Patches the resource.
		/// </summary>
		/// <param name="resource">The resource to patch.</param>
		/// <returns>True if the patch was successful.</returns>
		public abstract bool Patch(T resource);
		bool IResourcePatch.Patch(Resource resource) => Patch((T) resource);
		/// <summary>
		///  Checks if this resource can be patched by this patch.
		/// </summary>
		/// <param name="resource">The resource to patch.</param>
		/// <returns>True if this patch can be used on this resource.</returns>
		public virtual bool IsPatchable(Resource resource) {
			if (Name != ResourceId.Null && resource.Name != Name)
				return false;
			return resource?.GetType() == typeof(T);
		}

		#endregion



	}
}
