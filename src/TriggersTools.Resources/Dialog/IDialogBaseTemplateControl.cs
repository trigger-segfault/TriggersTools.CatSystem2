using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Resources.Enumerations;

namespace TriggersTools.Resources.Dialog {
	/// <summary>
	///  A template for a base dialog control.
	/// </summary>
	public interface IDialogBaseTemplateControl {
		/// <summary>
		///  Gets or sets the x-coordinate, in dialog box units, of the upper-left corner of the dialog control.
		/// </summary>
		short X { get; set; }
		/// <summary>
		///  Gets or sets the y-coordinate, in dialog box units, of the upper-left corner of the dialog control.
		/// </summary>
		short Y { get; set; }
		/// <summary>
		///  Gets or sets the width, in dialog box units, of the dialog control.
		/// </summary>
		short Width { get; set; }
		/// <summary>
		///  Gets or sets the height, in dialog box units, of the dialog control.
		/// </summary>
		short Height { get; set; }
		/// <summary>
		///  Gets or sets the dialog control style.
		/// </summary>
		uint Style { get; set; }
		/// <summary>
		///  Gets or sets the extended dialog control style.
		/// </summary>
		uint ExtendedStyle { get; set; }

		/// <summary>
		///  Gets or sets the dialog control caption.
		/// </summary>
		ResourceId CaptionId { get; set; }
		/// <summary>
		///  Gets or sets the dialog control class Id.
		/// </summary>
		ResourceId ControlClassId { get; set; }
		/// <summary>
		///  Gets the dialog control class.
		/// </summary>
		DialogItemClass ControlClass { get; }

		/// <summary>
		///  Gets or sets the additional dialog control creation data.
		/// </summary>
		byte[] CreationData { get; set; }

		/*/// <summary>
		///  Reads the dialog control template from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		void Read(BinaryReader reader);
		/// <summary>
		///  Writes the dialog control template to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		void Write(BinaryWriter writer);*/
		/// <summary>
		///  Creates a clone of this dialog base control template.
		/// </summary>
		/// <returns>A clone of this dialog base control template.</returns>
		IDialogBaseTemplateControl Clone();
	}
}
