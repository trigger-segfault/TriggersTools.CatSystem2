
namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a menu item.
	/// </summary>
	public interface IMenuTemplateItem : IMenuBaseTemplateItem {
		/// <summary>
		///  Creates a clone of this menu template item and all sub menu items if this is a popup.
		/// </summary>
		/// <returns>A clone of this menu template item.</returns>
		new IMenuTemplateItem Clone();
	}
}
