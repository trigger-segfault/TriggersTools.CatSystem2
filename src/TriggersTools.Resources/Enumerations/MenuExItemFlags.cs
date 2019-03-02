
namespace TriggersTools.Resources.Enumerations {
	/// <summary>
	///  pecifies whether the menu item is the last item in the menu bar, drop-down menu, submenu, or shortcut menu and
	///  whether it is an item that opens a drop-down menu or submenu. This member can be zero or more of these values.
	///  For 32-bit applications, this member is a word; for 16-bit applications, it is a byte.
	/// </summary>
	public enum MenuExItemFlags : ushort {
		/// <summary>
		///  No menu ex item flags.
		/// </summary>
		None = 0,
		/// <summary>
		///  The structure defines the last menu item in the menu bar, drop-down menu, submenu, or shortcut menu.
		/// </summary>
		Last = 0x80,
		/// <summary>
		///  The structure defines a item that opens a drop-down menu or submenu. Subsequent structures define menu
		///  items in the corresponding drop-down menu or submenu.
		/// </summary>
		Sub = 0x01
    }
}