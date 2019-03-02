using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TriggersTools.CatSystem2 {
	partial class VCodes {
		#region FindVCode

		/// <summary>
		///  Locates the V_CODE in the module file, which is used to decrypt the KIFINT archive's entry file names.
		/// </summary>
		/// <param name="exeFile">The file path to the executable or bin file.</param>
		/// <returns>The decrypted V_CODE string resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="exeFile"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="exeFile"/> is an empty string or whitespace.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  An error occurred during loading of the V_CODE resources.
		/// </exception>
		public static string FindVCode(string exeFile) {
			return Load(exeFile).VCode;
		}

		public static bool TryFindVCode(string exeFile, out string vcode) {
			try {
				vcode = Load(exeFile).VCode;
				return true;
			} catch {
				vcode = default;
				return false;
			}
		}

		#endregion

		#region FindVCode2

		/// <summary>
		///  Locates the V_CODE2 in the module file, which is used to decrypt the KIFINT archive's entry file names.
		/// </summary>
		/// <param name="exeFile">The file path to the executable or bin file.</param>
		/// <returns>The decrypted V_CODE2 string resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="exeFile"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="exeFile"/> is an empty string or whitespace.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  An error occurred during loading of the V_CODE resources.
		/// </exception>
		public static string FindVCode2(string exeFile) {
			return Load(exeFile).VCode2;
		}

		public static bool TryFindVCode2(string exeFile, out string vcode2) {
			try {
				vcode2 = Load(exeFile).VCode2;
				return true;
			} catch {
				vcode2 = default;
				return false;
			}
		}

		#endregion

		#region FindVCodes

		/// <summary>
		///  Locates the V_CODE2 in the module file, which is used to decrypt the KIFINT archive's entry file names.
		/// </summary>
		/// <param name="exeFile">The file path to the executable or bin file.</param>
		/// <returns>The decrypted V_CODE2 string resource.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="exeFile"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="exeFile"/> is an empty string or whitespace.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  An error occurred during loading of the V_CODE resources.
		/// </exception>
		public static void FindVCodes(string exeFile, out string vcode, out string vcode2, out byte[] key) {
			VCodes vcodes = VCodes.Load(exeFile);
			vcode = vcodes.VCode;
			vcode2 = vcodes.VCode2;
			key = vcodes.KeyCode;
		}

		public static bool TryFindVCodes(string exeFile, out string vcode, out string vcode2, out byte[] key) {
			try {
				VCodes vcodes = VCodes.Load(exeFile);
				vcode = vcodes.VCode;
				vcode2 = vcodes.VCode2;
				key = vcodes.KeyCode;
				return true;
			} catch {
				vcode = default;
				vcode2 = default;
				key = null;
				return false;
			}
		}
		
		#endregion
	}
}
