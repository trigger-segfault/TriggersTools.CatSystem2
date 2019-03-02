using System;
using Newtonsoft.Json;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.CatSystem2 {
	public struct VCode : IEquatable<VCode> {
		#region Constants

		/// <summary>
		///  Gets the default length for a V_CODE or V_CODE2.
		/// </summary>
		public const int DefaultLength = 16;

		#endregion

		#region Fields

		[JsonIgnore]
		private string code;
		[JsonIgnore]
		private int length;

		#endregion

		#region Constructors

		public VCode(string code) : this(code, DefaultLength) { }

		public VCode(string code, int length) {
			if (length <= 0)
				throw ArgumentOutOfRangeUtils.OutsideMin(nameof(Length), length, 0, false);
			this.code = code ?? throw new ArgumentNullException(nameof(code));
			this.length = length;
		}

		#endregion

		#region Properties

		[JsonProperty("code")]
		public string Code {
			get => code;
			set => code = value ?? throw new ArgumentNullException(nameof(Code));
		}
		[JsonProperty("length")]
		public int Length {
			get => length;
			set {
				if (value <= 0)
					throw ArgumentOutOfRangeUtils.OutsideMin(nameof(Length), value, 0, false);
				length = value;
			}
		}

		#endregion

		#region Object Overrides

		public override string ToString() => $"\"{code}\" Length={length}";

		public override int GetHashCode() => (code?.GetHashCode() ?? 0) ^ length;

		public override bool Equals(object obj) {
			return (obj is VCode vcode && Equals(vcode));
		}
		public bool Equals(VCode other) {
			return code == other.Code && length == other.Length;
		}

		#endregion

		#region Operators

		public static bool operator ==(VCode a, VCode b) => a.Equals(b);
		public static bool operator !=(VCode a, VCode b) => !a.Equals(b);

		#endregion

		#region Casting

		public static implicit operator string(VCode vcode) => vcode.code;

		#endregion
	}
}
