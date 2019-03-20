using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  Debug configuration settings for the CatSystem2 library.
	///  All default configuration settings besides SpeedTests are set for fastest processing.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	static class CatDebug {
		#region Processing

		/// <summary>
		///  Gets or sets if HG-X images are processed natively or in managed code.
		///  x86: ~43% slower, x64: ~15% slower.<para/>
		///  Also take into account that processing images is only a fraction of the time spent exporting HG-X files.
		/// </summary>
		public static bool NativeHgx { get; set; } = true;
		/// <summary>
		///  Gets or sets if KIFINT archives are decrypted using blowfish natively or in managed code.
		/// </summary>
		public static bool NativeBlowfish { get; set; } = true;
		/// <summary>
		///  Gets or sets if KIFINT entry extraction and decryption is streamed or done via ahead of time.
		///  Stream extract is a little bit faster by ~1-2% and saves some memory, but it's not a huge leap.<para/>
		///  Stream extract works best with very large files.
		/// </summary>
		public static bool StreamExtract { get; set; } = true;
		/// <summary>
		///  Gets or sets if managed blowfish encipher/decipher is done with the inlined ROUND function, or directly in-code.
		///  ROUND is slower by a little bit.
		/// </summary>
		public static bool ManagedBlowfishRound { get; set; } = false;

		#endregion

		#region Speed Testing

		/// <summary>
		///  Gets or sets if processing HG-X images are speed tested. This disables saving of images and times a
		///  stopwatch during image processing.
		/// </summary>
		public static bool SpeedTestHgx { get; set; } = false;
		/// <summary>
		///  Gets the watch used when <see cref="SpeedTestHgx"/> is true.
		/// </summary>
		public static Stopwatch HgxWatch { get; } = new Stopwatch();

		#endregion
	}
}
