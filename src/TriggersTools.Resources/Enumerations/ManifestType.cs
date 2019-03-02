
namespace TriggersTools.Resources.Enumerations {
	/// <summary>
	///  Within the <see cref="ResourceTypes.Manifest"/> resource, Windows reserves ID 1-16. A binary cannot have
	///  two IDs of resource type RT_MANIFEST within 1-16. Windows will refuse to load such binary in Windows
	///  XP/Windows Server 2003.<para/>
	///  
	///  Only three IDs are used today in Windows.
	/// </summary>
	public enum ManifestType : ushort {
		/// <summary>
		///  <see cref="CreateProcess"/> is used primarily for EXEs. If an executable has a resource of type
		///  <see cref="ResourceTypes.Manifest"/>, ID <see cref="CreateProcess"/>, Windows will create a process
		///  default activation context for the process. The process default activation context will be used by all
		///  components running in the process.<para/>
		///  
		///  <see cref="CreateProcess"/> can also used by DLLs. When Windows probe for dependencies, if the dll has a
		///  resource of type <see cref="ResourceTypes.Manifest"/>, ID <see cref="CreateProcess"/>, Windows will use
		///  that manifest as the dependency. 
		/// </summary>
		CreateProcess = 1,
		/// <summary>
		///  <see cref="IsolationAware"/> is used primarily for DLLs. It should be used if the dll wants private
		///  dependencies other than the process default. For example, if an dll depends on comctl32.dll version
		///  6.0.0.0. It should have a resource of type <see cref="ResourceTypes.Manifest"/>, ID
		///  <see cref="IsolationAware"/> to depend on comctl32.dll version 6.0.0.0, so that even if the process
		///  executable wants comctl32.dll version 5.1, the dll itself will still use the right version of
		///  comctl32.dll.<para/>
		///  
		///  When LoadLibrary is called, before loading the dependencies of the dll, the NT library loader checks to
		///  see if the dll has a resource of type <see cref="ResourceTypes.Manifest"/>, ID
		///  <see cref="IsolationAware"/>. If it does, the loader calls CreateActCtx with the resource, and use the
		///  generated activation context to probe the dll's static dependencies. This is reason why the dll can have
		///  private dependencies with the <see cref="IsolationAware"/> resource.<para/>
		///  
		///  The activation context created during LoadLibrary is stored in the loader data structure tracking the dll.
		///  <para/>
		///  
		///  Normally this activation context is used only during <see cref="Kernel32.LoadLibrary"/>, for the dll's
		///  static dependencies.<para/>
		///  
		///  Sometimes, you want to use the activation context outside of probing the dll's static dependencies. You
		///  can define macro ISOLATION_AWARE_ENABLED when you compile the module. 
		/// </summary>
		IsolationAware = 2,
		/// <summary>
		///  When IsolationAwareLoadLibraryExW is first called, it tries to retrieve the activation context stored in
		///  the loader data structure when the calling library is loaded. If the library does not have such activation
		///  context (for example, the library does not have a manifest of id <see cref="IsolationAware"/>),
		///  IsolationAwareLoadLibraryExW tries to create a new activation context with the calling library with id
		///  <see cref="IsolationAwareNonStaticImport"/> MAKEINTRESOURCE. If the creation fails,
		///  IsolationAwareLoadLibraryExW will use the process default activation context.
		/// </summary>
		IsolationAwareNonStaticImport = 3,
    }
}