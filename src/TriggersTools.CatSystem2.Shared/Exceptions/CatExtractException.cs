using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	/// <summary>
	///  An exception that occurred during CatSystem2 file and or resource extraction.
	/// </summary>
	public class CatExtractException : Exception {

		internal CatExtractException() { }
		internal CatExtractException(string message) : base(message) { }
		internal CatExtractException(string message, Exception innerException) : base(message, innerException) { }
		internal CatExtractException(Exception innerException) : base(innerException.Message, innerException) { }

	}
}
