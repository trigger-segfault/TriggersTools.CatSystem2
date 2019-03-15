using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.CatSystem2.Utils;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region ExtractHg3Internal

		private static HgxImage ExtractHg3Internal(HGXHDR hdr, BinaryReader reader, string fileName, string outputDir,
			HgxOptions options)
		{
			Stream stream = reader.BaseStream;
			
			List<Hg3FrameInfo> frameInfos = new List<Hg3FrameInfo>();
			Hg3FrameInfo frameInfo = null;
			HG3FRAMEHDR frameHdr = new HG3FRAMEHDR();
			long startPosition = stream.Position;

			do {
				stream.Position = startPosition + frameHdr.OffsetNext;

				// NEW-NEW METHOD: We now know the next offset ahead
				// of time from the HG3OFFSET we're going to read.
				// Usually skips 0 bytes, otherwise usually 1-7 bytes.
				//long startPosition = stream.Position;
				startPosition = stream.Position;
				frameHdr = reader.ReadUnmanaged<HG3FRAMEHDR>();

				HG3TAG tag = reader.ReadUnmanaged<HG3TAG>();
				if (!HG3STDINFO.HasTagSignature(tag.Signature))
					throw new Exception("Expected \"stdinfo\" tag!");

				HG3STDINFO stdInfo = reader.ReadUnmanaged<HG3STDINFO>();
				frameInfos.Add(frameInfo = new Hg3FrameInfo(frameHdr, stdInfo));
				HG3TAG previousTag;
				while (tag.OffsetNext != 0) {
					tag = reader.ReadUnmanaged<HG3TAG>();

					long position = stream.Position;

					string signature = tag.Signature;

					if (HG3IMG.HasTagSignature(signature, out int imdId)) { // "img####"
						HG3IMG img = reader.ReadUnmanaged<HG3IMG>();
						frameInfo.AddTagImg(stream, tag, img, imdId);
					}
					else if (HG3IMG_AL.HasTagSignature(signature)) { // "img_al"
						HG3IMG_AL img = reader.ReadUnmanaged<HG3IMG_AL>();
						frameInfo.AddTagImg(stream, tag, img, 0);
					}
					else if (HG3IMG_JPG.HasTagSignature(signature)) { // "img_jpg"
						// There is no image info, reading it would increment by one byte which we don't want
						frameInfo.AddTagImg(stream, tag, new HG3IMG_JPG(), 0);
					}
					else if (HG3ATS.HasTagSignature(signature, out int atsId)) { // "ats####"
						HG3ATS ats = reader.ReadUnmanaged<HG3ATS>();
						frameInfo.Ats.Add(atsId, ats);
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
					previousTag = tag;

					stream.Position = position + tag.Length;
				}
			} while (frameHdr.OffsetNext != 0);

			HgxImage hg3Image = new HgxImage(Path.GetFileName(fileName), hdr, frameInfos.ToArray(), options);
			if (outputDir != null) {
				for (int imgIndex = 0; imgIndex < frameInfos.Count; imgIndex++) {
					frameInfo = frameInfos[imgIndex];
					string pngFile = hg3Image.Frames[imgIndex].GetFrameFilePath(outputDir, false);
					switch (frameInfo.Type) {
					case Hg3ImageType.Image:
						ExtractHg3Image(reader, frameInfo, options, pngFile);
						break;
					case Hg3ImageType.Jpeg:
						ExtractHg3ImageJpeg(reader, frameInfo, options, pngFile);
						break;
					case Hg3ImageType.Alpha:
						ExtractHg3ImageAlpha(reader, frameInfo, options, pngFile);
						break;
					case Hg3ImageType.JpegAlpha:
						ExtractHg3ImageJpegAlpha(reader, frameInfo, options, pngFile);
						break;
					}
				}
			}
			
			return hg3Image;
		}

		#endregion

		#region ExtractHg3Image

		/// <summary>
		///  Extracts the <see cref="HG3IMG"/> from the HG-3 file.
		/// </summary>
		/// <param name="reader">The binary reader for the file.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="img">The image header used to process the image.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the PNG file to save to.</param>
		private static void ExtractHg3Image(BinaryReader reader, Hg3FrameInfo frameInfo, HgxOptions options,
			string pngFile)
		{
			HG3STDINFO std = frameInfo.StdInfo;
			HG3IMG img = frameInfo.Img.Data;
			reader.BaseStream.Position = frameInfo.Img.Offset;
			
			byte[] pixelBuffer = ProcessImage(reader, std.Width, std.Height, std.DepthBits, img.Data);

			if (!CatDebug.SpeedTestHgx) {
				// This image type is normally flipped, so reverse the option
				options ^= HgxOptions.Flip;
				WritePng(pixelBuffer, std, options, pngFile);
			}
		}
		private static void ExtractHg3ImageJpeg(BinaryReader reader, Hg3FrameInfo frameInfo, HgxOptions options,
			string pngFile)
		{
			HG3STDINFO std = frameInfo.StdInfo;
			HG3TAG tag = frameInfo.ImgJpg.Tag;
			reader.BaseStream.Position = frameInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);

			if (!CatDebug.SpeedTestHgx)
				WriteJpegToPng(buffer, std, options, pngFile);
		}
		private static void ExtractHg3ImageAlpha(BinaryReader reader, Hg3FrameInfo frameInfo, HgxOptions options,
			string pngFile)
		{
			HG3STDINFO std = frameInfo.StdInfo;
			HG3IMG_AL img = frameInfo.ImgAl.Data;
			reader.BaseStream.Position = frameInfo.ImgAl.Offset;

			byte[] alphaBuffer = Zlib.Decompress(reader, img.CompressedLength, img.DecompressedLength);

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
					byte alpha = unchecked((byte) (byte.MaxValue - alphaBuffer[alphaIndex]));
					pixelBuffer[pixelIndex + 0] = alpha;
					pixelBuffer[pixelIndex + 1] = alpha;
					pixelBuffer[pixelIndex + 2] = alpha;
					if (depthBytes == 4)
						pixelBuffer[pixelIndex + 3] = byte.MaxValue;
				}
			}

			if (!CatDebug.SpeedTestHgx)
				WritePng(pixelBuffer, std, options, pngFile);
		}
		private static void ExtractHg3ImageJpegAlpha(BinaryReader reader, Hg3FrameInfo frameInfo, HgxOptions options,
			string pngFile)
		{
			HG3STDINFO std = frameInfo.StdInfo;
			HG3IMG_AL img = frameInfo.ImgAl.Data;
			HG3TAG tag = frameInfo.ImgJpg.Tag;
			reader.BaseStream.Position = frameInfo.ImgAl.Offset;

			byte[] alphaBuffer = Zlib.Decompress(reader, img.CompressedLength, img.DecompressedLength);

			reader.BaseStream.Position = frameInfo.ImgJpg.Offset;

			byte[] buffer = reader.ReadBytes(tag.Length);

			if (!CatDebug.SpeedTestHgx)
				WriteJpegAlphaMaskToPng(buffer, alphaBuffer, std, options, pngFile);
		}

		#endregion
	}
}
