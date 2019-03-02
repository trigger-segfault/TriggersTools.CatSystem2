
namespace TriggersTools.Resources.Native {
	/// <summary>
	///  The action to be taken when loading the module using <see cref="Kernel32.LoadLibraryEx"/>.
	/// </summary>
	internal enum LoadLibraryExFlags : uint {
		/// <summary>
		///  The behavior of this function is identical to that of the LoadLibrary function.
		/// </summary>
		None = 0,
		/// <summary>
		///  If this value is used, and the executable module is a DLL, the system does not call DllMain for process
		///  and thread initialization and termination. Also, the system does not load additional executable modules
		///  that are referenced by the specified module.<para/>
		///  
		///  Note: Do not use this value; it is provided only for backward compatibility. If you are planning to access
		///  only data or resources in the DLL, use <see cref="LoadLibraryAsDatafileExclusive"/> or
		///  <see cref="LoadLibraryAsImageResource"/> or both. Otherwise, load the library as a DLL or executable module
		///  using the LoadLibrary function.
		/// </summary>
		DontResolveDllReferences = 0x00000001,
		/// <summary>
		///  If this value is used, the system does not check AppLocker rules or apply Software Restriction Policies
		///  for the DLL. This action applies only to the DLL being loaded and not to its dependencies. This value is
		///  recommended for use in setup programs that must run extracted DLLs during installation.
		/// </summary>
		LoadIgnoreCodeAuthzLevel = 0x00000010,
		/// <summary>
		///  If this value is used, the system maps the file into the calling process's virtual address space as if it
		///  were a data file. Nothing is done to execute or prepare to execute the mapped file. Therefore, you cannot
		///  call functions like GetModuleFileName, GetModuleHandle or GetProcAddress with this DLL. Using this value
		///  causes writes to read-only memory to raise an access violation. Use this flag when you want to load a DLL
		///  only to extract messages or resources from it.<para/>
		///  
		///  This value can be used with <see cref="LoadLibraryAsImageResource"/>. For more information, see Remarks.
		/// </summary>
		LoadLibraryAsDatafile = 0x00000002,
		/// <summary>
		///  Similar to <see cref="LoadLibraryAsDatafile"/>, except that the DLL file is opened with exclusive write
		///  access for the calling process. Other processes cannot open the DLL file for write access while it is in
		///  use. However, the DLL can still be opened by other processes.<para/>
		///  
		///  This value can be used with <see cref="LoadLibraryAsImageResource"/>. For more information, see Remarks.
		/// </summary>
		LoadLibraryAsDatafileExclusive = 0x00000040,
		/// <summary>
		///  If this value is used, the system maps the file into the process's virtual address space as an image file.
		///  However, the loader does not load the static imports or perform the other usual initialization steps. Use
		///  this flag when you want to load a DLL only to extract messages or resources from it.<para/>
		///  
		///  Unless the application depends on the file having the in-memory layout of an image, this value should be
		///  used with either <see cref="LoadLibraryAsDatafileExclusive"/> or <see cref="LoadLibraryAsDatafile"/>. For
		///  more information, see the Remarks section.
		/// </summary>
		LoadLibraryAsImageResource = 0x00000020,
		/// <summary>
		///  If this value is used, the application's installation directory is searched for the DLL and its
		///  dependencies. Directories in the standard search path are not searched. This value cannot be combined with
		///  <see cref="LoadWithAlteredSearchPath"/>. 
		/// </summary>
		LoadLibrarySearchApplicationDir = 0x00000200,
		/// <summary>
		///  This value is a combination of <see cref="LoadLibrarySearchApplicationDir"/>,
		///  <see cref="LoadLibrarySearchSystem32"/>, and <see cref="LoadLibrarySearchUserDirs"/>. Directories in the
		///  standard search path are not searched. This value cannot be combined with
		///  <see cref="LoadWithAlteredSearchPath"/>.<para/>
		///  
		///  This value represents the recommended maximum number of directories an application should include in its
		///  DLL search path.
		/// </summary>
		LoadLibrarySearchDefaultDirs = 0x00001000,
		/// <summary>
		///  If this value is used, the directory that contains the DLL is temporarily added to the beginning of the
		///  list of directories that are searched for the DLL's dependencies. Directories in the standard search path
		///  are not searched.<para/>
		/// 
		///  The lpFileName parameter must specify a fully qualified path. This value cannot be combined with
		///  <see cref="LoadWithAlteredSearchPath"/>.<para/>
		///  
		///  For example, if Lib2.dll is a dependency of C:\Dir1\Lib1.dll, loading Lib1.dll with this value causes the
		///  system to search for Lib2.dll only in C:\Dir1. To search for Lib2.dll in C:\Dir1 and all of the
		///  directories in the DLL search path, combine this value with <see cref="LoadLibrarySearchDefaultDirs"/>.
		/// </summary>
		LoadLibrarySearchDllLoadDir = 0x00000100,
		/// <summary>
		///  If this value is used, %windows%\system32 is searched for the DLL and its dependencies. Directories in the
		///  standard search path are not searched. This value cannot be combined with
		///  <see cref="LoadWithAlteredSearchPath"/>.
		/// </summary>
		LoadLibrarySearchSystem32 = 0x00000800,
		/// <summary>
		///  If this value is used, directories added using the AddDllDirectory or the SetDllDirectory function are
		///  searched for the DLL and its dependencies. If more than one directory has been added, the order in which
		///  the directories are searched is unspecified. Directories in the standard search path are not searched.
		///  This value cannot be combined with <see cref="LoadWithAlteredSearchPath"/>.
		/// </summary>
		LoadLibrarySearchUserDirs = 0x00000400,
		/// <summary>
		///  If this value is used and lpFileName specifies an absolute path, the system uses the alternate file search
		///  strategy discussed in the Remarks section to find associated executable modules that the specified module
		///  causes to be loaded. If this value is used and lpFileName specifies a relative path, the behavior is
		///  undefined.<para/>
		///  
		///  If this value is not used, or if lpFileName does not specify a path, the system uses the standard search
		///  strategy discussed in the Remarks section to find associated executable modules that the specified module
		///  causes to be loaded.<para/>
		///  
		///  This value cannot be combined with any LoadLibrarySearch flag.
		/// </summary>
		LoadWithAlteredSearchPath = 0x00000008,
	}
}
