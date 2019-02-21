using System;
using System.Collections.Generic;
using System.Reflection;

namespace TriggersTools.SharpUtils.Enums {
	/// <summary>
	///  A non-generic interface for an <see cref="IEnumInfo"/>'s field.
	/// </summary>
	public interface IEnumFieldInfo {
		#region Fields

		/// <summary>
		///  Gets the enum info containing this field.
		/// </summary>
		IEnumInfo EnumInfo { get; }
		/// <summary>
		///  Gets the field info for the enum field.
		/// </summary>
		FieldInfo FieldInfo { get; }

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
		///  Gets the name of the enum field.
		/// </summary>
		string Name { get; }
		/// <summary>
		///  Gets the value of the enum field.
		/// </summary>
		Enum Value { get; }
		/// <summary>
		///  Gets the <see cref="long"/> value of the enum field.
		/// </summary>
		long Int64Value { get; }

		#endregion

		#region GetAttribute
		
		/// <summary>
		///  Gets the attribute of the specified type if it exists.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <returns>The attribute of the specified type if it exists. Otherwise null.</returns>
		/// 
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		T GetAttribute<T>() where T : Attribute;
		/// <summary>
		///  Gets the attribute of the specified type if it exists.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <returns>The attribute of the specified type if it exists. Otherwise null.</returns>
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
		Attribute GetAttribute(Type attributeType);

		/// <summary>
		///  Tries to get the attribute of the specified type if it exists.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="attribute">The output attribute that was found, otherwise null.</param>
		/// <returns>True if the attribute was found, otherwise null.</returns>
		/// 
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		bool TryGetAttribute<T>(out T attribute) where T : Attribute;
		/// <summary>
		///  Tries to get the attribute of the specified type if it exists.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <param name="attribute">The output attribute that was found, otherwise null.</param>
		/// <returns>True if the attribute was found, otherwise null.</returns>
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
		bool TryGetAttribute(Type attributeType, out Attribute attribute);

		#endregion

		#region GetAttributes

		/// <summary>
		///  Gets a collection of attributes of the specified type if any exists.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <returns>The attribute collection of the specified type.</returns>
		/// 
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		IEnumerable<T> GetAttributes<T>() where T : Attribute;
		/// <summary>
		///  Gets a collection of attributes of the specified type if any exists.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <returns>The attribute collection of the specified type.</returns>
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
		IEnumerable<Attribute> GetAttributes(Type attributeType);

		/// <summary>
		///  Tries to get a collection of attributes of the specified type if any exists.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="attributes">The output attribute collection that was found, otherwise null.</param>
		/// <returns>True if the attribute was found, otherwise null.</returns>
		/// 
		/// <exception cref="AmbiguousMatchException">
		///  More than one of the requested attributes was found.
		/// </exception>
		/// <exception cref="TypeLoadException">
		///  A custom attribute type cannot be loaded.
		/// </exception>
		bool TryGetAttributes<T>(out IEnumerable<T> attributes) where T : Attribute;
		/// <summary>
		///  Tries to get a collection of attributes of the specified type if any exists.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to get.</param>
		/// <param name="attributes">The output attribute collection that was found, otherwise null.</param>
		/// <returns>True if the attribute was found, otherwise null.</returns>
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
		bool TryGetAttributes(Type attributeType, out IEnumerable<Attribute> attributes);

		#endregion

		#region IsDefined

		/// <summary>
		///  Gets if the attribute of the specified type is defined.
		/// </summary>
		/// <typeparam name="TAttr">The type of the attribute to check for.</typeparam>
		/// <returns>True if the attribute is defined.</returns>
		bool IsDefined<TAttr>() where TAttr : Attribute;
		/// <summary>
		///  Gets if the attribute of the specified type is defined.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to check for.</param>
		/// <returns>True if the attribute is defined.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="attributeType"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="attributeType"/> is not derived from <see cref="Attribute"/>.
		/// </exception>
		bool IsDefined(Type attributeType);

		#endregion
	}
}
