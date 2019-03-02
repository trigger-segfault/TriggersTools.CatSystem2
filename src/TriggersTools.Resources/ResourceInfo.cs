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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TriggersTools.Resources.Dialog;
using TriggersTools.Resources.Manifest;
using TriggersTools.Resources.Menu;
using TriggersTools.Resources.Native;
using TriggersTools.Resources.StringTable;
using TriggersTools.SharpUtils.Exceptions;

namespace TriggersTools.Resources {

	/// <summary>
	///     Resource info manager.
	/// </summary>
	public class ResourceInfo : IEnumerable<Resource>, IDisposable {
		#region Fields
		
		/// <summary>
		///  The inner exception thrown during native resource enumeration.<para/>
		///  This exists because exceptions cannot be caught through these calls.
		/// </summary>
		private Exception innerException;

		/// <summary>
		///     A dictionary of resources, the key is the resource type, eg. "REGISTRY" or "16" (version).
		/// </summary>
		private readonly Dictionary<ResourceId, ResourceCollection> resourceLists
			= new Dictionary<ResourceId, ResourceCollection>();
		//public Dictionary<ResourceId, ResourceCollection> resourceLists { get; }
		//	= new Dictionary<ResourceId, ResourceCollection>();

		#endregion

		#region Constructors

		public ResourceInfo() { }
		public ResourceInfo(string fileName, bool loadResources = true) {
			if (fileName == null)
				throw new ArgumentNullException(nameof(fileName));
			Load(fileName, loadResources);
		}
		public ResourceInfo(IntPtr hModule, bool loadResources = true) {
			if (hModule == IntPtr.Zero)
				throw new ArgumentNullException(nameof(hModule));
			Load(hModule, loadResources);
		}

		#endregion

		#region Properties

		public IReadOnlyCollection<ResourceId> ResourceTypes => resourceLists.Keys;

		public int Count => resourceLists.Values.Sum(r => r.Count);

		public IntPtr ModuleHandle { get; private set; }

