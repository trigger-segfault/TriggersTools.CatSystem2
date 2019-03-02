//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     ResourceLib Original Code from http://resourcelib.codeplex.com
//     Original Copyright (c) 2008-2009 Vestris Inc.
//     Changes Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
// <license>
// MIT License
using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
	/// <summary>
	///  Defines a menu item in an extended menu template. This structure definition is for explanation only; it is not
	///  present in any standard header file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MENUEX_TEMPLATE_ITEM {
		/// <summary>
		///  The menu item type. This member can be a combination of the type (beginning with MFT) values listed with
		///  the MENUITEMINFO structure.
		/// </summary>
		public uint dwType;
		/// <summary>
		///  The menu item state. This member can be a combination of the state (beginning with MFS) values listed with
		///  the MENUITEMINFO structure.
		/// </summary>
		public uint dwState;
		/// <summary>
		///  The menu item identifier. This is an application-defined value that identifies the menu item. In an
		///  extended menu resource, items that open drop-down menus or submenus as well as command items can have
		///  identifiers.
		/// </summary>
		public uint uId;
		/// <summary>
		///  Specifies whether the menu item is the last item in the menu bar, drop-down menu, submenu, or shortcut menu and whether it is an item that opens a drop-down menu or submenu. This member can be zero or more of these values. For 32-bit applications, this member is a word; for 16-bit applications, it is a byte.
		/// </summary>
		public ushort wFlags;
    }
}