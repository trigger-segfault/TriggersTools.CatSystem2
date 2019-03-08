using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Exceptions {
	public class HgxException : ExtractException {
		#region Constants

		private const string CorruptMessage = "The HG-X image data may be corrupt!";

		#endregion

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
	}
}
