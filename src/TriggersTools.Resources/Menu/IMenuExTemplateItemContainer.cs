using System.Collections.Generic;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a menu ex item container.
	/// </summary>
	public interface IMenuExTemplateItemContainer : IMenuBaseTemplateItemContainer {
		/// <summary>
		///  Gets the list of menu items within this container.
		/// </summary>
		new List<IMenuExTemplateItem> MenuItems { get; set; }
	}
}
