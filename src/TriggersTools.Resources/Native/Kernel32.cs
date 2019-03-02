using System;
using System.Runtime.InteropServices;

namespace TriggersTools.Resources.Native {
	/// <summary>
	///  A static class for Kernal32.dll native methods.
	/// </summary>
	internal static class Kernel32 {
		/*/// <summary>
		///  Language identifiers used in <see cref="UpdateResource"/>.
		/// </summary>
		public enum LanguageIdentifier : ushort {
			/// <summary>Default custom locale language</summary>
			Neutral = 0x0C00,
			/// <summary>English (en)</summary>
			English = 0x0C09,
			/// <summary>Japanese (ja)</summary>
			Japanese = 0x0411,
		}*/

		/// <summary>
		///  Loads the specified module into the address space of the calling process. The specified module may cause
		///  other modules to be loaded.<para/>
		///  
		///  For additional load options, use the <see cref="LoadLibraryEx"/> function.
		/// </summary>
		/// <param name="lpLibFileName">
		///  The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe
		///  file). The name specified is the file name of the module and is not related to the name stored in the
		///  library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file.<para/>
		///  
		///  If the string specifies a full path, the function searches only that path for the module.<para/>
		///  
		///  If the string specifies a relative path or a module name without a path, the function uses a standard
		///  search strategy to find the module; for more information, see the Remarks.<para/>
		///  
		///  If the function cannot find the module, the function fails. When specifying a path, be sure to use
		///  backslashes (), not forward slashes (/). For more information about paths, see Naming a File or Directory.
		///  <para/>
		///  
		///  If the string specifies a module name without a path and the file name extension is omitted, the function
		///  appends the default library extension .dll to the module name. To prevent the function from appending .dll
		///  to the module name, include a trailing point character (.) in the module name string.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the loaded module. If the function fails, the
		///  return value is null. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LoadLibrary(
			string lpLibFileName);

