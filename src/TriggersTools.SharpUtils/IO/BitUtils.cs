using System;

namespace TriggersTools.SharpUtils.IO {
	public static partial class BitUtils {
		#region GetBit (Char)

		/// <summary>
		///  Gets the bit at the specified index in the Unicode Character.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(char value, int bitIndex) {
			return (value & (1 << bitIndex)) != 0;
		}

		#endregion

		#region SetBit (Char)

		/// <summary>
		///  Sets the bit at the specified index in the Unicode Character.
		/// </summary>
		/// <param name="value">The value to set the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static char SetBit(char value, int bitIndex, bool on) {
			if (on)
				return unchecked((char) (value | (1 << bitIndex)));
			else
				return unchecked((char) (value & ~(1 << bitIndex)));
		}

		#endregion

		#region GetBit (Signed)

		/// <summary>
		///  Gets the bit at the specified index in the signed byte.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(sbyte value, int bitIndex) {
			return (value & (1 << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 16-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(short value, int bitIndex) {
			return (value & (1 << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 32-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(int value, int bitIndex) {
			return (value & (1 << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 64-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(long value, int bitIndex) {
			return (value & (1L << bitIndex)) != 0;
		}

		#endregion

		#region SetBit (Signed)

		/// <summary>
		///  Sets the bit at the specified index in the signed byte.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static sbyte SetBit(sbyte value, int bitIndex, bool on) {
			if (on)
				return unchecked((sbyte) (value | (sbyte) (1 << bitIndex)));
			else
				return unchecked((sbyte) (value & ~(1 << bitIndex)));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 16-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static short SetBit(short value, int bitIndex, bool on) {
			if (on)
				return unchecked((short) (value | (short) (1 << bitIndex)));
			else
				return unchecked((short) (value & ~(1 << bitIndex)));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 32-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static int SetBit(int value, int bitIndex, bool on) {
			if (on)
				return (value | (1 << bitIndex));
			else
				return (value & ~(1 << bitIndex));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 64-bit signed integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static long SetBit(long value, int bitIndex, bool on) {
			if (on)
				return (value | (1L << bitIndex));
			else
				return (value & ~(1L << bitIndex));
		}

		#endregion

		#region GetBit (Unsigned)

		/// <summary>
		///  Gets the bit at the specified index in the byte.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(byte value, int bitIndex) {
			return (value & (1 << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 16-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(ushort value, int bitIndex) {
			return (value & (1U << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 32-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(uint value, int bitIndex) {
			return (value & (1U << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index in the 64-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(ulong value, int bitIndex) {
			return (value & (1UL << bitIndex)) != 0;
		}

		#endregion

		#region SetBit (Unsigned)

		/// <summary>
		///  Sets the bit at the specified index in the byte.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static byte SetBit(byte value, int bitIndex, bool on) {
			if (on)
				return unchecked((byte) (value | (byte) (1 << bitIndex)));
			else
				return unchecked((byte) (value & ~(1 << bitIndex)));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 16-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static ushort SetBit(ushort value, int bitIndex, bool on) {
			if (on)
				return unchecked((ushort) (value | (ushort) (1 << bitIndex)));
			else
				return unchecked((ushort) (value & ~(1 << bitIndex)));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 32-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static uint SetBit(uint value, int bitIndex, bool on) {
			if (on)
				return (value | (1U << bitIndex));
			else
				return (value & ~(1U << bitIndex));
		}
		/// <summary>
		///  Sets the bit at the specified index in the 64-bit unsigned integer.
		/// </summary>
		/// <param name="value">The value to get the bit for.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static ulong SetBit(ulong value, int bitIndex, bool on) {
			if (on)
				return (value | (1UL << bitIndex));
			else
				return (value & ~(1UL << bitIndex));
		}

		#endregion

		#region GetBit (byte[])

		/// <summary>
		///  Gets the bit at the specified index in the byte of the byte array.
		/// </summary>
		/// <param name="bytes">The byte array to get the bit from.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero or greater than the length of <paramref name="bytes"/> * 8,
		///  minus 1.
		/// </exception>
		public static bool GetBit(byte[] bytes, int bitIndex) {
			int startIndex = bitIndex / 8;
			bitIndex %= 8;
			return (bytes[startIndex] & (1 << bitIndex)) != 0;
		}
		/// <summary>
		///  Gets the bit at the specified index and offset in the byte of the byte array.
		/// </summary>
		/// <param name="bytes">The byte array to get the bit from.</param>
		/// <param name="startIndex">The starting index in the byte array.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <returns>True if the bit is on.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="startIndex"/> + <paramref name="bitIndex"/> / 8 is less than zero or greater than the
		///  length of <paramref name="bytes"/> minus 1.-or- <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static bool GetBit(byte[] bytes, int startIndex, int bitIndex) {
			startIndex += bitIndex / 8;
			bitIndex %= 8;
			return (bytes[startIndex] & (1 << bitIndex)) != 0;
		}

		#endregion

		#region SetBit (byte[])

		/// <summary>
		///  Sets the bit at the specified index and offset in the byte of the byte array.
		/// </summary>
		/// <param name="bytes">The byte array to set the bit in.</param>
		/// <param name="startIndex">The starting index in the byte array.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bitIndex"/> is less than zero or greater than the length of <paramref name="bytes"/> * 8,
		///  minus 1.
		/// </exception>
		public static void SetBit(byte[] bytes, int bitIndex, bool on) {
			int startIndex = bitIndex / 8;
			bitIndex %= 8;
			if (on)
				bytes[startIndex] |= unchecked((byte) (1 << bitIndex));
			else
				bytes[startIndex] &= unchecked((byte) ~(1 << bitIndex));
		}
		/// <summary>
		///  Sets the bit at the specified index and offset in the byte of the byte array.
		/// </summary>
		/// <param name="bytes">The byte array to set the bit in.</param>
		/// <param name="startIndex">The starting index in the byte array.</param>
		/// <param name="bitIndex">The index of the bit, from LSB to MSB.</param>
		/// <param name="on">True if the bit is switched on.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="startIndex"/> + <paramref name="bitIndex"/> / 8 is less than zero or greater than the
		///  length of <paramref name="bytes"/> minus 1.-or- <paramref name="bitIndex"/> is less than zero.
		/// </exception>
		public static void SetBit(byte[] bytes, int startIndex, int bitIndex, bool on) {
			startIndex += bitIndex / 8;
			bitIndex %= 8;
			if (on)
				bytes[startIndex] |= unchecked((byte) (1 << bitIndex));
			else
				bytes[startIndex] &= unchecked((byte) ~(1 << bitIndex));
		}

		#endregion
	}
}
