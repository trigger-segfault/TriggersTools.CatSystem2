using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region ExtractHg2Internal

		private static HgxImage ExtractHg2Internal(HGXHDR hdr, BinaryReader reader, string fileName, string outputDir,
			HgxOptions options)
		{
			Stream stream = reader.BaseStream;
			
			List<Hg2FrameInfo> frameInfos = new List<Hg2FrameInfo>();
			Hg2FrameInfo frameInfo = null;
			long startPosition = stream.Position;

			do {
				stream.Position = startPosition + (frameInfo?.Img.OffsetNext ?? 0);
				startPosition = stream.Position;

				frameInfo = ReadHg2FrameInfo(reader, hdr, false);
				frameInfos.Add(frameInfo);
			} while (frameInfo.Img.OffsetNext != 0);

			HgxImage hg2Image = new HgxImage(Path.GetFileName(fileName), hdr, frameInfos.ToArray(), options);
			if (outputDir != null) {
				for (int imgIndex = 0; imgIndex < frameInfos.Count; imgIndex++) {
					frameInfo = frameInfos[imgIndex];
					string pngFile = hg2Image.Frames[imgIndex].GetFrameFilePath(outputDir, false);
					ExtractHg2Image(reader, frameInfo, options, pngFile);
				}
			}

			return hg2Image;
		}

		private static Hg2FrameInfo ReadHg2FrameInfo(BinaryReader reader, HGXHDR hdr, bool frameOnly) {
			Stream stream = reader.BaseStream;
			long frameOffset = reader.BaseStream.Position;

			HG2IMG img = reader.ReadUnmanaged<HG2IMG>();
			HG2IMG_BASE? imgBase = null;
			if (hdr.Type == 0x25) {
				if (frameOnly)
					stream.Position += HG2IMG_BASE.CbSize;
				else
					imgBase = reader.ReadUnmanaged<HG2IMG_BASE>();
			}

			return new Hg2FrameInfo(reader.BaseStream, img, imgBase, frameOffset);
		}

		private static void ExtractHg2ImageFromFrame(Stream stream, HgxFrame frame, string pngFile, HgxOptions options) {
			BinaryReader reader = new BinaryReader(stream);
			stream.Position = frame.FrameOffset;

			HGXHDR hdr = new HGXHDR { Type = frame.HgxImage.Type };
			Hg2FrameInfo frameInfo = ReadHg2FrameInfo(reader, hdr, true);

			ExtractHg2Image(reader, frameInfo, options, pngFile);
		}

		#endregion

		#region ExtractHg2Image

		/// <summary>
		///  Extracts the <see cref="HG2IMG"/> from the HG-2 file.
		/// </summary>
		/// <param name="reader">The binary reader for the file.</param>
		/// <param name="std">The HG3STDINFO containing image dimensions, etc.</param>
		/// <param name="img">The image header used to process the image.</param>
		/// <param name="expand">True if the image should be expanded to its full size.</param>
		/// <param name="pngFile">The path to the PNG file to save to.</param>
		private static void ExtractHg2Image(BinaryReader reader, Hg2FrameInfo frameInfo, HgxOptions options,
			string pngFile)
		{
			HG2IMG img = frameInfo.Img;
			reader.BaseStream.Position = frameInfo.Offset;
			int depthBytes = (img.DepthBits + 7) / 8;
			int stride = (img.Width * depthBytes + 3) & ~3;

			byte[] pixelBuffer = ProcessImage(reader, img.Width, img.Height, img.DepthBits, img.Data);

			if (!CatDebug.SpeedTestHgx) {
				// This image type is normally flipped, so reverse the option
				options ^= HgxOptions.Flip;
				WritePng(pixelBuffer, img, options, pngFile);
			}
		}

		#endregion
	}
}
