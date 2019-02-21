using System;
using System.Collections.Generic;
using System.Reflection;

namespace TriggersTools.SharpUtils.Enums {
	/// <summary>
	///  Cached info for an enum field of type <typeparamref name="TEnum"/>.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enum.</typeparam>
	public class EnumFieldInfo<TEnum> : IEnumFieldInfo where TEnum : Enum {

		#region Fields

		/// <summary>
		///  Gets the enum info containing this field.
		/// </summary>
		public EnumInfo<TEnum> EnumInfo { get; }
		IEnumInfo IEnumFieldInfo.EnumInfo => EnumInfo;
		/// <summary>
		/// Gets the field for the enum.
		/// </summary>
		public FieldInfo FieldInfo { get; }

		/// <summary>
		///  Gets the type of the enum field.
		/// </summary>
		public Type Type => EnumInfo.Type;
		/// <summary>
		///  Gets the underlying type of the enum.
		/// </summary>
		public Type UnderlyingType => EnumInfo.UnderlyingType;
		/// <summary>
		///  Gets if the enum underlying type is a <see cref="ulong"/>.
		/// </summary>
		public bool IsUInt64 => EnumInfo.IsUInt64;

		/// <summary>
		/// Gets the name of the enum field.
		/// </summary>
		public string Name => FieldInfo.Name;
		/// <summary>
		/// Gets the <typeparamref name="TEnum"/> value of the enum.
		/// </summary>
		public TEnum Value { get; }
		Enum IEnumFieldInfo.Value => Value;
		/// <summary>
		/// Gets the <see cref="long"/> value of the enum.
		/// </summary>
		public long Int64Value { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs the enum field info with the specified field and settings.
		/// </summary>
		/// <param name="enumInfo">The enum info containing this field.</param>
		/// <param name="field">The field info containing the enum value.</param>
		/// <param name="isUInt64">True if the enum should be converted from a <see cref="ulong"/>.</param>
		internal EnumFieldInfo(EnumInfo<TEnum> enumInfo, FieldInfo field, bool isUInt64) {
			EnumInfo = enumInfo;
			FieldInfo = field;
			Value = (TEnum) field.GetValue(null);
			if (isUInt64)
				Int64Value = unchecked((long) Convert.ToUInt64(Value));
			else
				Int64Value = unchecked(Convert.ToInt64(Value));
		}

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
		public T GetAttribute<T>() where T : Attribute {
			return FieldInfo.GetCustomAttribute<T>();
		}
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
		public Attribute GetAttribute(Type attributeType) {
			return FieldInfo.GetCustomAttribute(attributeType);
		}

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
		public bool TryGetAttribute<T>(out T attribute) where T : Attribute {
			attribute = GetAttribute<T>();
			return attribute != null;
		}
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
		public bool TryGetAttribute(Type attributeType, out Attribute attribute) {
			attribute = GetAttribute(attributeType);
			return attribute != null;
		}

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
		public IEnumerable<T> GetAttributes<T>() where T : Attribute {
			return FieldInfo.GetCustomAttributes<T>();
		}
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
		public IEnumerable<Attribute> GetAttributes(Type attributeType) {
			return FieldInfo.GetCustomAttributes(attributeType);
		}

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
		public bool TryGetAttributes<T>(out IEnumerable<T> attributes) where T : Attribute {
			attributes = GetAttributes<T>();
			return attributes != null;
		}
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
		public bool TryGetAttributes(Type attributeType, out IEnumerable<Attribute> attributes) {
			attributes = GetAttributes(attributeType);
			return attributes != null;
		}

		#endregion

		#region IsDefined

		/// <summary>
		///  Gets if the attribute of the specified type is defined.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to check for.</typeparam>
		/// <returns>True if the attribute is defined.</returns>
		public bool IsDefined<T>() where T : Attribute {
			return FieldInfo.IsDefined(typeof(T));
		}
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
		public bool IsDefined(Type attributeType) {
			return FieldInfo.IsDefined(attributeType);
		}

		#endregion
	}
}
