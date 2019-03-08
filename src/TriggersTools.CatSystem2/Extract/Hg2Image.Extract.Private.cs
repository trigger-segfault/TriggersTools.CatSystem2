using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class HgxImage {
		#region ExtractHg2

		private static HgxImage ExtractHg2Internal(HGXHDR hdr, BinaryReader reader, string fileName, string outputDir,
			HgxOptions options)
		{
			Stream stream = reader.BaseStream;
			List<Hg2FrameInfo> frameInfos = new List<Hg2FrameInfo>();
			Hg2FrameInfo frameInfo = null;

			bool hasBase = hdr.Type == 0x25;

			HG2IMG img;
			HG2IMG_BASE? imgBase;

			do {
				long startPosition = stream.Position;
				img = reader.ReadUnmanaged<HG2IMG>();
				if (hasBase)
					imgBase = reader.ReadUnmanaged<HG2IMG_BASE>();
				else
					imgBase = null;

				frameInfos.Add(new Hg2FrameInfo(stream, img, imgBase));

				stream.Position = startPosition + img.OffsetNext;
			} while (img.OffsetNext != 0);

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
			int stride = (img.Width * depthBytes + 3) / 4 * 4;

			byte[] pixelBuffer = ProcessImage(reader, img.Width, img.Height, img.DepthBits, img.Data);

			if (!CatUtils.SpeedTestHgx) {
				// This image type is normally flipped, so reverse the option
				options ^= HgxOptions.Flip;
				WritePng(pixelBuffer, img, options, pngFile);
			}
		}

		#endregion
	}
}
