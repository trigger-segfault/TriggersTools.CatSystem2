
using System;

namespace TriggersTools.Resources {
	/// <summary>
	///     A resource load exception.
	/// </summary>
	public class ResourceLoadException : Exception {
		#region Fields

		private Exception outerException;

		#endregion

		/// <summary>
		///     A new resource load exception.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="innerException">The inner exception thrown within a single resource.</param>
		/// <param name="outerException">The outer exception from the Win32 API.</param>
		public ResourceLoadException(string message, Exception innerException, Exception outerException) : base(message, innerException) {
			this.outerException = outerException;
		}

		/// <summary>
		///     A combined message of the inner and outer exception.
		/// </summary>
		public override string Message {
			get => outerException != null ? $"{base.Message} {outerException.Message}!" : base.Message;
		}
	}
}