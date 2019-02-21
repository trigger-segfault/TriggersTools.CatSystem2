using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using ClrPlus.Windows.Api.Enumerations;
using ClrPlus.Windows.PeBinary.ResourceLib;
using ClrPlus.Windows.PeBinary.Utility;

namespace TriggersTools.CatSystem2.Patcher {
	public class ProgramPatcher {

		//public PatchMarker PatchMarker { get; set; }

		private PatchSignaturePatch signaturePatcher;

		public string MD5 { get; set; }

		public string Signature {
			get => signaturePatcher?.Signature;
			set => signaturePatcher = (value == null ? null : new PatchSignaturePatch(value));
		}


		public void Add(IResourcePatch resourcePatch) => ResourcePatches.Add(resourcePatch);
		public void Add(BinaryStringsPatch binaryPatch) => BinaryPatches.Add(binaryPatch);

		public void AddEnableVisualStyles() {
			Add(new EnableVisualStylesPatch());
		}
		public void AddEnableVisualStyles(string typeFace, ushort pointSize) {
			Add(new EnableVisualStylesPatch());
			Add(new TypeFacePatch(typeFace, pointSize));
		}
		public void AddNewLanguage(ushort language, ushort codePage) {
			Add(new LanguagePatch(language, codePage));
		}

		public List<IResourcePatch> ResourcePatches { get; } = new List<IResourcePatch>();
		public List<BinaryStringsPatch> BinaryPatches { get; } = new List<BinaryStringsPatch>();

		public bool Patch(string srcFileName, string dstFileName) {
			if (MD5 != null) {
				if (PEInfo.Scan(srcFileName).MD5 != MD5)
					return false;
			}
			File.Copy(srcFileName, dstFileName, true);
			if (BinaryPatches.Count != 0) {
				Thread.Sleep(100);
				using (Stream inStream = File.OpenRead(srcFileName))
				using (Stream outStream = File.OpenWrite(dstFileName)) {
					foreach (BinaryStringsPatch patch in BinaryPatches) {
						if (!patch.Patch(inStream, outStream))
							return false;
					}
				}
			}
			if (signaturePatcher != null || ResourcePatches.Count != 0) {
				using (ResourceInfo resourceInfo = new ResourceInfo()) {
					resourceInfo.Load(srcFileName);

					HashSet<Resource> patchedResources = new HashSet<Resource>();

					ManifestResource manifest = (ManifestResource) resourceInfo[ResourceTypes.RT_MANIFEST].FirstOrDefault();
					if (signaturePatcher != null) {
						if (manifest == null)
							return false;
						if (!signaturePatcher.Patch(manifest))
							return false;
						patchedResources.Add(manifest);
					}
					foreach (Resource resource in resourceInfo.Resources.Values.SelectMany(list => list)) {
						foreach (IResourcePatch patch in ResourcePatches) {
							if (patch.IsPatchable(resource)) {
								if (!patch.Patch(resource))
									return false;
								patchedResources.Add(resource);
							}
						}
					}
					if (!Patch(resourceInfo, patchedResources))
						return false;

					Thread.Sleep(100);
					foreach (Resource resource in patchedResources) {
						resource.SaveTo(dstFileName);
					}
				}
			}
			

			return true;
		}

		protected virtual bool Patch(ResourceInfo resourceInfo, HashSet<Resource> patchedResources) {
			return true;
		}
	}
}
