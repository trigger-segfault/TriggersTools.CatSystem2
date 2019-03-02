using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
	/// <summary>
	///  An extended dialog box template begins with a <see cref="DLGTEMPLATEEX"/> header that describes the dialog box
	///  and specifies the number of controls in the dialog box. For each control in a dialog box, an extended dialog
	///  box template has a block of data that uses the <see cref="DLGITEMTEMPLATEEX"/> format to describe the control.
	///  <para/>
	///  
	///  The <see cref="DLGTEMPLATEEX"/> structure is not defined in any standard header file. The structure definition
	///  is provided here to explain the format of an extended template for a dialog box.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct DLGTEMPLATEEX {
		/// <summary>
		///  The version number of the extended dialog box template. This member must be set to 1.
		/// </summary>
		public ushort dlgVer;
		/// <summary>
		///  Indicates whether a template is an extended dialog box template. If signature is 0xFFFF, this is an
		///  extended dialog box template. In this case, the <see cref="dlgVer"/> member specifies the template version
		///  number. If signature is any value other than 0xFFFF, this is a standard dialog box template that uses the
		///  <see cref="DLGTEMPLATE"/> and <see cref="DLGITEMTEMPLATE"/> structures.
		/// </summary>
		public ushort signature;
		/// <summary>
		///  The help context identifier for the dialog box window. When the system sends a WM_HELP message, it passes
		///  this value in the wContextId member of the HELPINFO structure.
		/// </summary>
		public uint helpID;
		/// <summary>
		///  The extended windows styles. This member is not used when creating dialog boxes, but applications that use
		///  dialog box templates can use it to create other types of windows. For a list of values, see Extended
		///  Window Styles.
		/// </summary>
		public uint exStyle;
		/// <summary>
		///  The style of the dialog box.This member can be a combination of window style values and dialog box style
		///  values.<para/>
		///  
		///  If style includes the DS_SETFONT or DS_SHELLFONT dialog box style, the <see cref="DLGTEMPLATEEX"/> header
		///  of the extended dialog box template contains four additional members (pointsize, weight, italic, and
		///  typeface) that describe the font to use for the text in the client area and controls of the dialog box. If
		///  possible, the system creates a font according to the values specified in these members. Then the system
		///  sends a WM_SETFONT message to the dialog box and to each control to provide a handle to the font.<para/>
		///  
		///  For more information, see Dialog Box Fonts.
		/// </summary>
		public uint style;
		/// <summary>
		///  The number of controls in the dialog box.
		/// </summary>
		public ushort cDlgItems;
		/// <summary>
		///  The x-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short x;
		/// <summary>
		///  The y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short y;
		/// <summary>
		///  The width, in dialog box units, of the dialog box.
		/// </summary>
		public short cx;
		/// <summary>
		///  The height, in dialog box units, of the dialog box.
		/// </summary>
		public short cy;
    }
}