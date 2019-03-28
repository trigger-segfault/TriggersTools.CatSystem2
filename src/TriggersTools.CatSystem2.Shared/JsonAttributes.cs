using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json {
#if !JSON
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class JsonPropertyAttribute : Attribute {

		public JsonPropertyAttribute(string propertyName) { }
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class JsonConverterAttribute : Attribute {

		public JsonConverterAttribute(Type converterType) { }
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
	public class JsonObjectAttribute : Attribute {

		public JsonObjectAttribute() { }
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class JsonIgnoreAttribute : Attribute {

		public JsonIgnoreAttribute() { }
	}
#endif
}
