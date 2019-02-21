using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public interface IResourcePatch {
		bool Patch(Resource resource);
		bool IsPatchable(Resource resource);
	}
	public abstract class ResourcePatch<T> : IResourcePatch where T : Resource {

		public string Name { get; }

		public ResourcePatch() { }
		public ResourcePatch(string name) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}
		public ResourcePatch(ushort name) {
			Name = name.ToString();
		}

		public abstract bool Patch(T resource);
		bool IResourcePatch.Patch(Resource resource) => Patch((T) resource);

		public virtual bool IsPatchable(Resource resource) {
			if (Name != null && resource.Name.Name != Name)
				return false;
			return resource?.GetType() == typeof(T);
		}
	}
}
