
namespace TriggersTools.CatSystem2.Patcher.Patches {
	/// <summary>
	///  A range to look in when patching a binary file.
	/// </summary>
	public struct BinaryRange {
		/// <summary>
		///  The start range to look in.
		/// </summary>
		public long Start { get; }
		/// <summary>
		///  The end range to look in.
		/// </summary>
		public long End { get; }

		/// <summary>
		///  Constructs a binary range with just a start.
		/// </summary>
		/// <param name="start">The start position.</param>
		public BinaryRange(long start) : this(start, 0) { }
		/// <summary>
		///  Constructs a binary range with a start and end.
		/// </summary>
		/// <param name="start">The start position.</param>
		/// <param name="end">The end position.</param>
		public BinaryRange(long start, long end) {
			Start = start;
			End = end;
		}
	}
}
