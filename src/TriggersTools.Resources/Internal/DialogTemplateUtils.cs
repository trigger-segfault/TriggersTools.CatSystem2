using System.IO;
using TriggersTools.Resources.Dialog;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Internal {
	/// <summary>
	///     Dialog template utility functions.
	/// </summary>
	internal static class DialogTemplateUtils {
		public static ResourceId ReadResourceId(BinaryReader reader) {
			switch (reader.ReadUInt16()) {
			case 0x0000: // no predefined resource
				return ResourceId.Null;
			case 0xFFFF: // one additional element that specifies the ordinal value of the resource
				return new ResourceId(reader.ReadUInt16());
			default: // null-terminated Unicode string that specifies the name of the resource
				reader.BaseStream.Position -= 2;
				//reader.BaseStream.SkipPadding(2);
				return new ResourceId(reader.ReadTerminatedString());
			}
		}

		public static void WriteResourceId(BinaryWriter writer, ResourceId rc) {
			if (rc.IsNull) {
				writer.Write((ushort) 0x0000);
			}
			else if (rc.IsIntResource) {
				writer.Write((ushort) 0xFFFF);
				writer.Write((ushort) rc.Value);
			}
			else {
				//writer.Pad(2);
				writer.WriteTerminated(rc.Name);
			}
		}


		public static void ReadDialogTemplateBase(BinaryReader reader, IDialogBaseTemplate dialog) {
			dialog.MenuId = DialogTemplateUtils.ReadResourceId(reader);
			dialog.WindowClassId = DialogTemplateUtils.ReadResourceId(reader);
			dialog.Caption = reader.ReadTerminatedString();

			if (dialog.HasFont)
				dialog.PointSize = reader.ReadUInt16();
		}
		public static void WriteDialogTemplateBase(BinaryWriter writer, IDialogBaseTemplate dialog) {
			DialogTemplateUtils.WriteResourceId(writer, dialog.MenuId);
			DialogTemplateUtils.WriteResourceId(writer, dialog.WindowClassId);
			writer.WriteTerminated(dialog.Caption ?? string.Empty);

			if (dialog.HasFont)
				writer.Write(dialog.PointSize);
		}

		public static void ReadDialogTemplateControlBase(BinaryReader reader, IDialogBaseTemplateControl control) {
			control.ControlClassId = DialogTemplateUtils.ReadResourceId(reader);
			control.CaptionId = DialogTemplateUtils.ReadResourceId(reader);

			//reader.BaseStream.SkipPadding(2);
			ushort length = reader.ReadUInt16();
			control.CreationData = reader.ReadBytes(length);
		}
		public static void WriteDialogTemplateControlBase(BinaryWriter writer, IDialogBaseTemplateControl control) {
			DialogTemplateUtils.WriteResourceId(writer, control.ControlClassId);
			DialogTemplateUtils.WriteResourceId(writer, control.CaptionId);

			//writer.Pad(2);
			if (control.CreationData == null) {
				writer.Write((ushort) 0x0000);
			}
			else {
				writer.Write((ushort) control.CreationData.Length);
				writer.Write(control.CreationData);
			}
		}
	}
}