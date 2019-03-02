using System;
using System.Collections.Generic;
using System.IO;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A template for a base menu.
	/// </summary>
	public interface IMenuBaseTemplate : IMenuBaseTemplateItemContainer {

		/// <summary>
		///  Creates a command menu item template for this menu type.
		/// </summary>
		/// <returns>A command menu item template of this menu type.</returns>
		IMenuBaseTemplateItemCommand CreateCommand();
		/// <summary>
		///  Creates a popup menu item template for this menu type.
		/// </summary>
		/// <returns>A popup menu item template of this menu type.</returns>
		IMenuBaseTemplateItemPopup CreatePopup();

		/*/// <summary>
		///  Reads the menu template from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		void Read(BinaryReader reader);
		/// <summary>
		///  Writes the menu template to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		void Write(BinaryWriter writer);*/
		/// <summary>
		///  Creates a clone of this menu template and all menu items.
		/// </summary>
		/// <returns>A clone of this menu template.</returns>
		IMenuBaseTemplate Clone();
	}
}
