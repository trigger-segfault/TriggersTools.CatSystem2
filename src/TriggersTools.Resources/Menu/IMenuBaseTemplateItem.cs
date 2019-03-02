using System.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a base menu item.
	/// </summary>
	public interface IMenuBaseTemplateItem {
		/// <summary>
		///  Gets or sets the menu item text.
		/// </summary>
		string MenuString { get; set; }
		/// <summary>
		///  Gets or sets the options for the menu template item.
		/// </summary>
		ushort Option { get; set; }

		/*/// <summary>
		///  Reads the menu template item from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		void Read(BinaryReader reader);
		/// <summary>
		///  Writes the menu template item to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		void Write(BinaryWriter writer);*/
		/// <summary>
		///  Creates a clone of this menu template item and all sub menu items if this is a popup.
		/// </summary>
		/// <returns>A clone of this menu template item.</returns>
		IMenuBaseTemplateItem Clone();
	}
}
