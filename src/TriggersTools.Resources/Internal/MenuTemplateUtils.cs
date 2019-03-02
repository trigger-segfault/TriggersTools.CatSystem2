using System.Collections.Generic;
using System.IO;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Menu;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Internal {
	/// <summary>
	///  Internal utility methods for working with menu templates.
	/// </summary>
	internal static class MenuTemplateUtils {
		#region MenuItems
		
		/// <summary>
		///  Reads the menu items from <paramref name="reader"/> to <paramref name="menuItems"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		/// <param name="menuItems">The menu items list to add to.</param>
		public static void ReadMenuItems(BinaryReader reader, List<IMenuTemplateItem> menuItems) {
			ushort option;
			do {
				option = reader.ReadUInt16();
				reader.BaseStream.Position -= 2;

				IMenuTemplateItem menuItem = null;
				if ((option & (uint) MenuFlags.MF_POPUP) != 0)
					menuItem = new MenuTemplateItemPopup();
				else
					menuItem = new MenuTemplateItemCommand();

				((BinaryReadableWriteable) menuItem).Read(reader);
				menuItems.Add(menuItem);

			} while ((option & (uint) MenuFlags.MF_END) == 0);
		}
		/// <summary>
		///  Writes the menu items from <paramref name="menuItems"/> <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="menuItems">The menu items list write.</param>
		public static void WriteMenuItems(BinaryWriter writer, List<IMenuTemplateItem> menuItems) {
			for (int i = 0; i < menuItems.Count; i++) {
				var menuItem = menuItems[i];
				if (i + 1 == menuItems.Count)
					menuItem.Option |= (ushort) MenuFlags.MF_END;
				else
					menuItem.Option &= unchecked((ushort) ~MenuFlags.MF_END);
				((BinaryReadableWriteable) menuItem).Write(writer);
			}
		}
		/// <summary>
		///  Reads the menu ex items from <paramref name="reader"/> to <paramref name="menuItems"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		/// <param name="menuItems">The menu ex items list to add to.</param>
		public static void ReadMenuExItems(BinaryReader reader, List<IMenuExTemplateItem> menuItems) {
			ushort resInfo;
			do {
				resInfo = reader.ReadUInt16();
				reader.BaseStream.Position -= 2;
				
				IMenuExTemplateItem menuItem = null;
				if ((resInfo & (uint) MenuExItemFlags.Sub) != 0)
					menuItem = new MenuExTemplateItemPopup();
				else
					menuItem = new MenuExTemplateItemCommand();

				((BinaryReadableWriteable) menuItem).Read(reader);
				menuItems.Add(menuItem);
			} while ((resInfo & (uint) MenuExItemFlags.Last) == 0);
		}
		/// <summary>
		///  Writes the menu ex items from <paramref name="menuItems"/> <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="menuItems">The menu ex items list write.</param>
		public static void WriteMenuExItems(BinaryWriter writer, List<IMenuExTemplateItem> menuItems) {
			for (int i = 0; i < menuItems.Count; i++) {
				writer.Pad(4);
				var menuItem = menuItems[i];
				if (i + 1 == menuItems.Count)
					menuItem.Option |= (ushort) MenuExItemFlags.Last;
				else
					menuItem.Option &= unchecked((ushort) ~MenuExItemFlags.Last);
				((BinaryReadableWriteable) menuItem).Write(writer);
			}
		}

		#endregion

		#region MenuItemString

		/// <summary>
		///  Reads the menu item string from <paramref name="reader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		/// <returns>The read menu item string.</returns>
		public static string ReadMenuItemString(BinaryReader reader) {
			string menuString = reader.ReadTerminatedString();
			reader.BaseStream.SkipPadding(2);
			return (menuString.Length != 0 ? menuString : null);
		}
		/// <summary>
		///  Writes the menu item string to <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="menuString">The menu item string write.</param>
		public static void WriteMenuItemString(BinaryWriter writer, string menuString) {
			writer.WriteTerminated(menuString ?? string.Empty);
			writer.Pad(2);
		}
		/// <summary>
		///  Reads the menu ex item string from <paramref name="reader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		/// <returns>The read menu ex item string.</returns>
		public static string ReadMenuExItemString(BinaryReader reader) {
			string menuString = reader.ReadTerminatedString();
			reader.BaseStream.SkipPadding(4);
			return (menuString.Length != 0 ? menuString : null);
		}
		/// <summary>
		///  Writes the menu ex item string to <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="menuString">The menu ex item string write.</param>
		public static void WriteMenuExItemString(BinaryWriter writer, string menuString) {
			writer.WriteTerminated(menuString ?? string.Empty);
			writer.Pad(4);
		}

		#endregion
	}
}
