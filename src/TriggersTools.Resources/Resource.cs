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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TriggersTools.Resources.Native;

namespace TriggersTools.Resources {
	/// <summary>
	///  An abstract resource.
	/// </summary>
	public abstract class Resource {
		#region Fields
		
		/// <summary>
		///  Gets the type of the resource.
		/// </summary>
		public ResourceId Type { get; }
		/// <summary>
		///  Gets the name of the resource.
		/// </summary>
		public ResourceId Name { get; }
		/// <summary>
		///  Gets the resource language Id.
		/// </summary>
		public ushort Language { get; }

		#endregion

		#region Constructors

		/*/// <summary>
		///  Constructs a resource of the specified type.
		/// </summary>
		/// <param name="type">The resource type.</param>
		public Resource(ResourceId type) {
			Type = type;
		}*/
		/// <summary>
		///  Constructs a resource of the specified type, name, and language.
		/// </summary>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		public Resource(ResourceId type, ResourceId name, ushort language) {
			Type = type;
			Name = name;
			Language = language;
		}
		/// <summary>
		///  Constructs and loads a resource of the specified type, name, and language.
		/// </summary>
		/// <param name="fileName">The module file to load the resource from.</param>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		protected Resource(string fileName, ResourceId type, ResourceId name, ushort language)
			: this(type, name, language)
		{
			LoadFrom(fileName);
		}
		/// <summary>
		///  Constructs and loads a resource of the specified type, name, and language.
		/// </summary>
		/// <param name="fileName">The module pointer to load the resource from.</param>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="language">The resource language.</param>
		protected Resource(IntPtr hModule, ResourceId type, ResourceId name, ushort language)
			: this(type, name, language)
		{
			LoadFrom(hModule);
		}
		
		#endregion
		
		#region Properties

		/// <summary>
		///  Gets the string representation of the resource type.
		/// </summary>
		public string TypeName => Type.TypeName;
		/// <summary>
		///  Gets the resource Id pair for this resource.
		/// </summary>
		public ResourceIdPair IdPair => new ResourceIdPair(this);

		#endregion

		#region Clone
		
		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		/// 
		/// <exception cref="NotImplementedException">
		///  This resource has not implemented this feature.
		/// </exception>
		public Resource Clone(ResourceId? name = null, ushort? language = null) {
			return CreateClone(name ?? Name, language ?? Language);
		}

		#endregion

		#region ToBytes

		/// <summary>
		///  Saves the resource to a byte array.
		/// </summary>
		/// <returns>The byte array of the resource.</returns>
		public byte[] ToBytes() {
			using (var ms = new MemoryStream()) {
				Write(ms);
				return ms.ToArray();
			}
		}

		#endregion

		#region SaveTo

