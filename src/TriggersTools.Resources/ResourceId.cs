//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     ResourceLib Original Code from http://resourcelib.codeplex.com
//     Original Copyright (c) 2008-2009 Vestris Inc.
//     Changes Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
// <license>
// MIT License
// You may freely use and distribute this software under the terms of the following license agreement.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
// the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
// </license>
//-----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace TriggersTools.Resources {
	/// <summary>
	///  A resource Id.
	///  There're two types of resource Ids, reserved integer numbers (eg. RT_ICON) and custom string names (eg. "CUSTOM").
	/// </summary>
	public struct ResourceId : IComparable, IComparable<ResourceId>, IEquatable<ResourceId> {
		#region Constants

		/// <summary>
		///  Gets a null resource Id.
		/// </summary>
		public static readonly ResourceId Null = new ResourceId();

		#endregion

		#region Fields

		/// <summary>
		///  The resource Id as a string.
		/// </summary>
		private string name;
		/// <summary>
		///  Gets the resource type as an unsigned integer.<para/>
		///  This value is zero when <see cref="IsIntResource"/> is false.
		/// </summary>
		public ushort Value { get; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a resource Id from a <see cref="ushort"/> value Id or an allocated <see cref="string"/> Id.
		/// </summary>
		/// <param name="value">The integer or allocated string resource Id.</param>
		public ResourceId(IntPtr value) {
			if (ResourcePtr.IsPtrIntResource(value)) {
				Value = (ushort) value;
				name = null;
			}
			else {
				name = Marshal.PtrToStringUni(value);
				Value = 0;
			}
        }
		/// <summary>
		///  Constructs a resource Id from a <see cref="ushort"/> value Id.
		/// </summary>
		/// <param name="value">The integer resource Id.</param>
		public ResourceId(ushort value) {
			Value = value;
			name = null;
        }
		/// <summary>
		///  Constructs a resource Id from a well-known resource-type identifier.
		/// </summary>
		/// <param name="value">The well known resource type.</param>
		public ResourceId(ResourceTypes value) {
			Value = (ushort) value;
			name = null;
		}
		/// <summary>
		///  Constructs a resource Id from a string Id.
		/// </summary>
		/// <param name="value">The string resource Id</param>
		public ResourceId(string value) {
			name = value;
			Value = 0;
		}

		#endregion
		
		#region Properties

		/// <summary>
		///  Gets if the resource is null, or zero.
		/// </summary>
		public bool IsNull => Value == 0 && name == null;
		/// <summary>
		///  Gets if the resource is an integer resource.
		/// </summary>
		public bool IsIntResource => name == null;
		/// <summary>
		///  Gets the resource Id in a string format.
		/// </summary>
		public string Name => name ?? Value.ToString();
		/// <summary>
		///  Gets the string representation of a resource type name.
		/// </summary>
		public string TypeName => name ?? ResourceType.ToString();
        /// <summary>
        ///  Gets the enumerated resource type for built-in resource types only.
        /// </summary>
        public ResourceTypes ResourceType {
            get {
                if (IsIntResource)
                    return (ResourceTypes) Value;
                return ResourceTypes.Other;
            }
        }

		#endregion

		#region GetPtr

		/// <summary>
		///  Gets a disposable resource pointer that allocates strings into memory.
		/// </summary>
		/// <returns>The disposable resource pointer.</returns>
		/// 
		/// <remarks>
		///  <see cref="IDisposable.Dispose"/> must be called on the returned <see cref="ResourcePtr"/>.
		/// </remarks>
		public ResourcePtr GetPtr() => new ResourcePtr(this);

		#endregion

		#region Object Overrides

		/// <summary>
		///  Gets the string representation of the resource Id.
		/// </summary>
		/// <returns>The resource name.</returns>
		public override string ToString() => Name;

        /// <summary>
        ///  Gets the hash code for the resource Id. Either returns <see cref="Value"/> or
		///  <see cref="string.GetHashCode"/>.
        /// </summary>
        /// <returns>The resource Id hash code.</returns>
        public override int GetHashCode() => IsIntResource ? Value : name.GetHashCode();

        /// <summary>
        ///  Checks if the other object is a resource Id and is equal to this one.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if both resource Ids represent the same resource.</returns>
        public override bool Equals(object obj) {
			return (obj is ResourceId resId && Equals(resId));
		}
		/// <summary>
		///  Checks if the other resource Id is equal to this one.
		/// </summary>
		/// <param name="other">The other resource Id to compare.</param>
		/// <returns>True if both resource Ids represent the same resource.</returns>
		public bool Equals(ResourceId other) {
			if (IsIntResource && other.IsIntResource)
				return Value == other.Value;
			else if (!IsIntResource && !other.IsIntResource)
				return name == other.name;
			return false;
		}
		/// <summary>
		///  Checks if the other object is a resource Id and compares this resource Id to it.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of both resource Ids.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="obj"/> is not a <see cref="ResourceId"/>.
		/// </exception>
		public int CompareTo(object obj) {
			if (obj is ResourceId resId)
				return CompareTo(resId);
			throw new ArgumentException($"{nameof(obj)} is not of type {nameof(ResourceId)}!");
		}
		/// <summary>
		///  Compares this resource Id the other.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>The comparison of this resource Id to the other.</returns>
		public int CompareTo(ResourceId other) {
			// Strings come before int resources
			if (IsIntResource) {
				if (!other.IsIntResource)
					return 1;
				return Value.CompareTo(other.Value);
			}
			else {
				if (other.IsIntResource)
					return -1;
				return name.CompareTo(other.name);
			}
		}

		#endregion

		#region Operators

		public static bool operator ==(ResourceId a, ResourceId b) => a.Equals(b);
		public static bool operator !=(ResourceId a, ResourceId b) => !a.Equals(b);

		#endregion

		#region Casting

		public static implicit operator ResourceId(IntPtr value) => new ResourceId(value);
		public static implicit operator ResourceId(ushort value) => new ResourceId(value);
		public static implicit operator ResourceId(ResourceTypes value) => new ResourceId(value);
		public static implicit operator ResourceId(string value) => new ResourceId(value);

		#endregion
	}
}