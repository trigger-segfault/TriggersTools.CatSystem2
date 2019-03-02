using System;
using System.IO;
using System.Text;
using TriggersTools.Resources.Internal;

namespace TriggersTools.Resources.Menu {
	/// <summary>
	///  A menu template resource.
	/// </summary>
	public sealed class MenuResource : Resource {
		#region Fields

		/// <summary>
		///  The menu template for this resource.<para/>
		///  Either a <see cref="MenuTemplate"/> or <see cref="MenuExTemplate"/>.
		/// </summary>
		private IMenuBaseTemplate template;

		#endregion

		#region Constructors
		
		//public MenuResource() : base(ResourceTypes.RT_MENU) { }
		public MenuResource(ResourceId name, ushort language)
			: base(ResourceTypes.Menu, name, language)
		{
			Template = new MenuTemplate();
		}
		public MenuResource(string fileName, ResourceId name, ushort language)
			: base(fileName, ResourceTypes.Menu, name, language)
		{
		}
		public MenuResource(IntPtr hModule, ResourceId name, ushort language)
			: base(hModule, ResourceTypes.Menu, name, language)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the menu template for this resource.<para/>
		///  Either a <see cref="MenuTemplate"/> or <see cref="MenuExTemplate"/>.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public IMenuBaseTemplate Template {
			get => template;
			set => template = value ?? throw new ArgumentNullException(nameof(Template));
		}

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		public new MenuResource Clone(ResourceId? name = null, ushort? language = null) {
			return (MenuResource) base.Clone(name, language);
		}

		#endregion

		#region Resource Overrides

		/// <summary>
		///  Reads the resource from the module handle and or stream. The stream's length is the size of the resource.
		/// </summary>
		/// <param name="hModule">The open module handle.</param>
		/// <param name="stream">The unmanaged stream to the resource.</param>
		protected override void Read(IntPtr hModule, Stream stream) {
			BinaryReader reader = new BinaryReader(stream, Encoding.Unicode);

			ushort version = reader.ReadUInt16();
			stream.Position -= 2;

			switch (version) {
			case 0: template = new MenuTemplate(); break;
			case 1: template = new MenuExTemplate(); break;
			default: throw new NotSupportedException($"Unexpected menu header version {version}");
			}

			((BinaryReadableWriteable) template).Read(reader);
		}
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected override void Write(Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode);
			((BinaryReadableWriteable) template).Write(writer);
		}
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		protected override Resource CreateClone(ResourceId name, ushort language) {
			return new MenuResource(name, language) {
				template = template.Clone(),
			};
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the menu resource in the MENU format.
		/// </summary>
		/// <returns>The string representation of the menu resource.</returns>
		public override string ToString() => $"{base.ToString()} {Template}";

		#endregion
	}
}