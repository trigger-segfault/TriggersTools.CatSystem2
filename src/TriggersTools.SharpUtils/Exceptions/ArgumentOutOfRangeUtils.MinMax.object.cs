using System;
using System.Collections.Generic;
using System.Text;
using OutOfRange = System.ArgumentOutOfRangeException;

namespace TriggersTools.SharpUtils.Exceptions {
	partial class ArgumentOutOfRangeUtils {
		#region OutsideMin

		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMin(string name, object value) {
			return OutsideMin(name, value, null, null, true);
		}
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMinNoValue(string name, object min, bool inclusive = true) {
			return OutsideMin(name, null, null, min, inclusive);
		}
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMinNoValue(string name, string minName, object min, bool inclusive = true) {
			return OutsideMin(name, null, minName, min, inclusive);
		}
		
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMin(string name, object value, object min, bool inclusive = true) {
			return OutsideMin(name, value, null, min, inclusive);
		}
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMin(string name, object value, string minName, bool inclusive = true) {
			return OutsideMin(name, value, minName, null, inclusive);
		}
		/// <summary>
		///  Constructs a lower bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="minName">The name of the lower bounds.</param>
		/// <param name="min">The lower bounds.</param>
		/// <param name="inclusive">True if the lower bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMin(string name, object value, string minName, object min,
			bool inclusive = true)
		{
			return new OutOfRange(name, value, $"{Param(name, value)} is {Min(minName, min, inclusive)}!");
		}

		#endregion

		#region OutsideMax

		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMax(string name, object value) {
			return OutsideMax(name, value, null, null, true);
		}
		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMaxNoValue(string name, object max, bool inclusive = false) {
			return OutsideMax(name, null, null, max, inclusive);
		}
		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMaxNoValue(string name, string maxName, object max, bool inclusive = false) {
			return OutsideMax(name, null, maxName, max, inclusive);
		}

		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMax(string name, object value, object max, bool inclusive = false) {
			return OutsideMax(name, value, null, max, inclusive);
		}
		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMax(string name, object value, string maxName, bool inclusive = false) {
			return OutsideMax(name, value, maxName, null, inclusive);
		}
		/// <summary>
		///  Constructs an upper bounds <see cref="OutOfRange"/> with a message.
		/// </summary>
		/// <param name="name">The name of the argument.</param>
		/// <param name="value">The value of the argument.</param>
		/// <param name="maxName">The name of the upper bounds.</param>
		/// <param name="max">The upper bounds.</param>
		/// <param name="inclusive">True if the upper bounds is inside the range.</param>
		/// <returns>The constructed <see cref="OutOfRange"/>.</returns>
		public static OutOfRange OutsideMax(string name, object value, string maxName, object max,
			bool inclusive = false)
		{
			return new OutOfRange(name, value, $"{Param(name, value)} is {Max(maxName, max, inclusive)}!");
		}

		#endregion
    }
}
