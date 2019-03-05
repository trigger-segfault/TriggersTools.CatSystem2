using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes {
	public interface ISceneLine {

		/// <summary>
		///  Gets the type of the scene line.
		/// </summary>
		SceneLineType Type { get; }

		/// <summary>
		///  Gets or sets the content of the scene line.
		/// </summary>
		string Content { get; set; }
	}
}
