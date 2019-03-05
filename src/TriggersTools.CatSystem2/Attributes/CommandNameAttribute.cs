using System;
using System.Linq;
using TriggersTools.SharpUtils.Enums;

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

		#region ParseCommand

		public static TEnum Parse<TEnum>(string s) where TEnum : Enum {
			try {
				return EnumInfo.Get<TEnum>().Fields
											.Where(f => f.GetAttribute<CommandNameAttribute>()?.Command == s)
											.First().Value;
			} catch {
				throw new ArgumentException($"No command with name \"{s}\" found!");
			}
		}

		#endregion
	}
}
