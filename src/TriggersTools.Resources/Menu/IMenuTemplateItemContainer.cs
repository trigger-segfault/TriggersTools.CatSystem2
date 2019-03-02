using System.Collections.Generic;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a menu item container.
	/// </summary>
	public interface IMenuTemplateItemContainer : IMenuBaseTemplateItemContainer {
		/// <summary>
		///  Gets the list of menu items within this container.
		/// </summary>
		new List<IMenuTemplateItem> MenuItems { get; set; }
	}
}
