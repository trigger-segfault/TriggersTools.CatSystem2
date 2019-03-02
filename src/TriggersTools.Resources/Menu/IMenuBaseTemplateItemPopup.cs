
namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a base menu item popup.
	/// </summary>
	public interface IMenuBaseTemplateItemPopup : IMenuBaseTemplateItem, IMenuBaseTemplateItemContainer {

		/// <summary>
		///  Creates a clone of this menu template item popup and all sub menu items.
		/// </summary>
		/// <returns>A clone of this menu template item popup.</returns>
		new IMenuBaseTemplateItemPopup Clone();
	}
}
