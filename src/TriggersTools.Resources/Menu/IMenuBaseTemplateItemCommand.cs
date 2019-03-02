
namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a base menu item command.
	/// </summary>
	public interface IMenuBaseTemplateItemCommand : IMenuBaseTemplateItem {
		/// <summary>
		///  Gets or sets the command menu item id.
		/// </summary>
		ushort MenuId { get; set; }
		/// <summary>
		///  Gets or sets if the menu item is a separator.
		/// </summary>
		bool IsSeparator { get; set; }

		/// <summary>
		///  Creates a clone of this menu template item command.
		/// </summary>
		/// <returns>A clone of this menu template item command.</returns>
		new IMenuBaseTemplateItemCommand Clone();
	}
}
