/* blowfish.cs   C# class implementation of the BLOWFISH encryption algorithm
 * _THE BLOWFISH ENCRYPTION ALGORITHM_
 * by Bruce Schneier
 *
 * Revised code--3/20/94
 * Converted to C++ class 5/96, Jim Conger
 * Modified by Fuyin 2014/5/2
 * Ported to C# by Robert Jordan 2019/5/3
 */
using System;
using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  A managed implementation of the Blowfish encryption algorithm.
	/// </summary>
	internal unsafe partial class Blowfish {
		#region Aword Structs

		// DCBA - little endian - intel
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint dword;
			[FieldOffset(0)]
			public byte byte3;
			[FieldOffset(1)]
			public byte byte2;
			[FieldOffset(2)]
			public byte byte1;
			[FieldOffset(3)]
			public byte byte0;
		}

		/*// ABCD - big endian - motorola
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint dword;
			[FieldOffset(0)]
			public byte byte0;
			[FieldOffset(1)]
			public byte byte1;
			[FieldOffset(2)]
			public byte byte2;
			[FieldOffset(3)]
			public byte byte3;
		}

		// BADC - vax
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint dword;
			[FieldOffset(0)]
			public byte byte1;
			[FieldOffset(1)]
			public byte byte0;
			[FieldOffset(2)]
			public byte byte3;
			[FieldOffset(3)]
			public byte byte2;
		}*/

		#endregion

		#region Constants

		private const int NPass = 16;
		private const int PLength = NPass + 2;
		private const int SLength1 = 4;
		private const int SLength2 = 256;

		#endregion

		#region Fields

		private readonly uint[] PArray = new uint[PLength];
		private readonly uint[,] SBoxes = new uint[SLength1, SLength2];

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the blowfish encryption with a 4-byte key.
		/// </summary>
		/// <param name="key">The 4-byte key to use as a key.</param>
		public Blowfish(uint key) {
			Initialize((byte*) &key, 4);
		}
		/// <summary>
		///  Constructs the blowfish encryption with a byte array.
		/// </summary>
		/// <param name="key">The byte array to use as a key.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="key"/> is null.
		/// </exception>
		public Blowfish(byte[] key) {
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			fixed (byte* pKey = key) {
				Initialize(pKey, key.Length);
			}
		}
		/// <summary>
		///  Constructs the blowfish encryption with a byte array pointed to and the length.
		/// </summary>
		/// <param name="key">The address of the byte array to use as a key.</param>
		/// <param name="keyLength">The length of the byte array.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="key"/> is null.
		/// </exception>
		private Blowfish(byte* key, int keyLength) {
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			Initialize(key, keyLength);
		}

		#endregion

		#region Encrypt/Decrypt
		
		/// <summary>
		///  Encrypts the ulong value.
		/// </summary>
		/// <param name="value">The 8-byte value to encrypt.</param>
		public void Encrypt(ref ulong value) {
			ulong localValue = value;
			Encrypt((byte*) &localValue, 8);
			value = localValue;
		}
		/// <summary>
		///  Encrypts the buffer.
		/// </summary>
		/// <param name="buffer">The buffer to encrypt that must have a length padded to 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="buffer"/>'s length is not a multiple of 8.
		/// </exception>
		public void Encrypt(byte[] buffer) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			fixed (byte* pInput = buffer)
				Encrypt(pInput, buffer.Length);
		}
		/// <summary>
		///  Encrypts the buffer with the specified length.
		/// </summary>
		/// <param name="buffer">The buffer to encrypt.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bufferLength"/> is less than zero or greater than the length of <paramref name="buffer"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		public void Encrypt(byte[] buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength < 0 || bufferLength > buffer.Length) {
				throw ArgumentOutOfRangeUtils.OutsideRange(nameof(bufferLength), bufferLength, 0,
					$"{nameof(buffer)}.{nameof(buffer.Length)}", buffer.Length, true, true);
			}
			fixed (byte* pBuffer = buffer)
				Encrypt(pBuffer, bufferLength);
		}
		/// <summary>
		///  Encrypts the buffer pointed to with the specified length.
		/// </summary>
		/// <param name="buffer">The address of the buffer to encrypt.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		private void Encrypt(byte* buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength != GetOutputLength(bufferLength))
				throw new ArgumentException($"Buffer length is not a multiple of 8!");

			//Encode(buffer, bufferLength);
			for (int i = 0; i < bufferLength; i += 8) {
				Encipher((uint*) buffer, (uint*) (buffer + 4));
				buffer += 8;
			}
		}

		/// <summary>
		///  Decrypts the ulong value.
		/// </summary>
		/// <param name="value">The 8-byte value to decrypt.</param>
		public void Decrypt(ref ulong value) {
			ulong localValue = value;
			Decrypt((byte*) &localValue, 8);
			value = localValue;
		}
		/// <summary>
		///  Decrypts the buffer.
		/// </summary>
		/// <param name="buffer">The buffer to decrypt that must have a length padded to 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="buffer"/>'s length is not a multiple of 8.
		/// </exception>
		public void Decrypt(byte[] buffer) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			fixed (byte* pBuffer = buffer)
				Decrypt(pBuffer, buffer.Length);
		}
		/// <summary>
		///  Decrypts the buffer with the specified length.
		/// </summary>
		/// <param name="buffer">The buffer to decrypt.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="bufferLength"/> is less than zero or greater than the length of <paramref name="buffer"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		public void Decrypt(byte[] buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength > buffer.Length) {
				throw ArgumentOutOfRangeUtils.OutsideMax(nameof(bufferLength), bufferLength,
					$"{nameof(buffer)}.{nameof(buffer.Length)}", buffer.Length, true);
			}
			fixed (byte* pInput = buffer)
				Decrypt(pInput, bufferLength);
		}
		/// <summary>
		///  Decrypts the buffer pointed to with the specified length.
		/// </summary>
		/// <param name="buffer">The address of the buffer to decrypt.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		private void Decrypt(byte* buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength != GetOutputLength(bufferLength))
				throw new ArgumentException("Buffer Length is not a multiple of 8!");

			//Decode(buffer, bufferLength);
			for (int i = 0; i < bufferLength; i += 8) {
				Decipher((uint*) buffer, (uint*) (buffer + 4));
				buffer += 8;
			}
		}

		#endregion

		/*#region Encode/Decode

		/// <summary>
		///  Encode <paramref name="buffer"/>. Input length in <paramref name="bufferLength"/>.
		///  The buffer length MUST be MOD 8.
		/// </summary>
		/// <param name="buffer">The pointer to the buffer to encode.</param>
		/// <param name="bufferLength">The size of the buffer.</param>
		private void Encode(byte* buffer, int bufferLength) {
			for (int i = 0; i < bufferLength; i += 8) {
				Encipher((uint*) buffer, (uint*) (buffer + 4));
				buffer += 8;
			}
		}
		/// <summary>
		///  Decode <paramref name="buffer"/>. Input length in <paramref name="bufferLength"/>.
		///  The buffer length MUST be MOD 8.
		/// </summary>
		/// <param name="buffer">The pointer to the buffer to decode.</param>
		/// <param name="bufferLength">The size of the buffer.</param>
		private void Decode(byte* buffer, int bufferLength) {
			for (int i = 0; i < bufferLength; i += 8) {
				Decipher((uint*) buffer, (uint*) (buffer + 4));
				buffer += 8;
			}
		}

		#endregion*/

		#region Initialize

		/// <summary>
		///  Constructs the encryption sieve.
		/// </summary>
		/// <param name="key">The pointer to the key to initialize.</param>
		/// <param name="keyLength">The length of the key pointed to.</param>
		private void Initialize(byte* key, int keyLength) {
			unchecked {
				// first fill arrays from data tables
				for (int i = 0; i < 18; i++)
					PArray[i] = BfP[i];

				for (int i = 0; i < SLength1; i++) {
					for (int j = 0; j < SLength2; j++)
						SBoxes[i, j] = BfS[i, j];
				}

				for (int i = 0, j = 0; i < NPass + 2; i++) {
					Aword temp = new Aword {
						byte0 = key[j],
						byte1 = key[(j + 1) % keyLength],
						byte2 = key[(j + 2) % keyLength],
						byte3 = key[(j + 3) % keyLength],
					};
					PArray[i] ^= temp.dword;
					j = (j + 4) % keyLength;
				}

				uint datal = 0;
				uint datar = 0;

				for (int i = 0; i < NPass + 2; i += 2) {
					Encipher(&datal, &datar);
					PArray[i] = datal;
					PArray[i + 1] = datar;
				}

				for (int i = 0; i < SLength1; ++i) {
					for (int j = 0; j < SLength2; j += 2) {
						Encipher(&datal, &datar);
						SBoxes[i, j] = datal;
						SBoxes[i, j + 1] = datar;
					}
				}
			}
		}

		#endregion

		#region Encipher/Decipher

		/// <summary>
		///  The low level (private) encryption function.
		/// </summary>
		/// <param name="xl">The left (first 4 bytes) to encipher.</param>
		/// <param name="xr">The right (next 4 bytes) to encipher.</param>
		private void Encipher(uint* xl, uint* xr) {
			Aword Xl = new Aword();
			Aword Xr = new Aword();

			Xl.dword = *xl;
			Xr.dword = *xr;

			unchecked {
				Xl.dword ^= PArray[0];
				ROUND(&Xr, Xl, 1); ROUND(&Xl, Xr, 2);
				ROUND(&Xr, Xl, 3); ROUND(&Xl, Xr, 4);
				ROUND(&Xr, Xl, 5); ROUND(&Xl, Xr, 6);
				ROUND(&Xr, Xl, 7); ROUND(&Xl, Xr, 8);
				ROUND(&Xr, Xl, 9); ROUND(&Xl, Xr, 10);
				ROUND(&Xr, Xl, 11); ROUND(&Xl, Xr, 12);
				ROUND(&Xr, Xl, 13); ROUND(&Xl, Xr, 14);
				ROUND(&Xr, Xl, 15); ROUND(&Xl, Xr, 16);
				Xr.dword ^= PArray[17];
			}

			*xr = Xl.dword;
			*xl = Xr.dword;
		}
		/// <summary>
		///  The low level (private) decryption function.
		/// </summary>
		/// <param name="xl">The left (first 4 bytes) to decipher.</param>
		/// <param name="xr">The right (next 4 bytes) to decipher.</param>
		private void Decipher(uint* xl, uint* xr) {
			Aword Xl = new Aword();
			Aword Xr = new Aword();

			Xl.dword = *xl;
			Xr.dword = *xr;

			unchecked {
				Xl.dword ^= PArray[17];
				ROUND(&Xr, Xl, 16); ROUND(&Xl, Xr, 15);
				ROUND(&Xr, Xl, 14); ROUND(&Xl, Xr, 13);
				ROUND(&Xr, Xl, 12); ROUND(&Xl, Xr, 11);
				ROUND(&Xr, Xl, 10); ROUND(&Xl, Xr, 9);
				ROUND(&Xr, Xl, 8); ROUND(&Xl, Xr, 7);
				ROUND(&Xr, Xl, 6); ROUND(&Xl, Xr, 5);
				ROUND(&Xr, Xl, 4); ROUND(&Xl, Xr, 3);
				ROUND(&Xr, Xl, 2); ROUND(&Xl, Xr, 1);
				Xr.dword ^= PArray[0];
			}

			*xl = Xr.dword;
			*xr = Xl.dword;
		}

		#endregion

		#region Private Helpers

		//#define S(x,i) (SBoxes[i][x.w.byte##i])
		//#define bf_F(x) (((S(x,0) + S(x,1)) ^ S(x,2)) + S(x,3))
		//#define ROUND(a,b,n) (a.dword ^= bf_F(b) ^ PArray[n])

		/// <summary>
		///  Performs blowfish rounding inside <see cref="Encipher"/> and <see cref="Decipher"/>.
		/// </summary>
		/// <param name="a">The aword to save to.</param>
		/// <param name="b">The aword to get the indecies from for <see cref="SBoxes"/>.</param>
		/// <param name="n">The nth index of <see cref="PArray"/></param>
		private void ROUND(Aword* a, Aword b, int n) {
			unchecked {
				a->dword ^= (((SBoxes[0, b.byte0] + SBoxes[1, b.byte1]) ^
							   SBoxes[2, b.byte2]) + SBoxes[3, b.byte3]) ^ PArray[n];
			}
		}
		/// <summary>
		///  Get the output length, which must be even MOD 8.
		/// </summary>
		/// <param name="inputLength">The original length of the buffer size.</param>
		/// <returns>The input length padded to MOD 8</returns>
		private static int GetOutputLength(int inputLength) {
			return MathUtils.Pad(inputLength, 8);
		}

		#endregion
	}
}
