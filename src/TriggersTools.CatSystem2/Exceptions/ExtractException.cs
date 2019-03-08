using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	public class ExtractException : Exception {

		public ExtractException() { }
		public ExtractException(string message) : base(message) { }
		public ExtractException(string message, Exception innerException) : base(message, innerException) { }
		public ExtractException(Exception innerException) : base(innerException.Message, innerException) { }

	}
}
