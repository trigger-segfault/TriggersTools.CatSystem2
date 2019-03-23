using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Text {
	public interface IScriptRun {

		string Raw { get; }

		int Size { get; }

		Color Color { get; }

		StringAlignment Alignment { get; }
		/// <summary>
		///  Gets the number of characters being waited for before displaying.
		/// </summary>
		int Wiat { get; }
	}
}
