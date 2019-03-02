using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Menu {
    /// <summary>
    ///  A menu extended template for a menu resource.
    /// </summary>
    public class MenuExTemplate : BinaryReadableWriteable, IMenuExTemplateItemContainer, IMenuBaseTemplate {
		#region Fields

		/// <summary>
		///  The header for the menu.
		/// </summary>
		private MENUEX_TEMPLATE_HEADER header;
		/// <summary>
		///  The list of menu ex items within this menu.
		/// </summary>
		private List<IMenuExTemplateItem> menuItems = new List<IMenuExTemplateItem>();

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the list of root menu items within this menu.
		/// </summary>
		public List<IMenuExTemplateItem> MenuItems {
            get => menuItems;
			set => menuItems = value ?? throw new ArgumentNullException(nameof(MenuItems));
        }
		IReadOnlyList<IMenuBaseTemplateItem> IMenuBaseTemplateItemContainer.MenuItems => MenuItems;
		/// <summary>
		///  Gets or sets the menu help Id.
		/// </summary>
		public uint HelpId { get; set; }

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of this menu ex template and all menu items.
		/// </summary>
		/// <returns>A clone of this menu ex template.</returns>
		public MenuExTemplate Clone() {
			return new MenuExTemplate {
				header = header,
				menuItems = new List<IMenuExTemplateItem>(menuItems.Select(mi => mi.Clone())),
			};
		}
		IMenuBaseTemplate IMenuBaseTemplate.Clone() => Clone();

		#endregion

		#region Create

		IMenuBaseTemplateItemCommand IMenuBaseTemplate.CreateCommand() => new MenuExTemplateItemCommand();
		IMenuBaseTemplateItemPopup IMenuBaseTemplate.CreatePopup() => new MenuExTemplateItemPopup();

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<MENUEX_TEMPLATE_HEADER>();
			MenuTemplateUtils.ReadMenuExItems(reader, menuItems);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			writer.WriteZeroBytes(header.wOffset);
			MenuTemplateUtils.WriteMenuExItems(writer, menuItems);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu in the MENUEX format.
		/// </summary>
		/// <returns>The string representation of the menu.</returns>
		public override string ToString() => "MENUEX";

		#endregion
	}
}