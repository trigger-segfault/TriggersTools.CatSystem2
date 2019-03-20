using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Exceptions;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2.Utils {
	/// <summary>
	///  A result type that is returned from native Zlib compress and decompress functions.
	///  This result states the error that occurred.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	enum ZResult : int {
		OK = 0,
		StreamEnd = 1,
		NeedDictionary = 1,
		IOError = -1,
		StreamError = -2,
		DataError = -3,
		MemoryError = -4,
		BufferError = -5,
		VersionError = -6,
	}
	/// <summary>
	///  An exception thrown during Zlib compression and decompression.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	class ZlibException : Exception {

		/// <summary>
		///  The <see cref="ZResult"/> that was returned from the Zlib function.
		/// </summary>
		public ZResult Result { get; }
		/// <summary>
		///  The input buffer used in the Zlib function. Null if <see cref="Result"/> is <see cref="ZResult.OK"/>.
		/// </summary>
		public byte[] InputBuffer { get; }
		/// <summary>
		///  The resuling buffer used in the Zlib function. Null if <see cref="Result"/> is <see cref="ZResult.OK"/>.
		/// </summary>
		public byte[] OutputBuffer { get; }

		public ZlibException(string message) : base(message) { }
		public ZlibException(string message, Exception innerException) : base(message, innerException) { }
		internal ZlibException(ZResult result, bool compress, byte[] input, byte[] output)
			: base(ResultMessage(result, compress))
		{
			Result = result;
			InputBuffer = input;
			OutputBuffer = output;
		}

		private static string ResultMessage(ZResult result, bool compress) {
			if (compress) {
				switch (result) {
				case ZResult.MemoryError: return "There was not enough memory to complete the Zlib compression!";
				case ZResult.BufferError: return "The Zlib compressed destination buffer was not large enough!";
				}
			}
			else {
				switch (result) {
				case ZResult.MemoryError: return "There was not enough memory to complete the Zlib decompression!";
				case ZResult.BufferError: return "The Zlib decompressed destination buffer was not large enough!";
				case ZResult.DataError: return "The Zlib compression data is corrupted or incomplete!";
				}
			}
			return $"Unhandled Zlib error status {result}!";
		}
	}

	/// <summary>
	///  A static class for zlib1.dll native methods.
	/// </summary>
#if CAT_DEBUG
	public
#else
	internal
