using System;
using System.Collections.Generic;
using System.Reflection;

namespace TriggersTools.SharpUtils.Enums {
	/// <summary>
	///  A static class for accessing enum type info helpers.
	/// </summary>
	public static class EnumInfo {
		#region Fields

		/// <summary>
		///  The dictionary of enum infos stored by type.
		/// </summary>
		private static readonly Dictionary<Type, IEnumInfo> enumInfos = new Dictionary<Type, IEnumInfo>();

		#endregion

		#region Get

		/// <summary>
		///  Gets the generic enum info for the specified enum type.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to get the info for.</typeparam>
		/// <returns>The generic enum info.</returns>
		public static EnumInfo<TEnum> Get<TEnum>() where TEnum : Enum {
			return EnumInfo<TEnum>.Instance;
		}
		/// <summary>
		///  Gets the non-generic enum info for the specified enum type.
		/// </summary>
		/// <param name="enumType">The enum type to get the info for.</param>
		/// <returns>The non-generic enum info. Constructs and adds the info first if it doesn't exist.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="enumType"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  <paramref name="enumType"/> is not an enum.
		/// </exception>
		public static IEnumInfo Get(Type enumType) {
			if (!enumInfos.TryGetValue(enumType, out IEnumInfo enumInfo)) {
				Type genericType = typeof(EnumInfo<>).MakeGenericType(enumType);
				enumInfo = (IEnumInfo) genericType.GetProperty("Instance").GetValue(null);
				//enumInfo = (IEnumInfo) Activator.CreateInstance();
				enumInfos.Add(enumType, enumInfo);
			}
			return enumInfo;
		}

		#endregion

		#region GetAttribute

		/// <summary>
		///  Gets the field's attribute for the specified enum value.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to get the info for.</typeparam>
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
		public static T GetAttribute<TEnum, T>(TEnum value) where TEnum : Enum where T : Attribute {
			return EnumInfo<TEnum>.Instance.GetAttribute<T>(value);
		}
		/// <summary>
		///  Gets the field's attribute for the specified enum value.
		/// </summary>
		/// <typeparam name="TEnum">The enum type to get the info for.</typeparam>
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
		public static Attribute GetAttribute<TEnum>(Type attributeType, TEnum value) where TEnum : Enum {
			return EnumInfo<TEnum>.Instance.GetAttribute(attributeType, value);
		}

		#endregion
	}
}
