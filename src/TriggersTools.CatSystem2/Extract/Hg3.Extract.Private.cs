using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TriggersTools.CatSystem2.Native;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class Hg3 {
		#region Extract

		private static Hg3 Extract(Stream stream, string fileName, string directory, bool saveFrames, bool expand) {
			BinaryReader reader = new BinaryReader(stream);
			HG3HDR hdr = reader.ReadUnmanaged<HG3HDR>();

			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, HG3HDR.ExpectedSignature);

			//int backtrack = Marshal.SizeOf<HG3TAG>() - 1;
			List<KeyValuePair<HG3STDINFO, List<long>>> imageOffsets = new List<KeyValuePair<HG3STDINFO, List<long>>>();

			for (int i = 0; ; i++) {

				// NEW-NEW METHOD: We now know the next offset ahead
				// of time from the HG3OFFSET we're going to read.
				// Usually skips 0 bytes, otherwise usually 1-7 bytes.
				long startPosition = stream.Position;
				HG3OFFSET offset = reader.ReadUnmanaged<HG3OFFSET>();

				HG3TAG tag = reader.ReadUnmanaged<HG3TAG>();
				if (!HG3STDINFO.HasTagSignature(tag.Signature))
					throw new Exception("Expected \"stdinfo\" tag!");

				// NEW METHOD: Keep searching for the next stdinfo
				// This way we don't miss any images
				/*int offset = 0;
				while (!tag.Signature.StartsWith("stdinfo")) {
					if (stream.IsEndOfStream())
						break;
					stream.Position -= backtrack;
					tag = reader.ReadStruct<HG3TAG>();
					offset++;
				}
				if (stream.IsEndOfStream())
					break;*/

				// OLD METHOD: Missed entries in a few files
				//if (!tag.signature.StartsWith(StdInfoSignature))
				//	break;

				HG3STDINFO stdInfo = reader.ReadUnmanaged<HG3STDINFO>();

				List<long> frameOffsets = new List<long>();
				imageOffsets.Add(new KeyValuePair<HG3STDINFO, List<long>>(stdInfo, frameOffsets));

				while (tag.OffsetNext != 0) {
					tag = reader.ReadUnmanaged<HG3TAG>();

					string signature = tag.Signature;
					if (HG3IMG.HasTagSignature(signature)) { // "img####"
						frameOffsets.Add(stream.Position);
						// Skip this tag
						stream.Position += tag.Length;
					}
					/*else if (HG3ATS.HasTagSignature(signature)) { // "ats####"
						// Skip this tag
						stream.Position += tag.Length;
					}
					else if (HG3CPTYPE.HasTagSignature(signature)) { // "cptype"
						// Skip this tag
						stream.Position += tag.Length;
					}
					else if (HG3IMG_AL.HasTagSignature(signature)) { // "img_al"
						// Skip this tag
						stream.Position += tag.Length;
					}
					else if (HG3IMG_JPG.HasTagSignature(signature)) { // "img_jpg"
						// Skip this tag
						stream.Position += tag.Length;
					}
					else if (HG3IMGMODE.HasTagSignature(signature)) { // "imgmode"
						// Skip this tag
						stream.Position += tag.Length;
					}*/
					else {
						// Skip this unknown tag
						stream.Position += tag.Length;
					}
				}

				if (offset.OffsetNext == 0)
					break; // End of stream

				stream.Position = startPosition + offset.OffsetNext;
			}

			HG3STDINFO[] stdInfos = imageOffsets.Select(p => p.Key).ToArray();
			long[][] allFrameOffsets = imageOffsets.Select(p => p.Value.ToArray()).ToArray();
			Hg3 hg3 = new Hg3(Path.GetFileName(fileName), hdr, stdInfos, allFrameOffsets, saveFrames && expand);
			// Save any frames after we've located them all.
			// This way we truely know if something is an animation.
			if (saveFrames) {
				for (int imgIndex = 0; imgIndex < hg3.Count; imgIndex++) {
					HG3STDINFO stdInfo = stdInfos[imgIndex];
					Hg3Image hg3Image = hg3.Images[imgIndex];
					for (int frmIndex = 0; frmIndex < hg3Image.FrameCount; frmIndex++) {
						stream.Position = hg3Image.FrameOffsets[frmIndex];
						HG3IMG imghdr = reader.ReadUnmanaged<HG3IMG>();
						string pngFile = hg3.GetFrameFilePath(directory, imgIndex, frmIndex);
						ExtractImage(reader, stdInfo, imghdr, expand, pngFile);
					}
				}
			}

			return hg3;
		}

		#endregion

		#region Extract
		
		public static Hg3 ExtractAllTags(string hg3File) {
			using (Stream stream = File.OpenRead(hg3File))
				return ExtractAllTags(stream, hg3File, null, false, false);
		}
		public static Hg3 ExtractAllTagsAndImages(string hg3File, string directory, bool saveFrames, bool expand) {
			using (Stream stream = File.OpenRead(hg3File))
				return ExtractAllTags(stream, hg3File, directory, true, false);
		}
		public static Hg3 ExtractAllTags(Stream stream, string fileName, string directory, bool saveFrames,
			bool expand)
		{
			BinaryReader reader = new BinaryReader(stream);
			HG3HDR hdr = reader.ReadUnmanaged<HG3HDR>();

			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, HG3HDR.ExpectedSignature);

			//int backtrack = Marshal.SizeOf<HG3TAG>() - 1;
			//List<KeyValuePair<HG3STDINFO, List<long>>> imageOffsets = new List<KeyValuePair<HG3STDINFO, List<long>>>();
			List<Hg3ImageInfo> atomInfos = new List<Hg3ImageInfo>();
			Hg3ImageInfo atomInfo = null;

			for (int i = 0; ; i++) {

				// NEW-NEW METHOD: We now know the next offset ahead
				// of time from the HG3OFFSET we're going to read.
				// Usually skips 0 bytes, otherwise usually 1-7 bytes.
				long startPosition = stream.Position;
				HG3OFFSET offset = reader.ReadUnmanaged<HG3OFFSET>();

				HG3TAG tag = reader.ReadUnmanaged<HG3TAG>();
				if (!HG3STDINFO.HasTagSignature(tag.Signature))
					throw new Exception("Expected \"stdinfo\" tag!");

				HG3STDINFO stdInfo = reader.ReadUnmanaged<HG3STDINFO>();
				atomInfos.Add(atomInfo = new Hg3ImageInfo(stdInfo));

				void AddAtom<THg3Atom>(THg3Atom atom) {
					if (atomInfo.HasAtom(atom))
						atomInfos.Add(atomInfo = new Hg3ImageInfo(stdInfo));
					atomInfo.AddAtom(stream, tag, atom);
				}

				//List<long> frameOffsets = new List<long>();
				//imageOffsets.Add(new KeyValuePair<HG3STDINFO, List<long>>(stdInfo, frameOffsets));
				
				while (tag.OffsetNext != 0) {
					tag = reader.ReadUnmanaged<HG3TAG>();

					long position = stream.Position;

					string signature = tag.Signature;
					
					if (HG3IMG.HasTagSignature(signature)) { // "img####"
						HG3IMG img = reader.ReadUnmanaged<HG3IMG>();
						AddAtom(img);
					}
					else if (HG3IMG_AL.HasTagSignature(signature)) { // "img_al"
						HG3IMG_AL img = reader.ReadUnmanaged<HG3IMG_AL>();
						AddAtom(img);
					}
					else if (HG3IMG_JPG.HasTagSignature(signature)) { // "img_jpg"
						// There is no atom data, reading it would increment by one byte which we don't want
						AddAtom(new HG3IMG_JPG());
					}
					else if (HG3ATS.HasTagSignature(signature)) { // "ats####"
						HG3ATS ats = reader.ReadUnmanaged<HG3ATS>();
						atomInfo.Ats.Add(ats);
					}
					else if (HG3CPTYPE.HasTagSignature(signature)) { // "cptype"
						HG3CPTYPE cpType = reader.ReadUnmanaged<HG3CPTYPE>();
						atomInfo.CpType = cpType;
					}
					else if (HG3IMGMODE.HasTagSignature(signature)) { // "imgmode"
						HG3IMGMODE imgMode = reader.ReadUnmanaged<HG3IMGMODE>();
						atomInfo.ImgMode = imgMode;
					}
					else {
						Trace.WriteLine($"UNKNOWN TAG: \"{signature}\"");
						//info.Add(null);
						// Skip this unknown tag
						//stream.Position += tag.Length;
					}

					stream.Position = position + tag.Length;
				}

				if (offset.OffsetNext == 0)
					break; // End of stream

				stream.Position = startPosition + offset.OffsetNext;
			}

			HG3STDINFO[] stdInfos = atomInfos.Select(info => info.StdInfo).ToArray();
			//long[][] allFrameOffsets = imageOffsets.Select(p => p.Value.ToArray()).ToArray();
			Hg3 hg3 = new Hg3(Path.GetFileName(fileName), hdr, stdInfos, false);// saveFrames && expand);
			// Save any frames after we've located them all.
			// This way we truely know if something is an animation.
			if (saveFrames) {
				for (int imgIndex = 0; imgIndex < atomInfos.Count; imgIndex++) {
					atomInfo = atomInfos[imgIndex];
					string pngFile = hg3.GetFrameFilePath(directory, imgIndex, 0);
					switch (atomInfo.Type) {
					case Hg3ImageType.Image:
						ExtractImage(reader, atomInfo, expand, pngFile);
						break;
					case Hg3ImageType.Jpeg:
						ExtractImageJpeg(reader, atomInfo, expand, pngFile);
						break;
					case Hg3ImageType.Alpha:
						ExtractImageAlpha(reader, atomInfo, expand, pngFile);
						break;
					case Hg3ImageType.JpegAlpha:
						ExtractImageJpegAlpha(reader, atomInfo, expand, pngFile);
						break;
					}
				}
			}
			
			return hg3;
		}

		#endregion

		#region ExtractBitmap

		private static void ExtractImage(BinaryReader reader, HG3STDINFO std, HG3IMG img, bool expand, string pngFile) {
			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (std.Width * depthBytes + 3) / 4 * 4;
			//int minStride = (std.Width * depthBytes);

			byte[] bufferTmp = reader.ReadBytes(img.DataLength);
			byte[] cmdBufferTmp = reader.ReadBytes(img.CmdLength);
			byte[] pixelBuffer;

			// Perform heavy processing that's faster in native code
			Asmodean.ProcessImageNative(
				bufferTmp,
				img.DataLength,
				img.OriginalDataLength,
				cmdBufferTmp,
				img.CmdLength,
				img.OriginalCmdLength,
				out IntPtr pRgbaBuffer,
				out int rgbaLength,
				std.Width,
				std.Height,
				depthBytes);

			pixelBuffer = FlipBufferAndDispose(pRgbaBuffer, rgbaLength, stride, std.Height);
			try {
				// Vertically flip the buffer so its in the correct setup to load into Bitmap
				pixelBuffer = new byte[rgbaLength];
				for (int y = 0; y < std.Height; y++) {
					int src = y * stride;
					int dst = (std.Height - (y + 1)) * stride;
					Marshal.Copy(pRgbaBuffer + src, pixelBuffer, dst, stride);
				}
			} finally {
				Marshal.FreeHGlobal(pRgbaBuffer);
			}

			WritePng(pixelBuffer, std, expand, pngFile);
		}

		/// <summary>
		///  Extracts the <see cref="HG3IMG"/> from the HG-3 file.
		/// </summary>
		/// <param name="reader">The binary reader for the file.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="img">The image header used to process the image.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the PNG file to save to.</param>
		private static void ExtractImage(BinaryReader reader, Hg3ImageInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG img = atomInfo.Img.Atom;
			reader.BaseStream.Position = atomInfo.Img.Offset;

			int depthBytes = (std.DepthBits + 7) / 8;
			int stride = (std.Width * depthBytes + 3) / 4 * 4;

			byte[] bufferTmp = reader.ReadBytes(img.DataLength);
			byte[] cmdBufferTmp = reader.ReadBytes(img.CmdLength);
			byte[] pixelBuffer;

			// Perform heavy processing that's faster in native code
			Asmodean.ProcessImageNative(
				bufferTmp,
				img.DataLength,
				img.OriginalDataLength,
				cmdBufferTmp,
				img.CmdLength,
				img.OriginalCmdLength,
				out IntPtr pRgbaBuffer,
				out int rgbaLength,
				std.Width,
				std.Height,
				depthBytes);

			pixelBuffer = FlipBufferAndDispose(pRgbaBuffer, rgbaLength, stride, std.Height);
			try {
				// Vertically flip the buffer so its in the correct setup to load into Bitmap
				pixelBuffer = new byte[rgbaLength];
				for (int y = 0; y < std.Height; y++) {
					int src = y * stride;
					int dst = (std.Height - (y + 1)) * stride;
					Marshal.Copy(pRgbaBuffer + src, pixelBuffer, dst, stride);
				}
			} finally {
				Marshal.FreeHGlobal(pRgbaBuffer);
			}

			WritePng(pixelBuffer, std, expand, pngFile);
		}
		private static void ExtractImageJpeg(BinaryReader reader, Hg3ImageInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3TAG tag = atomInfo.ImgJpg.Tag;
			reader.BaseStream.Position = atomInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);

			WriteJpegToPng(buffer, std, expand, pngFile);
		}
		private static void ExtractImageAlpha(BinaryReader reader, Hg3ImageInfo atomInfo, bool expand, string pngFile) {
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG_AL img = atomInfo.ImgAl.Atom;
			reader.BaseStream.Position = atomInfo.ImgAl.Offset;

			byte[] compressed = reader.ReadBytes(img.Length);
			byte[] decompressed = new byte[img.OriginalLength];
			
			ZLib1.Uncompress(decompressed, ref img.OriginalLength, compressed, img.Length);

			// This decompressed image does not containg the required stride padding, let's add it

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
		private static void ExtractImageJpegAlpha(BinaryReader reader, Hg3ImageInfo atomInfo, bool expand,
			string pngFile)
		{
			HG3STDINFO std = atomInfo.StdInfo;
			HG3IMG_AL img = atomInfo.ImgAl.Atom;
			HG3TAG tag = atomInfo.ImgJpg.Tag;
			reader.BaseStream.Position = atomInfo.ImgAl.Offset;

			byte[] compressed = reader.ReadBytes(img.Length);
			byte[] decompressed = new byte[img.OriginalLength];

			ZLib1.Uncompress(decompressed, ref img.OriginalLength, compressed, img.Length);

			reader.BaseStream.Position = atomInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);
			
			WriteJpegAlphaMaskToPng(buffer, decompressed, std, expand, pngFile);
		}
		private static byte[] FlipBufferAndDispose(IntPtr pRgbaBuffer, int rgbaLength, int stride, int height) {
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
				// Once this handle is freed, the bitmap loaded from
				// scan0 will be invalidated after garbage collection.
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
					//bitmap.Dispose();
				}
			}
		}

		#endregion
	}
}
