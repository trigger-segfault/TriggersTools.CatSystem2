using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryReaderExtensions {
		#region ReadBE

		public static short ReadInt16BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			short value = reader.ReadInt16();
			unsafe { BitUtils.SwapBytes((byte*) &value, 2); }
			return value;
		}
		public static ushort ReadUInt16BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			ushort value = reader.ReadUInt16();
			unsafe { BitUtils.SwapBytes((byte*) &value, 2); }
			return value;
		}
		public static int ReadInt32BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			int value = reader.ReadInt32();
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			return value;
		}
		public static uint ReadUInt32BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			uint value = reader.ReadUInt32();
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			return value;
		}
		public static long ReadInt64BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			long value = reader.ReadInt64();
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			return value;
		}
		public static ulong ReadUInt64BE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			ulong value = reader.ReadUInt64();
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			return value;
		}
		public static float ReadSingleBE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			float value = reader.ReadSingle();
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			return value;
		}
		public static double ReadDoubleBE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			double value = reader.ReadDouble();
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			return value;
		}
		public static decimal ReadDecimalBE(this BinaryReader reader) {
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			decimal value = reader.ReadDecimal();
			unsafe { BitUtils.SwapBytes((byte*) &value, 16); }
			return value;
		}

		#endregion
	}
}
