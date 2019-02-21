using System.Runtime.InteropServices;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2.Structs {
	/// <summary>
	///  The header for an ANM animation file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12, CharSet = CharSet.Ansi)]
	internal struct ANMHDR {
		#region Constants

		/// <summary>
		///  The expected value of <see cref="Signature"/>.
		/// </summary>
		public const string ExpectedSignature = "ANM";

		#endregion

		#region Fields

		/// <summary>
		///  The raw character array for the header's signature. This should be "ANM\0".
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
		public char[] SignatureRaw; // "ANM\0"
		/// <summary>
		///  Unknown 4-byte value 1.
		/// </summary>
		public int Unknown;
		/// <summary>
		///  The number of frame entries in the animation.
		/// </summary>
		public int FrameCount;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the header's signature from the null-terminated character array.
		/// </summary>
		public string Signature => SignatureRaw.ToNullTerminatedString();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ANM header.
		/// </summary>
		/// <returns>The ANM header's string representation.</returns>
		public override string ToString() => $"ANMHDR \"{Signature}\"";

		#endregion
	}
	/// <summary>
	///  The frame entry in an ANM animation file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 68, CharSet = CharSet.Ansi)]
	internal struct ANMFRAME {
		#region Fields

		/// <summary>
		///  The type of the command for this frame.
		/// </summary>
		public int Type;
		/// <summary>
		///  The parameters for the command.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ANMPARAM[] Parameters;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ANM frame.
		/// </summary>
		/// <returns>The ANM frame's string representation.</returns>
		public override string ToString() => $"{Type} {string.Join(" ", Parameters)}";

		#endregion
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
	internal struct ANMPARAM {
		#region Fields

		/// <summary>
		///  True if <see cref="Value"/> is a variable being referenced instead of a constant.
		/// </summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool IsVariable;
		/// <summary>
		///  Gets the value of the parameter. If <see cref="IsVariable"/> is true, this value is the index of the
		///  variable being referenced.
		/// </summary>
		public int Value;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the ANM parameters.
		/// </summary>
		/// <returns>The ANM parameters's string representation.</returns>
		public override string ToString() => (IsVariable ? $"@{Value}" : $"{Value}");

		#endregion
	}
}
