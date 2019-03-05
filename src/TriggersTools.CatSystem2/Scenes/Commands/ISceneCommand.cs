using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Script;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	/// <summary>
	///  The interface for a CatScene <see cref="SceneLineType.Command"/>.
	/// </summary>
	public interface ISceneCommand : ISceneLine {
		#region Properties

		/*/// <summary>
		///  Gets the cat string entry for this scene command.
		/// </summary>
		SceneLine CatString { get; }*/
		/// <summary>
		///  Gets the unparsed content of the string command.
		/// </summary>
		//string Content { get; set; }
		/// <summary>
		///  Gets the name of the command.
		/// </summary>
		string CommandName { get; }
		/// <summary>
		///  Gets the parameters for the command, including the comamnd name..
		/// </summary>
		IReadOnlyList<string> Parameters { get; }
		/// <summary>
		///  Gets the number of parameters in the command.
		/// </summary>
		int Count { get; }


		#endregion

		#region IsCommand

		int IsCommandPriority { get; }
		bool IsCommand(string name, string content, string[] parameters);

		#endregion
	}
}
