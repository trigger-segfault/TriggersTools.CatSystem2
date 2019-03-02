using System.IO;

namespace TriggersTools.Resources.Internal {
	/// <summary>
	///  A base class for internal read and write methods.
	/// </summary>
	public abstract class BinaryReadableWriteable {
		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal abstract void Read(BinaryReader reader);
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal abstract void Write(BinaryWriter writer);
	}
}
