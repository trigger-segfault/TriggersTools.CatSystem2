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

namespace TriggersTools.CatSystem2.Utils {
	internal unsafe partial class Blowfish {
		#region Private Structs

		// DCBA - little endian - intel
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct aword {
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
		private struct aword {
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
		private struct aword {
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

		public Blowfish(uint key) {
			Initialize((byte*) &key, 4);
		}
		public Blowfish(byte[] key) {
			fixed (byte* pKey = key) {
				Initialize(pKey, key.Length);
			}
		}
		private Blowfish(byte* pKey, int keyLength) {
			Initialize(pKey, keyLength);
		}

		#endregion

		#region Encrypt/Decrypt
		
		public void Encrypt(ref ulong value) {
			ulong localValue = value;
			Encrypt((byte*) &localValue, 8);
			value = localValue;
		}
		public void Encrypt(byte[] input) {
			fixed (byte* pInput = input)
				Encrypt(pInput, input.Length);
		}
		public void Encrypt(byte[] input, int inputLength) {
			if (inputLength > input.Length) {
				throw ArgumentOutOfRangeUtils.OutsideMax(nameof(inputLength), inputLength,
					$"{nameof(input)}.{nameof(input.Length)}", input.Length, true);
			}
			fixed (byte* pInput = input)
				Encrypt(pInput, inputLength);
		}
		private void Encrypt(byte* pInput, int inputLength) {
			if (inputLength != GetOutputLength(inputLength))
				throw new ArgumentException("%s", "Input len != Output len");

			Encode(pInput, inputLength);
		}
		
		public void Decrypt(ref ulong value) {
			ulong localValue = value;
			Decrypt((byte*) &localValue, 8);
			value = localValue;
		}
		public void Decrypt(byte[] input) {
			fixed (byte* pInput = input)
				Decrypt(pInput, input.Length);
		}
		public void Decrypt(byte[] input, int inputLength) {
			if (inputLength > input.Length) {
				throw ArgumentOutOfRangeUtils.OutsideMax(nameof(inputLength), inputLength,
					$"{nameof(input)}.{nameof(input.Length)}", input.Length, true);
			}
			fixed (byte* pInput = input)
				Decrypt(pInput, inputLength);
		}
		private void Decrypt(byte* pInput, int inputLength) {
			if (inputLength != GetOutputLength(inputLength))
				throw new ArgumentException("Input Length is not a multiple of 8!");

			Decode(pInput, inputLength);
		}

		#endregion

		#region Initialize
		
		/// <summary>
		///  Constructs the encryption sieve.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keybytes"></param>
		private void Initialize(byte* key, int keybytes) {
			unchecked {
				// first fill arrays from data tables
				for (int i = 0; i < 18; i++)
					PArray[i] = BfP[i];

				for (int i = 0; i < SLength1; i++) {
					for (int j = 0; j < SLength2; j++)
						SBoxes[i, j] = BfS[i, j];
				}

				for (int i = 0, j = 0; i < NPass + 2; i++) {
					aword temp = new aword {
						byte0 = key[j],
						byte1 = key[(j + 1) % keybytes],
						byte2 = key[(j + 2) % keybytes],
						byte3 = key[(j + 3) % keybytes],
					};
					PArray[i] ^= temp.dword;
					j = (j + 4) % keybytes;
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

		#region Private Helpers

		//#define S(x,i) (SBoxes[i][x.w.byte##i])
		//#define bf_F(x) (((S(x,0) + S(x,1)) ^ S(x,2)) + S(x,3))
		//#define ROUND(a,b,n) (a.dword ^= bf_F(b) ^ PArray[n])

		private void ROUND(aword* a, aword b, int n) {
			unchecked {
				a->dword ^= (((SBoxes[0, b.byte0] + SBoxes[1, b.byte1]) ^
							   SBoxes[2, b.byte2]) + SBoxes[3, b.byte3]) ^ PArray[n];
			}
		}
		/// <summary>
		///  Get the output length, which must be even MOD 8.
		/// </summary>
		/// <param name="lInputLong"></param>
		/// <returns></returns>
		private static int GetOutputLength(int lInputLong) {
			int lVal = lInputLong % 8;  // find out if uneven number of bytes at the end
			if (lVal != 0)
				return lInputLong + 8 - lVal;
			else
				return lInputLong;
		}

		#endregion

		#region Encipher/Decipher

		/// <summary>
		///  The low level (private) encryption function.
		/// </summary>
		/// <param name="xl"></param>
		/// <param name="xr"></param>
		private void Encipher(uint* xl, uint* xr) {
			aword Xl = new aword();
			aword Xr = new aword();

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
		/// <param name="xl"></param>
		/// <param name="xr"></param>
		private void Decipher(uint* xl, uint* xr) {
			aword Xl = new aword();
			aword Xr = new aword();

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
		/// <summary>
		///  Encode pIntput into pOutput.  Input length in lSize.  Returned value
		///  is length of output which will be even MOD 8 bytes.  Inputbuffer and
		///  output buffer can be the same, but be sure buffer length is even MOD 8.
		/// </summary>
		/// <param name="pInput"></param>
		/// <param name="inputSize"></param>
		private void Encode(byte* pInput, int inputSize) {
			for (int lCount = 0; lCount < inputSize; lCount += 8) {
				Encipher((uint*) pInput, (uint*) (pInput + 4));
				pInput += 8;
			}
		}
		/// <summary>
		///  Decode pIntput into pOutput.  Input length in lSize.  Inputbuffer and
		///  output buffer can be the same, but be sure buffer length is even MOD 8.
		/// </summary>
		/// <param name="pInput"></param>
		/// <param name="inputSize"></param>
		private void Decode(byte* pInput, int inputSize) {
			for (int lCount = 0; lCount < inputSize; lCount += 8) {
				Decipher((uint*) pInput, (uint*) (pInput + 4));
				pInput += 8;
			}
		}

		#endregion
	}
}
