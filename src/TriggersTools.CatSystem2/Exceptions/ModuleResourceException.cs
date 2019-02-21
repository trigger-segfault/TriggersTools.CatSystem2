using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	/// <summary>
	///  An exception thrown during a failure with a resource within a module.
	/// </summary>
	public class ModuleResourceException : Exception {
		/// <summary>
		///  Gets the name of the resource.
		/// </summary>
		public string Name { get; }
		/// <summary>
		///  Gets the type of the resource.
		/// </summary>
		public string Type { get; }

		/// <summary>
		///  Constructs the exception and creates a message based on the parameters.
		/// </summary>
		/// <param name="name">The name of the resource being looked for.</param>
		/// <param name="type">The type of the resource being looked for.</param>
		/// <param name="action">The action that failed while looking for the resource.</param>
		internal ModuleResourceException(string name, string type, string action)
			: base($"Failed to {action} resource '{name}:{type}'!")
		{
			Name = name;
			Type = type;
		}
	}
}
