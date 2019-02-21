using System;
using System.Collections.Generic;
using System.Text;
using OutOfRange = System.ArgumentOutOfRangeException;

namespace TriggersTools.SharpUtils.Exceptions {
	partial class ArgumentOutOfRangeUtils {
		#region OutsideRange (without bounds)

		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value) {
			return new OutOfRange(name, value, $"{Param(name, value)} is out of range!");
		}

		#endregion

		#region OutsideRange (without value)

		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, object min, object max, bool minInclusive = true,
			bool maxInclusive = false)
		{
			return OutsideRange(name, null, null, min, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, string minName, object max, bool minInclusive = true,
			bool maxInclusive = false)
		{
			return OutsideRange(name, null, minName, null, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, object min, string maxName, bool minInclusive = true,
			bool maxInclusive = false)
		{
			return OutsideRange(name, null, null, min, maxName, null, minInclusive, maxInclusive);
		}
		
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, string minName, object min, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, null, minName, min, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, object min, string maxName, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, null, null, min, maxName, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRangeNoValue(string name, string minName, object min, string maxName, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, null, minName, min, maxName, max, minInclusive, maxInclusive);
		}

		#endregion

		#region OutsideRange (with value)

		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, object min, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, null, min, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, string minName, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, minName, null, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, object min, string maxName,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, null, min, maxName, null, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, string minName, string maxName,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, minName, null, maxName, null, minInclusive, maxInclusive);
		}

		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, string minName, object min, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, minName, min, null, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, object min, string maxName, object max,
			bool minInclusive = true, bool maxInclusive = false)
		{
			return OutsideRange(name, value, null, min, maxName, max, minInclusive, maxInclusive);
		}
		/// <summary>
		///  Constructs an <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="minInclusive">True if the lower bounds is inside the range.</param>
		/// <param name="maxInclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideRange(string name, object value, string minName, object min, string maxName,
			object max, bool minInclusive = true, bool maxInclusive = false)
		{
			if (value != null) {
				if (min is IComparable minComparable && minComparable.CompareTo(value) < 0 + (!minInclusive ? 1 : 0)) {
					return OutsideMin(name, value, minName, min, minInclusive);
				}
				if (max is IComparable maxComparable && maxComparable.CompareTo(value) > 0 - (!maxInclusive ? 1 : 0)) {
					return OutsideMax(name, value, maxName, max, maxInclusive);
				}
			}
			return new OutOfRange(name, value, $"{Param(name, value)} is " +
				$"{Min(minName, min, minInclusive)} or {Max(maxName, max, maxInclusive)}!");
		}

		#endregion
	}
}
