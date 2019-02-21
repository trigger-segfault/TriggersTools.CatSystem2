using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class TypeFacePatch : DialogResourcePatch {

		public string TypeFace { get; }
		public ushort PointSize { get; }

		public TypeFacePatch(string typeFace, int pointSize) {
			TypeFace = typeFace ?? throw new ArgumentNullException(nameof(typeFace));
			PointSize = (ushort) pointSize;
		}

		public override bool Patch(DialogTemplate dialog) {
			if (dialog.TypeFace != TypeFace || dialog.PointSize != PointSize) {
				dialog.TypeFace = TypeFace;
				dialog.PointSize = PointSize;
				return true;
			}
			return false;
		}
		public override bool Patch(DialogExTemplate dialogEx) {
			if (dialogEx.TypeFace != TypeFace || dialogEx.PointSize != PointSize) {
				dialogEx.TypeFace = TypeFace;
				dialogEx.PointSize = PointSize;
				return true;
			}
			return false;
		}
	}
}