		/// <summary>
		///     A collection of resources.
		/// </summary>
		/// <param name="type">Resource type.</param>
		/// <returns>A collection of resources of a given type.</returns>
		public ResourceCollection this[ResourceId type] {
			get => resourceLists[type];
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				if (value.Type != type)
					throw new ArgumentException($"{nameof(ResourceCollection)}.{nameof(ResourceCollection.Type)} " +
												$"{value.Type} must be the same type as {nameof(type)} {type}!");
				resourceLists[type] = value;
			}
		}

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///     Enumerates all resources within this resource info collection.
		/// </summary>
		/// <returns>Resources enumerator.</returns>
		public IEnumerator<Resource> GetEnumerator() {
			var resourceTypesEnumerator = resourceLists.GetEnumerator();
			while (resourceTypesEnumerator.MoveNext()) {
				var resourceEnumerator = resourceTypesEnumerator.Current.Value.GetEnumerator();
				while (resourceEnumerator.MoveNext()) {
					yield return resourceEnumerator.Current;
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///  Disposes of the resource info if loadResources was false when constructing from a fileName.
		/// </summary>
		public void Dispose() {
			if (ModuleHandle != IntPtr.Zero) {
				ModuleHandle = IntPtr.Zero;
				Kernel32.FreeLibrary(ModuleHandle);
			}
		}

		#endregion

		public void Add(ResourceCollection collection) {
			if (resourceLists.ContainsKey(collection.Type)) {
				throw new ArgumentException($"Already contains a {nameof(ResourceCollection)} of type " +
											$"{collection.Type}!");
			}
			resourceLists.Add(collection.Type, collection);
		}
		public bool Remove(ResourceCollection collection) {
			var pair = new KeyValuePair<ResourceId, ResourceCollection>(collection.Type, collection);
			return ((ICollection<KeyValuePair<ResourceId, ResourceCollection>>) resourceLists).Remove(pair);
		}
		public bool Remove(ResourceId type) {
			return resourceLists.Remove(type);
		}

		public bool Add(Resource resource) {
			if (!resourceLists.TryGetValue(resource.Type, out var resources)) {
				resources = new ResourceCollection(resource.Type);
				resourceLists[resource.Type] = resources;
			}
			return resources.Add(resource);
		}
		public bool Remove(Resource resource) {
			if (!resourceLists.TryGetValue(resource.Type, out var resources))
				return false;
			return resources.Remove(resource);
		}

		/// <summary>
		///     Save resource to a file.
		/// </summary>
		/// <param name="fileName">Target filename.</param>
		public void Save(string fileName) {
			Resource.SaveTo(fileName, this);
		}


		/// <summary>
		///     Load an executable or a DLL and read its resources.
		/// </summary>
		/// <param name="fileName">Source filename.</param>
		private void Load(string fileName, bool loadResources) {
			// load DLL
			IntPtr hModule = Kernel32.LoadLibraryEx(
				fileName,
				IntPtr.Zero,
				LoadLibraryExFlags.DontResolveDllReferences | LoadLibraryExFlags.LoadLibraryAsDatafile);
				//LoadLibraryExFlags.LoadLibraryAsDatafileExclusive | LoadLibraryExFlags.LoadLibraryAsImageResource);
			if (IntPtr.Zero == hModule)
				throw new Win32Exception();
			try {
				if (!loadResources) {
					ModuleHandle = hModule;
					return;
				}
				// enumerate resource types
				// for each type, enumerate resource names
				// for each name, enumerate resource languages
				// for each resource language, enumerate actual resources
				if (!Kernel32.EnumResourceTypes(hModule, EnumResourceTypesImpl, IntPtr.Zero)) {
					innerException?.Rethrow();
					throw new Win32Exception();
				}
			} catch (Exception ex) {
				throw new ResourceLoadException(string.Format("Error loading '{0}'.", fileName), innerException, ex);
			} finally {
				if (loadResources)
					Kernel32.FreeLibrary(hModule);
			}
		}
		private void Load(IntPtr hModule, bool loadResources) {
			try {
				if (!loadResources) {
					ModuleHandle = hModule;
					return;
				}
				// enumerate resource types
				// for each type, enumerate resource names
				// for each name, enumerate resource languages
				// for each resource language, find the resource and add it
				if (!Kernel32.EnumResourceTypes(hModule, EnumResourceTypesImpl, IntPtr.Zero)) {
					innerException?.Rethrow();
					throw new Win32Exception();
				}
			} catch (Exception ex) {
				throw new ResourceLoadException(string.Format("Error loading '{0}'.", hModule), innerException, ex);
			}
		}

		/// <summary>
		///     Create a resource of a given type.
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="hGlobal">Pointer to the resource in memory.</param>
		/// <param name="type">Resource type.</param>
		/// <param name="name">Resource name.</param>
		/// <param name="language">Language ID.</param>
		/// <param name="size">Size of resource.</param>
		/// <returns>A specialized or a generic resource.</returns>
		protected Resource CreateResource(IntPtr hModule, ResourceId type, ResourceId name, ushort language) {
			if (type.IsIntResource) {
				switch (type.ResourceType) {
				/*case Resources.ResourceTypes.RT_VERSION:
					return new VersionResource(hModule, name, language);
				case Resources.ResourceTypes.RT_GROUP_CURSOR:
					return new CursorDirectoryResource(hModule, name, language);
				case Resources.ResourceTypes.RT_GROUP_ICON:
					return new IconDirectoryResource(hModule, name, language);*/
				case Resources.ResourceTypes.Manifest:
					return new ManifestResource(hModule, name, language);
				/*case Resources.ResourceTypes.RT_BITMAP:
					return new BitmapResource(hModule, hResourceGlobal, type, name, wIDLanguage, size);*/
				case Resources.ResourceTypes.Menu:
					return new MenuResource(hModule, name, language);
				case Resources.ResourceTypes.Dialog:
					return new DialogResource(hModule, name, language);
				case Resources.ResourceTypes.String:
					return new StringResource(hModule, name, language);
				/*case Resources.ResourceTypes.RT_FONTDIR:
					return new FontDirectoryResource(hModule, name, language);
				case Resources.ResourceTypes.RT_FONT:
					return new FontResource(hModule, name, language);
				case Resources.ResourceTypes.RT_ACCELERATOR:
					return new AcceleratorResource(hModule, name, language);*/
				}
			}

			return new GenericResource(hModule, type, name, language);
		}

		/// <summary>
		///     Enumerate resource types.
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceTypesImpl(IntPtr hModule, IntPtr lpszType, IntPtr lParam) {
			//var type = new ResourceId(lpszType);
			//_resourceTypes.Add(type);

			// enumerate resource names
			if (!Kernel32.EnumResourceNames(hModule, lpszType, EnumResourceNamesImpl, IntPtr.Zero)) {
				innerException?.Rethrow();
				throw new Win32Exception();
			}

			return true;
		}
		/// <summary>
		///     Enumerate resource names within a resource by type
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lpszName">Resource name.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceNamesImpl(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam) {
			if (!Kernel32.EnumResourceLanguages(hModule, lpszType, lpszName, EnumResourceLanguages, IntPtr.Zero)) {
				innerException?.Rethrow();
				throw new Win32Exception();
			}

			return true;
		}
		/// <summary>
		///     Enumerate resource languages within a resource by name
		/// </summary>
		/// <param name="hModule">Module handle.</param>
		/// <param name="lpszType">Resource type.</param>
		/// <param name="lpszName">Resource name.</param>
		/// <param name="wIDLanguage">Language ID.</param>
		/// <param name="lParam">Additional parameter.</param>
		/// <returns>TRUE if successful.</returns>
		private bool EnumResourceLanguages(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, ushort wIDLanguage, IntPtr lParam) {
			ResourceId type = lpszType;
			ResourceId name = lpszName;
			try {
				if (!resourceLists.TryGetValue(type, out var resources)) {
					resources = new ResourceCollection(type);
					resourceLists[type] = resources;
				}
				resources.Add(CreateResource(hModule, type, name, wIDLanguage));
			} catch (Exception ex) {
				innerException = new Exception(string.Format("Error loading resource '{0}' {1} ({2}).", name, type.TypeName, wIDLanguage), ex);
				throw innerException;
			}

			return true;
		}
	}
}