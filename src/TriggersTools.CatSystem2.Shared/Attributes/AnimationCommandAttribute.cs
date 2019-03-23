using System;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.CatSystem2.Attributes {
	/// <summary>
	///  An attribute that specified information about an <see cref="AnmLineType"/> command.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class AnimationCommandAttribute : CommandNameAttribute {
		#region Properties
		
		/// <summary>
		///  Gets the maximum number of parameters for the command.
		/// </summary>
		public int Count { get; }
		/// <summary>
		///  Gets if the command's last parameter is optional if it is the same as the previous.
		/// </summary>
		public bool IsRange { get; }
		/// <summary>
		///  Gets if the command is a jump, making the last parameter a potential label.
		/// </summary>
		public bool IsJump { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the animation command attribute with the specified command, parameter length, and other
		///  settings.
		/// </summary>
		/// <param name="command">The name of the command. Null if there is no command name.</param>
		/// <param name="count">The maximum number of parameters in the command.</param>
		/// <param name="isRange">The command's last parameter is optional if it is the same as the previous.</param>
		/// <param name="isJump">The command is a jump, making the last parameter a potential label.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="count"/> is less than zero.
		/// </exception>
		public AnimationCommandAttribute(string command, int count, bool isRange = false, bool isJump = false)
			: base(command)
		{
			if (count < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(count), count, 0, true);
			Count = count;
			IsRange = isRange;
			IsJump = isJump;
		}

		#endregion
	}
}
