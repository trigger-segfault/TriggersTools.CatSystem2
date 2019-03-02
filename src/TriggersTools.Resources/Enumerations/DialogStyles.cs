
namespace TriggersTools.Resources.Enumerations {
	/// <summary>
	///  Dialog styles. http://msdn.microsoft.com/en-us/library/ms644994(VS.85).aspx
	/// </summary>
	public enum DialogStyles : uint {
        /// <summary>
        ///  Specifying this style in the dialog template tells Windows that the dtX and dtY values of the
		///  DIALOGTEMPLATE struct are relative to the screen origin, not the owner of the dialog box.
        /// </summary>
        DS_ABSALIGN = 0x01,
        /// <summary>
        ///  Create a dialog box with the WS_EX_TOPMOST flag. This flag cannot be combined with the DS_CONTROL style.
		///  This flag is obsolete and is included for compatibility with 16-bit versions of Windows.
        /// </summary>
        DS_SYSMODAL = 0x02,
        /// <summary>
        ///  Applies to 16-bit applications only. This style directs edit controls in the dialog box to allocate memory
		///  from the application data segment. Otherwise, edit controls allocate storage from a global memory object.
        /// </summary>
        DS_LOCALEDIT = 0x20,
        /// <summary>
        ///  Indicates that the header of the dialog box template contains additional data specifying the font to use
		///  for text in the client area and controls of the dialog box.
        /// </summary>
        DS_SETFONT = 0x40,
        /// <summary>
        ///  Creates a dialog box with a modal dialog-box frame that can be combined with a title bar and window menu
		///  by specifying the WS_CAPTION and WS_SYSMENU styles.
        /// </summary>
        DS_MODALFRAME = 0x80,
        /// <summary>
        ///  Suppresses WM_ENTERIDLE messages that the system would otherwise send to the owner of the dialog box while
		///  the dialog box is displayed.
        /// </summary>
        DS_NOIDLEMSG = 0x100,
        /// <summary>
        ///  Causes the system to use the SetForegroundWindow function to bring the dialog box to the foreground.
        /// </summary>
        DS_SETFOREGROUND = 0x200,
        /// <summary>
        ///  Gives the dialog box a nonbold font and draws three-dimensional borders around control windows in the
		///  dialog box.
        /// </summary>
        DS_3DLOOK = 0x0004,
        /// <summary>
        ///  Causes the dialog box to use the SYSTEM_FIXED_FONT instead of the default SYSTEM_FONT. This is a monospace
		///  font compatible with the System font in 16-bit versions of Windows earlier than 3.0.
        /// </summary>
        DS_FIXEDSYS = 0x0008,
        /// <summary>
        ///  Creates the dialog box even if errors occur — for example, if a child window cannot be created or if the
		///  system cannot create a special data segment for an edit control.
        /// </summary>
        DS_NOFAILCREATE = 0x0010,
        /// <summary>
        ///  Creates a dialog box that works well as a child window of another dialog box, much like a page in a
		///  property sheet. This style allows the user to tab among the control windows of a child dialog box, use its
		///  accelerator keys, and so on.
        /// </summary>
        DS_CONTROL = 0x0400,
        /// <summary>
        ///  Centers the dialog box in the working area; that is, the area not obscured by the tray.
        /// </summary>
        DS_CENTER = 0x0800,
        /// <summary>
        ///  Centers the dialog box on the mouse cursor.
        /// </summary>
        DS_CENTERMOUSE = 0x1000,
        /// <summary>
        ///  Includes a question mark in the title bar of the dialog box. When the user clicks the question mark, the
		///  cursor changes to a question mark with a pointer. If the user then clicks a control in the dialog box, the
		///  control receives a WM_HELP message. The control should pass the message to the dialog box procedure, which
		///  should call the function using the HELP_WM_HELP command. The help application displays a pop-up window
		///  that typically contains help for the control.
        /// </summary>
        DS_CONTEXTHELP = 0x2000,
        /// <summary>
        ///  Indicates that the dialog box should use the system font.
        /// </summary>
        DS_SHELLFONT = DS_SETFONT | DS_FIXEDSYS,
        /// <summary>
        /// </summary>
        DS_USEPIXELS = 0x8000,
    }
}