#endif
	static class Zlib {
		#region Static Constructors
		
		static Zlib() {
			string arch = (Environment.Is64BitProcess ? "x64" : "x86");
			string path = Path.Combine(CatUtils.NativeDllExtractPath, arch);
			Directory.CreateDirectory(path);

			// Load the embedded zlib1 dll
			string ResPath = $"zlib1.{arch}.dll";
			string dllPath = Path.Combine(path, "zlib1.dll");
			Embedded.LoadNativeDll(ResPath, dllPath);
		}

		#endregion
		
		#region Native

		/// <summary>
		///  Returns nn upper bound on the compressed size after Compress() on <see cref="srcLength"/> bytes.
		/// </summary>
		/// <param name="srcLength">The lenght of the decompressed data.</param>
		/// <returns>
		///  An upper bound on the compressed size after calling Compress() on <see cref="srcLength"/> bytes.
		/// </returns>
		[DllImport("zlib1.dll", EntryPoint = "compressBound", CallingConvention = CallingConvention.Cdecl)]
		public extern static int CompressBoundNative(
			int srcLength);

		[DllImport("zlib1.dll", EntryPoint = "compress", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I4)]
		private extern static ZResult CompressNative(
			byte[] dst,
			ref int dstLength,
			byte[] src,
			int srcLength);
		[DllImport("zlib1.dll", EntryPoint = "uncompress", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I4)]
		private extern static ZResult UncompressNative(
			byte[] dst,
			ref int dstLength,
			byte[] src,
			int srcLength);


		#endregion
		
		#region CompressedLength

		/// <summary>
		///  Gets the upper bounds of compressed data length of the decompressed data length.
		/// </summary>
		/// <param name="decompressedLength">The length of the decompressed data.</param>
		/// <returns>The upper bounds of compressed data length.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int CompressedBounds(int decompressedLength) {
			if (decompressedLength < 0)
				throw new ZlibException($"{nameof(decompressedLength)} is less than zero!");
			int compressedLength = CompressBoundNative(decompressedLength);
			if (compressedLength < 0) {
				throw new ZlibException($"Compressed length of {nameof(decompressedLength)} is greater " +
					$"than {nameof(Int32)}.{nameof(int.MaxValue)}!");
			}
			return compressedLength;
		}
		/// <summary>
		///  Gets the compressed length of the decompressed data.
		/// </summary>
		/// <param name="decompressed">The decompressed data.</param>
		/// <returns>The length of the compressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int CompressedLength(byte[] decompressed) {
			return CompressedLength(decompressed, decompressed?.Length ?? 0);
		}
		/// <summary>
		///  Gets the compressed length of the decompressed data and length.
		/// </summary>
		/// <param name="decompressed">The decompressed data.</param>
		/// <param name="decompressedLength">The actual length of the decompressed data.</param>
		/// <returns>The length of the compressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int CompressedLength(byte[] decompressed, int decompressedLength) {
			if (decompressed == null)
				throw new ArgumentNullException(nameof(decompressed));
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);

			int compressedLength = CompressedBounds(decompressedLength);
			byte[] compressed = new byte[compressedLength];
			ZResult result = CompressNative(compressed, ref compressedLength, decompressed, decompressedLength);
			if (result != ZResult.OK)// && result != ZResult.BufferError)
				throw new ZlibException(result, true, decompressed, compressed);

			if (compressedLength < 0) {
				throw new ZlibException($"Compressed length of {nameof(decompressedLength)} is greater " +
					$"than {nameof(Int32)}.{nameof(int.MaxValue)}!");
			}
			return compressedLength;
		}

		#endregion

		#region DecompressedLength

		/// <summary>
		///  Gets the decompressed length of the compressed data.
		/// </summary>
		/// <param name="compressed">The compressed data.</param>
		/// <returns>The length of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int DecompressedLength(byte[] compressed) {
			return DecompressedLength(compressed, compressed?.Length ?? 0);
		}
		/// <summary>
		///  Gets the decompressed length of the compressed data and length.
		/// </summary>
		/// <param name="compressed">The compressed data.</param>
		/// <param name="compressedLength">The actual length of the compressed data.</param>
		/// <returns>The length of the compressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int DecompressedLength(byte[] compressed, int compressedLength) {
			if (compressed == null)
				throw new ArgumentNullException(nameof(compressed));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);

			int decompressedLength = 0;
			ZResult result = UncompressNative(new byte[0], ref decompressedLength, compressed, compressedLength);
			if (result != ZResult.OK && result != ZResult.BufferError)
				throw new ZlibException(result, false, compressed, new byte[0]);

			if (decompressedLength < 0) {
				throw new ZlibException($"Decompressed length of {nameof(compressedLength)} is greater " +
					$"than {nameof(Int32)}.{nameof(int.MaxValue)}!");
			}
			return decompressedLength;
		}

		#endregion

		#region Compress (byte[])

		/// <summary>
		///  Gets the compressed data of the decompressed data.
		/// </summary>
		/// <param name="decompressed">The decompressed data.</param>
		/// <returns>The compressed data of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Compress(byte[] decompressed) {
			return Compress(decompressed, decompressed?.Length ?? 0);
		}
		/// <summary>
		///  Gets the compressed data of the decompressed data and length.
		/// </summary>
		/// <param name="decompressed">The decompressed data.</param>
		/// <param name="decompressedLength">The actual length of the decompressed data.</param>
		/// <returns>The compressed data of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Compress(byte[] decompressed, int decompressedLength) {
			int compressedLength = CompressedLength(decompressed, decompressedLength);
			byte[] compressed = new byte[decompressedLength];
			Compress(compressed, ref compressedLength, decompressed, decompressedLength);
			return compressed;
		}
		/// <summary>
		///  Compresses the decompressed data to the output compression buffer.
		/// </summary>
		/// <param name="compressed">The compression buffer.</param>
		/// <param name="decompressed">The decompressed data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Compress(byte[] compressed, byte[] decompressed) {
			int compressedLength = compressed?.Length ?? 0;
			Compress(compressed, ref compressedLength, decompressed, decompressed?.Length ?? 0);
		}
		/// <summary>
		///  Compresses the decompressed data and length to the output compression buffer and length.
		/// </summary>
		/// <param name="compressed">The compression buffer.</param>
		/// <param name="compressedLength">The actual length of the compression buffer.</param>
		/// <param name="decompressed">The decompressed data.</param>
		/// <param name="decompressedLength">The actual length of the decompressed data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> or <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Compress(byte[] compressed, ref int compressedLength, byte[] decompressed,
			int decompressedLength)
		{
			if (compressed == null)
				throw new ArgumentNullException(nameof(compressed));
			if (decompressed == null)
				throw new ArgumentNullException(nameof(decompressed));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);

			ZResult result = CompressNative(compressed, ref compressedLength, decompressed, decompressedLength);
			if (result != ZResult.OK)
				throw new ZlibException(result, true, decompressed, compressed);
		}

		#endregion

		#region Compress (Stream)

		/// <summary>
		///  Compresses the decompressed data and writes the compressed data to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="decompressed">The decompressed data to compress.</param>
		/// <param name="decompressedLength">The length of the decompressed data.</param>
		/// <returns>The actual compressed length of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int Compress(BinaryWriter writer, byte[] decompressed) {
			return Compress(writer, decompressed, decompressed?.Length ?? 0);
		}
		/// <summary>
		///  Compresses the decompressed data and writes the compressed data to the output stream.
		/// </summary>
		/// <param name="stream">The output <see cref="Stream"/> to read with.</param>
		/// <param name="decompressed">The decompressed data to compress.</param>
		/// <param name="decompressedLength">The length of the decompressed data.</param>
		/// <returns>The actual compressed length of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static int Compress(Stream stream, byte[] decompressed) {
			return Compress(stream, decompressed, decompressed?.Length ?? 0);
		}
		/// <summary>
		///  Compresses the decompressed data and length and writes the compressed data to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write with.</param>
		/// <param name="decompressed">The decompressed data to compress.</param>
		/// <param name="decompressedLength">The length of the decompressed data to read.</param>
		/// <param name="compressedLength">The length of the compressed data.</param>
		/// <returns>The actual compressed length of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="writer"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred.
		/// </exception>
		public static int Compress(BinaryWriter writer, byte[] decompressed, int decompressedLength) {
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (decompressed == null)
				throw new ArgumentNullException(nameof(decompressed));
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);
			int compressedLength = CompressedBounds(decompressedLength);
			byte[] compressed = new byte[compressedLength];
			Compress(compressed, ref compressedLength, decompressed, decompressedLength);
			writer.Write(compressed, 0, compressedLength);
			return compressed.Length;
		}
		/// <summary>
		///  Compresses the decompressed data and length and writes the compressed data to the output stream.
		/// </summary>
		/// <param name="stream">The output <see cref="Stream"/> to read with.</param>
		/// <param name="decompressed">The decompressed data to compress.</param>
		/// <param name="decompressedLength">The length of the decompressed data to read.</param>
		/// <param name="compressedLength">The length of the compressed data.</param>
		/// <returns>The actual compressed length of the decompressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> or <paramref name="decompressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred.
		/// </exception>
		public static int Compress(Stream stream, byte[] decompressed, int decompressedLength) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (decompressed == null)
				throw new ArgumentNullException(nameof(decompressed));
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);
			int compressedLength = CompressedBounds(decompressedLength);
			byte[] compressed = new byte[compressedLength];
			Compress(compressed, ref compressedLength, decompressed, decompressedLength);
			stream.Write(compressed, 0, compressedLength);
			return decompressedLength;
		}

		#endregion

		#region Decompress (byte[])

		/// <summary>
		///  Gets the decompressed data of the compressed data.
		/// </summary>
		/// <param name="compressed">The compressed data.</param>
		/// <returns>The decompressed data of the compressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Decompress(byte[] compressed) {
			return Decompress(compressed, compressed?.Length ?? 0);
		}
		/// <summary>
		///  Gets the decompressed data of the compressed data and length.
		/// </summary>
		/// <param name="compressed">The compressed data.</param>
		/// <param name="compressedLength">The actual length of the compressed data.</param>
		/// <returns>The decompressed data of the compressed data.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Decompress(byte[] compressed, int compressedLength) {
			int decompressedLength = DecompressedLength(compressed, compressedLength);
			byte[] decompressed = new byte[decompressedLength];
			Decompress(decompressed, ref decompressedLength, compressed, compressedLength);
			return decompressed;
		}
		/// <summary>
		///  Decompresses the compressed data to the output decompression buffer.
		/// </summary>
		/// <param name="decompressed">The decompression buffer.</param>
		/// <param name="compressed">The compressed data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> or <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Decompress(byte[] dst, byte[] src) {
			int decompressedLength = dst?.Length ?? 0;
			Decompress(dst, ref decompressedLength, src, src?.Length ?? 0);
		}
		/// <summary>
		///  Decompresses the compressed data and length to the output decompression buffer and length.
		/// </summary>
		/// <param name="decompressed">The decompression buffer.</param>
		/// <param name="decompressedLength">The actual length of the decompression buffer.</param>
		/// <param name="compressed">The compressed data.</param>
		/// <param name="compressedLength">The actual length of the compressed data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="decompressed"/> or <paramref name="compressed"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="decompressedLength"/> or <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static void Decompress(byte[] decompressed, ref int decompressedLength, byte[] compressed,
			int compressedLength)
		{
			if (decompressed == null)
				throw new ArgumentNullException(nameof(decompressed));
			if (compressed == null)
				throw new ArgumentNullException(nameof(compressed));
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);

			ZResult result = UncompressNative(decompressed, ref decompressedLength, compressed, compressedLength);
			if (result != ZResult.OK)
				throw new ZlibException(result, false, compressed, decompressed);
		}

		#endregion

		#region Decompress (Stream)

		/// <summary>
		///  Reads the compressed data from the binary reader and returns the decompressed data.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="compressedLength">The length of the compressed data to read.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Decompress(BinaryReader reader, int compressedLength) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);
			return Decompress(reader.ReadBytes(compressedLength));
		}
		/// <summary>
		///  Reads the compressed data from the input stream and returns the decompressed data.
		/// </summary>
		/// <param name="stream">The input <see cref="Stream"/> to read with.</param>
		/// <param name="compressedLength">The length of the compressed data to read.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred, or the resulting length was greater than <see cref="int.MaxValue"/>.
		/// </exception>
		public static byte[] Decompress(Stream stream, int compressedLength) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);
			byte[] compressed = new byte[compressedLength];
			stream.Read(compressed, 0, compressedLength);
			return Decompress(compressed);
		}
		/// <summary>
		///  Reads the compressed data from the binary reader and returns the decompressed data.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read with.</param>
		/// <param name="compressedLength">The length of the compressed data to read.</param>
		/// <param name="decompressedLength">The length of the decompression data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> or <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred.
		/// </exception>
		public static byte[] Decompress(BinaryReader reader, int compressedLength, int decompressedLength) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);
			byte[] compressed = reader.ReadBytes(compressedLength);
			byte[] decompressed = new byte[decompressedLength];
			Decompress(decompressed, ref decompressedLength, compressed, compressedLength);
			return decompressed;
		}
		/// <summary>
		///  Reads the compressed data from the input stream and returns the decompressed data.
		/// </summary>
		/// <param name="stream">The input <see cref="Stream"/> to read with.</param>
		/// <param name="compressedLength">The length of the compressed data to read.</param>
		/// <param name="decompressedLength">The length of the decompression data.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="reader"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="compressedLength"/> or <paramref name="decompressedLength"/> is less than zero.
		/// </exception>
		/// <exception cref="ZlibException">
		///  A Zlib error occurred.
		/// </exception>
		public static byte[] Decompress(Stream stream, int compressedLength, int decompressedLength) {
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (compressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(compressedLength), compressedLength, 0, true);
			if (decompressedLength < 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(decompressedLength), decompressedLength, 0, true);
			byte[] compressed = new byte[compressedLength];
			stream.Read(compressed, 0, compressedLength);
			byte[] decompressed = new byte[decompressedLength];
			Decompress(decompressed, ref decompressedLength, compressed, compressedLength);
			return decompressed;
		}

		#endregion
	}
}
