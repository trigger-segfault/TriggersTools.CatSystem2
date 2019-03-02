using System;
using System.IO;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources {
	/// <summary>
	///  A resource that is just a data buffer.
	///  Can store and save any kind of resource as long as the data is properly formatted.
	/// </summary>
	public class GenericResource : Resource {
		#region Fields

		/// <summary>
		///  The raw resource data.
		/// </summary>
		private byte[] data;

		#endregion

		#region Constructors

		//public GenericResource(ResourceId type) : base(type) { }
		public GenericResource(ResourceId type, ResourceId name, ushort language)
			: base(type, name, language)
		{
		}
		public GenericResource(string fileName, ResourceId type, ResourceId name, ushort language)
			: base(fileName, type, name, language)
		{
		}
		public GenericResource(IntPtr hModule, ResourceId type, ResourceId name, ushort language)
			: base(hModule, type, name, language)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the raw resource data.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public byte[] Data {
			get => data;
			set => data = value ?? throw new ArgumentNullException(nameof(Data));
		}
		/// <summary>
		///  Gets the length of the data.
		/// </summary>
		public int Length => Data.Length;

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		public new GenericResource Clone(ResourceId? name = null, ushort? language = null) {
			return (GenericResource) base.Clone(name, language);
		}

		#endregion

		#region Resource Overrides

		/// <summary>
		///  Reads the resource from the module handle and or stream. The stream's length is the size of the resource.
		/// </summary>
		/// <param name="hModule">The open module handle.</param>
		/// <param name="stream">The unmanaged stream to the resource.</param>
		protected override void Read(IntPtr hModule, Stream stream) {
			BinaryReader reader = new BinaryReader(stream);
			data = stream.ReadToEnd();
		}
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected override void Write(Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(data);
		}
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		protected override Resource CreateClone(ResourceId name, ushort language) {
			GenericResource generic = new GenericResource(Type, name, language) {
				data = new byte[data.Length],
			};
			Array.Copy(data, generic.data, data.Length);
			return generic;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the resource and data.
		/// </summary>
		/// <returns>The string representation of the resource and data.</returns>
		public override string ToString() => $"{base.ToString()} Length={Data.Length}";

		#endregion
	}
}