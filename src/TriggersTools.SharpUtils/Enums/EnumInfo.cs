using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TriggersTools.SharpUtils.Enums {
	/// <summary>
	///  A generic class for caching information about an enum of type <typeparamref name="TEnum"/>.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enum.</typeparam>
	public sealed class EnumInfo<TEnum> : IEnumInfo where TEnum : Enum {
		#region Constants

		/// <summary>
		///  Gets the singular instance for this enum info.
		/// </summary>
		public static EnumInfo<TEnum> Instance { get; } = new EnumInfo<TEnum>();

		#endregion

		#region Fields

		private Dictionary<string, EnumFieldInfo<TEnum>> nameLookup;
		private Dictionary<string, EnumFieldInfo<TEnum>> nameLookupIgnoreCase;
		private Dictionary<TEnum, EnumFieldInfo<TEnum>> valueLookup;

		private Dictionary<string, EnumFieldInfo<TEnum>> GetNameLookup(bool ignoreCase) {
			return (ignoreCase ? nameLookupIgnoreCase : nameLookup);
		}

		/// <summary>
		///  Gets the default enum value of 0.
		/// </summary>
		public TEnum DefaultValue { get; }
		Enum IEnumInfo.DefaultValue => DefaultValue;
		/// <summary>
		///  Gets if the enum has a field for the default value of zero.
		/// </summary>
		public bool HasDefaultField { get; }
		/// <summary>
		///  Gets the type name of the enum.
		/// </summary>
		public string Name { get; }
		/// <summary>
		///  Gets the type of the enum.
		/// </summary>
		public Type Type { get; }
		/// <summary>
		///  Gets the underlying type of the enum.
		/// </summary>
		public Type UnderlyingType { get; }
		/// <summary>
		///  Gets if the enum underlying type is a <see cref="ulong"/>.
		/// </summary>
		public bool IsUInt64 { get; }

		/// <summary>
		///  Gets the list of fields for the enum.
		/// </summary>
		public IReadOnlyList<EnumFieldInfo<TEnum>> Fields { get; }
		IReadOnlyList<IEnumFieldInfo> IEnumInfo.Fields => Fields;

		#endregion

		#region Constructors

		/// <summary>
		///  Initializes the <see cref="EnumInfo{TEnum}"/> for the enum type <typeparamref name="TEnum"/>.
		/// </summary>
		internal EnumInfo() {
			Type = typeof(TEnum);
			UnderlyingType = Enum.GetUnderlyingType(Type);
			IsUInt64 = UnderlyingType == typeof(ulong);
			DefaultValue = default;

			nameLookup =
				new Dictionary<string, EnumFieldInfo<TEnum>>();
			nameLookupIgnoreCase =
				new Dictionary<string, EnumFieldInfo<TEnum>>(StringComparer.InvariantCultureIgnoreCase);
			valueLookup =
				new Dictionary<TEnum, EnumFieldInfo<TEnum>>();

			foreach (FieldInfo field in Type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
				var info = new EnumFieldInfo<TEnum>(this, field, IsUInt64);
				nameLookup.Add(info.Name, info);
				// Just incase this enum can't handle ignore-case.
				if (!nameLookupIgnoreCase.ContainsKey(info.Name))
					nameLookupIgnoreCase.Add(info.Name, info);
				// Ignore duplicate enum values
				if (!valueLookup.ContainsKey(info.Value))
					valueLookup.Add(info.Value, info);
			}
			Fields = Array.AsReadOnly(nameLookup.Values.ToArray());
			HasDefaultField = valueLookup.ContainsKey(DefaultValue);
		}

		#endregion

		#region Converting

		/// <summary>
		///  Gets the <see cref="long"/> representation of the enum value.<para/> This handles unchecked conversion
		///  when the underlying type is <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to get the <see cref="long"/> value of.</param>
		/// <returns>The value converted to a <see cref="long"/>.</returns>
		public long ToInt64(TEnum value) {
			if (IsUInt64)
				return unchecked((long) Convert.ToUInt64(value));
			else
				return unchecked(Convert.ToInt64(value));
		}
		/// <summary>
		///  Gets the enum representation of the <see cref="long"/> value.
		/// </summary>
		/// <param name="value">the value to get the enum value of.</param>
		/// <returns>The value converter to <typeparamref name="TEnum"/>.</returns>
		public TEnum ToEnum(long value) {
			return (TEnum) Enum.ToObject(Type, value);
		}
		/// <summary>
		///  Gets the enum representation of the <see cref="ulong"/> value.
		/// </summary>
		/// <param name="value">the value to get the enum value of.</param>
		/// <returns>The value converter to <typeparamref name="TEnum"/>.</returns>
		public TEnum ToEnum(ulong value) {
			return (TEnum) Enum.ToObject(Type, value);
		}

		#endregion

		#region Attribute

		/// <summary>
		///  Gets the field's attribute for the specified enum value.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="value">The value of the enum.</param>
		/// <returns>The enum field info's attribute for this value.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  No enum field with <paramref name="value"/> exists.
		/// </exception>
		public T GetAttribute<T>(TEnum value) where T : Attribute {
			return valueLookup[value].GetAttribute<T>();
		}
		T IEnumInfo.GetAttribute<T>(object value) => GetAttribute<T>((TEnum) value);
		/// <summary>
		///  Gets the field's attribute for the specified enum value.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <param name="value">The value of the enum.</param>
		/// <returns>The enum field info's attribute for this value.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  No enum field with <paramref name="value"/> exists.
		/// </exception>
		public Attribute GetAttribute(Type attributeType, TEnum value) {
			return valueLookup[value].GetAttribute(attributeType);
		}
		Attribute IEnumInfo.GetAttribute(Type attributeType, object value) {
			return GetAttribute(attributeType, (TEnum) value);
		}
		/// <summary>
		///  Tries to get the field's attribute for the specified enum value.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="value">The value of the enum.</param>
		/// <param name="attribute">The output attribute if the field is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		public bool TryGetAttribute<T>(TEnum value, out T attribute) where T : Attribute {
			if (TryGetField(value, out EnumFieldInfo<TEnum> field))
				return field.TryGetAttribute(out attribute);
			attribute = null;
			return false;
		}
		bool IEnumInfo.TryGetAttribute<T>(object value, out T attribute) {
			return TryGetAttribute((TEnum) value, out attribute);
		}
		/// <summary>
		///  Tries to get the field's attribute for the specified enum value.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <param name="value">The value of the enum.</param>
		/// <param name="attribute">The output attribute if the field is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		public bool TryGetAttribute(Type attributeType, TEnum value, out Attribute attribute) {
			if (TryGetField(value, out EnumFieldInfo<TEnum> field))
				return field.TryGetAttribute(attributeType, out attribute);
			attribute = null;
			return false;
		}
		bool IEnumInfo.TryGetAttribute(Type attributeType, object value, out Attribute attribute) {
			return TryGetAttribute(attributeType, (TEnum) value, out attribute);
		}

		#endregion

		#region Attribute Flags

		/// <summary>
		/// Gets all flag attributes for a specified enum value flag combination.
		/// </summary>
		/// <param name="value">The enum value flag combination to get each flag for.</param>
		/// <returns>An enumerable of enum fields' attributes.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="value"/> does not exist.
		/// </exception>
		public T[] GetAttributes<T>(TEnum value) where T : Attribute {
			return GetFields(value).Select(f => f.GetAttribute<T>()).Where(a => a != null).ToArray();
		}
		T[] IEnumInfo.GetAttributes<T>(object value) => GetAttributes<T>((TEnum) value);
		/// <summary>
		///  Gets all flag attributes for a specified enum value flag combination.
		/// </summary>
		/// <param name="attributeType">The type of the attributes to get.</param>
		/// <param name="value">The enum value flag combination to get each flag for.</param>
		/// <returns>An array of enum fields' attributes.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="value"/> does not exist.
		/// </exception>
		public Attribute[] GetAttributes(Type attributeType, TEnum value) {
			return GetFields(value).Select(f => f.GetAttribute(attributeType)).Where(a => a != null).ToArray();
		}
		Attribute[] IEnumInfo.GetAttributes(Type attributeType, object value) {
			return GetAttributes(attributeType, (TEnum) value);
		}

		#endregion

		#region Field

		/// <summary>
		///  Gets the field for the specified enum value.
		/// </summary>
		/// <param name="value">The value of the enum.</param>
		/// <returns>The enum field info with this value.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  No enum field with <paramref name="value"/> exists.
		/// </exception>
		public EnumFieldInfo<TEnum> GetField(TEnum value) => valueLookup[value];
		IEnumFieldInfo IEnumInfo.GetField(object value) => GetField((TEnum) value);
		/// <summary>
		///  Gets the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <returns>The enum field info with this name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  No enum field with <paramref name="name"/> exists.
		/// </exception>
		public EnumFieldInfo<TEnum> GetField(string name) => nameLookup[name];
		IEnumFieldInfo IEnumInfo.GetField(string name) => GetField(name);
		/// <summary>
		///  Gets the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <returns>The enum field info with this name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  No enum field with <paramref name="name"/> exists.
		/// </exception>
		public EnumFieldInfo<TEnum> GetField(string name, bool ignoreCase) => GetNameLookup(ignoreCase)[name];
		IEnumFieldInfo IEnumInfo.GetField(string name, bool ignoreCase) => GetField(name, ignoreCase);

		/// <summary>
		///  Tries to get the field for the specified enum value.
		/// </summary>
		/// <param name="value">The value of the enum.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		public bool TryGetField(TEnum value, out EnumFieldInfo<TEnum> field) {
			return valueLookup.TryGetValue(value, out field);
		}
		bool IEnumInfo.TryGetField(object value, out IEnumFieldInfo field) {
			if (TryGetField((TEnum) value, out EnumFieldInfo<TEnum> genericField)) {
				field = genericField;
				return true;
			}
			field = null;
			return false;
		}
		/// <summary>
		///  Tries to get the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="name"/> is null.
		/// </exception>
		public bool TryGetField(string name, out EnumFieldInfo<TEnum> field) {
			return nameLookup.TryGetValue(name, out field);
		}
		bool IEnumInfo.TryGetField(string name, out IEnumFieldInfo field) {
			if (TryGetField(name, out EnumFieldInfo<TEnum> genericField)) {
				field = genericField;
				return true;
			}
			field = null;
			return false;
		}
		/// <summary>
		///  Tries to get the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="name"/> is null.
		/// </exception>
		public bool TryGetField(string name, bool ignoreCase, out EnumFieldInfo<TEnum> field) {
			return GetNameLookup(ignoreCase).TryGetValue(name, out field);
		}
		bool IEnumInfo.TryGetField(string name, bool ignoreCase, out IEnumFieldInfo field) {
			if (TryGetField(name, ignoreCase, out EnumFieldInfo<TEnum> genericField)) {
				field = genericField;
				return true;
			}
			field = null;
			return false;
		}

		#endregion

		#region Field Flags

		/// <summary>
		///  Gets all flags for a specified enum value flag combination.
		/// </summary>
		/// <param name="value">The enum value flag combination to get each flag for.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="value"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(TEnum value) {
			if (value.Equals(DefaultValue)) return new EnumFieldInfo<TEnum>[0];
			string[] flags = value.ToString().Split(new[] { ", " }, StringSplitOptions.None);
			return flags.Select(f => nameLookup[f]).ToArray();
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(object value) {
			return GetFields((TEnum) value);
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names) {
			return GetFields(names, false);
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names) {
			return GetFields(names, false);
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names, bool ignoreCase) {
			return GetFields(names, ignoreCase, ',', '|', ' ', '\t');
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names, bool ignoreCase) {
			return GetFields(names, ignoreCase, ',', '|', ' ', '\t');
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <param name="separators">The valid separators to split the names with.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names, params char[] separators) {
			return GetFields(names, false, separators);
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names, params char[] separators) {
			return GetFields(names, false, separators);
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <param name="separators">The valid separators to split the names with.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names, bool ignoreCase, params char[] separators) {
			// There is no flags and no default value flag. Return nothing
			if (names == "0") return new EnumFieldInfo<TEnum>[0];
			string[] flags = names.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			var nameLookup = GetNameLookup(ignoreCase);
			return flags.Select(f => nameLookup[f.Trim()]).ToArray();
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names, bool ignoreCase, params char[] separators) {
			return GetFields(names, ignoreCase, separators);
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <param name="separators">The valid separators to split the names with.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names, params string[] separators) {
			return GetFields(names, false, separators);
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names, params string[] separators) {
			return GetFields(names, false, separators);
		}
		/// <summary>
		///  Gets all flags for a specified enum value flag combination as a string.
		/// </summary>
		/// <param name="names">
		///  The names of the enum flags. They can be separated by whitespace, ','s, or '|'s.
		/// </param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <param name="separators">The valid separators to split the names with.</param>
		/// <returns>An enumerable of enum fields.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="names"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="names"/> does not exist.
		/// </exception>
		public EnumFieldInfo<TEnum>[] GetFields(string names, bool ignoreCase, params string[] separators) {
			// There is no flags and no default value flag. Return nothing
			if (names == "0") return new EnumFieldInfo<TEnum>[0];
			string[] flags = names.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			var nameLookup = GetNameLookup(ignoreCase);
			return flags.Select(f => nameLookup[f.Trim()]).ToArray();
		}
		IEnumFieldInfo[] IEnumInfo.GetFields(string names, bool ignoreCase, params string[] separators) {
			return GetFields(names, ignoreCase, separators);
		}

		#endregion
	}
}
