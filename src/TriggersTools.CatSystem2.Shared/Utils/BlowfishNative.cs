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
using System.IO;
using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  A blowfish structure is used solely for P/Invoke. This is contained within <see cref="BlowfishNative"/> at the
	///  same offsets as it's own arrays so that they line up when overwritten.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct BlowfishStruct {
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = Blowfish.PLength)]
		public uint[] PArray;
		[FieldOffset(Blowfish.PLength * sizeof(uint))]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = Blowfish.SLength)]
		public uint[] SBoxes;
	}
	/// <summary>
	///  A native wrapper for the Blowfish encryption algorithm using asmodean.dll.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
#if CAT_DEBUG
	public
#else
	internal
#endif
	unsafe partial class BlowfishNative : Blowfish {
		#region Fields

		/// <summary>
		///  A blowfish structure is used solely for P/Invoke. This is contained within <see cref="BlowfishNative"/> at the
		///  same offsets as it's own arrays so that they line up when overwritten.
		/// </summary>
		[FieldOffset(0)]
		private BlowfishStruct Struct;
		/// <summary>
		///  The array of 18 <see cref="uint"/>s seeded by bf_P during initialization.
		/// </summary>
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = PLength)]
		private uint[] PArray = new uint[PLength];
		/// <summary>
		///  The array of 4 * 256 <see cref="uint"/>s seeded by bf_S during initialization.
		/// </summary>
		[FieldOffset(PLength * sizeof(uint))]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = SLength)]
		private uint[] SBoxes = new uint[SLength];

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs the blowfish encryption with a 4-byte key.
		/// </summary>
		/// <param name="key">The 4-byte key to use as a key.</param>
		public BlowfishNative(uint key) {
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
		public BlowfishNative(byte[] key) {
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
		private BlowfishNative(byte* key, int keyLength) {
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

			Asmodean.EncryptBlowfish(Struct, buffer, bufferLength);
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
				throw new ArgumentException("Buffer Length is not a multiple of 8!");

			Asmodean.DecryptBlowfish(Struct, buffer, bufferLength);
		}

		#endregion

		#region Initialize

		/// <summary>
		///  Constructs the encryption sieve.
		/// </summary>
		/// <param name="key">The pointer to the key to initialize.</param>
		/// <param name="keyLength">The length of the key pointed to.</param>
		protected override void Initialize(byte* key, int keyLength) {
			Asmodean.InitializeBlowfish(ref Struct, key, keyLength);
		}

		#endregion
	}
}
