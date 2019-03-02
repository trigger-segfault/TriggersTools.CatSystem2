
namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a menu ex item.
	/// </summary>
	public interface IMenuExTemplateItem : IMenuBaseTemplateItem {
		
		/// <summary>
		///  Gets or sets the type of the menu item.
		/// </summary>
		uint Type { get; set; }
		/// <summary>
		///  Gets or sets the state of the menu item.
		/// </summary>
		uint State { get; set; }

		/// <summary>
		///  Gets or sets the Id of the menu item.
		/// </summary>
		ushort MenuId { get; set; }

		/// <summary>
		///  Creates a clone of this menu ex template item and all sub menu items if this is a popup.
		/// </summary>
		/// <returns>A clone of this menu ex template item.</returns>
		new IMenuExTemplateItem Clone();
	}
}
