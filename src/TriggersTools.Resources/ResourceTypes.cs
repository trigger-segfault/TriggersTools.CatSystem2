
namespace TriggersTools.Resources {
	/// <summary>
	///  The predefined resource types.
	/// </summary>
	public enum ResourceTypes : ushort {
        /// <summary>
        ///   For resource types not covered by the standard integer types.
        /// </summary>
        Other = 0,
        /// <summary>
        ///  Hardware-dependent cursor resource.
        /// </summary>
        Cursor = 1,
        /// <summary>
        ///  Bitmap resource.
        /// </summary>
        Bitmap = 2,
        /// <summary>
        ///  Hardware-dependent icon resource.
        /// </summary>
        Icon = 3,
        /// <summary>
        ///  Menu resource.
        /// </summary>
        Menu = 4,
        /// <summary>
        ///  Dialog box.
        /// </summary>
        Dialog = 5,
        /// <summary>
        ///  String-table entry.
        /// </summary>
        String = 6,
        /// <summary>
        ///  Font directory resource.
        /// </summary>
        FontDir = 7,
        /// <summary>
        ///  Font resource.
        /// </summary>
        Font = 8,
        /// <summary>
        ///  Accelerator table.
        /// </summary>
        Accelerator = 9,
        /// <summary>
        ///  Application-defined resource (raw data).
        /// </summary>
        RCData = 10,
        /// <summary>
        ///  Message-table entry.
        /// </summary>
        MessageTable = 11,
        /// <summary>
        ///  Hardware-independent cursor resource.
        /// </summary>
        GroupCursor = 12,
        /// <summary>
        ///  Hardware-independent icon resource.
        /// </summary>
        GroupIcon = 14,
        /// <summary>
        ///  Version resource.
        /// </summary>
        Version = 16,
        /// <summary>
        ///  Allows a resource editing tool to associate a string with an .rc file.
        /// </summary>
        DlgInclude = 17,
        /// <summary>
        ///  Plug and Play resource.
        /// </summary>
        PlugPlay = 19,
        /// <summary>
        ///  VXD.
        /// </summary>
        VXD = 20,
        /// <summary>
        ///  Animated cursor.
        /// </summary>
        AniCursor = 21,
        /// <summary>
        ///  Animated icon.
        /// </summary>
        AniIcon = 22,
        /// <summary>
        ///  HTML.
        /// </summary>
        HTML = 23,
        /// <summary>
        ///  Microsoft Windows XP: Side-by-Side Assembly XML Manifest.
        /// </summary>
        Manifest = 24,
    }
}