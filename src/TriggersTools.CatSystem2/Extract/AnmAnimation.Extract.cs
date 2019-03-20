using System;
using System.IO;
using TriggersTools.CatSystem2.Structs;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.CatSystem2 {
	partial class AnmAnimation {
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
		public static AnmAnimation Extract(string anmFile) {
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
		public static AnmAnimation Extract(Stream stream, string fileName) {
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			BinaryReader reader = new BinaryReader(stream);

			ANMHDR hdr = reader.ReadUnmanaged<ANMHDR>();
			UnexpectedFileTypeException.ThrowIfInvalid(hdr.Signature, ANMHDR.ExpectedSignature);
			
			reader.ReadBytes(20); // Unused (?)

			ANMLINE[] frames = new ANMLINE[hdr.LineCount];
			for (int i = 0; i < hdr.LineCount; i++) {
				frames[i] = reader.ReadUnmanaged<ANMLINE>();
			}

			return new AnmAnimation(Path.GetFileName(fileName), hdr, frames);
		}

		#endregion
	}
	partial class KifintEntryExtensions {
		#region ExtractAnimation

		/// <summary>
		///  Extracts the ANM animation script from the entry.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <returns>The extracted animation script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> is null.
		/// </exception>
		public static AnmAnimation ExtractAnimation(this KifintEntry entry) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream())
				return AnmAnimation.Extract(stream, entry.FileName);
		}
		/// <summary>
		///  Extracts the ANM animation script from the open KIFINT archive stream.
		/// </summary>
		/// <param name="entry">The entry to extract from.</param>
		/// <param name="kifintStream">The stream to the open KIFINT archive.</param>
		/// <returns>The extracted animation script.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="entry"/> or <paramref name="kifintStream"/> is null.
		/// </exception>
		public static AnmAnimation ExtractAnimation(this KifintEntry entry, KifintStream kifintStream) {
			if (entry == null) throw new ArgumentNullException(nameof(entry));
			using (var stream = entry.ExtractToStream(kifintStream))
				return AnmAnimation.Extract(stream, entry.FileName);
		}

		#endregion
	}
}
