using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.Mathematics;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  The base class for the Blowfish encryption algorithm. This will either be a <see cref="BlowfishNative"/> or
	///  <see cref="BlowfishManaged"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
#if CAT_DEBUG
	public
#else
	internal
#endif
	unsafe abstract class Blowfish {
		#region Constants
		
		/// <summary>
		///  The number of passes during enciphering/deciphering with ROUND.
		/// </summary>
		protected const int NPass = 16;
		/// <summary>
		///  The length of the PArray.
		/// </summary>
		internal const int PLength = NPass + 2;
		/// <summary>
		///  The full length of the SBoxes.
		/// </summary>
		internal const int SLength = 4 * 256;

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
			fixed (byte* pBuffer = buffer)
				Encrypt(pBuffer, buffer.Length);
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
		///  Encrypts the buffer with the specified length.
		/// </summary>
		/// <param name="buffer">The buffer to encrypt.</param>
		/// <param name="index">The index to start encrypting the buffer at.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="index"/> or <paramref name="bufferLength"/> is less than zero.-or-
		///  <paramref name="index"/> + <paramref name="bufferLength"/> is greater than the length of
		///  <paramref name="buffer"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		public void Encrypt(byte[] buffer, int index, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(index), index, 0, true);
			if (bufferLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(bufferLength), bufferLength, 0, true);
			if (index + bufferLength > buffer.Length) {
				throw ArgumentOutOfRangeUtils.OutsideRange($"{nameof(index)} + {nameof(bufferLength)}",
					index + bufferLength, 0, $"{nameof(buffer)}.{nameof(Array.Length)}", buffer.Length, true, false);
			}
			fixed (byte* pBuffer = buffer)
				Encrypt(pBuffer + index, bufferLength);
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
		protected abstract void Encrypt(byte* buffer, int bufferLength);

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
			if (bufferLength < 0 || bufferLength > buffer.Length) {
				throw ArgumentOutOfRangeUtils.OutsideRange(nameof(bufferLength), bufferLength, 0,
					$"{nameof(buffer)}.{nameof(buffer.Length)}", buffer.Length, true, true);
			}
			fixed (byte* pBuffer = buffer)
				Decrypt(pBuffer, bufferLength);
		}
		/// <summary>
		///  Decrypts the buffer with the specified length and starting index.
		/// </summary>
		/// <param name="buffer">The buffer to decrypt.</param>
		/// <param name="index">The index to start decrypting the buffer at.</param>
		/// <param name="bufferLength">The buffer length which must be a multiple of 8.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="index"/> or <paramref name="bufferLength"/> is less than zero.-or-
		///  <paramref name="index"/> + <paramref name="bufferLength"/> is greater than the length of
		///  <paramref name="buffer"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="bufferLength"/> is not a multiple of 8.
		/// </exception>
		public void Decrypt(byte[] buffer, int index, int bufferLength) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(index), index, 0, true);
			if (bufferLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(bufferLength), bufferLength, 0, true);
			if (index + bufferLength > buffer.Length) {
				throw ArgumentOutOfRangeUtils.OutsideRange($"{nameof(index)} + {nameof(bufferLength)}",
					index + bufferLength, 0, $"{nameof(buffer)}.{nameof(Array.Length)}", buffer.Length, true, false);
			}
			fixed (byte* pBuffer = buffer)
				Decrypt(pBuffer + index, bufferLength);
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
		protected abstract void Decrypt(byte* buffer, int bufferLength);

		#endregion
		
		#region Initialize

		/// <summary>
		///  Constructs the encryption sieve. This must be called during construction of the blowfish class.
		/// </summary>
		/// <param name="key">The pointer to the key to initialize.</param>
		/// <param name="keyLength">The length of the key pointed to.</param>
		protected abstract void Initialize(byte* key, int keyLength);

		#endregion

		#region Protected Helpers

		/// <summary>
		///  Get the output length, which must be even MOD 8.
		/// </summary>
		/// <param name="inputLength">The original length of the buffer size.</param>
		/// <returns>The input length padded to MOD 8.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int GetOutputLength(int inputLength) => MathUtils.Pad(inputLength, 8);

		#endregion
	}
}
