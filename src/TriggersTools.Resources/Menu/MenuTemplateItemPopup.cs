using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A menu template item popup for a menu template.
	/// </summary>
	public class MenuTemplateItemPopup
		: BinaryReadableWriteable, IMenuTemplateItem, IMenuTemplateItemContainer, IMenuBaseTemplateItemPopup
	{
		#region Fields

		/// <summary>
		///  The header for the menu item.
		/// </summary>
		private MENUITEMTEMPLATE header;
		/// <summary>
		///  The list of menu items within this popup.
		/// </summary>
		private List<IMenuTemplateItem> menuItems = new List<IMenuTemplateItem>();
		/// <summary>
		///  Gets or sets the menu item text.
		/// </summary>
		public string MenuString { get; set; }

		#endregion

		#region Constructors

		public MenuTemplateItemPopup() {
			header.mtOption |= (ushort) MenuFlags.MF_POPUP;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the options for the menu template item.
		/// </summary>
		public ushort Option {
			get => header.mtOption;
			set => header.mtOption = value;
		}
		/// <summary>
		///  Gets or sets the sub menu items within this popup.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public List<IMenuTemplateItem> MenuItems {
            get => menuItems;
			set => menuItems = value ?? throw new ArgumentNullException(nameof(MenuItems));
        }
		IReadOnlyList<IMenuBaseTemplateItem> IMenuBaseTemplateItemContainer.MenuItems => MenuItems;

		#endregion
		
		#region Clone

		/// <summary>
		///  Creates a clone of this menu template item popup and all sub menu items.
		/// </summary>
		/// <returns>A clone of this menu template item popup.</returns>
		public MenuTemplateItemPopup Clone() {
			return new MenuTemplateItemPopup {
				header = header,
				menuItems = new List<IMenuTemplateItem>(menuItems.Select(mi => mi.Clone())),
				MenuString = MenuString,
			};
		}
		IMenuTemplateItem IMenuTemplateItem.Clone() => Clone();
		IMenuBaseTemplateItemPopup IMenuBaseTemplateItemPopup.Clone() => Clone();
		IMenuBaseTemplateItem IMenuBaseTemplateItem.Clone() => Clone();

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<MENUITEMTEMPLATE>();
			MenuString = MenuTemplateUtils.ReadMenuItemString(reader);
			MenuTemplateUtils.ReadMenuItems(reader, menuItems);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			MenuTemplateUtils.WriteMenuItemString(writer, MenuString);
			MenuTemplateUtils.WriteMenuItems(writer, menuItems);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu popup in the MENU format.
		/// </summary>
		/// <returns>The string representation of the menu popup.</returns>
		public override string ToString() => $"POPUP \"{MenuString?.Replace("\t", @"\t")}\"";

		#endregion
	}
}