		/// <summary>
		///  Loads the specified module into the address space of the calling process. The specified module may cause
		///  other modules to be loaded.
		/// </summary>
		/// <param name="lpLibFileName">
		///  A string that specifies the file name of the module to load. This name is not related to the name stored
		///  in a library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file.<para/>
		///  
		///  The module can be a library module(a.dll file) or an executable module(an.exe file). If the specified
		///  module is an executable module, static imports are not loaded; instead, the module is loaded as if
		///  <see cref="LoadLibraryExFlags.DontResolveDllReferences"/> was specified. See the dwFlags parameter for
		///  more information.<para/>
		///  
		///  If the string specifies a module name without a path and the file name extension is omitted, the function
		///  appends the default library extension .dll to the module name.To prevent the function from appending .dll
		///  to the module name, include a trailing point character (.) in the module name string.<para/>
		///  
		///  If the string specifies a fully qualified path, the function searches only that path for the module. When
		///  specifying a path, be sure to use backslashes (), not forward slashes (/). For more information about
		///  paths, see Naming Files, Paths, and Namespaces.<para/>
		///  
		///  If the string specifies a module name without a path and more than one loaded module has the same base
		///  name and extension, the function returns a handle to the module that was loaded first.<para/>
		///  
		///  If the string specifies a module name without a path and a module of the same name is not already loaded,
		///  or if the string specifies a module name with a relative path, the function searches for the specified
		///  module. The function also searches for modules if loading the specified module causes the system to load
		///  other associated modules (that is, if the module has dependencies). The directories that are searched and
		///  the order in which they are searched depend on the specified path and the dwFlags parameter. For more
		///  information, see Remarks.<para/>
		///  
		///  If the function cannot find the module or one of its dependencies, the function fails.
		/// </param>
		/// <param name="hFile">This parameter is reserved for future use. It must be null.</param>
		/// <param name="dwFlags">The action to be taken when loading the module.</param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the loaded module. If the function fails, the
		///  return value is null. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LoadLibraryEx(
			string lpLibFileName,
			IntPtr hFile,
			[MarshalAs(UnmanagedType.U4)] LoadLibraryExFlags dwFlags);

		/// <summary>
		///  Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When
		///  the reference count reaches zero, the module is unloaded from the address space of the calling process and
		///  the handle is no longer valid.
		/// </summary>
		/// <param name="hLibModule">
		///  A handle to the loaded library module. The <see cref="LoadLibrary"/>, or <see cref="LoadLibraryEx"/>
		///  returns this handle.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is true.<para/>
		///  
		///  If the function fails, the return value is false.To get extended error information, call the GetLastError
		///  function.
	    /// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static bool FreeLibrary(
			IntPtr hLibModule);

		/// <summary>
		///  Determines the location of a resource with the specified type and name in the specified module.<para/>
		///  
		///  To specify a language, use the <see cref="FindResourceEx"/> function.
		/// </summary>
		/// <param name="hModule">
		///  A handle to the module whose portable executable file or an accompanying MUI file contains the resource.
		///  If this parameter is null, the function searches the module used to create the current process.
		/// </param>
		/// <param name="lpName">
		///  The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID),
		///  where ID is the integer identifier of the resource. For more information, see the Remarks section below.
		/// </param>
		/// <param name="lpType">
		///  The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		///  ID is the integer identifier of the given resource type.For standard resource types, see Resource Types.
		///  For more information, see the Remarks section below.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the specified resource's information block. To
		///  obtain a handle to the resource, pass this handle to the <see cref="LoadResource"/> function.<para/>
		///  
		///  If the function fails, the return value is null. To get extended error information, call GetLastError.
	    /// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static IntPtr FindResource(
			IntPtr hModule,
			IntPtr lpName,
			IntPtr lpType);

		/// <summary>
		///  Determines the location of a resource with the specified type and name in the specified module.<para/>
		///  
		///  To specify a language, use the <see cref="FindResourceEx"/> function.
		/// </summary>
		/// <param name="hModule">
		///  A handle to the module whose portable executable file or an accompanying MUI file contains the resource.
		///  If this parameter is null, the function searches the module used to create the current process.
		/// </param>
		/// <param name="lpName">
		///  The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID),
		///  where ID is the integer identifier of the resource. For more information, see the Remarks section below.
		/// </param>
		/// <param name="lpType">
		///  The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		///  ID is the integer identifier of the given resource type.For standard resource types, see Resource Types.
		///  For more information, see the Remarks section below.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the specified resource's information block. To
		///  obtain a handle to the resource, pass this handle to the <see cref="LoadResource"/> function.<para/>
		///  
		///  If the function fails, the return value is null. To get extended error information, call GetLastError.
		/// </returns>
		/*[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static IntPtr FindResource(
			IntPtr hModule,
			string lpName,
			string lpType);*/

		/// <summary>
		///  Determines the location of the resource with the specified type, name, and language in the specified
		///  module.
		/// </summary>
		/// <param name="hModule">
		///  A handle to the module whose portable executable file or an accompanying MUI file contains the resource.
		///  If this parameter is null, the function searches the module used to create the current process.
		/// </param>
		/// <param name="lpName">
		///  The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID),
		///  where ID is the integer identifier of the resource. For more information, see the Remarks section below.
		/// </param>
		/// <param name="lpType">
		///  The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		///  ID is the integer identifier of the given resource type.For standard resource types, see Resource Types.
		///  For more information, see the Remarks section below.
		/// </param>
		/// <param name="wLanguage">
		///  The language of the resource. If this parameter is MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), the current
		///  language associated with the calling thread is used.<para/>
		///  
		///  To specify a language other than the current language, use the MAKELANGID macro to create this parameter.
		///  For more information, see MAKELANGID.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the specified resource's information block. To
		///  obtain a handle to the resource, pass this handle to the <see cref="LoadResource"/> function.<para/>
		///  
		///  If the function fails, the return value is null. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static IntPtr FindResourceEx(
			IntPtr hModule,
			IntPtr lpName,
			IntPtr lpType,
			ushort wLanguage);

		/// <summary>
		///  Determines the location of the resource with the specified type, name, and language in the specified
		///  module.
		/// </summary>
		/// <param name="hModule">
		///  A handle to the module whose portable executable file or an accompanying MUI file contains the resource.
		///  If this parameter is null, the function searches the module used to create the current process.
		/// </param>
		/// <param name="lpName">
		///  The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID),
		///  where ID is the integer identifier of the resource. For more information, see the Remarks section below.
		/// </param>
		/// <param name="lpType">
		///  The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		///  ID is the integer identifier of the given resource type.For standard resource types, see Resource Types.
		///  For more information, see the Remarks section below.
		/// </param>
		/// <param name="wLanguage">
		///  The language of the resource. If this parameter is MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), the current
		///  language associated with the calling thread is used.<para/>
		///  
		///  To specify a language other than the current language, use the MAKELANGID macro to create this parameter.
		///  For more information, see MAKELANGID.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the specified resource's information block. To
		///  obtain a handle to the resource, pass this handle to the <see cref="LoadResource"/> function.<para/>
		///  
		///  If the function fails, the return value is null. To get extended error information, call GetLastError.
		/// </returns>
		/*[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static IntPtr FindResourceEx(
			IntPtr hModule,
			string lpName,
			string lpType,
			ushort wLanguage);*/

		/// <summary>
		///  Retrieves a handle that can be used to obtain a pointer to the first byte of the specified resource in
		///  memory.
		/// </summary>
		/// <param name="hModule">
		///  A handle to the module whose executable file contains the resource. If hModule is NULL, the system loads
		///  the resource from the module that was used to create the current process.
		/// </param>
		/// <param name="hResInfo">
		///  A handle to the resource to be loaded. This handle is returned by the <see cref="FindResource"/> or
		///  <see cref="FindResourceEx"/> function.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle to the data associated with the resource.<para/>
		///  
		///  If the function fails, the return value is null. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LoadResource(
			IntPtr hModule,
			IntPtr hResInfo);

		/// <summary>
		///  Retrieves the size, in bytes, of the specified resource.
		/// </summary>
		/// <param name="hModule">A handle to the module whose executable file contains the resource.</param>
		/// <param name="hResInfo">
		///  A handle to the resource to be loaded. This handle is returned by the <see cref="FindResource"/> or
		///  <see cref="FindResourceEx"/> function.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is the number of bytes in the resource.<para/>
		///  
		///  If the function fails, the return value is zero.To get extended error information, call GetLastError.
	    /// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static int SizeofResource(
			IntPtr hModule,
			IntPtr hResInfo);

		/// <summary>
		///  Retrieves a pointer to the specified resource in memory.
		/// </summary>
		/// <param name="hGlobal">
		///  A handle to the resource to be accessed. The <see cref="LoadResource"/> function returns this handle. Note
		///  that this parameter is listed as an HGLOBAL variable only for backward compatibility. Do not pass any
		///  value as a parameter other than a successful return value from the LoadResource function.
		/// </param>
		/// <returns>
		///  If the loaded resource is available, the return value is a pointer to the first byte of the resource;
		///  otherwise, it is null.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LockResource(
			IntPtr hGlobal);

		/// <summary>
		///  Retrieves a handle that can be used by the <see cref="UpdateResource"/> function to add, delete, or
		///  replace resources in a binary module.
		/// </summary>
		/// <param name="pFileName">
		///  The binary file in which to update resources. An application must be able to obtain write-access to this
		///  file; the file referenced by <paramref name="pFileName"/> cannot be currently executing. If pFileName does
		///  not specify a full path, the system searches for the file in the current directory.
		/// </param>
		/// <param name="bDeleteExistingResources">
		///  Indicates whether to delete the <paramref name="pFileName"/> parameter's existing resources. If this
		///  parameter is true, existing resources are deleted and the updated file includes only resources added with
		///  the <see cref="UpdateResource"/> function. If this parameter is false, the updated file includes existing
		///  resources unless they are explicitly deleted or replaced by using <see cref="UpdateResource"/>.
		/// </param>
		/// <returns>
		///  If the function succeeds, the return value is a handle that can be used by the
		///  <see cref="UpdateResource"/> and <see cref="EndUpdateResource"/> functions. The return value is null if
		///  the specified file is not a PE, the file does not exist, or the file cannot be opened for writing. 
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr BeginUpdateResource(
			string pFileName,
			[MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

		/// <summary>
		///  Adds, deletes, or replaces a resource in a portable executable (PE) file. There are some restrictions on
		///  resource updates in files that contain Resource Configuration (RC Config) data: language-neutral (LN)
		///  files and language-specific resource (.mui) files.
		/// </summary>
		/// <param name="hUpdate">
		///  A module handle returned by the <see cref="BeginUpdateResource"/> function, referencing the file to be
		///  updated.
		/// </param>
		/// <param name="lpType">The type name of the resource.</param>
		/// <param name="lpName">The name of the resource.</param>
		/// <param name="wLanguage">The Language Identifier of the resource to be updated.</param>
		/// <param name="lpData">
		///  The resource data to be inserted into the file indicated by hUpdate. If the resource is one of the
		///  predefined types, the data must be valid and properly aligned.
		/// </param>
		/// <param name="cbData">The size, in bytes, of the resource data at lpData.</param>
		/// <returns>True if successful or false otherwise.</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static bool UpdateResource(
			IntPtr hUpdate,
			IntPtr lpType,
			IntPtr lpName,
			ushort wLanguage,
			byte[] lpData,
			int cbData);

		/*/// <summary>
		///  Adds, deletes, or replaces a resource in a portable executable (PE) file. There are some restrictions on
		///  resource updates in files that contain Resource Configuration (RC Config) data: language-neutral (LN)
		///  files and language-specific resource (.mui) files.
		/// </summary>
		/// <param name="hUpdate">
		///  A module handle returned by the <see cref="BeginUpdateResource"/> function, referencing the file to be
		///  updated.
		/// </param>
		/// <param name="lpType">The type name of the resource.</param>
		/// <param name="lpName">The name of the resource.</param>
		/// <param name="wLanguage">The Language Identifier of the resource to be updated.</param>
		/// <param name="lpData">
		///  The resource data to be inserted into the file indicated by hUpdate. If the resource is one of the
		///  predefined types, the data must be valid and properly aligned.
		/// </param>
		/// <param name="cbData">The size, in bytes, of the resource data at lpData.</param>
		/// <returns>True if successful or false otherwise.</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public extern static bool UpdateResource(
			IntPtr hUpdate,
			string lpType,
			string lpName,
			ushort wLanguage,
			byte[] lpData,
			int cbData);*/

		/// <summary>
		///  Commits or discards changes made prior to a call to <see cref="UpdateResource"/>.
		/// </summary>
		/// <param name="hUpdate">
		///  A module handle returned by the <see cref="BeginUpdateResource"/> function, and used by
		///  <see cref="UpdateResource"/>, referencing the file to be updated.
		/// </param>
		/// <param name="fDiscard">
		///  Indicates whether to write the resource updates to the file. If this parameter is true, no changes are
		///  made. If it is false, the changes are made: the resource updates will take effect.
		/// </param>
		/// <returns>Returns true if the function succeeds; false otherwise.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static bool EndUpdateResource(
			IntPtr hUpdate,
			bool fDiscard);
		
		/// <summary>
		///  Enumerates resource types within a binary module. Starting with Windows Vista, this is typically a
		///  language-neutral Portable Executable (LN file), and the enumeration also includes resources from one of
		///  the corresponding language-specific resource files (.mui files)—if one exists—that contain localizable
		///  language resources. It is also possible to use hModule to specify a .mui file, in which case only that
		///  file is searched for resource types.
		/// </summary>
		/// <param name="hModule">
		///  A handle to a module to be searched. This handle must be obtained through <see cref="LoadLibrary"/> or
		///  <see cref="LoadLibraryEx"/>.<para/>
		///  
		///  If this parameter is null, that is equivalent to passing in a handle to the module used to create the
		///  current process.
		/// </param>
		/// <param name="lpEnumFunc">
		///  A pointer to the callback function to be called for each enumerated resource type. For more information,
		///  see the <see cref="EnumResTypeProc"/> function.
		/// </param>
		/// <param name="lParam">An application-defined value passed to the callback function.</param>
		/// <returns>
		///  Returns true if successful; otherwise, false. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumResourceTypes(
			IntPtr hModule,
			EnumResTypeProc lpEnumFunc,
			IntPtr lParam);
		
		/// <summary>
		///  Enumerates resources of a specified type within a binary module. For Windows Vista and later, this is
		///  typically a language-neutral Portable Executable (LN file), and the enumeration will also include
		///  resources from the corresponding language-specific resource files (.mui files) that contain localizable
		///  language resources. It is also possible for hModule to specify an .mui file, in which case only that file
		///  is searched for resources.
		/// </summary>
		/// <param name="hModule">
		///  A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate
		///  .mui files (if any exist) are included in the search.<para/>
		///  
		///  If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the
		///  current process.
		/// </param>
		/// <param name="lpszType">
		///  The type of the resource for which the name is being enumerated. Alternately, rather than a pointer, this
		///  parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource
		///  type. For a list of predefined resource types, see Resource Types. For more information, see the Remarks
		///  section below.
		/// </param>
		/// <param name="lpEnumFunc">
		///  A pointer to the callback function to be called for each enumerated resource name or ID. For more
		///  information, see <see cref="EnumResNameProc"/>.
		/// </param>
		/// <param name="lParam">
		///  An application-defined value passed to the callback function. This parameter can be used in error
		///  checking.
		/// </param>
		/// <returns>
		///  The return value is TRUE if the function succeeds or FALSE if the function does not find a resource of
		///  the type specified, or if the function fails for another reason. To get extended error information, call
		///  GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumResourceNames(
			IntPtr hModule,
			IntPtr lpszType,
			EnumResNameProc lpEnumFunc,
			IntPtr lParam);

		/// <summary>
		///  Enumerates language-specific resources, of the specified type and name, associated with a binary module.
		/// </summary>
		/// <param name="hModule">
		///  A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate
		///  .mui files (if any exist) are included in the search.<para/>
		///  
		///  If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the
		///  current process.
		/// </param>
		/// <param name="lpszType">
		///  The type of the resource for which the name is being enumerated. Alternately, rather than a pointer, this
		///  parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource
		///  type. For a list of predefined resource types, see Resource Types. For more information, see the Remarks
		///  section below.
		/// </param>
		/// <param name="lpszName">
		///  The name of the resource for which the language is being enumerated. Alternately, rather than a pointer,
		///  this parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the resource. For more
		///  information, see the Remarks section below.
		/// </param>
		/// <param name="lpEnumFunc">
		///  A pointer to the callback function to be called for each enumerated resource language. For more
		///  information, see <see cref="EnumResLangProc "/>.
		/// </param>
		/// <param name="lParam">
		///  An application-defined value passed to the callback function. This parameter can be used in error
		///  checking.
		/// </param>
		/// <returns>
		///  The return value is TRUE if the function succeeds or FALSE if the function does not find a resource of
		///  the type specified, or if the function fails for another reason. To get extended error information, call
		///  GetLastError.
		/// </returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumResourceLanguages(
			IntPtr hModule,
			IntPtr lpszType,
			IntPtr lpszName,
			EnumResLangProc  lpEnumFunc,
			IntPtr lParam);
	}
}
