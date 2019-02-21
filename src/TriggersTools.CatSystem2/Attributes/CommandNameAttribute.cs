using System;

namespace TriggersTools.CatSystem2.Attributes {
	/// <summary>
	///  An attribute that specifieds the output command name to use for an enum field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class CommandNameAttribute : Attribute {
		#region Properties

		/// <summary>
		///  Gets the output command name to use for this enum field.
		/// </summary>
		public string Command { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the command name attribute with the specified command.
		/// </summary>
		/// <param name="command">The name of the command. Null if there is no command name.</param>
		public CommandNameAttribute(string command) {
			Command = command;
		}

		#endregion
	}
}
