using System;
using System.Runtime.InteropServices;

namespace TriggersTools.Resources {
	/// <summary>
	///  A pointer for a resource Id that can be used when updating a resource.
	/// </summary>
	public sealed class ResourcePtr : IDisposable {
		#region Fields

		/// <summary>
		///  Gets the assicated resource Id.
		/// </summary>
		public ResourceId Id { get; }
		/// <summary>
		///  Gets the allocated string or value pointer for the resource.
		/// </summary>
		public IntPtr Ptr { get; }

		#endregion

		#region Properties

		/// <summary>
		///  Gets if the resource is an integer resource and not a string.
		/// </summary>
		public bool IsIntResource => IsPtrIntResource(Ptr);

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a disposable resource pointer from a resource Id.
		/// </summary>
		/// <param name="resId">The resource Id to get the pointer from.</param>
		public ResourcePtr(ResourceId resId) {
			Id = resId;
			if (resId.IsIntResource)
				Ptr = new IntPtr(resId.Value);
			else
				Ptr = Marshal.StringToHGlobalUni(resId.Name);
		}

		#endregion

		#region Casting

		public static implicit operator IntPtr(ResourcePtr resPtr) {
			return resPtr?.Ptr ?? IntPtr.Zero;
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Disposes of the resource pointer if it is an allocated string.
		/// </summary>
		public void Dispose() {
			if (!IsIntResource)
				Marshal.FreeHGlobal(Ptr);
		}

		#endregion

		#region Static Helpers

		/// <summary>
		///  Returns true if the resource is an integer resource.
		/// </summary>
		/// <param name="value">The resource pointer.</param>
		public static bool IsPtrIntResource(IntPtr value) => (uint) value <= ushort.MaxValue;

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the resource Id.
		/// </summary>
		/// <returns>The resource pointer and value or name.</returns>
		public override string ToString() {
			if (IsIntResource)
				return $"0x{Ptr:X8} {Id.Value}";
			else
				return $"0x{Ptr:X8} \"{Id.Name}\"";
		}

		#endregion
	}
}
