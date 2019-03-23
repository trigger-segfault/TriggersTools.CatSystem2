using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.SharpUtils.Enums;

namespace TriggersTools.CatSystem2.Scenes.Commands.Abstract {
	public abstract class SceneCommandBase : ISceneLine {

		/// <summary>
		///  Gets the type of the scene line.
		/// </summary>
		public SceneLineType Type => SceneLineType.Command;

		public abstract string Content { get; set; }
		
		#region ToString Override

		/// <summary>
		///  Gets the string representation of the scene line.
		/// </summary>
		/// <returns>The string representation of the scene line.</returns>
		public override string ToString() {
			string type = $"[{EnumInfo.GetAttribute<SceneLineType, CommandNameAttribute>(Type).Command}]";
			if (!string.IsNullOrEmpty(Content))
				return $"{type} {Content}";
			return type;
		}

		#endregion
	}
}
