using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
    /// <summary>
    ///     Defines the header for an extended menu template.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MENUEX_TEMPLATE_HEADER {
        /// <summary>
        ///     Template version number. This member must be 1 for extended menu templates.
        /// </summary>
        public ushort wVersion;

        /// <summary>
        ///     Offset of the first MENUEXITEMTEMPLATE structure, relative to the end of this structure member. If the first item definition immediately follows the dwHelpId member, this member should be 4.
        /// </summary>
        public ushort wOffset;
    }
}