		/// <summary>
		///  Saves the resource to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public void SaveTo(IntPtr hUpdate) {
			Update(hUpdate, false);
		}
		/// <summary>
		///  Saves the resource to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public void SaveTo(string fileName) {
			Update(fileName, false);
		}
		/// <summary>
		///  Saves all resources to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="resources">The resources to save.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void SaveTo(IntPtr hUpdate, params Resource[] resources) {
			Update(hUpdate, false, resources);
		}
		/// <summary>
		///  Saves all resources to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="resources">The resources to save.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void SaveTo(IntPtr hUpdate, IEnumerable<Resource> resources) {
			Update(hUpdate, false, resources);
		}
		/// <summary>
		///  Saves all resources to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="resources">The resources to save.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void SaveTo(string fileName, params Resource[] resources) {
			Update(fileName, false, resources);
		}
		/// <summary>
		///  Saves all resources to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="resources">The resources to save.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void SaveTo(string fileName, IEnumerable<Resource> resources) {
			Update(fileName, false, resources);
		}

		#endregion

		#region DeleteFrom

		/// <summary>
		///  Saves the resource to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public void DeleteFrom(IntPtr hUpdate) {
			Update(hUpdate, true);
		}
		/// <summary>
		///  Saves the resource to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public void DeleteFrom(string fileName) {
			Update(fileName, true);
		}
		/// <summary>
		///  Deletes all resources from the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="resources">The resources to delete.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void DeleteFrom(IntPtr hUpdate, params Resource[] resources) {
			Update(hUpdate, true, resources);
		}
		/// <summary>
		///  Deletes all resources from the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="resources">The resources to delete.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void DeleteFrom(IntPtr hUpdate, IEnumerable<Resource> resources) {
			Update(hUpdate, true, resources);
		}
		/// <summary>
		///  Deletes all resources from the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="resources">The resources to delete.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void DeleteFrom(string fileName, params Resource[] resources) {
			Update(fileName, true, resources);
		}
		/// <summary>
		///  Deletes all resources from the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="resources">The resources to delete.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		public static void DeleteFrom(string fileName, IEnumerable<Resource> resources) {
			Update(fileName, true, resources);
		}

		#endregion

		#region Abstract/Virtual Methods

		/// <summary>
		///  Reads the resource from the module handle and or stream. The stream's length is the size of the resource.
		/// </summary>
		/// <param name="hModule">The open module handle.</param>
		/// <param name="stream">The unmanaged stream to the resource.</param>
		protected abstract void Read(IntPtr hModule, Stream stream);
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected abstract void Write(Stream stream);
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		/// 
		/// <exception cref="NotImplementedException">
		///  This resource has not implemented this feature.
		/// </exception>
		protected virtual Resource CreateClone(ResourceId name, ushort language) {
			throw new NotImplementedException($"This resource has not implemented {nameof(CreateClone)}!");
		}

		#endregion

		#region LoadFrom

		/// <summary>
		///     Load a resource from an executable (.exe or .dll) file.
		/// </summary>
		/// <param name="fileName">An executable (.exe or .dll) file.</param>
		private void LoadFrom(string fileName) {
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			IntPtr hModule = IntPtr.Zero;
			try {
				hModule = Kernel32.LoadLibraryEx(
					fileName,
					IntPtr.Zero,
					LoadLibraryExFlags.DontResolveDllReferences | LoadLibraryExFlags.LoadLibraryAsDatafile);
					//LoadLibraryExFlags.LoadLibraryAsDatafileExclusive | LoadLibraryExFlags.LoadLibraryAsImageResource);
				if (hModule == IntPtr.Zero)
					throw new Win32Exception();

				LoadFrom(hModule);
			} finally {
				if (hModule != IntPtr.Zero)
					Kernel32.FreeLibrary(hModule);
			}
		}

		/// <summary>
		///  Load a resource from an executable (.exe or .dll) module.
		/// </summary>
		/// <param name="hModule">An executable (.exe or .dll) module.</param>
		/// <param name="type">The resource type.</param>
		/// <param name="name">The resource name.</param>
		/// <param name="lang">The resource language.</param>
		private unsafe void LoadFrom(IntPtr hModule) {
			IntPtr hRes, hGlobal, lpRes;
			if (hModule == IntPtr.Zero)
				throw new ArgumentNullException(nameof(hModule));

			using (var typeId = Type.GetPtr())
			using (var nameId = Name.GetPtr())
				hRes = Kernel32.FindResourceEx(hModule, typeId, nameId, Language);
			if (hRes == IntPtr.Zero)
				throw new Win32Exception();

			hGlobal = Kernel32.LoadResource(hModule, hRes);
			if (hGlobal == IntPtr.Zero)
				throw new Win32Exception();

			lpRes = Kernel32.LockResource(hGlobal);
			if (lpRes == IntPtr.Zero)
				throw new Win32Exception();

			int size = Kernel32.SizeofResource(hModule, hRes);
			if (size == 0)
				throw new Win32Exception();

			using (var ms = new UnmanagedMemoryStream((byte*) lpRes.ToPointer(), size)) {
				Read(hModule, ms);
			}
		}

		#endregion

		#region Update

		/// <summary>
		///  Updates the resource to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="delete">True if the resource should be deleted.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		private void Update(IntPtr hUpdate, bool delete) {
			if (hUpdate == IntPtr.Zero)
				throw new ArgumentNullException(nameof(hUpdate));
			using (var typeId = Type.GetPtr())
			using (var nameId = Name.GetPtr()) {
				byte[] data = (delete ? null : ToBytes());
				int length = (data == null ? 0 : data.Length);
				if (!Kernel32.UpdateResource(hUpdate, typeId, nameId, Language, data, length))
					throw new Win32Exception();
			}
		}
		/// <summary>
		///  Updates the resource to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="delete">True if the resource should be deleted.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		private void Update(string fileName, bool delete) {
			Update(fileName, delete, new[] { this });
		}
		/// <summary>
		///  Updates all resources to the update handle of the module.
		/// </summary>
		/// <param name="hUpdate">The update handle of the module.</param>
		/// <param name="delete">True if the resource should be deleted.</param>
		/// <param name="resources">The resources to update.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="hUpdate"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		private static void Update(IntPtr hUpdate, bool delete, IEnumerable<Resource> resources) {
			if (resources == null)
				throw new ArgumentNullException(nameof(resources));
			foreach (Resource r in resources) {
				r.Update(hUpdate, delete);
			}
		}
		/// <summary>
		///  Updates all resources to the module file.
		/// </summary>
		/// <param name="fileName">The module file name.</param>
		/// <param name="delete">True if the resource should be deleted.</param>
		/// <param name="resources">The resources to update.</param>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="fileName"/> or <paramref name="resources"/> is null.
		/// </exception>
		/// <exception cref="Win32Exception">
		///  A native error occurred.
		/// </exception>
		private static void Update(string fileName, bool delete, IEnumerable<Resource> resources) {
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			IntPtr hUpdate = Kernel32.BeginUpdateResource(fileName, false);
			if (hUpdate == IntPtr.Zero)
				throw new Win32Exception();

			Update(hUpdate, delete, resources);

			if (!Kernel32.EndUpdateResource(hUpdate, false))
				throw new Win32Exception();
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the resource.
		/// </summary>
		/// <returns>The string representation of the resource.</returns>
		public override string ToString() => $"{TypeName} : {Name} : {Language}";

		#endregion
	}
}