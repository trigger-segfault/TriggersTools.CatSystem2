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
using System.IO;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Dialog {
	/// <summary>
	///     A container for a control within a dialog template.
	/// </summary>
	public class DialogTemplateControl : BinaryReadableWriteable, IDialogBaseTemplateControl {
		#region Fields

		private DLGITEMTEMPLATE header;
		/// <summary>
		///     Dialog caption.
		/// </summary>
		public ResourceId CaptionId { get; set; }
		/// <summary>
		///     Window class Id.
		/// </summary>
		public ResourceId ControlClassId { get; set; }
		/// <summary>
		///     Additional creation data.
		/// </summary>
		public byte[] CreationData { get; set; }

		#endregion

		/// <summary>
		///     Window class of the control.
		/// </summary>
		public DialogItemClass ControlClass => (DialogItemClass) ControlClassId.Value;

		/// <summary>
		///     X-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short X {
			get => header.x;
			set => header.x = value;
		}

		/// <summary>
		///     Y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short Y {
			get => header.y;
			set => header.y = value;
		}

		/// <summary>
		///     Width, in dialog box units, of the dialog box.
		/// </summary>
		public short Width {
			get => header.cx;
			set => header.cx = value;
		}

		/// <summary>
		///     Height, in dialog box units, of the dialog box.
		/// </summary>
		public short Height {
			get => header.cy;
			set => header.cy = value;
		}

		/// <summary>
		///     Dialog style.
		/// </summary>
		public uint Style {
			get => header.style;
			set => header.style = value;
		}

		/// <summary>
		///     Extended dialog style.
		/// </summary>
		public uint ExtendedStyle {
			get => header.dwExtendedStyle;
			set => header.dwExtendedStyle = value;
		}

		/// <summary>
		///     Control identifier.
		/// </summary>
		public short Id {
			get => header.id;
			set => header.id = value;
		}

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<DLGITEMTEMPLATE>();
			DialogTemplateUtils.ReadDialogTemplateControlBase(reader, this);
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			writer.WriteUnmanaged(header);
			DialogTemplateUtils.WriteDialogTemplateControlBase(writer, this);
		}

		#endregion
		
		/// <summary>
		///  Creates a clone of this dialog control template.
		/// </summary>
		/// <returns>A clone of this dialog control template.</returns>
		public DialogTemplateControl Clone() {
			DialogTemplateControl control = new DialogTemplateControl {
				header = header,
				CaptionId = CaptionId,
				ControlClassId = ControlClassId,
			};
			if (CreationData != null) {
				control.CreationData = new byte[CreationData.Length];
				Array.Copy(CreationData, control.CreationData, CreationData.Length);
			}
			return control;
		}
		IDialogBaseTemplateControl IDialogBaseTemplateControl.Clone() => Clone();

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the dialog control in the DIALOG format.
		/// </summary>
		/// <returns>The string representation of the dialog control.</returns>
		public override string ToString() => $"CONTROL \"{CaptionId}\"";

		#endregion
	}
}