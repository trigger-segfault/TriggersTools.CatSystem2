using System;
using System.Collections.Generic;
using System.Linq;
using TriggersTools.CatSystem2.Attributes;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;
using TriggersTools.SharpUtils.Enums;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  The types of commands an ANM animatino frame can have.
	/// </summary>
	public enum AnimationFrameType {
		/// <summary>
		///  The frame displays an image.<para/>
		///  <Format: code>[ID] [min] (max)</code>
		/// </summary>
		[AnimationCommand(null, 3, isRange: true)]
		Image = 0,
		/// <summary>
		///  The frame sets the value of a variable.<para/> 
		///  Format: <code>set [var] [min] (max)</code>
		/// </summary>
		[AnimationCommand("set", 3, isRange: true)]
		Set = 1,
		/// <summary>
		///  The frame jumps to the label index and decrements the variable if it is greater than zero.<para/>
		///  Format: <code>loop [var] [label]</code>
		/// </summary>
		[AnimationCommand("loop", 2, isJump: true)]
		Loop = 2,
		/// <summary>
		///  The frame jumps to the label index.<para/>
		///  Format: <code>jump [label]</code>
		/// </summary>
		[AnimationCommand("jump", 1, isJump: true)]
		Jump = 3,
		/// <summary>
		///  The frame jumps to the label index if the variable is greater than zero.<para/>
		///  Format: <code>if [var] [label]</code>
		/// </summary>
		[AnimationCommand("if", 2, isJump: true)]
		If = 4,
		/// <summary>
		///  The frame jumps to the label index if the variable is equal to the value.<para/>
		///  Format: <code>ife [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ife", 3, isJump: true)]
		IfEqual = 5,
		/// <summary>
		///  The frame jumps to the label index if the variable is not equal to the value.<para/>
		///  Format: <code>ifn [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifn", 3, isJump: true)]
		IfNotEqual = 6,
		/// <summary>
		///  The frame jumps to the label index if the variable is greater than the value the value.<para/>
		///  Format: <code>ifg [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifg", 3, isJump: true)]
		IfGreater = 7,
		/// <summary>
		///  The frame jumps to the label index if the variable is less than the value the value.<para/>
		///  Format: <code>ifs [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifs", 3, isJump: true)]
		IfLess = 8,
		/// <summary>
		///  The frame jumps to the label index if the variable is greater than or equal to the value the value.<para/>
		///  Format: <code>ifge [var] [value] [label]</code>
		/// </summary>
		[AnimationCommand("ifge", 3, isJump: true)]
		IfGreaterOrEqual = 9,
		/// <summary>
		///  The frame jumps to the label index if the variable is less than or equal to the value the value.<para/>
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
		///  The frame adds the specified value to the specified variable.<para/>
		///  Format: <code>add [var] [value]</code>
		/// </AnimationCommand>
		[AnimationCommand("add", 2)]
		Add = 16,
		/// <summary>
		///  The frame subtracts the specified value to the specified variable.<para/>
		///  Format: <code>sub [var] [value]</code>
		/// </summary>
		[AnimationCommand("sub", 2)]
		Subtract = 17,
	}
	public static class AnimationFrameTypeExtensions {
		public static bool IsJump(this AnimationFrameType type) {
			return EnumInfo.GetAttribute<AnimationFrameType, AnimationCommandAttribute>(type).IsJump;
			/*switch (type) {
			case AnimationFrameType.Image:
			case AnimationFrameType.Set:
			case AnimationFrameType.Max:
			case AnimationFrameType.Blend:
			case AnimationFrameType.Display:
			case AnimationFrameType.Position:
			case AnimationFrameType.Wait:
			case AnimationFrameType.Add:
			case AnimationFrameType.Subtract:
				return false;
			case AnimationFrameType.Loop:
			case AnimationFrameType.Jump:
			case AnimationFrameType.If:
			case AnimationFrameType.IfEqual:
			case AnimationFrameType.IfNotEqual:
			case AnimationFrameType.IfGreater:
			case AnimationFrameType.IfLess:
			case AnimationFrameType.IfGreaterOrEqual:
			case AnimationFrameType.IfLessOrEqual:
				return true;
			default:
				throw new ArgumentException($"Invalid {nameof(AnimationFrameType)} {type}!");
			}*/
		}
		public static int GetParameterCount(this AnimationFrameType type) {
			return EnumInfo.GetAttribute<AnimationFrameType, AnimationCommandAttribute>(type).Count;
			/*switch (type) {
			case AnimationFrameType.Image:
			case AnimationFrameType.Set:
			case AnimationFrameType.IfEqual:
			case AnimationFrameType.IfNotEqual:
			case AnimationFrameType.IfGreater:
			case AnimationFrameType.IfLess:
			case AnimationFrameType.IfGreaterOrEqual:
			case AnimationFrameType.IfLessOrEqual:
				return 3;
			case AnimationFrameType.Loop:
			case AnimationFrameType.If:
			case AnimationFrameType.Position:
			case AnimationFrameType.Wait:
			case AnimationFrameType.Add:
			case AnimationFrameType.Subtract:
				return 2;
			case AnimationFrameType.Jump:
			case AnimationFrameType.Max:
			case AnimationFrameType.Blend:
			case AnimationFrameType.Display:
				return 1;
			default:
				throw new ArgumentException($"Invalid {nameof(AnimationFrameType)} {type}!");
			}*/
		}
		public static bool IsRange(this AnimationFrameType type) {
			return EnumInfo.GetAttribute<AnimationFrameType, AnimationCommandAttribute>(type).IsRange;
			/*switch (type) {
			case AnimationFrameType.Image:
			case AnimationFrameType.Set:
				return true;
			case AnimationFrameType.Loop:
			case AnimationFrameType.Jump:
			case AnimationFrameType.If:
			case AnimationFrameType.IfEqual:
			case AnimationFrameType.IfNotEqual:
			case AnimationFrameType.IfGreater:
			case AnimationFrameType.IfLess:
			case AnimationFrameType.IfGreaterOrEqual:
			case AnimationFrameType.IfLessOrEqual:
			case AnimationFrameType.Max:
			case AnimationFrameType.Blend:
			case AnimationFrameType.Display:
			case AnimationFrameType.Position:
			case AnimationFrameType.Wait:
			case AnimationFrameType.Add:
			case AnimationFrameType.Subtract:
				return false;
			default:
				throw new ArgumentException($"Invalid {nameof(AnimationFrameType)} {type}!");
			}*/
		}
		public static string GetCommand(this AnimationFrameType type) {
			return EnumInfo.GetAttribute<AnimationFrameType, AnimationCommandAttribute>(type).Command;
		}
	}
	/// <summary>
	///  An animation frame used in an .anm file and the <see cref="Animation"/> class.
	/// </summary>
	public sealed class AnimationFrame {
		#region Fields
		
		/// <summary>
		///  Gets the type of the animation frame command.
		/// </summary>
		[JsonProperty("type")]
		public AnimationFrameType Type { get; private set; }
		/// <summary>
		///  Gets the parameters for the animation frame command.
		/// </summary>
		[JsonProperty("parameters")]
		public IReadOnlyList<AnimationParameter> Parameters { get; private set; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets the number of parameters in the frame.
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
		///  Constructs an unassigned animation frame for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public AnimationFrame() { }
		/// <summary>
		///  Constructs an animation frame with the specified file name and <see cref="ANMFRAME"/>.
		/// </summary>
		/// <param name="frame">The ANMFRAME struct containing frame information.</param>
		internal AnimationFrame(ANMFRAME frame) {
			Type = (AnimationFrameType) frame.Type;
			int count = Type.GetParameterCount();
			AnimationParameter[] parameters = frame.Parameters.Take(count)
															  .Select(p => new AnimationParameter(p))
															  .ToArray();
			Parameters = Array.AsReadOnly(parameters);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the animation frame.
		/// </summary>
		/// <returns>The animation frame's string representation.</returns>
		public override string ToString() {
			string command = Type.GetCommand();
			var paramsWithoutDupeRange = Parameters.Take(IsDuplicateRange ? Count - 1 : Count);
			return $"{(command != null ? $"{command} " : "")}{string.Join(" ", paramsWithoutDupeRange)}";
		}

		#endregion
	}
	public struct AnimationParameter {
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
		///  Constructs the animation parameter from an <see cref="ANMPARAM"/> struct.
		/// </summary>
		/// <param name="param">The ANMPARAM struct information for the parameter.</param>
		internal AnimationParameter(ANMPARAM param) {
			IsVariable = param.IsVariable;
			Value = param.Value;
		}

		#endregion

		#region Object Overrides

		public override string ToString() => (IsVariable ? $"@{Value}" : $"{Value}");

		public override int GetHashCode() => (IsVariable ? unchecked((int) 0x80000000) | Value : Value);

		public override bool Equals(object obj) {
			if (obj is AnimationParameter param)
				return Value == param.Value && IsVariable == param.IsVariable;
			return false;
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
