using System.IO;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A menu extended template item command for a menu template.
	/// </summary>
	public class MenuExTemplateItemCommand : BinaryReadableWriteable, IMenuExTemplateItem, IMenuBaseTemplateItemCommand {
		#region Fields

		/// <summary>
		///  The header for the menu item.
		/// </summary>
		private MENUEX_TEMPLATE_ITEM header;
		/// <summary>
		///  Gets or sets the menu item text.
		/// </summary>
		public string MenuString { get; set; }

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
		///  Gets or sets if the menu item is a separator.
		/// </summary>
		public bool IsSeparator {
            get {
                return Type == (uint) MenuFlags.MFT_SEPARATOR ||
                    ((Option == 0xFFFF || Option == 0) && MenuId == 0 && MenuString == null);
            }
			set {
				Type = (uint) MenuFlags.MFT_SEPARATOR;
				Option = 0;
				MenuId = 0;
				MenuString = null;
			}
        }

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of this menu ex template item command.
		/// </summary>
		/// <returns>A clone of this menu ex template item command.</returns>
		public MenuExTemplateItemCommand Clone() {
			return new MenuExTemplateItemCommand {
				header = header,
				MenuString = MenuString,
			};
		}
		IMenuExTemplateItem IMenuExTemplateItem.Clone() => Clone();
		IMenuBaseTemplateItemCommand IMenuBaseTemplateItemCommand.Clone() => Clone();
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
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			MenuTemplateUtils.WriteMenuExItemString(writer, MenuString);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu command in the MENUEX format.
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