using System;
using System.Reflection;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2.Json {
	internal sealed class JsonStringEnumIgnoreCaseConverter : JsonStringEnumConverter {
		#region Constructors

		public JsonStringEnumIgnoreCaseConverter() {
			IgnoreCase = true;
		}

		#endregion
	}
	internal class JsonStringEnumConverter : JsonConverter {
		#region Fields

		protected bool IgnoreCase { get; set; }

		#endregion

		#region JsonConverter Overrides

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			string enumStr = value.ToString();
			foreach (var field in value.GetType().GetFields()) {
				string name = field.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? field.Name;
				if (!field.IsStatic)
					continue;
				if (value.Equals(field.GetValue(null))) {
					writer.WriteValue(name);
					return;
				}
			}
			throw new InvalidOperationException("Unknown enum value");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.Value == null)
				return null;
			string enumStr = (string) reader.Value;
			objectType = Nullable.GetUnderlyingType(objectType) ?? objectType;
			foreach (var field in objectType.GetFields()) {
				string name = field.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? field.Name;
				if (!field.IsStatic)
					continue;
				if (string.Compare(enumStr, name, IgnoreCase) == 0)
					return field.GetValue(null);
			}
			throw new InvalidOperationException("Unknown enum name!");
		}

		public override bool CanConvert(Type objectType) => objectType == typeof(string);

		public override bool CanWrite => true;

		#endregion
	}
}
