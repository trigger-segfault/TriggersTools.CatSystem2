using System.Collections.Generic;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a base menu item container.
	/// </summary>
	public interface IMenuBaseTemplateItemContainer {
		/// <summary>
		///  Gets the list of menu items within this container.
		/// </summary>
		IReadOnlyList<IMenuBaseTemplateItem> MenuItems { get; }
	}
}
