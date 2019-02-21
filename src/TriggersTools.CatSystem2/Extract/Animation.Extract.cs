using System;
using System.IO;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class Animation {
		#region Extract

		/// <summary>
		///  Extracts the ANM animation from an ANM file.
		/// </summary>
		/// <param name="anmFile">The path to the ANM file to extract.</param>
		/// <returns>The extracted ANM animation.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="anmFile"/> is null.
		/// </exception>
		public static Animation Extract(string anmFile) {
			using (var stream = File.OpenRead(anmFile))
				return Extract(stream, anmFile);
		}
		/// <summary>
		///  Extracts the ANM animation from an ANM file stream.
		/// </summary>
		/// <param name="stream">The stream to extract the ANM from.</param>
		/// <param name="fileName">The path or name of the ANM file being extracted.</param>
		/// <returns>The extracted ANM animation.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="stream"/> or <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  The <paramref name="stream"/> is closed.
		/// </exception>
		public static Animation Extract(Stream stream, string fileName) {
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			BinaryReader reader = new BinaryReader(stream);

			ANMHDR hdr = reader.ReadUnmanaged<ANMHDR>();
			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, ANMHDR.ExpectedSignature);
			
			reader.ReadBytes(20); // Unused (?)

			ANMFRAME[] frames = new ANMFRAME[hdr.FrameCount];
			for (int i = 0; i < hdr.FrameCount; i++) {
				frames[i] = reader.ReadUnmanaged<ANMFRAME>();
			}

			return new Animation(Path.GetFileName(fileName), hdr, frames);
		}

		#endregion
	}
	partial class KifintEntry {
		#region ExtractAnimation

		/// <summary>
		///  Extracts the ANM animation script from the entry.
		/// </summary>
		/// <returns>The extracted animation script.</returns>
		public Animation ExtractAnimation(KifintEntry entry) {
			using (MemoryStream ms = ExtractToStream())
				return Animation.Extract(ms, FileName);
		}
		/// <summary>
		///  Extracts the ANM animation script from the open KIFINT archive stream.
		/// </summary>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted animation script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="kifintStream"/> is null.
		/// </exception>
		public Animation ExtractAnimation(KifintStream kifintStream) {
			using (MemoryStream ms = ExtractToStream(kifintStream))
				return Animation.Extract(ms, FileName);
		}

		#endregion
	}
}
