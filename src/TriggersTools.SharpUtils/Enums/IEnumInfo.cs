using System;
using System.Collections.Generic;
using System.Reflection;

namespace TriggersTools.SharpUtils.Enums {
	/// <summary>
	///  A non-generic enum info class.
	/// </summary>
	public interface IEnumInfo {
		#region Properties

		/// <summary>
		///  Gets the default enum value of 0.
		/// </summary>
		Enum DefaultValue { get; }
		/// <summary>
		///  Gets if the enum has a field for the default value of zero.
		/// </summary>
		bool HasDefaultField { get; }
		/// <summary>
		///  Gets the type name of the enum.
		/// </summary>
		string Name { get; }
		/// <summary>
		///  Gets the type of the enum.
		/// </summary>
		Type Type { get; }
		/// <summary>
		///  Gets the underlying type of the enum.
		/// </summary>
		Type UnderlyingType { get; }
		/// <summary>
		///  Gets if the enum underlying type is a <see cref="ulong"/>.
		/// </summary>
		bool IsUInt64 { get; }
		/// <summary>
		///  Gets the list of fields for the enum.
		/// </summary>
		IReadOnlyList<IEnumFieldInfo> Fields { get; }

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
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		T GetAttribute<T>(object value) where T : Attribute;
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
		/// <exception cref="ArgumentNullException">
		///  <paramref name="attributeType"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="attributeType"/> is not derived from <see cref="Attribute"/>.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		Attribute GetAttribute(Type attributeType, object value);
		/// <summary>
		///  Tries to get the field's attribute for the specified enum value.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="value">The value of the enum.</param>
		/// <param name="attribute">The output attribute if the field is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		/// 
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		bool TryGetAttribute<T>(object value, out T attribute) where T : Attribute;
		/// <summary>
		///  Tries to get the field's attribute for the specified enum value.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <param name="value">The value of the enum.</param>
		/// <param name="attribute">The output attribute if the field is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="attributeType"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="attributeType"/> is not derived from <see cref="Attribute"/>.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		bool TryGetAttribute(Type attributeType, object value, out Attribute attribute);

		#endregion

		#region Attribute Flags

		/// <summary>
		///  Gets all flag attributes for a specified enum value flag combination.
		/// </summary>
		/// <typeparam name="T">The type of the attributes to get.</typeparam>
		/// <param name="value">The enum value flag combination to get each flag for.</param>
		/// <returns>An array of enum fields' attributes.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		///  One of the enum values within <paramref name="value"/> does not exist.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		T[] GetAttributes<T>(object value) where T : Attribute;
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
		/// <exception cref="ArgumentNullException">
		///  <paramref name="attributeType"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="attributeType"/> is not derived from <see cref="Attribute"/>.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		Attribute[] GetAttributes(Type attributeType, object value);

		#endregion

		#region Field

		/// <summary>
		/// Gets the field for the specified enum value.
		/// </summary>
		/// <param name="value">The value of the enum.</param>
		/// <returns>The enum field info with this value.</returns>
		/// 
		/// <exception cref="KeyNotFoundException">
		/// No enum field with <paramref name="value"/> exists.
		/// </exception>
		IEnumFieldInfo GetField(object value);
		/// <summary>
		/// Gets the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <returns>The enum field info with this name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		/// No enum field with <paramref name="name"/> exists.
		/// </exception>
		IEnumFieldInfo GetField(string name);
		/// <summary>
		/// Gets the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <returns>The enum field info with this name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is null.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		/// No enum field with <paramref name="name"/> exists.
		/// </exception>
		IEnumFieldInfo GetField(string name, bool ignoreCase);

		/// <summary>
		/// Tries to get the field for the specified enum value.
		/// </summary>
		/// <param name="value">The value of the enum.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified value.</returns>
		bool TryGetField(object value, out IEnumFieldInfo field);
		/// <summary>
		/// Tries to get the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is null.
		/// </exception>
		bool TryGetField(string name, out IEnumFieldInfo field);
		/// <summary>
		/// Tries to get the field for the specified enum value name.
		/// </summary>
		/// <param name="name">The name of the enum value.</param>
		/// <param name="ignoreCase">True if the name casing can be ignored.</param>
		/// <param name="field">The output field if one is found, otherwise null.</param>
		/// <returns>True if a field was found with the specified name.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is null.
		/// </exception>
		bool TryGetField(string name, bool ignoreCase, out IEnumFieldInfo field);

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
		IEnumFieldInfo[] GetFields(object value);
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
		IEnumFieldInfo[] GetFields(string names);
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
		IEnumFieldInfo[] GetFields(string names, bool ignoreCase);
		/// <summary>
		/// Gets all flags for a specified enum value flag combination as a string.
		///  </summary>
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
		IEnumFieldInfo[] GetFields(string names, params char[] separators);
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
		IEnumFieldInfo[] GetFields(string names, bool ignoreCase, params char[] separators);
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
		IEnumFieldInfo[] GetFields(string names, params string[] separators);
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
		IEnumFieldInfo[] GetFields(string names, bool ignoreCase, params string[] separators);

		#endregion
	}
}
