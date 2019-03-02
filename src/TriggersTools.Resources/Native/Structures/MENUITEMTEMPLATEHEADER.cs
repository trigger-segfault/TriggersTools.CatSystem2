using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
	/// <summary>
	///     Defines the header for a menu template. A complete menu template consists of a header and one or more menu item lists.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct MENUITEMTEMPLATEHEADER {
        /// <summary>
        ///     Specifies the version number. This member must be zero.
        /// </summary>
        public ushort wVersion;

        /// <summary>
        ///     Specifies the offset, in bytes, from the end of the header. The menu item list begins at this offset. Usually, this member is zero, and the menu item list follows immediately after the header.
        /// </summary>
        public ushort wOffset;
    }
}