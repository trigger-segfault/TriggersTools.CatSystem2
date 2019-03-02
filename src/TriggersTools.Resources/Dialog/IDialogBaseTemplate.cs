using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.Resources.Dialog {
	/// <summary>
	///  A template for a base dialog.
	/// </summary>
	public interface IDialogBaseTemplate {
		/// <summary>
		///  Gets or sets the x-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		short X { get; set; }

		/// <summary>
		///  Gets or sets the y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		short Y { get; set; }

		/// <summary>
		///  Gets or sets the width, in dialog box units, of the dialog box.
		/// </summary>
		short Width { get; set; }

		/// <summary>
		///  Gets or sets the height, in dialog box units, of the dialog box.
		/// </summary>
		short Height { get; set; }

		/// <summary>
		///  Gets or sets the dialog style.
		/// </summary>
		uint Style { get; set; }

		/// <summary>
		///  Gets or sets the extended dialog style.
		/// </summary>
		uint ExtendedStyle { get; set; }


		/// <summary>
		///  Gets or sets the dialog caption.
		/// </summary>
		string Caption { get; set; }
		/// <summary>
		///  Gets or sets the dialog menu resource Id.
		/// </summary>
		ResourceId MenuId { get; set; }
		/// <summary>
		///  Gets or sets the dialog window class Id.
		/// </summary>
		ResourceId WindowClassId { get; set; }
		/// <summary>
		///  Gets or sets the dialog font typeface.
		/// </summary>
		string TypeFace { get; set; }
		/// <summary>
		///  Gets or sets the dialog font point size.
		/// </summary>
		ushort PointSize { get; set; }

		/// <summary>
		///  Gets if the dialog style allows for a font.
		/// </summary>
		bool HasFont { get; }

		/// <summary>
		///  Gets the list of controls within this dialog.
		/// </summary>
		IReadOnlyList<IDialogBaseTemplateControl> Controls { get; }
		

		/// <summary>
		///  Creates a dialog control template for this dialog type.
		/// </summary>
		/// <returns>A dialog control template of this dialog type.</returns>
		IDialogBaseTemplateControl CreateControl();

		/*/// <summary>
		///  Reads the dialog template from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		void Read(BinaryReader reader);
		/// <summary>
		///  Writes the dialog template to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		void Write(BinaryWriter writer);*/
		/// <summary>
		///  Creates a clone of this dialog base template and all dialog controls.
		/// </summary>
		/// <returns>A clone of this dialog base template.</returns>
		IDialogBaseTemplate Clone();
	}
}
