using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.SharpUtils.Mathematics {
	partial class MathUtils {
		#region Padding (Offset

		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount with the supplied additional offset.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static int Padding(int value, int padding, int offset) {
			if (padding == 0) throw new ArgumentException("Padding cannot be zero!", nameof(padding));
			offset = ((offset % padding) + padding) % padding; // Helps prevent overflows
			int mod = (((value + (padding - offset)) % padding) + padding) % padding;
			return (mod != 0 ? (padding - mod) : 0);
		}
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount with the supplied additional offset.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static uint Padding(uint value, uint padding, uint offset) {
			if (padding == 0) throw new ArgumentException("Padding cannot be zero!", nameof(padding));
			offset = ((offset % padding) + padding) % padding; // Helps prevent overflows
			uint mod = (((value + (padding - offset)) % padding) + padding) % padding;
			return (mod != 0 ? (padding - mod) : 0);
		}
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount with the supplied additional offset.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static long Padding(long value, long padding, long offset) {
			if (padding == 0) throw new ArgumentException("Padding cannot be zero!", nameof(padding));
			offset = ((offset % padding) + padding) % padding; // Helps prevent overflows
			long mod = (((value + (padding - offset)) % padding) + padding) % padding;
			return (mod != 0 ? (padding - mod) : 0);
		}
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount with the supplied additional offset.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static ulong Padding(ulong value, ulong padding, ulong offset) {
			if (padding == 0) throw new ArgumentException("Padding cannot be zero!", nameof(padding));
			offset = ((offset % padding) + padding) % padding; // Helps prevent overflows
			ulong mod = (((value + (padding - offset)) % padding) + padding) % padding;
			return (mod != 0 ? (padding - mod) : 0);
		}

		#endregion

		#region Padding (No Offset)

		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static int Padding(int value, int padding) => Padding(value, padding, 0);
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static uint Padding(uint value, uint padding) => Padding(value, padding, 0);
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static long Padding(long value, long padding) => Padding(value, padding, 0);
		/// <summary>
		///  Gets the pad amount needed to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to get the padding for.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding to add to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static ulong Padding(ulong value, ulong padding) => Padding(value, padding, 0);

		#endregion

		#region Pad (Offset)

		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount with the supplied
		///  additional offset.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static int Pad(int value, int padding, int offset) {
			return value + Padding(value, padding, offset);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount with the supplied
		///  additional offset.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static uint Pad(uint value, uint padding, uint offset) {
			return value + Padding(value, padding, offset);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount with the supplied
		///  additional offset.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static long Pad(long value, long padding, long offset) {
			return value + Padding(value, padding, offset);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount with the supplied
		///  additional offset.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <param name="offset">The additional offset before the padding is calculated.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <remarks>
		///  Offset is applied by subtracting. So an offset of one means the result would be one greater
		///  (assuming the modulus does not wrap then).
		/// </remarks>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static ulong Pad(ulong value, ulong padding, ulong offset) {
			return value + Padding(value, padding, offset);
		}

		#endregion

		#region Pad (No Offset)

		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static int Pad(int value, int padding) {
			return value + Padding(value, padding, 0);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static uint Pad(uint value, uint padding) {
			return value + Padding(value, padding, 0);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static long Pad(long value, long padding) {
			return value + Padding(value, padding, 0);
		}
		/// <summary>
		///  Returns <paramref name="value"/> with padding applied to comform to the padding amount.
		/// </summary>
		/// <param name="value">The value to add the padding to.</param>
		/// <param name="padding">The padding to conform to.</param>
		/// <returns>The calulated padding added to <paramref name="value"/>.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="padding"/> equals zero.
		/// </exception>
		public static ulong Pad(ulong value, ulong padding) {
			return value + Padding(value, padding, 0);
		}

		#endregion
	}
}
