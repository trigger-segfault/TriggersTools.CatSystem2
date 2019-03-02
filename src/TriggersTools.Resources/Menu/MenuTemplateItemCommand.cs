using System.IO;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A menu template item command for a menu template.
	/// </summary>
	public class MenuTemplateItemCommand : BinaryReadableWriteable, IMenuTemplateItem, IMenuBaseTemplateItemCommand {
		#region Fields

		/// <summary>
		///  The header for the menu item.
		/// </summary>
		private MENUITEMTEMPLATE header;
		/// <summary>
		///  Gets or sets the menu item text.
		/// </summary>
		public string MenuString { get; set; }
		/// <summary>
		///  Gets or sets the command menu item id.
		/// </summary>
		public ushort MenuId { get; set; }

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
		///  Gets or sets if the menu item is a separator.
		/// </summary>
		public bool IsSeparator {
            get {
                return ((header.mtOption & (uint) MenuFlags.MF_SEPARATOR) != 0) ||
						(header.mtOption == 0 && MenuString == null && MenuId == 0);
            }
			set {
				header.mtOption |= (ushort) MenuFlags.MF_SEPARATOR;
				MenuString = null;
				MenuId = 0;
			}
        }

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of this menu template item command.
		/// </summary>
		/// <returns>A clone of this menu template item command.</returns>
		public MenuTemplateItemCommand Clone() {
			return new MenuTemplateItemCommand {
				header = header,
				MenuString = MenuString,
				MenuId = MenuId,
			};
		}
		IMenuTemplateItem IMenuTemplateItem.Clone() => Clone();
		IMenuBaseTemplateItemCommand IMenuBaseTemplateItemCommand.Clone() => Clone();
		IMenuBaseTemplateItem IMenuBaseTemplateItem.Clone() => Clone();

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<MENUITEMTEMPLATE>();
			MenuId = reader.ReadUInt16();
			MenuString = MenuTemplateUtils.ReadMenuItemString(reader);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			writer.Write(MenuId);
			MenuTemplateUtils.WriteMenuItemString(writer, MenuString);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu command in the MENU format.
		/// </summary>
		/// <returns>The string representation of the menu command.</returns>
		public override string ToString() {
			if (IsSeparator)
				return "MENUITEM SEPARATOR";
			else
				return $"MENUITEM \"{MenuString.Replace("\t", @"\t")}";
        }

		#endregion
	}
}