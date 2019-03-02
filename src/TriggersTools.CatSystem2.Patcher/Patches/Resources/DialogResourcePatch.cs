using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Resources;
using TriggersTools.Resources.Dialog;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public abstract class DialogResourcePatch : ResourcePatch<DialogResource> {

		public bool AllowNormal { get; protected set; } = true;
		public bool AllowExtended { get; protected set; } = true;

		public DialogResourcePatch() { }
		public DialogResourcePatch(ResourceId name) : base(name) { }

		public override bool IsPatchable(Resource resource) {
			if (!base.IsPatchable(resource))
				return false;
			DialogResource dialogRes = (DialogResource) resource;
			switch (dialogRes.Template) {
			case DialogTemplate dialog: return AllowNormal;
			case DialogExTemplate dialogEx: return AllowExtended;
			}
			return false;
		}
		public override bool Patch(DialogResource dialogRes) {
			switch (dialogRes.Template) {
			case DialogTemplate dialog: return Patch(dialog);
			case DialogExTemplate dialogEx: return Patch(dialogEx);
			default: return false;
			}
		}
		public virtual bool Patch(DialogTemplate dialog) => true;
		public virtual bool Patch(DialogExTemplate dialogEx) => true;

	}
}
