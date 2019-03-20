using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using TriggersTools.CatSystem2.Exceptions;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;
using ReturnCode = TriggersTools.CatSystem2.Utils.Asmodean.ReturnCode;

#if NET451
using Buffer = TriggersTools.CatSystem2.Utils.BufferMemoryCopy;
#endif

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region ExtractInternal

		private static HgxImage ExtractInternal(Stream stream, string fileName, string outputDir, HgxOptions options) {
			BinaryReader reader = new BinaryReader(stream);
			HGXHDR hdr = reader.ReadUnmanaged<HGXHDR>();

			try {
				if (hdr.Signature == HGXHDR.ExpectedHg3Signature)
					return ExtractHg3Internal(hdr, reader, fileName, outputDir, options);
				if (hdr.Signature == HGXHDR.ExpectedHg2Signature)
					return ExtractHg2Internal(hdr, reader, fileName, outputDir, options);
			} catch (ArgumentNullException) {
				// This exception shouldn't be triggered by failure to read the file, so
				// we'll throw it as an error as itself because it's not supposed to happen.
				throw;
			} catch (NullReferenceException) {
				// This exception shouldn't be triggered by failure to read the file, so
				// we'll throw it as an error as itself because it's not supposed to happen.
				throw;
			} catch (Exception ex) {
				throw new HgxException(ex);
			} /*catch (EndOfStreamException ex) {
				throw new HgxException(ex);
			}*/
			throw new UnexpectedFileTypeException($"{HGXHDR.ExpectedHg2Signature} or {HGXHDR.ExpectedHg3Signature}");
		}

		#endregion

		#region ThrowHelpers

		private static void ThrowIfUnsupportedDepthBytes(int depthBits) {
			if (depthBits != 24 && depthBits != 32)
				throw new HgxException($"Unsupported depthBytes! must be either 24 or 32, got {depthBits}.");
		}
		private static void ThrowIfInvalidDimensions(int width, int height) {
			if (width < 0)
				throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("width", width, 0, false));
			if (height < 0)
				throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("height", height, 0, false));
		}
		private static void ThrowIfDimensionsTooLarge(int height, int stride) {
			if (stride * height > Asmodean.MaxRgbaLength)
				throw new HgxException("Image dimensions are too large!");
		}

		#endregion

		#region ProcessImage

		private static byte[] ProcessImage(BinaryReader reader, int width, int height, int depthBits,
			HGXIMGDATA data)
		{
			if (data.CompressedDataLength <= 0) {
				throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin(nameof(data.CompressedDataLength),
					data.CompressedDataLength, 0, false));
			}

			int depthBytes = (depthBits + 7) / 8;
			int stride = (width * depthBytes + 3) & ~3;
			byte[] dataBuffer = Zlib.Decompress(reader, data.CompressedDataLength, data.DecompressedDataLength);
			byte[] cmdBuffer = Zlib.Decompress(reader, data.CompressedCmdLength, data.DecompressedCmdLength);
			
			if (depthBits != 24 && depthBits != 32)
				throw new HgxException($"Unsupported depthBits! must be either 24 or 32, got {depthBits}.");
			if (width <= 0)
				throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("width", width, 0, false));
			if (height <= 0)
				throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("height", height, 0, false));
			if (stride * height > Asmodean.MaxRgbaLength)
				throw new HgxException("Image dimensions are too large!");

			//ThrowIfUnsupportedDepthBytes(depthBits);
			//ThrowIfInvalidDimensions(width, height);
			//ThrowIfDimensionsTooLarge(height, stride);

			byte[] rgbaBuffer;

			if (CatDebug.SpeedTestHgx)
				CatDebug.HgxWatch.Start();

			if (!CatDebug.NativeHgx) {
				rgbaBuffer = ProcessImageManaged(
					dataBuffer,
					cmdBuffer,
					width,
					height,
					depthBytes,
					stride);
			}
			else {
				// Minimum unrle buffer size is 1024 bytes
				rgbaBuffer = new byte[Math.Max(stride * height, 1024)];
				// Perform heavy processing that's faster in native code
				ReturnCode result = Asmodean.ProcessImageNative(
					dataBuffer,
					dataBuffer.Length,
					cmdBuffer,
					cmdBuffer.Length,
					rgbaBuffer,
					rgbaBuffer.Length,
					width,
					height,
					depthBytes,
					stride);

				switch (result) {
				case ReturnCode.Success:
					break;

				case ReturnCode.UnrleDataIsCorrupt:
					throw new HgxException("dataBuffer, cmdBuffer, or unrleBuffer ran out of data!");
				case ReturnCode.DataBufferTooSmall:
					throw new HgxException("dataBuffer is too small!");
				case ReturnCode.CmdBufferTooSmall:
					throw new HgxException("cmdBuffer is too small!");
				case ReturnCode.RgbaBufferTooSmall:
					throw new HgxException("rgbaBuffer is too small!");
				case ReturnCode.UnrleBufferTooSmall:
					throw new HgxException("unrleBuffer is too small!");

				case ReturnCode.DimensionsTooLarge:
					throw new HgxException("Image dimensions are too large!");
				case ReturnCode.InvalidDimensions:
					if (width <= 0)
						throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("width", width, 0, false));
					else
						throw new HgxException(ArgumentOutOfRangeUtils.OutsideMin("height", height, 0, false));
				case ReturnCode.InvalidDepthBytes:
					throw new HgxException($"Unsupported depthBits! must be either 24 or 32, got {depthBits}.");

				case ReturnCode.DataBufferIsNull:
					throw new ArgumentNullException("dataBuffer");
				case ReturnCode.CmdBufferIsNull:
					throw new ArgumentNullException("cmdBuffer");
				case ReturnCode.RgbaBufferIsNull:
					throw new ArgumentNullException("rgbaBuffer");

				case ReturnCode.AllocationFailed:
					throw new HgxException("A memory allocation while processing the HG-X image failed!");

				default:
					throw new HgxException($"Unexpected return code {result}!");
				}
			}

			if (CatDebug.SpeedTestHgx)
				CatDebug.HgxWatch.Stop();

			return rgbaBuffer;
		}

		#endregion

		#region Buffer

		/*private static byte[] CopyFlipBufferAndDispose(IntPtr pRgbaBuffer, int stride, int height) {
			try {
				// Vertically flip the buffer so its in the correct setup to load into Bitmap
				byte[] pixelBuffer = new byte[stride * height];
				for (int y = 0; y < height; y++) {
					int src = y * stride;
					int dst = (height - (y + 1)) * stride;
					Marshal.Copy(pRgbaBuffer + src, pixelBuffer, dst, stride);
				}
				return pixelBuffer;
			} finally {
				Marshal.FreeHGlobal(pRgbaBuffer);
			}
		}*/
		/*private static byte[] PrepareBufferAndDispose(IntPtr pRgbaBuffer, int stride, int height, bool flip) {
			try {
				byte[] pixelBuffer = new byte[stride * height];
				if (!flip) {
					Marshal.Copy(pRgbaBuffer, pixelBuffer, 0, stride * height);
				}
				else {
					// Vertically flip the buffer so its in the correct setup to load into Bitmap
					for (int y = 0; y < height; y++) {
						int src = y * stride;
						int dst = (height - (y + 1)) * stride;
						Marshal.Copy(pRgbaBuffer + src, pixelBuffer, dst, stride);
					}
				}
				return pixelBuffer;
			} finally {
				Marshal.FreeHGlobal(pRgbaBuffer);
			}
		}*/
		/*private static void FlipBuffer(byte[] rgbaBuffer, int width, int height, int depthBits, bool flip) {
			if (!flip)
				return;
			int depthBytes = (depthBits + 7) / 8;
			int stride = (width * depthBytes + 3) & ~3;

			// Vertically flip the buffer so its in the correct setup to load into Bitmap
			byte[] strideBuffer = new byte[stride];
			// No issue if height is odd and we skip the middle line, we'd be swapping it with itself.
			for (int y = 0; y < height / 2; y++) {
				int src = y * stride;
				int dst = (height - (y + 1)) * stride;
				// Copy src to swap stride
				Buffer.BlockCopy(rgbaBuffer, src, strideBuffer, 0, stride);
				// Swap src with dst stride
				Buffer.BlockCopy(rgbaBuffer, dst, rgbaBuffer, src, stride);
				// Swap dst with swap stride
				Buffer.BlockCopy(strideBuffer, 0, rgbaBuffer, dst, stride);
			}
		}*/

		#endregion

		#region ExtractBitmap

		/// <summary>
		///  Writes the bitmap buffer to <paramref name="pngFile"/> and optional performs expansion if
		///  <paramref name="expand"/> is true.
		/// </summary>
		/// <param name="buffer">The buffer to the image bits.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the file to save to.</param>
		private static void WritePng(byte[] buffer, HG3STDINFO std, HgxOptions options, string pngFile) {
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr scan0 = handle.AddrOfPinnedObject();
				int depthBytes = (std.DepthBits + 7) / 8;
				int stride = (std.Width * depthBytes + 3) & ~3;
				PixelFormat format, expandFormat = PixelFormat.Format32bppArgb;
				switch (std.DepthBits) {
				case 24: format = PixelFormat.Format24bppRgb; break;
				case 32: format = PixelFormat.Format32bppArgb; break;
				default: throw new HgxException($"Unsupported depth bits {std.DepthBits}!");
				}
				// Do expansion here, and up to 32 bits if not 32 bits already.
				bool expand = options.HasFlag(HgxOptions.Expand);
				if (expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight)) {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0))
					using (var bitmapExpand = new Bitmap(std.TotalWidth, std.TotalHeight, expandFormat))
					using (Graphics g = Graphics.FromImage(bitmapExpand)) {
						if (options.HasFlag(HgxOptions.Flip))
							bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
						g.DrawImageUnscaled(bitmap, std.OffsetX, std.OffsetY);
						bitmapExpand.Save(pngFile, ImageFormat.Png);
					}
				}
				else {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0)) {
						if (options.HasFlag(HgxOptions.Flip))
							bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
						bitmap.Save(pngFile, ImageFormat.Png);
					}
				}

			} finally {
				// Thing to note that gave me headaches earlier:
				//  Once this handle is freed, the bitmap loaded from
				//  scan0 will be invalidated after garbage collection.
				handle.Free();
			}
		}
		/// <summary>
		///  Writes the bitmap buffer to <paramref name="pngFile"/> and optional performs expansion if
		///  <paramref name="expand"/> is true.
		/// </summary>
		/// <param name="buffer">The buffer to the image bits.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the file to save to.</param>
		private static void WritePng(byte[] buffer, HG2IMG std, HgxOptions options, string pngFile) {
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr scan0 = handle.AddrOfPinnedObject();
				int depthBytes = (std.DepthBits + 7) / 8;
				int stride = (std.Width * depthBytes + 3) & ~3;
				PixelFormat format, expandFormat = PixelFormat.Format32bppArgb;
				switch (std.DepthBits) {
				case 24: format = PixelFormat.Format24bppRgb; break;
				case 32: format = PixelFormat.Format32bppArgb; break;
				default: throw new HgxException($"Unsupported depth bits {std.DepthBits}!");
				}
				// Do expansion here, and up to 32 bits if not 32 bits already.
				bool expand = options.HasFlag(HgxOptions.Expand);
				if (expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight)) {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0))
					using (var bitmapExpand = new Bitmap(std.TotalWidth, std.TotalHeight, expandFormat))
					using (Graphics g = Graphics.FromImage(bitmapExpand)) {
						if (options.HasFlag(HgxOptions.Flip))
							bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
						g.DrawImageUnscaled(bitmap, std.OffsetX, std.OffsetY);
						bitmapExpand.Save(pngFile, ImageFormat.Png);
					}
				}
				else {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0)) {
						if (options.HasFlag(HgxOptions.Flip))
							bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
						bitmap.Save(pngFile, ImageFormat.Png);
					}
				}

			} finally {
				// Thing to note that gave me headaches earlier:
				//  Once this handle is freed, the bitmap loaded from
				//  scan0 will be invalidated after garbage collection.
				handle.Free();
			}
		}
		/// <summary>
		///  Writes the bitmap buffer to <paramref name="pngFile"/> and optional performs expansion if
		///  <paramref name="expand"/> is true.
		/// </summary>
		/// <param name="buffer">The buffer to the image bits.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the file to save to.</param>
		private static void WriteJpegToPng(byte[] buffer, HG3STDINFO std, HgxOptions options, string pngFile) {
			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (std.Width * depthBytes + 3) & ~3;
			PixelFormat expandFormat = PixelFormat.Format32bppArgb;
			// Do expansion here, and up to 32 bits if not 32 bits already.
			bool expand = options.HasFlag(HgxOptions.Expand);
			if (expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight)) {
				using (var ms = new MemoryStream(buffer))
				using (var jpeg = (Bitmap) Image.FromStream(ms))
				using (var bitmapExpand = new Bitmap(std.TotalWidth, std.TotalHeight, expandFormat))
				using (Graphics g = Graphics.FromImage(bitmapExpand)) {
					if (options.HasFlag(HgxOptions.Flip))
						jpeg.RotateFlip(RotateFlipType.RotateNoneFlipY);
					g.DrawImageUnscaled(jpeg, std.OffsetX, std.OffsetY);
					bitmapExpand.Save(pngFile, ImageFormat.Png);
				}
			}
			else {
				using (var ms = new MemoryStream(buffer))
				using (var jpeg = (Bitmap) Image.FromStream(ms)) {
					if (options.HasFlag(HgxOptions.Flip))
						jpeg.RotateFlip(RotateFlipType.RotateNoneFlipY);
					jpeg.Save(pngFile, ImageFormat.Png);
				}
			}
		}
		/// <summary>
		///  Writes the bitmap buffer to <paramref name="pngFile"/> and optional performs expansion if
		///  <paramref name="expand"/> is true.
		/// </summary>
		/// <param name="buffer">The buffer to the image bits.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the file to save to.</param>
		private static unsafe void WriteJpegAlphaMaskToPng(byte[] buffer, byte[] alpha, HG3STDINFO std,
			HgxOptions options, string pngFile)
		{
			bool expand = options.HasFlag(HgxOptions.Expand);
			expand = expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight);
			int offsetX = (expand ? std.OffsetX : 0);
			int offsetY = (expand ? std.OffsetY : 0);
			int width   = (expand ? std.TotalWidth  : std.Width);
			int height  = (expand ? std.TotalHeight : std.Height);

			int depthBytes = 4;
			int jpgStride = std.Width * depthBytes;
			int stride = width * depthBytes;
			int bufferSize = height * stride;
			int alphaStride = std.Width;
			BitmapData jpgData = null, bmpData = null;
			using (var ms = new MemoryStream(buffer))
			using (var jpeg = (Bitmap) Image.FromStream(ms))
			using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb)) {
				try {
					Rectangle rect = new Rectangle(0, 0, std.Width, std.Height);
					jpgData = jpeg.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					byte* pJpg = (byte*) jpgData.Scan0.ToPointer();
					byte* pBmp = (byte*) bmpData.Scan0.ToPointer();

					// Copy over the jpeg pixels first
					if (expand) {
						for (int y = 0; y < std.Height; y++) {
							int src = y * jpgStride;
							int dst = (y + offsetY) * stride + offsetX * depthBytes;
							Buffer.MemoryCopy(pJpg + src, pBmp + dst, bufferSize, jpgStride);
						}
					}
					else {
						Buffer.MemoryCopy(pJpg, pBmp, bufferSize, bufferSize);
					}

					// Now apply the alpha to the pixels
					for (int y = 0; y < std.Height; y++) {
						int src = y * alphaStride;
						int dst = (y + offsetY) * stride;
						for (int x = 0; x < std.Width; x++) {
							int alphaIndex = src + x;
							int pixelIndex = dst + (x + offsetX) * depthBytes;

							pBmp[pixelIndex + 3] = alpha[alphaIndex];
						}
					}
				} finally {
					if (jpgData != null)
						jpeg.UnlockBits(jpgData);
					if (bmpData != null)
						bitmap.UnlockBits(bmpData);
				}
				if (options.HasFlag(HgxOptions.Flip))
					bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
				bitmap.Save(pngFile, ImageFormat.Png);
			}
		}

		#endregion
	}
}
