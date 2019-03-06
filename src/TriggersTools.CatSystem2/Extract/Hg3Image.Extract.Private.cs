using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using TriggersTools.CatSystem2.Native;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class Hg3Image {
		#region ExtractHg3
		
		private static Hg3Image ExtractHg3Internal(HGXHDR hdr, BinaryReader reader, string fileName, string outputDir,
			bool expand)
		{
			Stream stream = reader.BaseStream;
			
			List<Hg3FrameInfo> frameInfos = new List<Hg3FrameInfo>();
			Hg3FrameInfo frameInfo = null;
			HG3FRAMEHDR frameHdr;

			do {
				// NEW-NEW METHOD: We now know the next offset ahead
				// of time from the HG3OFFSET we're going to read.
				// Usually skips 0 bytes, otherwise usually 1-7 bytes.
				long startPosition = stream.Position;
				frameHdr = reader.ReadUnmanaged<HG3FRAMEHDR>();

				HG3TAG tag = reader.ReadUnmanaged<HG3TAG>();
				if (!HG3STDINFO.HasTagSignature(tag.Signature))
					throw new Exception("Expected \"stdinfo\" tag!");

				HG3STDINFO stdInfo = reader.ReadUnmanaged<HG3STDINFO>();
				frameInfos.Add(frameInfo = new Hg3FrameInfo(frameHdr, stdInfo));
				
				while (tag.OffsetNext != 0) {
					tag = reader.ReadUnmanaged<HG3TAG>();

					long position = stream.Position;
					int number;

					string signature = tag.Signature;
					Debug.WriteLine(tag);

					if (HG3IMG.HasTagSignature(signature, out number)) { // "img####"
						HG3IMG img = reader.ReadUnmanaged<HG3IMG>();
						frameInfo.AddAtom(stream, tag, img);
					}
					else if (HG3IMG_AL.HasTagSignature(signature)) { // "img_al"
						HG3IMG_AL img = reader.ReadUnmanaged<HG3IMG_AL>();
						frameInfo.AddAtom(stream, tag, img);
					}
					else if (HG3IMG_JPG.HasTagSignature(signature)) { // "img_jpg"
						// There is no atom data, reading it would increment by one byte which we don't want
						frameInfo.AddAtom(stream, tag, new HG3IMG_JPG());
					}
					else if (HG3ATS.HasTagSignature(signature, out number)) { // "ats####"
						HG3ATS ats = reader.ReadUnmanaged<HG3ATS>();
						frameInfo.Ats.Add(number, ats);
					}
					else if (HG3CPTYPE.HasTagSignature(signature)) { // "cptype"
						HG3CPTYPE cpType = reader.ReadUnmanaged<HG3CPTYPE>();
						frameInfo.CpType = cpType;
					}
					else if (HG3IMGMODE.HasTagSignature(signature)) { // "imgmode"
						HG3IMGMODE imgMode = reader.ReadUnmanaged<HG3IMGMODE>();
						frameInfo.ImgMode = imgMode;
					}
					else {
						Trace.WriteLine($"UNKNOWN TAG: \"{signature}\"");
					}

					stream.Position = position + tag.Length;
				}

				if (frameHdr.OffsetNext == 0)
					break; // End of stream

				stream.Position = startPosition + frameHdr.OffsetNext;
			} while (frameHdr.OffsetNext != 0);

			Hg3Image hg3Image = new Hg3Image(Path.GetFileName(fileName), hdr, frameInfos.ToArray(), expand);
			if (outputDir != null) {
				for (int imgIndex = 0; imgIndex < frameInfos.Count; imgIndex++) {
					frameInfo = frameInfos[imgIndex];
					string name = $"{Path.GetFileNameWithoutExtension(fileName)}+{frameInfo.Header.Id:D4}.png";
					string pngFile = Path.Combine(outputDir, name);
					switch (frameInfo.Type) {
					case Hg3ImageType.Image:
						ExtractImage(reader, frameInfo, expand, pngFile);
						break;
					case Hg3ImageType.Jpeg:
						ExtractImageJpeg(reader, frameInfo, expand, pngFile);
						break;
					case Hg3ImageType.Alpha:
						ExtractImageAlpha(reader, frameInfo, expand, pngFile);
						break;
					case Hg3ImageType.JpegAlpha:
						ExtractImageJpegAlpha(reader, frameInfo, expand, pngFile);
						break;
					}
				}
			}
			
			return hg3Image;
		}

		#endregion

		#region ExtractBitmap

		private static byte[] ExtractImagePixelBuffer(BinaryReader reader, int width, int height, int depthBits,
			HGXIMGDATA data)
		{
			int depthBytes = (depthBits + 7) / 8;
			int stride = (width * depthBytes + 3) / 4 * 4;
			byte[] bufferTmp = reader.ReadBytes(data.DataLength);
			byte[] cmdBufferTmp = reader.ReadBytes(data.CmdLength);
			byte[] buffer = new byte[data.OriginalDataLength];
			byte[] cmdBuffer = new byte[data.OriginalCmdLength];
			Zlib.Uncompress(buffer, ref data.OriginalDataLength, bufferTmp, data.DataLength);
			Zlib.Uncompress(cmdBuffer, ref data.OriginalCmdLength, cmdBufferTmp, data.CmdLength);

#if !NATIVE_UNDELTAFILTER
			// Perform heavy processing that's faster in native code
			Asmodean.ProcessImageNative(
				buffer/*Tmp*/,
				//data.DataLength,
				data.OriginalDataLength,
				cmdBuffer/*Tmp*/,
				//data.CmdLength,
				data.OriginalCmdLength,
				out IntPtr pRgbaBuffer,
				out int rgbaLength,
				width,
				height,
				depthBytes);
			
			//Marshal.FreeHGlobal(pRgbaBuffer);
			//return new byte[0];
			return FlipBufferAndDispose(pRgbaBuffer, stride, height);
#else
			byte[] rgbaBuffer = ProcessImageInternal(
				buffer/*Tmp*/,
				//data.DataLength,
				//data.OriginalDataLength,
				cmdBuffer/*Tmp*/,
				//data.CmdLength,
				//data.OriginalCmdLength,
				//out byte[] rgbaBuffer,
				//out int rgbaLength,
				width,
				height,
				depthBytes);

			//return new byte[0];
			return FlipBufferAndDispose(rgbaBuffer, stride, height);
#endif
		}

		/// <summary>
		///  Extracts the <see cref="HG3IMG"/> from the HG-3 file.
		/// </summary>
		/// <param name="reader">The binary reader for the file.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="img">The image header used to process the image.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the PNG file to save to.</param>
		private static void ExtractImage(BinaryReader reader, Hg3FrameInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG img = atomInfo.Img.Atom;
			reader.BaseStream.Position = atomInfo.Img.Offset;
			
			byte[] pixelBuffer = ExtractImagePixelBuffer(reader, std.Width, std.Height, std.DepthBits, img.Data);
			
			WritePng(pixelBuffer, std, expand, pngFile);
		}
		private static void ExtractImageJpeg(BinaryReader reader, Hg3FrameInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3TAG tag = atomInfo.ImgJpg.Tag;
			reader.BaseStream.Position = atomInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);

			WriteJpegToPng(buffer, std, expand, pngFile);
		}
		private static void ExtractImageAlpha(BinaryReader reader, Hg3FrameInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG_AL img = atomInfo.ImgAl.Atom;
			reader.BaseStream.Position = atomInfo.ImgAl.Offset;

			byte[] compressed = reader.ReadBytes(img.Length);
			byte[] decompressed = new byte[img.OriginalLength];
			
			Zlib.Uncompress(decompressed, ref img.OriginalLength, compressed, img.Length);

			// This decompressed image does not contain the required stride padding, let's add it

			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (std.Width * depthBytes + 3) / 4 * 4;
			int minStride = (std.Width * depthBytes);
			int alphaStride = std.Width;
			
			byte[] pixelBuffer = new byte[stride * std.Height];
			for (int y = 0; y < std.Height; y++) {
				int src = y * alphaStride;
				int dst = y * stride;
				for (int x = 0; x < std.Width; x++) {
					int alphaIndex = src + x;
					int pixelIndex = dst + x * depthBytes;
					byte alpha = unchecked((byte) (byte.MaxValue - decompressed[alphaIndex]));
					pixelBuffer[pixelIndex + 0] = alpha;
					pixelBuffer[pixelIndex + 1] = alpha;
					pixelBuffer[pixelIndex + 2] = alpha;
					if (depthBytes == 4)
						pixelBuffer[pixelIndex + 3] = byte.MaxValue;
				}
			}

			WritePng(pixelBuffer, std, expand, pngFile);
		}
		private static void ExtractImageJpegAlpha(BinaryReader reader, Hg3FrameInfo atomInfo, bool expand,
			string pngFile)
		{
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG_AL img = atomInfo.ImgAl.Atom;
			HG3TAG tag = atomInfo.ImgJpg.Tag;
			reader.BaseStream.Position = atomInfo.ImgAl.Offset;

			byte[] compressed = reader.ReadBytes(img.Length);
			byte[] decompressed = new byte[img.OriginalLength];

			Zlib.Uncompress(decompressed, ref img.OriginalLength, compressed, img.Length);

			reader.BaseStream.Position = atomInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);
			
			WriteJpegAlphaMaskToPng(buffer, decompressed, std, expand, pngFile);
		}
		private static byte[] FlipBufferAndDispose(IntPtr pRgbaBuffer, int stride, int height) {
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
		}
		private static byte[] FlipBufferAndDispose(byte[] rgbaBuffer, int stride, int height) {
			// Vertically flip the buffer so its in the correct setup to load into Bitmap
			byte[] pixelBuffer = new byte[stride * height];
			for (int y = 0; y < height; y++) {
				int src = y * stride;
				int dst = (height - (y + 1)) * stride;
				Buffer.BlockCopy(rgbaBuffer, src, pixelBuffer, dst, stride);
			}
			return pixelBuffer;
		}
		/// <summary>
		///  Writes the bitmap buffer to <paramref name="pngFile"/> and optional performs expansion if
		///  <paramref name="expand"/> is true.
		/// </summary>
		/// <param name="buffer">The buffer to the image bits.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the file to save to.</param>
		private static void WritePng(byte[] buffer, HG3STDINFO std, bool expand, string pngFile) {
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				IntPtr scan0 = handle.AddrOfPinnedObject();
				int depthBytes = (std.DepthBits + 7) / 8;
				int stride = (std.Width * depthBytes + 3) / 4 * 4;
				PixelFormat format, expandFormat = PixelFormat.Format32bppArgb;
				switch (std.DepthBits) {
				case 24: format = PixelFormat.Format24bppRgb; break;
				case 32: format = PixelFormat.Format32bppArgb; break;
				default: throw new Exception($"Unsupported depth bits {std.DepthBits}!");
				}
				// Do expansion here, and up to 32 bits if not 32 bits already.
				if (expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight)) {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0))
					using (var bitmapExpand = new Bitmap(std.TotalWidth, std.TotalHeight, expandFormat))
					using (Graphics g = Graphics.FromImage(bitmapExpand)) {
						g.DrawImageUnscaled(bitmap, std.OffsetX, std.OffsetY);
						bitmapExpand.Save(pngFile, ImageFormat.Png);
					}
				}
				else {
					using (var bitmap = new Bitmap(std.Width, std.Height, stride, format, scan0))
						bitmap.Save(pngFile, ImageFormat.Png);
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
		private static void WriteJpegToPng(byte[] buffer, HG3STDINFO std, bool expand, string pngFile) {
			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (std.Width * depthBytes + 3) / 4 * 4;
			PixelFormat expandFormat = PixelFormat.Format32bppArgb;
			// Do expansion here, and up to 32 bits if not 32 bits already.
			if (expand && (std.Width != std.TotalWidth || std.Height != std.TotalHeight)) {
				using (var ms = new MemoryStream(buffer))
				using (var jpeg = (Bitmap) Image.FromStream(ms))
				using (var bitmapExpand = new Bitmap(std.TotalWidth, std.TotalHeight, expandFormat))
				using (Graphics g = Graphics.FromImage(bitmapExpand)) {
					g.DrawImageUnscaled(jpeg, std.OffsetX, std.OffsetY);
					bitmapExpand.Save(pngFile, ImageFormat.Png);
				}
			}
			else {
				using (var ms = new MemoryStream(buffer))
				using (var jpeg = (Bitmap) Image.FromStream(ms))
					jpeg.Save(pngFile, ImageFormat.Png);
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
		private static unsafe void WriteJpegAlphaMaskToPng(byte[] buffer, byte[] alpha, HG3STDINFO std, bool expand,
			string pngFile)
		{
			int offsetX = (expand ? std.OffsetX : 0);
			int offsetY = (expand ? std.OffsetY : 0);
			int width   = (expand ? std.TotalWidth  : std.Width);
			int height  = (expand ? std.TotalHeight : std.Height);

			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (expand ? std.TotalWidth : std.Width) * 4;
			int alphaStride = std.Width;
			BitmapData jpgData = null, bmpData = null;
			using (var ms = new MemoryStream(buffer))
			using (var jpeg = (Bitmap) Image.FromStream(ms))
			using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb)) {
				try {
					Rectangle rect = new Rectangle(0, 0, std.Width, std.Height);
					jpgData = jpeg.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					int bufferSize = height * stride;
					void* pJpg = jpgData.Scan0.ToPointer();
					byte* pBmp = (byte*) bmpData.Scan0.ToPointer();
					// Copy over the jpeg pixels first
					Buffer.MemoryCopy(pJpg, pBmp, bufferSize, bufferSize);

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
					bitmap.Save(pngFile, ImageFormat.Png);
				} finally {
					if (jpgData != null)
						jpeg.UnlockBits(jpgData);
					if (bmpData != null)
						bitmap.UnlockBits(bmpData);
				}
			}
		}

		#endregion
	}
}
