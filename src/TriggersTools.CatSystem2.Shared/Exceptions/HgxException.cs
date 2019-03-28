using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	/// <summary>
	///  An exception thrown during failure to extract an <see cref="HgxImage"/>.
	/// </summary>
	public class HgxException : CatExtractException {
		#region Constants

		/// <summary>
		///  The message displayed after the base message, warning users of possible corrupt data.
		/// </summary>
		private const string CorruptMessage = "The HG-X image data may be corrupt, or there could be issues with the reader.";

		#endregion

		#region Constructors

		internal HgxException() : base(CorruptMessage) { }
		internal HgxException(string message) : base($"{message} {CorruptMessage}") { }
		internal HgxException(string message, Exception innerException)
			: base($"{message} {CorruptMessage}", innerException)
		{
		}
		internal HgxException(Exception innerException)
			: base($"{innerException.Message} {CorruptMessage}", innerException)
		{
		}

		#endregion
	}
}
