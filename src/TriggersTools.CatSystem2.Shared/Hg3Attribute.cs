using System.Drawing;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Json;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  An attribute attached to an HG-3 frame which includes positioning and color information.
	/// </summary>
	public sealed class Hg3Attribute {
		#region Fields

		/// <summary>
		///  Gets the HG-3 image that contains this attribute's frame.
		/// </summary>
		[JsonIgnore]
		public HgxImage Hg3Image => Hg3Frame?.HgxImage;
		/// <summary>
		///  Gets the HG-3 frame that contains this attribute.
		/// </summary>
		[JsonIgnore]
		public HgxFrame Hg3Frame { get; internal set; }

		/// <summary>
		///  Gets the numeric identifier of the attribute.
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; private set; }
		/// <summary>
		///  Gets the x coordinate of the attribute.
		/// </summary>
		[JsonProperty("x")]
		public int X { get; private set; }
		/// <summary>
		///  Gets the y coordinate of the attribute.
		/// </summary>
		[JsonProperty("y")]
		public int Y { get; private set; }
		/// <summary>
		///  Gets the width of the attribute.
		/// </summary>
		[JsonProperty("width")]
		public int Width { get; private set; }
		/// <summary>
		///  Gets the height of the attribute.
		/// </summary>
		[JsonProperty("height")]
		public int Height { get; private set; }
		/// <summary>
		///  Gets the color to display the attribute with.
		/// </summary>
		[JsonProperty("color")]
		[JsonConverter(typeof(JsonColorConverter))]
		public Color Color { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned HG-3 attribute for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		private Hg3Attribute() { }
		/// <summary>
		///  Constructs an HG-3 attribute with the specified Id and <see cref="HG3ATS"/>.
		/// </summary>
		/// <param name="id">The identifier for the attribute.</param>
		/// <param name="ats">The HG3ATS struct containing attribute information.</param>
		/// <param name="hg3Frame">The HG-3 frame containing this attribute.</param>
		internal Hg3Attribute(int id, HG3ATS ats, HgxFrame hg3Frame) {
			Hg3Frame = hg3Frame;
			Id = id;
			X = ats.X;
			Y = ats.Y;
			Width = ats.Width;
			Height = ats.Height;
			Color = Color.FromArgb(ats.Color);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the HG-3 attribute.
		/// </summary>
		/// <returns>The string representation of the HG-3 attribute.</returns>
		public override string ToString() => $"HG-3 Attribute {X},{Y} {Width}x{Height} #{Color.ToArgb():X8}";

		#endregion
	}
}
