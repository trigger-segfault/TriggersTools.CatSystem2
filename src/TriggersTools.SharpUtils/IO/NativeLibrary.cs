using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.IO {
	public static class NativeLibrary {

		public static void Load(string path) {
			IntPtr h = LoadLibrary(path);
			if (h == IntPtr.Zero) {
				Exception ex = new Win32Exception();
				throw new DllNotFoundException($"Failed to load library \"{Path.GetFileName(path)}\"!", ex);
			}
		}

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
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private extern static IntPtr LoadLibrary(
			string lpLibFileName);
	}
}
