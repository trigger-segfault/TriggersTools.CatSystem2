using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A menu template for a menu resource.
	/// </summary>
	public class MenuTemplate : BinaryReadableWriteable, IMenuTemplateItemContainer, IMenuBaseTemplate {
		#region Fields

		/// <summary>
		///  The header for the menu.
		/// </summary>
		private MENUITEMTEMPLATEHEADER header;
		/// <summary>
		///  The list of menu items within this menu.
		/// </summary>
		private List<IMenuTemplateItem> menuItems = new List<IMenuTemplateItem>();

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the list of root menu items within this menu.
		/// </summary>
		public List<IMenuTemplateItem> MenuItems {
            get => menuItems;
			set => menuItems = value ?? throw new ArgumentNullException(nameof(MenuItems));
		}
		IReadOnlyList<IMenuBaseTemplateItem> IMenuBaseTemplateItemContainer.MenuItems => MenuItems;

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of this menu template and all menu items.
		/// </summary>
		/// <returns>A clone of this menu template.</returns>
		public MenuTemplate Clone() {
			return new MenuTemplate {
				header = header,
				menuItems = new List<IMenuTemplateItem>(menuItems.Select(mi => mi.Clone())),
			};
		}
		IMenuBaseTemplate IMenuBaseTemplate.Clone() => Clone();

		#endregion

		#region Create

		IMenuBaseTemplateItemCommand IMenuBaseTemplate.CreateCommand() => new MenuTemplateItemCommand();
		IMenuBaseTemplateItemPopup IMenuBaseTemplate.CreatePopup() => new MenuTemplateItemPopup();

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<MENUITEMTEMPLATEHEADER>();
			reader.BaseStream.Position += header.wOffset;
			MenuTemplateUtils.ReadMenuItems(reader, menuItems);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			writer.WriteZeroBytes(header.wOffset);
			MenuTemplateUtils.WriteMenuItems(writer, menuItems);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu in the MENU format.
		/// </summary>
		/// <returns>The string representation of the menu.</returns>
		public override string ToString() => "MENU";

		#endregion
	}
}