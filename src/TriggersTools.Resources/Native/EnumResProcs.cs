using System;

namespace TriggersTools.Resources.Native {
	/// <summary>
	///  An application-defined callback function used with the <see cref="Kernel32.EnumResourceTypes"/> and
	///  <see cref="Kernel32.EnumResourceTypesEx"/> functions. It receives resource types. The
	///  <see cref="EnumResTypeProc"/> type defines a pointer to this callback function. <see cref="EnumResTypeProc"/>
	///  is a placeholder for the application-defined function name. 
	/// </summary>
	/// <param name="hModule">
	///  A handle to the module whose executable file contains the resources for which the types are to be enumerated.
	///  If this parameter is null, the function enumerates the resource types in the module used to create the current
	///  process. 
	/// </param>
	/// <param name="lpszType">
	///  The type of resource for which the type is being enumerated. Alternately, rather than a pointer, this
	///  parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the given resource type. For
	///  standard resource types, see Resource Types. For more information, see the Remarks section below.
	/// </param>
	/// <param name="lParam">
	///  An application-defined parameter passed to the <see cref="Kernel32.EnumResourceTypes"/> or
	///  <see cref="Kernel32.EnumResourceTypesEx"/> function. This parameter can be used in error checking.
	/// </param>
	/// <returns>Returns true to continue enumeration or false to stop enumeration.</returns>
	internal delegate bool EnumResTypeProc(
		IntPtr hModule,
		IntPtr lpszType,
		IntPtr lParam);

	/// <summary>
	///  An application-defined callback function used with the <see cref="Kernel32.EnumResourceNames"/> and
	///  <see cref="Kernel32.EnumResourceNamesEx"/> functions. It receives the type and name of a resource. The
	///  <see cref="EnumResNameProc"/> type defines a pointer to this callback function. <see cref="EnumResNameProc"/>
	///  is a placeholder for the application-defined function name. 
	/// </summary>
	/// <param name="hModule">
	///  A handle to the module whose executable file contains the resources that are being enumerated. If this
	///  parameter is null, the function enumerates the resource names in the module used to create the current
	///  process.
	/// </param>
	/// <param name="lpszType">
	///  The type of resource for which the name is being enumerated. Alternately, rather than a pointer, this
	///  parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource type.
	///  For standard resource types, see Resource Types. For more information, see the Remarks section below. 
	/// </param>
	/// <param name="lpszName">
	///  The name of a resource of the type being enumerated. Alternately, rather than a pointer, this parameter can be
	///  MAKEINTRESOURCE(ID), where ID is the integer identifier of the resource. For more information, see the Remarks
	///  section below. 
	/// </param>
	/// <param name="lParam">
	///  An application-defined parameter passed to the <see cref="Kernel32.EnumResourceNames"/> or
	///  <see cref="Kernel32.EnumResourceNamesEx"/> function. This parameter can be used in error checking.
	/// </param>
	/// <returns>Returns true to continue enumeration or false to stop enumeration.</returns>
	internal delegate bool EnumResNameProc(
		IntPtr hModule,
		IntPtr lpszType,
		IntPtr lpszName,
		IntPtr lParam);

	/// <summary>
	///  An application-defined callback function used with the <see cref="Kernel32.EnumResourceLanguages"/> and
	///  <see cref="Kernel32.EnumResourceLanguagesEx"/> functions. It receives the type, name, and language of a
	///  resource item. The <see cref="EnumResLangProc"/> type defines a pointer to this callback function.
	///  <see cref="EnumResLangProc"/> is a placeholder for the application-defined function name.
	/// </summary>
	/// <param name="hModule">
	///  A handle to the module whose executable file contains the resources for which the languages are being
	///  enumerated. If this parameter is null, the function enumerates the resource languages in the module used to
	///  create the current process.
	/// </param>
	/// <param name="lpszType">
	///  The type of resource for which the language is being enumerated. Alternately, rather than a pointer, this
	///  parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource type.
	///  For standard resource types, see Resource Types. For more information, see the Remarks section below.
	/// </param>
	/// <param name="lpszName">
	///  The name of the resource for which the language is being enumerated. Alternately, rather than a pointer, this
	///  parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the resource. For more
	///  information, see the Remarks section below. 
	/// </param>
	/// <param name="wIDLanguage">
	///  The language identifier for the resource for which the language is being enumerated. The
	///  <see cref="Kernel32.EnumResourceLanguages"/> or <see cref="Kernel32.EnumResourceLanguagesEx"/> function
	///  provides this value. For a list of the primary language identifiers and sublanguage identifiers that
	///  constitute a language identifier, see MAKELANGID.
	/// </param>
	/// <param name="lParam">
	///  The application-defined parameter passed to the <see cref="Kernel32.EnumResourceLanguages"/> or
	///  <see cref="Kernel32.EnumResourceLanguagesEx"/> function. This parameter can be used in error checking. 
	/// </param>
	/// <returns>Returns true to continue enumeration or false to stop enumeration.</returns>
	public delegate bool EnumResLangProc(
		IntPtr hModule,
		IntPtr lpszType,
		IntPtr lpszName,
		ushort wIDLanguage,
		IntPtr lParam);
}
