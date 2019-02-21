
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggersTools.CatSystem2.Attributes;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  An enum for determining how a KIFINT archive's files are used.
	/// </summary>
	public enum KifintKnownType {
		/// <summary>The file type is known, and can be used without special conversion.</summary>
		Known = 0,
		/// <summary>The file type is "known", but requires the use of an existing converter.</summary>
		Convert = 1,
		/// <summary>The file type is unknown and cannot be used without further investigation.</summary>
		Unknown = 2,
	}
	/// <summary>
	///  The types of known KIFINT archive files.
	/// </summary>
	public enum KifintType {
		/// <summary>No special file type. Use default methods for handling the archive.</summary>
		Unknown = 0,

		/// <summary>.ogg background music</summary>
		[KifintWildcard("bgm*.int")]
		[KifintFileTypes(KifintKnownType.Known, ".ogg")]
		Bgm,

		/// <summary>.cvs unknown | .txt unknown | .xml config | .dat cglist</summary>
		[KifintWildcard("config.int")]
		[KifintFileTypes(KifintKnownType.Known, ".cvs", ".txt", ".xml")]
		[KifintFileTypes(KifintKnownType.Unknown, ".dat")]
		Config,

		/// <summary>.zt packed data to export from game</summary>
		[KifintWildcard("export.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".zt")]
		Export,

		/// <summary>.fes unknown</summary>
		[KifintWildcard("fes.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".fes")]
		Fes,

		/// <summary>.RRD unknown (likely fonts)</summary>
		[KifintWildcard("font.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".RRD")]
		Font,

		/// <summary>.ogg sound effects</summary>
		[KifintWildcard("hse*.int")]
		[KifintFileTypes(KifintKnownType.Known, ".ogg")]
		Hse,

		/// <summary>.hg3 images | .anm animations</summary>
		[KifintWildcard("image*.int")]
		[KifintFileTypes(KifintKnownType.Convert, ".anm", ".hg3")]
		Image,

		/// <summary>.js javascripts</summary>
		[KifintWildcard("js.int")]
		[KifintFileTypes(KifintKnownType.Known, ".js")]
		Js,

		/// <summary>.kcs script data</summary>
		[KifintWildcard("kcs.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kcs")]
		Kcs,

		/// <summary>.kx2 plain data</summary>
		[KifintWildcard("kx2.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kx2")]
		Kx2,

		/// <summary>.kx3 plain data</summary>
		[KifintWildcard("kx3.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kx3")]
		Kx3,

		/// <summary>.??? 3D scene data</summary>
		[KifintWildcard("kxs.int")]
		//[KifintFileTypes(KifintKnownType.Unknown, ".kx2")]
		Kxs,

		/// <summary>.kcs motion scripts</summary>
		[KifintWildcard("mot.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kcs")]
		Mot,

		/// <summary>.mpg movies</summary>
		[KifintWildcard("movie*.int")]
		[KifintFileTypes(KifintKnownType.Known, ".mpg")]
		Movie,

		/// <summary>.ogg voices</summary>
		[KifintWildcard("pcm_?.int")]
		[KifintFileTypes(KifintKnownType.Known, ".ogg")]
		Pcm,

		/// <summary>.ogg voices</summary>
		[KifintWildcard("pcm_tag.int")]
		[KifintFileTypes(KifintKnownType.Known, ".ogg")]
		PcmTag,

		/// <summary>.kcs partcle scripts</summary>
		[KifintWildcard("ptcl.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kcs")]
		Ptcl,

		/// <summary>.kcs partcle script plug-ins</summary>
		[KifintWildcard("ptclpi.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".kcs")]
		Ptclpi,

		/// <summary>.cst scene scripts</summary>
		[KifintWildcard("scene*.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".cst")]
		Scene,

		/// <summary>.ogg sound effects</summary>
		[KifintWildcard("se*.int")]
		[KifintFileTypes(KifintKnownType.Known, ".ogg")]
		Se,

		/// <summary>.shd shader files</summary>
		[KifintWildcard("shd.int")]
		[KifintFileTypes(KifintKnownType.Unknown, ".shd")]
		Shd,

		/// <summary>.hg3 textures</summary>
		[KifintWildcard("texture.int")]
		[KifintFileTypes(KifintKnownType.Convert, ".hg3")]
		Texture,

		/// <summary>.bmp wallpapers</summary>
		[KifintWildcard("wp.int")]
		[KifintFileTypes(KifintKnownType.Known, ".bmp")]
		Wp,

		/// <summary>Archive used to overwrite other archive entries.</summary>
		[KifintWildcard("update*.int")]
		Update,
	}
	public class KifintTypeInfo {

		public bool IsUpdate { get; }

		public IReadOnlyList<string> Wildcards { get; }

		public IReadOnlyList<string> Extensions { get; }

		public KifintTypeInfo(string wildcard, params string[] extensions)
			: this(new string[] { wildcard }, false, extensions)
		{
		}
		public KifintTypeInfo(string[] wildcards, params string[] extensions)
			: this(wildcards, false, extensions)
		{
		}
		internal KifintTypeInfo(string wildcard, bool isUpdate, params string[] extensions)
			: this(new string[] { wildcard }, isUpdate, extensions)
		{
		}
		internal KifintTypeInfo(string[] wildcards, bool isUpdate, params string[] extensions) {
			Wildcards = Array.AsReadOnly(wildcards.Select(w => AddExtension(w)).ToArray());
			Extensions = Array.AsReadOnly(extensions);
			IsUpdate = isUpdate;
		}

		private string AddExtension(string wildcard) {
			if (!Path.HasExtension(wildcard))
				return Path.ChangeExtension(wildcard, ".int");
			return wildcard;
		}
	}
}
