using System;
using System.Collections.Generic;
using System.Text;
using OutOfRange = System.ArgumentOutOfRangeException;

namespace TriggersTools.SharpUtils.Exceptions {
	/// <summary>
	///  Quick access functions for writing detailed <see cref="OutOfRange"/> messages.
	/// </summary>
	public static partial class ArgumentOutOfRangeUtils {
		#region OutsideMin

		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMinNoValue(string name) {
			return OutsideMin(name, null, null, null, true);
		}
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMinNoValue(string name, string minName, bool inclusive = true) {
			return OutsideMin(name, null, minName, null, inclusive);
		}

		#endregion

		#region OutsideMax

		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMaxNoValue(string name) {
			return OutsideMax(name, null, null, null, true);
		}
		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMaxNoValue(string name, string maxName, bool inclusive = false) {
			return OutsideMax(name, null, maxName, null, inclusive);
		}

		#endregion

		#region OutsideRange

		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name) {
			return new OutOfRange(name, $"{name} is out of range!");
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, string minName, string maxName, bool minInclusive = true,
			bool maxInclusive = false)
		{
			return OutsideRange(name, null, minName, null, maxName, null, minInclusive, maxInclusive);
		}

		#endregion

		#region Private

		/// <summary>
		///  Describes the lower bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the lower bounds.</param>
		/// <param name="min">The optional lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The description of the lower bounds.</returns>
		private static string Min(string name, object min, bool inclusive) {
			string lessThan = "less than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (min != null)
					return $"{lessThan} {name} ({min})";
				else
					return $"{lessThan} {name}";
			}
			else {
				if (min != null)
					return $"{lessThan} {min}";
				else
					return $"{lessThan} lower bounds";
			}
		}
		/*/// <summary>
		///  Describes the lower bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the lower bounds.</param>
		/// <param name="min">The optional lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The description of the lower bounds.</returns>
		private static string Min(string name, long? min, bool inclusive) {
			string lessThan = "less than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (min.HasValue)
					return $"{lessThan} {name} ({min.Value})";
				else
					return $"{lessThan} {name}";
			}
			else {
				if (min.HasValue)
					return $"{lessThan} {min.Value}";
				else
					return $"{lessThan} lower bounds";
			}
		}
		/// <summary>
		///  Describes the lower bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the lower bounds.</param>
		/// <param name="min">The optional lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The description of the lower bounds.</returns>
		private static string Min(string name, ulong? min, bool inclusive) {
			string lessThan = "less than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (min.HasValue)
					return $"{lessThan} {name} ({min.Value})";
				else
					return $"{lessThan} {name}";
			}
			else {
				if (min.HasValue)
					return $"{lessThan} {min.Value}";
				else
					return $"{lessThan} lower bounds";
			}
		}
		/// <summary>
		///  Describes the lower bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the lower bounds.</param>
		/// <param name="min">The optional lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The description of the lower bounds.</returns>
		private static string Min<T>(string name, T? min, bool inclusive) where T : struct {
			string lessThan = "less than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (min.HasValue)
					return $"{lessThan} {name} ({min.Value})";
				else
					return $"{lessThan} {name}";
			}
			else {
				if (min.HasValue)
					return $"{lessThan} {min.Value}";
				else
					return $"{lessThan} lower bounds";
			}
		}*/

		/// <summary>
		///  Describes the upper bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the upper bounds.</param>
		/// <param name="max">The optional lower upper.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The description of the upper bounds.</returns>
		private static string Max(string name, object max, bool inclusive) {
			string greaterThan = "greater than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (max != null)
					return $"{greaterThan} {name} ({max})";
				else
					return $"{greaterThan} {name}";
			}
			else {
				if (max != null)
					return $"{greaterThan} {max}";
				else
					return $"{greaterThan} upper bounds";
			}
		}
		/*/// <summary>
		///  Describes the upper bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the upper bounds.</param>
		/// <param name="max">The optional lower upper.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The description of the upper bounds.</returns>
		private static string Max(string name, long? max, bool inclusive) {
			string greaterThan = "greater than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (max.HasValue)
					return $"{greaterThan} {name} ({max.Value})";
				else
					return $"{greaterThan} {name}";
			}
			else {
				if (max.HasValue)
					return $"{greaterThan} {max.Value}";
				else
					return $"{greaterThan} upper bounds";
			}
		}
		/// <summary>
		///  Describes the upper bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the upper bounds.</param>
		/// <param name="max">The optional lower upper.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The description of the upper bounds.</returns>
		private static string Max(string name, ulong? max, bool inclusive) {
			string greaterThan = "greater than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (max.HasValue)
					return $"{greaterThan} {name} ({max.Value})";
				else
					return $"{greaterThan} {name}";
			}
			else {
				if (max.HasValue)
					return $"{greaterThan} {max.Value}";
				else
					return $"{greaterThan} upper bounds";
			}
		}
		/// <summary>
		///  Describes the upper bounds based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the upper bounds.</param>
		/// <param name="max">The optional lower upper.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The description of the upper bounds.</returns>
		private static string Max<T>(string name, T? max, bool inclusive) where T : struct {
			string greaterThan = "greater than" + (!inclusive ? " or equal to" : "");
			if (name != null) {
				if (max.HasValue)
					return $"{greaterThan} {name} ({max.Value})";
				else
					return $"{greaterThan} {name}";
			}
			else {
				if (max.HasValue)
					return $"{greaterThan} {max.Value}";
				else
					return $"{greaterThan} upper bounds";
			}
		}*/

		/// <summary>
		///  Describes a parameter based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the parameter.</param>
		/// <param name="value">The optional value of the parameter.</param>
		/// <returns>The description of the parameter.</returns>
		private static string Param(string name, object value) {
			if (name != null) {
				if (value != null)
					return $"{name} ({value})";
				else
					return name;
			}
			else {
				if (value != null)
					return value.ToString();
				else
					return "value";
			}
		}
		/*/// <summary>
		///  Describes a parameter based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the parameter.</param>
		/// <param name="value">The optional value of the parameter.</param>
		/// <returns>The description of the parameter.</returns>
		private static string Param(string name, long? value) {
			if (name != null) {
				if (value.HasValue)
					return $"{name} ({value.Value})";
				else
					return name;
			}
			else {
				if (value.HasValue)
					return value.Value.ToString();
				else
					return "value";
			}
		}
		/// <summary>
		///  Describes a parameter based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the parameter.</param>
		/// <param name="value">The optional value of the parameter.</param>
		/// <returns>The description of the parameter.</returns>
		private static string Param(string name, ulong? value) {
			if (name != null) {
				if (value.HasValue)
					return $"{name} ({value.Value})";
				else
					return name;
			}
			else {
				if (value.HasValue)
					return value.Value.ToString();
				else
					return "value";
			}
		}
		/// <summary>
		///  Describes a parameter based on the parameters.
		/// </summary>
		/// <param name="name">The optional name of the parameter.</param>
		/// <param name="value">The optional value of the parameter.</param>
		/// <returns>The description of the parameter.</returns>
		private static string Param<T>(string name, T? value) where T : struct {
			if (name != null) {
				if (value.HasValue)
					return $"{name} ({value.Value})";
				else
					return name;
			}
			else {
				if (value.HasValue)
					return value.Value.ToString();
				else
					return "value";
			}
		}*/

		#endregion
	}
}
