using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes {
	public struct StringToken {
		public TokenType Type { get; }
		public string Value { get; }
		public int Variable {
			get {
				if (Type == TokenType.Variable && int.TryParse(Value, out int variable))
					return variable;
				return -1;
			}
		}

		public StringToken(string value) {
			Value = value ?? throw new ArgumentNullException(nameof(value));
			Type = TokenType.Value;
		}
		public StringToken(int variable) {
			Value = variable.ToString();
			Type = TokenType.Variable;
		}

		public static StringToken Parse(string s) {
			if (s.StartsWith("$"))
				return new StringToken(int.Parse(s.Substring(1)));
			return new StringToken(s);
		}

		public override string ToString() {
			switch (Type) {
			case TokenType.Value:
				return Value;
			case TokenType.Variable:
				return $"${Value}";
			}
			return "<INVALID>";
		}
	}
	public enum TokenType : byte {
		Invalid = 0,
		Value,
		Variable,
		Relative,
	}
	public enum IntTokenStyle : byte {
		Decimal = 0,
		Hex0x,
		HexDollar,
	}
	public struct IntToken {

		public static IntToken Relative { get; } = new IntToken(0, TokenType.Relative);

		public IntTokenStyle Style { get; }
		public TokenType Type { get; }
		public int Value { get; }
		public int Variable => (Type == TokenType.Variable ? Value : -1);

		public IntToken(int value, IntTokenStyle style = IntTokenStyle.Decimal) {
			Value = value;
			Type = TokenType.Value;
			Style = style;
		}
		public IntToken(int value, TokenType type, IntTokenStyle style = IntTokenStyle.Decimal) {
			Value = value;
			Type = type;
			Style = style;
		}

		public override string ToString() {
			switch (Type) {
			case TokenType.Invalid:
			case TokenType.Value:
				switch (Style) {
				case IntTokenStyle.Decimal: return $"{Value}";
				case IntTokenStyle.Hex0x: return $"0x{Value:X}";
				case IntTokenStyle.HexDollar: return $"${Value:X}";
				}
				break;
			case TokenType.Variable: return $"#{Value}";
			case TokenType.Relative: return "@";
			}
			return "<INVALID>";
		}

		public static IntToken Parse(string s) {
			if (s == "@")
				return new IntToken(0, TokenType.Relative);
			if (s.StartsWith("#"))
				return new IntToken(int.Parse(s.Substring(1)), TokenType.Variable);
			if (s.StartsWith("$"))
				return new IntToken(int.Parse(s.Substring(1)), IntTokenStyle.HexDollar);
			if (s.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
				return new IntToken(int.Parse(s.Substring(1)), IntTokenStyle.Hex0x);
			return new IntToken(int.Parse(s));
		}
	}
}
