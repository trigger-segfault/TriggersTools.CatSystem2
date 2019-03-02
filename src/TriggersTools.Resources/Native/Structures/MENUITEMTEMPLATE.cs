using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
	/// <summary>
	///     Defines a menu item in a menu template.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct MENUITEMTEMPLATE {
        /// <summary>
        ///     Specifies one or more of the following predefined menu options that control the appearance of the menu item. TODO
        /// </summary>
        public ushort mtOption;
    }
}