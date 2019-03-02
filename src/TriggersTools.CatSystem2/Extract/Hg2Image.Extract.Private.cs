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
	partial class Hg3Image {
		#region ExtractHg2

		public static Hg3Image ExtractInternal(Stream stream, string fileName, string outputDir, bool expand) {
			BinaryReader reader = new BinaryReader(stream);
			HGXHDR hdr = reader.ReadUnmanaged<HGXHDR>();

			if (hdr.Signature == HGXHDR.ExpectedHg3Signature)
				return ExtractHg3Internal(hdr, reader, fileName, outputDir, expand);
			if (hdr.Signature == HGXHDR.ExpectedHg2Signature)
				return ExtractHg2Internal(hdr, reader, fileName, outputDir, expand);
			throw new UnexpectedFileTypeException($"{HGXHDR.ExpectedHg2Signature} or {HGXHDR.ExpectedHg3Signature}");
		}

		private static Hg3Image ExtractHg2Internal(HGXHDR hdr, BinaryReader reader, string fileName, string outputDir,
			bool expand)
		{
			Stream stream = reader.BaseStream;
			//int backtrack = Marshal.SizeOf<HG3TAG>() - 1;
			//List<KeyValuePair<HG3STDINFO, List<long>>> imageOffsets = new List<KeyValuePair<HG3STDINFO, List<long>>>();
			List<Hg2FrameInfo> frameInfos = new List<Hg2FrameInfo>();
			Hg2FrameInfo frameInfo = null;
			//HG3FRAMEHDR frameHdr;

			bool ex = hdr.Type == 0x25;

			HG2IMG img;

			do {
				long startPosition = stream.Position;
				img = reader.ReadUnmanaged<HG2IMG>();
				frameInfo = new Hg2FrameInfo(img);
				if (ex)
					frameInfo.ImgEx = reader.ReadUnmanaged<HG2IMG_EX>();
				frameInfos.Add(frameInfo);
				//stream.Skip(img.Data.DataLength);
				//stream.Skip(img.Data.CmdLength);
				stream.Position = startPosition + img.OffsetNext;
			} while (img.OffsetNext != 0);

			/*Hg3Image hg3Image = new Hg3Image(Path.GetFileName(fileName), hdr, frameInfos.ToArray(), expand);
			if (outputDir != null) {
				for (int imgIndex = 0; imgIndex < frameInfos.Count; imgIndex++) {
					frameInfo = frameInfos[imgIndex];
					string pngFile = $"{Path.GetFileNameWithoutExtension(fileName)}_{frameInfo.Header.Id:D4}.png";
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
			}*/

			return null;
		}

		#endregion
	}
}
