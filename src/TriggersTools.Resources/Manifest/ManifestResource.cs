using System;
using System.IO;
using System.Text;
using System.Xml;
using TriggersTools.Resources.Enumerations;

namespace TriggersTools.Resources.Manifest {
	/// <summary>
	///  An embedded SxS manifest resource.
	/// </summary>
	public class ManifestResource : Resource {
		#region Fields

		/// <summary>
		///  The embedded manifest XML document.
		/// </summary>
		private XmlDocument doc = new XmlDocument();

		#endregion
		
		#region Constructors

		public ManifestResource(ushort language)
			: this((ushort) ManifestType.CreateProcess, language)
		{
		}
		public ManifestResource(ManifestType manifestType, ushort language)
			: this((ushort) manifestType, language)
		{
		}
		public ManifestResource(ResourceId name, ushort language)
			: base(ResourceTypes.Manifest, name, language)
		{
			doc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
						"<assembly xmlns=\"urn:schemas-microsoft-com:asm.v1\" manifestVersion=\"1.0\" />");
		}
		public ManifestResource(string fileName, ManifestType manifestType, ushort language)
			: this(fileName, (ushort) manifestType, language)
		{
		}
		public ManifestResource(string fileName, ResourceId name, ushort language)
			: base(fileName, ResourceTypes.Manifest, name, language)
		{
		}
		public ManifestResource(IntPtr hModule, ManifestType manifestType, ushort language)
			: this(hModule, (ushort) manifestType, language)
		{
		}
		public ManifestResource(IntPtr hModule, ResourceId name, ushort language)
			: base(hModule, ResourceTypes.Manifest, name, language)
		{
		}
		
		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the embedded manifest XML document.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public XmlDocument Docment {
			get => doc;
			set => doc = value ?? throw new ArgumentNullException(nameof(Docment));
		}
		/// <summary>
		///  Gets or sets the embedded manifest XML text.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public string Xml {
			get => Docment.OuterXml;
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(Xml));
				doc = new XmlDocument();
				doc.LoadXml(value);
			}
		}
		/// <summary>
		///  Gets the manifest type. This is the <see cref="Name"/> of the resource.
		/// </summary>
		public ManifestType ManifestType => (ManifestType) Name.Value;

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		public new ManifestResource Clone(ResourceId? name = null, ushort? language = null) {
			return (ManifestResource) base.Clone(name, language);
		}

		#endregion

		#region Resource Overrides

		/// <summary>
		///  Reads the resource from the module handle and or stream. The stream's length is the size of the resource.
		/// </summary>
		/// <param name="hModule">The open module handle.</param>
		/// <param name="stream">The unmanaged stream to the resource.</param>
		protected override void Read(IntPtr hModule, Stream stream) {
			doc = new XmlDocument();
			doc.Load(stream);
		}
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected override void Write(Stream stream) {
			var settings = new XmlWriterSettings {
				ConformanceLevel = ConformanceLevel.Document,
				Encoding = new UTF8Encoding(false),
				OmitXmlDeclaration = false,
				Indent = true,
				IndentChars = "  ",
			};
			using (var writer = XmlWriter.Create(stream, settings))
				doc.WriteTo(writer);
		}
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		protected override Resource CreateClone(ResourceId name, ushort language) {
			return new ManifestResource(name, language) {
				Xml = Xml,
			};
		}

		#endregion
	}
}