using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2.Json {
#if !JSON
	internal sealed class JsonColorConverter { }
#else
	internal sealed class JsonColorConverter : JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value is Color color) {
				writer.WriteValue($"#{color.ToArgb():X8}");
				return;
			}
			throw new ArgumentException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.Value is string s && s.StartsWith("#")) {
				s = s.Substring(1);
				return Color.FromArgb(int.Parse(s, NumberStyles.HexNumber));
			}
			else if (reader.Value is long l) {
				uint ui = (uint) l;
				return Color.FromArgb(unchecked((int) ui));
			}
			throw new ArgumentException();
		}

		public override bool CanConvert(Type objectType) {
			return (objectType == typeof(Color));
		}
	}
#endif
}
