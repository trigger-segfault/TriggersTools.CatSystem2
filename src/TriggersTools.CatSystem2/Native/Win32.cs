using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Native {
	/// <summary>
	///  A static class for Windows native methods.
	/// </summary>
	internal static class Win32 {
		/// <summary>
		///  The action to be taken when loading the module using see <see cref="LoadLibraryEx"/>.
		/// </summary>
		public enum LoadLibraryExFlags : uint {
			None = 0,
			LoadLibraryAsImageResource = 0x00000020,
		}
		/// <summary>
		///  Language identifiers used in <see cref="UpdateResource"/>.
		/// </summary>
		public enum LanguageIdentifier : ushort {
			/// <summary>Default custom locale language</summary>
			Neutral = 0x0C00,
			/// <summary>English (en)</summary>
			English = 0x0C09,
			/// <summary>Japanese (ja)</summary>
			Japanese = 0x0411,
		}

		/// <summary>
		///  Loads the specified module into the address space of the calling process. The specified module may cause
		///  other modules to be loaded.
		/// </summary>
		/// <param name="lpLibFileName">A string that specifies the file name of the module to load.</param>
		/// <param name="hFile">This parameter is reserved for future use. It must be null.</param>
		/// <param name="dwFlags">The action to be taken when loading the module.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LoadLibraryEx(
			string lpLibFileName,
			IntPtr hFile,
			[MarshalAs(UnmanagedType.U4)] LoadLibraryExFlags dwFlags);

		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static bool FreeLibrary(
			IntPtr hLibModule);

		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr FindResource(
			IntPtr hModule,
			string lpName,
			string lpType);

		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LoadResource(
			IntPtr hModule,
			IntPtr hResInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static int SizeofResource(
			IntPtr hModule,
			IntPtr hResInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static IntPtr LockResource(
			IntPtr hGlobal);

		/// <summary>
		///  Retrieves a handle that can be used by the UpdateResource function to add, delete, or replace resources in
		///  a binary module.
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
		/// <param name="wLanguage">The <see cref="LanguageIdentifier"/> of the resource to be updated.</param>
		/// <param name="lpData">
		///  The resource data to be inserted into the file indicated by hUpdate. If the resource is one of the
		///  predefined types, the data must be valid and properly aligned.
		/// </param>
		/// <param name="cbData">The size, in bytes, of the resource data at lpData.</param>
		/// <returns>True if successful or false otherwise.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public extern static bool UpdateResource(
			IntPtr hUpdate,
			string lpType,
			string lpName,
			[MarshalAs(UnmanagedType.U2)] LanguageIdentifier wLanguage,
			byte[] lpData,
			int cbData);

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
	}
}
