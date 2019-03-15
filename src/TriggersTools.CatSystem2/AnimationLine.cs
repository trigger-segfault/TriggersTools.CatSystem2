using System;
using System.Collections.Generic;
using System.Linq;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;
using TriggersTools.SharpUtils.Enums;
using System.Collections.Immutable;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  The types of commands an ANM animation script line can have.
	/// </summary>
	public enum AnimationLineType {
		/// <summary>
		///  The command displays an image.<para/>
		///  <Format: code>[ID] [min] (max)</code>
		/// </summary>
		[AnimationCommand(null, 3, isRange: true)]
		Image = 0,
		/// <summary>
		///  The command sets the value of a variable.<para/> 
		///  Format: <code>set [var] [min] (max)</code>
		/// </summary>
		[AnimationCommand("set", 3, isRange: true)]
		Set = 1,
		/// <summary>
		///  The command jumps to the label index and decrements the variable if it is greater than zero.<para/>
		///  Format: <code>loop [var] [label]</code>
		/// </summary>
		[AnimationCommand("loop", 2, isJump: true)]
		Loop = 2,
		/// <summary>
		///  The command jumps to the label index.<para/>
		///  Format: <code>jump [label]</code>
		/// </summary>
		[AnimationCommand("jump", 1, isJump: true)]
		Jump = 3,
		/// <summary>
		///  The command jumps to the label index if the variable is greater than zero.<para/>
		///  Format: <code>if [var] [label]</code>
		/// </summary>
		[AnimationCommand("if", 2, isJump: true)]
		If = 4,
		/// <summary>
		///  The command jumps to the label index if the variable is equal to the value.<para/>
		///  Format: <code>ife [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ife", 3, isJump: true)]
		IfEqual = 5,
		/// <summary>
		///  The command jumps to the label index if the variable is not equal to the value.<para/>
		///  Format: <code>ifn [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifn", 3, isJump: true)]
		IfNotEqual = 6,
		/// <summary>
		///  The command jumps to the label index if the variable is greater than the value the value.<para/>
		///  Format: <code>ifg [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifg", 3, isJump: true)]
		IfGreater = 7,
		/// <summary>
		///  The command jumps to the label index if the variable is less than the value the value.<para/>
		///  Format: <code>ifs [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifs", 3, isJump: true)]
		IfLess = 8,
		/// <summary>
		///  The command jumps to the label index if the variable is greater than or equal to the value the value.
		///  <para/>
		///  Format: <code>ifge [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifge", 3, isJump: true)]
		IfGreaterOrEqual = 9,
		/// <summary>
		///  The command jumps to the label index if the variable is less than or equal to the value the value.<para/>
		///  Format: <code>ifse [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifse", 3, isJump: true)]
		IfLessOrEqual = 10,
		/// <summary>
		///  Unknown functionality.<para/>
		///  Format: <code>max [param1]</code>
		/// </summary>
		[AnimationCommand("max", 1)]
		Max = 11,
		/// <summary>
		///  Unknown functionality.<para/>
		///  Format: <code>blend [param1]</code>
		/// </summary>
		[AnimationCommand("blend", 1)]
		Blend = 12,
		/// <summary>
		///  Unknown functionality. Most likely hides the animation.<para/>
		///  Format: <code>disp [bool]</code>
		/// </summary>
		[AnimationCommand("disp", 1)]
		Display = 13,
		/// <summary>
		///  Unknown functionality. Most likely sets the position of the animation.<para/>
		///  Format: <code>pos [x] [y]</code>
		/// </summary>
		[AnimationCommand("pos", 2)]
		Position = 14,
		/// <summary>
		///  Unknown functionality.<para/>
		///  Format: <code>wait [param1] [param2]</code>
		/// </summary>
		[AnimationCommand("wait", 2)]
		Wait = 15,
		/// <summary>
		///  The command adds the specified value to the specified variable.<para/>
		///  Format: <code>add [var] [value]</code>
		/// </AnimationCommand>
		[AnimationCommand("add", 2)]
		Add = 16,
		/// <summary>
		///  The command subtracts the specified value to the specified variable.<para/>
		///  Format: <code>sub [var] [value]</code>
		/// </summary>
		[AnimationCommand("sub", 2)]
		Subtract = 17,
	}
	public static class AnimationLineTypeExtensions {
		public static bool IsJump(this AnimationLineType type) {
			return EnumInfo.GetAttribute<AnimationLineType, AnimationCommandAttribute>(type).IsJump;
		}
		public static int GetParameterCount(this AnimationLineType type) {
			return EnumInfo.GetAttribute<AnimationLineType, AnimationCommandAttribute>(type).Count;
		}
		public static bool IsRange(this AnimationLineType type) {
			return EnumInfo.GetAttribute<AnimationLineType, AnimationCommandAttribute>(type).IsRange;
		}
		public static string GetCommand(this AnimationLineType type) {
			return EnumInfo.GetAttribute<AnimationLineType, AnimationCommandAttribute>(type).Command;
		}
	}
	/// <summary>
	///  An animation script line used in an ANM file and the <see cref="Animation"/> class.
	/// </summary>
	public sealed class AnimationLine {
		#region Fields
		
		/// <summary>
		///  Gets the type of the animation script line command.
		/// </summary>
		[JsonProperty("type")]
		public AnimationLineType Type { get; private set; }
		/// <summary>
		///  Gets the parameters for the animation script line command.
		/// </summary>
		[JsonProperty("parameters")]
		public IReadOnlyList<AnimationParameter> Parameters { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of parameters in the script line command.
		/// </summary>
		[JsonIgnore]
		public int Count => Parameters.Count;
		/// <summary>
		///  Gets if this command type is a range, and the min and max values are the same.
		/// </summary>
		[JsonIgnore]
		public bool IsDuplicateRange {
			get => (Count > 1 && Type.IsRange() && Parameters[Count - 2] == Parameters[Count - 1]);
		}

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned animation script line for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public AnimationLine() { }
		/// <summary>
		///  Constructs an animation script line with the specified file name and <see cref="ANMLINE"/>.
		/// </summary>
		/// <param name="line">The ANMFRAME struct containing script line information.</param>
		internal AnimationLine(ANMLINE line) {
			Type = (AnimationLineType) line.Type;
			int count = Type.GetParameterCount();
			Parameters = line.Parameters.Take(count)
							  .Select(p => new AnimationParameter(p))
							  .ToImmutableArray();
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the animation script line.
		/// </summary>
		/// <returns>The animation script line's string representation.</returns>
		public override string ToString() {
			string command = Type.GetCommand();
			var paramsWithoutDupeRange = Parameters.Take(IsDuplicateRange ? Count - 1 : Count);
			return $"{(command != null ? $"{command} " : "")}{string.Join(" ", paramsWithoutDupeRange)}";
		}

		#endregion
	}
	/// <summary>
	///  A single parameter in an animation script line.
	/// </summary>
	public struct AnimationParameter : IEquatable<AnimationParameter> {
		#region Fields

		/// <summary>
		///  Gets if the value represents a variable index and not a constant.
		/// </summary>
		[JsonProperty("variable")]
		public bool IsVariable { get; private set; }
		/// <summary>
		///  Gets the value of the parameter. If <see cref="IsVariable"/> is true, this is the index of a variable.
		/// </summary>
		[JsonProperty("value")]
		public int Value { get; private set; }

		#endregion

		#region Constructors
		
		/// <summary>
		///  Constructs an animation parameter with a value that can optionally reference a variable.
		/// </summary>
		/// <param name="value">
		///  The value of the parameter or the variable index if <paramref name="isVariable"/> is true.
		/// </param>
		/// <param name="isVariable">True if the parameter references a variable.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="isVariable"/> is true and <see cref="value"/> is less than 0 or greater than 63.
		/// </exception>
		public AnimationParameter(int value, bool isVariable) {
			IsVariable = isVariable;
			Value = value;
			if (isVariable && value < 0 || value > 63) {
				throw new ArgumentOutOfRangeException($"{nameof(value)} must be between 0 and 63 when " +
					$"{nameof(isVariable)} is true!");
			}
		}
		/// <summary>
		///  Constructs the animation parameter from an <see cref="ANMPARAM"/> struct.
		/// </summary>
		/// <param name="param">The ANMPARAM struct information for the parameter.</param>
		internal AnimationParameter(ANMPARAM param) {
			IsVariable = param.IsVariable;
			Value = param.Value;
		}

		#endregion

		#region Object Overrides

		/// <summary>
		///  Gets the string representation of the ANM animation script line parameter.
		/// </summary>
		/// <returns>The ANM animation script line parameter's string representation.</returns>
		public override string ToString() => (IsVariable ? $"@{Value}" : $"{Value}");
		/// <summary>
		///  Gets the hash code for the script parameter.
		/// </summary>
		/// <returns>The hash code for the script parameter.</returns>
		public override int GetHashCode() => (IsVariable ? unchecked((int) 0x80000000) | Value : Value);
		/// <summary>
		///  Checks if the object is an animation script parameter and is the same as this parameter.
		/// </summary>
		/// <param name="obj">The object to check for equality.</param>
		/// <returns>True if the object is an animation script parameter and is equal to this parameter.</returns>
		public override bool Equals(object obj) {
			if (obj is AnimationParameter param)
				return Value == param.Value && IsVariable == param.IsVariable;
			return false;
		}
		/// <summary>
		///  Checks if the animation script parameter and is the same as this parameter.
		/// </summary>
		/// <param name="obj">The animation script parameter to check for equality.</param>
		/// <returns>True if the animation script parameter and is equal to this parameter.</returns>
		public bool Equals(AnimationParameter other) {
			return this == other;
		}

		#endregion

		#region Operators

		public static bool operator ==(AnimationParameter a, AnimationParameter b) {
			return a.Value == b.Value && a.IsVariable == b.IsVariable;
		}
		public static bool operator !=(AnimationParameter a, AnimationParameter b) {
			return a.Value != b.Value || a.IsVariable != b.IsVariable;
		}

		#endregion
	}
}
