using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	/// <summary>
	///  An exception thrown during failure to extract an <see cref="HgxImage"/>.
	/// </summary>
	public class HgxException : ExtractException {
		#region Constants

		/// <summary>
		///  The message displayed after the base message, warning users of possible corrupt data.
		/// </summary>
		private const string CorruptMessage = "The HG-X image data may be corrupt, or there could be issues with the reader.";

		#endregion

		#region Constructors

		public HgxException() : base(CorruptMessage) { }
		public HgxException(string message) : base($"{message} {CorruptMessage}") { }
		public HgxException(string message, Exception innerException)
			: base($"{message} {CorruptMessage}", innerException)
		{
		}
		public HgxException(Exception innerException)
			: base($"{innerException.Message} {CorruptMessage}", innerException)
		{
		}

		#endregion
	}
}
