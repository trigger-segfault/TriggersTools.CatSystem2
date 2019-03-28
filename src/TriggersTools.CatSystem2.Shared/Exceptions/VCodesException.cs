using System;
using System.Collections.Generic;
using System.Text;

namespace TriggersTools.CatSystem2.Exceptions {
	/// <summary>
	///  An exception thrown during failure to extract a <see cref="VCodes"/>.
	/// </summary>
	public class VCodesException : CatExtractException {
		#region Constants

		/// <summary>
		///  The message displayed after the base message, warning users of failure to extract.
		/// </summary>
		private const string FailedMessage = "Failed to extract V_CODEs.";

		#endregion

		#region Constructors

		internal VCodesException() : base(FailedMessage) { }
		internal VCodesException(string message) : base($"{message} {FailedMessage}") { }
		internal VCodesException(string message, Exception innerException)
			: base($"{FailedMessage} {message}", innerException)
		{
		}
		internal VCodesException(Exception innerException)
			: base($"{FailedMessage} {innerException.Message}", innerException)
		{
		}

		#endregion
	}
}
