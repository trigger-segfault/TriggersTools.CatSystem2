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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  A managed implementation of the Blowfish encryption algorithm.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	unsafe sealed partial class BlowfishManaged : Blowfish {
		#region Aword Structs

		// DCBA - little endian - intel
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint Dword;
			[FieldOffset(0)]
			public byte Byte3;
			[FieldOffset(1)]
			public byte Byte2;
			[FieldOffset(2)]
			public byte Byte1;
			[FieldOffset(3)]
			public byte Byte0;
		}

		/*// ABCD - big endian - motorola
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint Dword;
			[FieldOffset(0)]
			public byte Byte0;
			[FieldOffset(1)]
			public byte Byte1;
			[FieldOffset(2)]
			public byte Byte2;
			[FieldOffset(3)]
			public byte Byte3;
		}

		// BADC - vax
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct Aword {
			[FieldOffset(0)]
			public uint Dword;
			[FieldOffset(0)]
			public byte Byte1;
			[FieldOffset(1)]
			public byte Byte0;
			[FieldOffset(2)]
			public byte Byte3;
			[FieldOffset(3)]
			public byte Byte2;
		}*/

		#endregion

		#region Fields

		/// <summary>
		///  The array of 18 <see cref="uint"/>s seeded by <see cref="bf_P"/> during initialization.
		/// </summary>
		private readonly uint[] PArray = (uint[]) bf_P.Clone();// = new uint[PLength];
		/// <summary>
		///  The array of 4 * 256 <see cref="uint"/>s seeded by <see cref="bf_S"/> during initialization.
		/// </summary>
		private readonly uint[] SBoxes = (uint[]) bf_S.Clone();// = new uint[SLength];

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the blowfish encryption with a 4-byte key.
		/// </summary>
		/// <param name="key">The 4-byte key to use as a key.</param>
		public BlowfishManaged(uint key) {
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
		public BlowfishManaged(byte[] key) {
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
		private BlowfishManaged(byte* key, int keyLength) {
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			Initialize(key, keyLength);
		}

		#endregion

		#region Encrypt/Decrypt

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
		protected override void Encrypt(byte* buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength != GetOutputLength(bufferLength))
				throw new ArgumentException($"Buffer length is not a multiple of 8!");

			Encode(buffer, bufferLength);
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
		protected override void Decrypt(byte* buffer, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (bufferLength != GetOutputLength(bufferLength))
				throw new ArgumentException("Buffer length is not a multiple of 8!");
			
			Decode(buffer, bufferLength);
		}

		#endregion

		#region Encode/Decode

		/// <summary>
		///  Encode <paramref name="buffer"/>. Input length in <paramref name="bufferLength"/>.
		///  The buffer length MUST be MOD 8. Used for <see cref="Initialize"/>.
		/// </summary>
		/// <param name="buffer">The pointer to the buffer to encode.</param>
		/// <param name="bufferLength">The size of the buffer.</param>
		/// <param name="datal">The left data to continuously encode.</param>
		/// <param name="datar">The right data to continuously encode.</param>
		/// <param name="p">The pointer to <see cref="PArray"/>.</param>
		/// <param name="s">The pointer to <see cref="SBoxes"/>.</param>
		private void EncodeInit(uint* array, int bufferLength, ref uint datal, ref uint datar, uint* p, uint* s) {
			unchecked {
				for (int i = 0; i < bufferLength; i += 2, array += 2) {
					Aword Xl = new Aword();
					Aword Xr = new Aword();

					Xl.Dword = datal;
					Xr.Dword = datar;

					Xl.Dword ^= PArray[0];
					if (CatDebug.ManagedBlowfishRound) {
						ROUND(ref Xr, ref Xl,  1, p,s); ROUND(ref Xl, ref Xr,  2, p,s);
						ROUND(ref Xr, ref Xl,  3, p,s); ROUND(ref Xl, ref Xr,  4, p,s);
						ROUND(ref Xr, ref Xl,  5, p,s); ROUND(ref Xl, ref Xr,  6, p,s);
						ROUND(ref Xr, ref Xl,  7, p,s); ROUND(ref Xl, ref Xr,  8, p,s);
						ROUND(ref Xr, ref Xl,  9, p,s); ROUND(ref Xl, ref Xr, 10, p,s);
						ROUND(ref Xr, ref Xl, 11, p,s); ROUND(ref Xl, ref Xr, 12, p,s);
						ROUND(ref Xr, ref Xl, 13, p,s); ROUND(ref Xl, ref Xr, 14, p,s);
						ROUND(ref Xr, ref Xl, 15, p,s); ROUND(ref Xl, ref Xr, 16, p,s);
					}
					else {
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[1];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[2];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[3];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[4];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[5];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[6];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[7];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[8];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[9];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[10];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[11];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[12];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[13];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[14];
						Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[15];
						Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[16];
					}
					Xr.Dword ^= PArray[17];

					datal = Xr.Dword;
					datar = Xl.Dword;

					array[0] = datal;
					array[1] = datar;
				}
			}
		}
		/// <summary>
		///  Encode <paramref name="buffer"/>. Input length in <paramref name="bufferLength"/>.
		///  The buffer length MUST be MOD 8.
		/// </summary>
		/// <param name="buffer">The pointer to the buffer to encode.</param>
		/// <param name="bufferLength">The size of the buffer.</param>
		private void Encode(byte* buffer, int bufferLength) {
			unchecked {
				fixed (uint* pPArray = PArray)
				fixed (uint* pSBoxes = SBoxes) {
					uint* p = pPArray;
					uint* s = pSBoxes;
					for (int i = 0; i < bufferLength; i += 8, buffer += 8) {
						Aword Xl = new Aword();
						Aword Xr = new Aword();

						Xl.Dword = *((uint*) (buffer));
						Xr.Dword = *((uint*) (buffer + 4));

						Xl.Dword ^= PArray[0];
						if (CatDebug.ManagedBlowfishRound) {
							ROUND(ref Xr, ref Xl,  1, p,s); ROUND(ref Xl, ref Xr,  2, p,s);
							ROUND(ref Xr, ref Xl,  3, p,s); ROUND(ref Xl, ref Xr,  4, p,s);
							ROUND(ref Xr, ref Xl,  5, p,s); ROUND(ref Xl, ref Xr,  6, p,s);
							ROUND(ref Xr, ref Xl,  7, p,s); ROUND(ref Xl, ref Xr,  8, p,s);
							ROUND(ref Xr, ref Xl,  9, p,s); ROUND(ref Xl, ref Xr, 10, p,s);
							ROUND(ref Xr, ref Xl, 11, p,s); ROUND(ref Xl, ref Xr, 12, p,s);
							ROUND(ref Xr, ref Xl, 13, p,s); ROUND(ref Xl, ref Xr, 14, p,s);
							ROUND(ref Xr, ref Xl, 15, p,s); ROUND(ref Xl, ref Xr, 16, p,s);
						}
						else {
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[1];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[2];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[3];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[4];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[5];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[6];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[7];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[8];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[9];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[10];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[11];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[12];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[13];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[14];
							Xr.Dword ^= (((pSBoxes[Xl.Byte0] + pSBoxes[256 | Xl.Byte1]) ^ pSBoxes[512 | Xl.Byte2]) + pSBoxes[768 | Xl.Byte3]) ^ pPArray[15];
							Xl.Dword ^= (((pSBoxes[Xr.Byte0] + pSBoxes[256 | Xr.Byte1]) ^ pSBoxes[512 | Xr.Byte2]) + pSBoxes[768 | Xr.Byte3]) ^ pPArray[16];
						}
						Xr.Dword ^= PArray[17];

						*((uint*) (buffer)) = Xr.Dword;
						*((uint*) (buffer + 4)) = Xl.Dword;
					}
				}
			}
		}
		/// <summary>
		///  Decode <paramref name="buffer"/>. Input length in <paramref name="bufferLength"/>.
		///  The buffer length MUST be MOD 8.
		/// </summary>
		/// <param name="buffer">The pointer to the buffer to decode.</param>
		/// <param name="bufferLength">The size of the buffer.</param>
		private void Decode(byte* buffer, int bufferLength) {
			unchecked {
				fixed (uint* pPArray = PArray)
				fixed (uint* pSBoxes = SBoxes) {
					uint* p = pPArray;
					uint* s = pSBoxes;
					for (int i = 0; i < bufferLength; i += 8, buffer += 8) {
						Aword Xl = new Aword();
						Aword Xr = new Aword();

						Xl.Dword = *((uint*) (buffer));
						Xr.Dword = *((uint*) (buffer + 4));

						Xl.Dword ^= p[17];
						if (CatDebug.ManagedBlowfishRound) {
							ROUND(ref Xr, ref Xl, 16, p,s); ROUND(ref Xl, ref Xr, 15, p,s);
							ROUND(ref Xr, ref Xl, 14, p,s); ROUND(ref Xl, ref Xr, 13, p,s);
							ROUND(ref Xr, ref Xl, 12, p,s); ROUND(ref Xl, ref Xr, 11, p,s);
							ROUND(ref Xr, ref Xl, 10, p,s); ROUND(ref Xl, ref Xr,  9, p,s);
							ROUND(ref Xr, ref Xl,  8, p,s); ROUND(ref Xl, ref Xr,  7, p,s);
							ROUND(ref Xr, ref Xl,  6, p,s); ROUND(ref Xl, ref Xr,  5, p,s);
							ROUND(ref Xr, ref Xl,  4, p,s); ROUND(ref Xl, ref Xr,  3, p,s);
							ROUND(ref Xr, ref Xl,  2, p,s); ROUND(ref Xl, ref Xr,  1, p,s);
						}
						else {
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[16];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[15];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[14];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[13];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[12];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[11];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[10];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[9];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[8];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[7];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[6];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[5];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[4];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[3];
							Xr.Dword ^= (((s[Xl.Byte0] + s[256 | Xl.Byte1]) ^ s[512 | Xl.Byte2]) + s[768 | Xl.Byte3]) ^ p[2];
							Xl.Dword ^= (((s[Xr.Byte0] + s[256 | Xr.Byte1]) ^ s[512 | Xr.Byte2]) + s[768 | Xr.Byte3]) ^ p[1];
						}
						Xr.Dword ^= p[0];

						*((uint*) (buffer)) = Xr.Dword;
						*((uint*) (buffer + 4)) = Xl.Dword;
					}
				}
			}
		}

		#endregion

		#region Initialize

		/// <summary>
		///  Constructs the encryption sieve.
		/// </summary>
		/// <param name="key">The pointer to the key to initialize.</param>
		/// <param name="keyLength">The length of the key pointed to.</param>
		protected override void Initialize(byte* key, int keyLength) {
			unchecked {
				//PArray = (uint[]) BfP.Clone();
				//SBoxes = (uint[]) BfS.Clone();
				fixed (uint* pPArray = PArray)
				fixed (uint* pSBoxes = SBoxes) {
					for (int i = 0, j = 0; i < NPass + 2; i++) {
						Aword temp = new Aword {
							Byte0 = key[j],
							Byte1 = key[(j + 1) % keyLength],
							Byte2 = key[(j + 2) % keyLength],
							Byte3 = key[(j + 3) % keyLength],
						};
						pPArray[i] ^= temp.Dword;
						j = (j + 4) % keyLength;
					}

					uint datal = 0;
					uint datar = 0;
					EncodeInit(pPArray, PLength, ref datal, ref datar, pPArray, pSBoxes);
					EncodeInit(pSBoxes, SLength, ref datal, ref datar, pPArray, pSBoxes);
				}
			}
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
		/// <param name="n">The nth index of <see cref="PArray"/>.</param>
		/// <param name="pPArray">The pointer to <see cref="PArray"/>.</param>
		/// <param name="pSBoxes">The pointer to <see cref="SBoxes"/>.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ROUND(ref Aword a, ref Aword b, in int n, in uint* pPArray, in uint* pSBoxes) {
			unchecked {
				a.Dword ^= (((pSBoxes[      b.Byte0]  + pSBoxes[256 | b.Byte1]) ^
							  pSBoxes[512 | b.Byte2]) + pSBoxes[768 | b.Byte3]) ^ pPArray[n];
			}
		}

		#endregion
	}
}
