using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.SharpUtils.IO {
	partial class BinaryWriterExtensions {

		public static void WriteBE(this BinaryWriter writer, short value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 2); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, int value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, long value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			writer.Write(value);
		}

		public static void WriteBE(this BinaryWriter writer, ushort value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 2); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, uint value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, ulong value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			writer.Write(value);
		}

		public static void WriteBE(this BinaryWriter writer, float value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 4); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, double value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 8); }
			writer.Write(value);
		}
		public static void WriteBE(this BinaryWriter writer, decimal value) {
			unsafe { BitUtils.SwapBytes((byte*) &value, 16); }
			writer.Write(value);
		}
	}
}
