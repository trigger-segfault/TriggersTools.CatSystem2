using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TriggersTools.SharpUtils.Exceptions;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.StringTable {
	/// <summary>
	///  A string, RT_STRING resource.<para/>
	///  Each string resource block has 16 strings, each represented as an ordered pair
	///  (length, text).<para/>Length is a WORD that specifies the size, in terms of the number of characters,
	///  in the text that follows.<para/>Text follows length and contains the string in Unicode without the
	///  null terminating character.<para/>There may be no characters in text, in which case length is zero.
	/// </summary>
	public sealed class StringResource : Resource, IEnumerable<StringEntry> {
		#region Constants

		/// <summary>
		///  The total number of strings allotted for a <see cref="StringResource"/> and <see cref="StringTable"/>.
		/// </summary>
		public const int StringsPerTable = 16;

		#endregion

		#region Fields
		
		/// <summary>
		///  The fixed array of <see cref="StringsPerTable"/> strings.
		/// </summary>
		private readonly string[] strings = new string[StringsPerTable];

		#endregion

		#region Constructors

		//public StringResource() : base(ResourceTypes.RT_STRING) { }
		public StringResource(ResourceId name, ushort language)
			: base(ResourceTypes.String, name, language)
		{
		}
		public StringResource(string fileName, ResourceId name, ushort language)
			: base(fileName, ResourceTypes.String, name, language)
		{
		}
		public StringResource(IntPtr hModule, ResourceId name, ushort language)
			: base(hModule, ResourceTypes.String, name, language)
		{
		}

		#endregion

		#region Properties
		
		/// <summary>
		///  Gets or sets the string with the given string Id.
		/// </summary>
		/// <param name="stringId">The string Id, which is associated with <see cref="BlockId"/>.</param>
		/// <returns>A string of a given Id.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="stringId"/> does not have the same block Id as <see cref="BlockId"/>.
		/// </exception>
		public string this[ushort stringId] {
            get => strings[GetStringIndex(BlockId, stringId)];
			set => strings[GetStringIndex(BlockId, stringId)] = value;
        }
        /// <summary>
        ///  Gets the string table block Id.
        /// </summary>
        public ushort BlockId => Name.Value;
		/// <summary>
		///  Gets the last block Id in this string table.
		/// </summary>
		public ushort EndBlockId => (ushort) (Name.Value - 1 + StringsPerTable);

		#endregion

		#region GetBlockId
		
		/// <summary>
		///  Gets the block Id from the <paramref name="stringId"/>.
		/// </summary>
		/// <param name="stringId">The string Id.</param>
		/// <returns>The block Id.</returns>
		public static ushort GetBlockId(ushort stringId) {
            return (ushort) (stringId / StringsPerTable + 1);
        }
		/// <summary>
		///  Gets the string Id from the <paramref name="blockId"/> and the <paramref name="index"/>.
		/// </summary>
		/// <param name="blockId">The block Id of the string resource.</param>
		/// <param name="index">The index of the string.</param>
		/// <returns>The string Id.</returns>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		///  <paramref name="index"/> is less than zero or greater than or equal to <see cref="StringsPerTable"/>.
		/// </exception>
		public static ushort GetStringId(ushort blockId, int index) {
			if (index < 0 || index > StringsPerTable) {
				throw ArgumentOutOfRangeUtils.OutsideRange(nameof(index), index, 0, nameof(StringsPerTable),
					StringsPerTable, true, false);
			}
			return (ushort) ((blockId - 1) * StringsPerTable + index);
		}
		/// <summary>
		///  Gets the index of the string from the <paramref name="stringId"/> and <paramref name="blockId"/>.
		/// </summary>
		/// <param name="blockId">The block Id of the string resource.</param>
		/// <param name="stringId">The string Id of the string.</param>
		/// <returns>The index of the string.</returns>
		/// 
		/// <exception cref="ArgumentException">
		///  <paramref name="stringId"/> does not have the same block Id as <paramref name="blockId"/>.
		/// </exception>
		public static ushort GetStringIndex(ushort blockId, ushort stringId) {
			ushort stringBlockId = GetBlockId(stringId);
			if (stringBlockId != blockId)
				throw new ArgumentException($"{nameof(stringId)} {stringId} is not within" +
											$"{nameof(blockId)} {blockId}!");
			return (ushort) (stringId - (blockId - 1) * StringsPerTable);
		}

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		public new StringResource Clone(ResourceId? name = null, ushort? language = null) {
			return (StringResource) base.Clone(name, language);
		}

		#endregion

		#region Resource Overrides

		/// <summary>
		///  Reads the resource from the module handle and or stream. The stream's length is the size of the resource.
		/// </summary>
		/// <param name="hModule">The open module handle.</param>
		/// <param name="stream">The unmanaged stream to the resource.</param>
		protected override void Read(IntPtr hModule, Stream stream) {
			BinaryReader reader = new BinaryReader(stream, Encoding.Unicode);
			for (int i = 0; i < StringsPerTable; i++) {
				ushort length = reader.ReadUInt16();
				if (length != 0)
					strings[i] = reader.ReadFixedString(length);
			}
			//table.Read(reader);
		}
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected override void Write(Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode);
			for (int i = 0; i < StringsPerTable; i++) {
				string s = strings[i];
				if (s != null) {
					writer.Write((ushort) s.Length);
					writer.WriteFixed(s);
				}
				else {
					writer.Write((ushort) 0);
				}
			}
			//table.Write(writer);
		}
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		protected override Resource CreateClone(ResourceId name, ushort language) {
			StringResource table = new StringResource(name, language);
			Array.Copy(strings, table.strings, StringsPerTable);
			return table;
		}

		#endregion

		#region IEnumerable Implementation

		/// <summary>
		///  Gets the enumerator for the strings at the specified index in the table.
		/// </summary>
		/// <returns>The key value pair enumerator for strings and their indecies.</returns>
		public IEnumerator<StringEntry> GetEnumerator() {
			for (int i = 0; i < StringsPerTable; i++) {
				string s = strings[i];
				if (s != null)
					yield return new StringEntry(GetStringId(BlockId, i), s);
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the string resource in the STRINGTABLE format.
		/// </summary>
		/// <returns>The string representation of the string resource.</returns>
		public override string ToString() => $"{base.ToString()} STRINGTABLE";
		
		#endregion
	}
}