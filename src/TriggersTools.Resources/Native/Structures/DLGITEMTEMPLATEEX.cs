using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native.Structures {
	/// <summary>
	///  A block of text used by an extended dialog box template to describe the extended dialog box. For a description
	///  of the format of an extended dialog box template, see <see cref="DLGTEMPLATEEX"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct DLGITEMTEMPLATEEX {
		/// <summary>
		///  The help context identifier for the control. When the system sends a WM_HELP message, it passes the
		///  <see cref="helpID"/> value in the dwContextId member of the HELPINFO structure.
		/// </summary>
		public uint helpID;
		/// <summary>
		///  The extended styles for a window. This member is not used to create controls in dialog boxes, but
		///  applications that use dialog box templates can use it to create other types of windows. For a list of
		///  values, see Extended Window Styles.
		/// </summary>
		public uint exStyle;
		/// <summary>
		///  The style of the control. This member can be a combination of window style values (such as WS_BORDER) and
		///  one or more of the control style values (such as BS_PUSHBUTTON and ES_LEFT).
		/// </summary>
		public uint style;
		/// <summary>
		///  The x-coordinate, in dialog box units, of the upper-left corner of the control. This coordinate is always
		///  relative to the upper-left corner of the dialog box's client area.
		/// </summary>
		public short x;
		/// <summary>
		///  The y-coordinate, in dialog box units, of the upper-left corner of the control. This coordinate is always
		///  relative to the upper-left corner of the dialog box's client area.
		/// </summary>
		public short y;
		/// <summary>
		///  The width, in dialog box units, of the control.
		/// </summary>
		public short cx;
		/// <summary>
		///  The height, in dialog box units, of the control.
		/// </summary>
		public short cy;
		/// <summary>
		///  The control identifier.
		/// </summary>
		public int id;
    }
}