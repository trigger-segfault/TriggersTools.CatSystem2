
namespace TriggersTools.Resources.Enumerations {
	/// <summary>
	///  One or more of the following predefined menu options that control the appearance of the menu item as shown in
	///  the following table.
	/// </summary>
	public enum MenuFlags : uint {
        /// <summary>
        /// </summary>
        MF_INSERT = 0x00000000,
        /// <summary>
        /// </summary>
        MF_CHANGE = 0x00000080,
        /// <summary>
        /// </summary>
        MF_APPEND = 0x00000100,
        /// <summary>
        /// </summary>
        MF_DELETE = 0x00000200,
        /// <summary>
        /// </summary>
        MF_REMOVE = 0x00001000,
        /// <summary>
        /// </summary>
        MF_BYCOMMAND = 0x00000000,
        /// <summary>
        /// </summary>
        MF_BYPOSITION = 0x00000400,
        /// <summary>
        /// </summary>
        MF_SEPARATOR = 0x00000800,
        /// <summary>
        /// </summary>
        MF_ENABLED = 0x00000000,
        /// <summary>
        ///  Indicates that the menu item is initially inactive and drawn with a gray effect.
        /// </summary>
        MF_GRAYED = 0x00000001,
        /// <summary>
        /// </summary>
        MF_DISABLED = 0x00000002,
        /// <summary>
        /// </summary>
        MF_UNCHECKED = 0x00000000,
        /// <summary>
        ///  Indicates that the menu item has a check mark next to it.
        /// </summary>
        MF_CHECKED = 0x00000008,
        /// <summary>
        /// </summary>
        MF_USECHECKBITMAPS = 0x00000200,
        /// <summary>
        /// </summary>
        MF_STRING = 0x00000000,
        /// <summary>
        /// </summary>
        MF_BITMAP = 0x00000004,
        /// <summary>
        ///  Indicates that the owner window of the menu is responsible for drawing all visual aspects of the menu
		///  item, including highlighted, selected, and inactive states. This option is not valid for an item in a
		///  menu bar.
        /// </summary>
        MF_OWNERDRAW = 0x00000100,
        /// <summary>
        ///  Indicates that the item is one that opens a drop-down menu or submenu.
        /// </summary>
        MF_POPUP = 0x00000010,
        /// <summary>
        ///  Indicates that the menu item is placed in a new column. The old and new columns are separated by a bar.
        /// </summary>
        MF_MENUBARBREAK = 0x00000020,
        /// <summary>
        ///  Indicates that the menu item is placed in a new column.
        /// </summary>
        MF_MENUBREAK = 0x00000040,
        /// <summary>
        /// </summary>
        MF_UNHILITE = 0x00000000,
        /// <summary>
        /// </summary>
        MF_HILITE = 0x00000080,
        /// <summary>
        /// </summary>
        MF_DEFAULT = 0x00001000,
        /// <summary>
        /// </summary>
        MF_SYSMENU = 0x00002000,
        /// <summary>
        ///  Indicates that the menu item has a vertical separator to its left.
        /// </summary>
        MF_HELP = 0x00004000,
        /// <summary>
        /// </summary>
        MF_RIGHTJUSTIFY = 0x00004000,
        /// <summary>
        /// </summary>
        MF_MOUSESELECT = 0x00008000,
        /// <summary>
        /// </summary>
        MF_END = 0x00000080,

        /// <summary>
        /// </summary>
        MFT_STRING = MF_STRING,
        /// <summary>
        /// </summary>
        MFT_BITMAP = MF_BITMAP,
        /// <summary>
        /// </summary>
        MFT_MENUBARBREAK = MF_MENUBARBREAK,
        /// <summary>
        /// </summary>
        MFT_MENUBREAK = MF_MENUBREAK,
        /// <summary>
        /// </summary>
        MFT_OWNERDRAW = MF_OWNERDRAW,
        /// <summary>
        /// </summary>
        MFT_RADIOCHECK = 0x00000200,
        /// <summary>
        /// </summary>
        MFT_SEPARATOR = MF_SEPARATOR,
        /// <summary>
        /// </summary>
        MFT_RIGHTORDER = 0x00002000,
        /// <summary>
        /// </summary>
        MFT_RIGHTJUSTIFY = MF_RIGHTJUSTIFY,

        /// <summary>
        /// </summary>
        MFS_GRAYED = 0x00000003,
        /// <summary>
        /// </summary>
        MFS_DISABLED = MFS_GRAYED,
        /// <summary>
        /// </summary>
        MFS_CHECKED = MF_CHECKED,
        /// <summary>
        /// </summary>
        MFS_HILITE = MF_HILITE,
        /// <summary>
        /// </summary>
        MFS_ENABLED = MF_ENABLED,
        /// <summary>
        /// </summary>
        MFS_UNCHECKED = MF_UNCHECKED,
        /// <summary>
        /// </summary>
        MFS_UNHILITE = MF_UNHILITE,
        /// <summary>
        /// </summary>
        MFS_DEFAULT = MF_DEFAULT,
    }
}