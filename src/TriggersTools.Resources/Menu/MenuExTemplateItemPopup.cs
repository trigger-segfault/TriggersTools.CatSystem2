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
	///  A menu extended template item popup for a menu template.
	/// </summary>
	public class MenuExTemplateItemPopup
		: BinaryReadableWriteable, IMenuExTemplateItem, IMenuExTemplateItemContainer, IMenuBaseTemplateItemPopup
	{
		#region Fields

		/// <summary>
		///  The header for the menu item.
		/// </summary>
		private MENUEX_TEMPLATE_ITEM header;
		/// <summary>
		///  The list of menu ex items within this popup.
		/// </summary>
		private List<IMenuExTemplateItem> menuItems = new List<IMenuExTemplateItem>();
		/// <summary>
		///  Gets or sets the menu help Id.
		/// </summary>
		public uint HelpId { get; set; }
		/// <summary>
		///  Gets or sets the menu text.
		/// </summary>
		public string MenuString { get; set; }

		#endregion

		#region Constructors

		public MenuExTemplateItemPopup() {
			header.wFlags |= (ushort) MenuExItemFlags.Sub;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the options for the menu template item.
		/// </summary>
		public ushort Option {
			get => header.wFlags;
			set => header.wFlags = value;
		}
		/// <summary>
		///  Gets or sets the type of the menu item.
		/// </summary>
		public uint Type {
			get => header.dwType;
			set => header.dwType = value;
		}
		/// <summary>
		///  Gets or sets the state of the menu item.
		/// </summary>
		public uint State {
			get => header.dwState;
			set => header.dwState = value;
		}
		/// <summary>
		///  Gets or sets the command menu item id.
		/// </summary>
		public ushort MenuId {
			get => (ushort) header.uId;
			set => header.uId = value;
		}

		/// <summary>
		///  Gets or sets the sub menu items within this popup.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public List<IMenuExTemplateItem> MenuItems {
			get => menuItems;
			set => menuItems = value ?? throw new ArgumentNullException(nameof(MenuItems));
		}
		IReadOnlyList<IMenuBaseTemplateItem> IMenuBaseTemplateItemContainer.MenuItems => MenuItems;

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of this menu ex template item popup and all sub menu items.
		/// </summary>
		/// <returns>A clone of this menu ex template item popup.</returns>
		public MenuExTemplateItemPopup Clone() {
			return new MenuExTemplateItemPopup {
				header = header,
				menuItems = new List<IMenuExTemplateItem>(menuItems.Select(mi => mi.Clone())),
				MenuString = MenuString,
				HelpId = HelpId,
			};
		}
		IMenuExTemplateItem IMenuExTemplateItem.Clone() => Clone();
		IMenuBaseTemplateItemPopup IMenuBaseTemplateItemPopup.Clone() => Clone();
		IMenuBaseTemplateItem IMenuBaseTemplateItem.Clone() => Clone();

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<MENUEX_TEMPLATE_ITEM>();
			MenuString = MenuTemplateUtils.ReadMenuExItemString(reader);
			HelpId = reader.ReadUInt32();
			MenuTemplateUtils.ReadMenuExItems(reader, menuItems);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			MenuTemplateUtils.WriteMenuExItemString(writer, MenuString);
			writer.Write(HelpId);
			MenuTemplateUtils.WriteMenuExItems(writer, menuItems);
		}

		#endregion
		
		#region ToString Override
		
		/// <summary>
		///  Gets the string representation of the menu popup in the MENUEX format.
		/// </summary>
		/// <returns>The string representation of the menu popup.</returns>
		public override string ToString() => $"POPUP \"{MenuString?.Replace("\t", @"\t")}\"";

		#endregion
	}
}