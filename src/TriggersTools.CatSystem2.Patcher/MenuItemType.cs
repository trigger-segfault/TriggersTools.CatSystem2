using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.ResourceTranslator {
	/// <summary>
	///  The type of menu item.
	/// </summary>
	public enum MenuItemType : ushort {
		/// <summary>
		///  The menu item is the last in this submenu or menu resource; this flag is used internally by the system.
		/// </summary>
		End = 0x80,
		/// <summary>
		///  The menu item opens a menu or a submenu; the flag is used internally by the system. 
		/// </summary>
		Popup = 0x01,
	}
}
