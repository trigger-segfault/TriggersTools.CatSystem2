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
using System.IO;
using System.Linq;
using TriggersTools.Resources.Enumerations;
using TriggersTools.Resources.Internal;
using TriggersTools.Resources.Native.Structures;
using TriggersTools.SharpUtils.IO;

namespace TriggersTools.Resources.Dialog {
	/// <summary>
	///  A dialog extended template for a dialog resource.
	/// </summary>
	public class DialogExTemplate : BinaryReadableWriteable, IDialogBaseTemplate {
		#region Fields

		/// <summary>
		///  The header for the dialog.
		/// </summary>
		private DLGTEMPLATEEX header;
		/// <summary>
		///  The list of controls for the dialog.
		/// </summary>
		private List<DialogExTemplateControl> controls = new List<DialogExTemplateControl>();

		/// <summary>
		///  Gets or sets the dialog caption.
		/// </summary>
		public string Caption { get; set; }
		/// <summary>
		///  Gets or sets the dialog menu resource Id.
		/// </summary>
		public ResourceId MenuId { get; set; }
		/// <summary>
		///  Gets or sets the dialog window class Id.
		/// </summary>
		public ResourceId WindowClassId { get; set; }
		/// <summary>
		///  Gets or sets the dialog font typeface.
		/// </summary>
		public string TypeFace { get; set; }
		/// <summary>
		///  Gets or sets the dialog font point size.
		/// </summary>
		public ushort PointSize { get; set; }
		/// <summary>
		///  Gets or sets the dialog font character set.
		/// </summary>
		public byte CharacterSet { get; set; }
		/// <summary>
		///  Gets or sets the dialog font weight.
		/// </summary>
		public ushort Weight { get; set; }
		/// <summary>
		///  Gets or sets if the dialog font is italic.
		/// </summary>
		public bool Italic { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs a new dialog ex template.
		/// </summary>
		public DialogExTemplate() {
			header.dlgVer = 1;
			header.signature = 0xFFFF;
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets if the dialog style allows for a font.
		/// </summary>
		public bool HasFont {
			get {
				return	(header.style & (uint) DialogStyles.DS_SETFONT) != 0 ||
						(header.style & (uint) DialogStyles.DS_SHELLFONT) != 0;
			}
		}

		/// <summary>
		///  Gets or sets the list of controls within the dialog.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public List<DialogExTemplateControl> Controls {
			get => controls;
			set => controls = value ?? throw new ArgumentNullException(nameof(Controls));
		}
		IReadOnlyList<IDialogBaseTemplateControl> IDialogBaseTemplate.Controls => Controls;

		/// <summary>
		///  Gets or sets the x-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short X {
			get => header.x;
			set => header.x = value;
		}
		/// <summary>
		///  Gets or sets the y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
		/// </summary>
		public short Y {
			get => header.y;
			set => header.y = value;
		}
		/// <summary>
		///  Gets or sets the width, in dialog box units, of the dialog box.
		/// </summary>
		public short Width {
			get => header.cx;
			set => header.cx = value;
		}
		/// <summary>
		///  Gets or sets the height, in dialog box units, of the dialog box.
		/// </summary>
		public short Height {
			get => header.cy;
			set => header.cy = value;
		}
		/// <summary>
		///  Gets or sets the dialog style.
		/// </summary>
		public uint Style {
			get => header.style;
			set => header.style = value;
		}
		/// <summary>
		///  Gets or sets the extended dialog style.
		/// </summary>
		public uint ExtendedStyle {
			get => header.exStyle;
			set => header.exStyle = value;
		}

		#endregion

		#region Read/Write

		/// <summary>
		///  Reads the object from the binary reader.
		/// </summary>
		/// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
		internal override void Read(BinaryReader reader) {
			header = reader.ReadUnmanaged<DLGTEMPLATEEX>();

			DialogTemplateUtils.ReadDialogTemplateBase(reader, this);

			if (HasFont) {
				Weight = reader.ReadUInt16();
				Italic = reader.ReadBoolean();
				CharacterSet = reader.ReadByte();
				TypeFace = reader.ReadTerminatedString();
			}

			for (int i = 0; i < header.cDlgItems; i++) {
				reader.BaseStream.SkipPadding(4);
				DialogExTemplateControl control = new DialogExTemplateControl();
				((BinaryReadableWriteable) control).Read(reader);
				controls.Add(control);
			}
		}
		/// <summary>
		///  Writes the object to the binary writer.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		internal override void Write(BinaryWriter writer) {
			header.cDlgItems = (ushort) Controls.Count;
			writer.WriteUnmanaged(header);

			DialogTemplateUtils.WriteDialogTemplateBase(writer, this);

			if (HasFont) {
				writer.Write(Weight);
				writer.Write(Italic);
				writer.Write(CharacterSet);
				writer.WriteTerminated(TypeFace ?? string.Empty);
			}

			for (int i = 0; i < header.cDlgItems; i++) {
				writer.Pad(4);
				((BinaryReadableWriteable) controls[i]).Write(writer);
			}
		}

		#endregion
		

		/// <summary>
		///  Creates a clone of this dialog template and all dialog controls.
		/// </summary>
		/// <returns>A clone of this dialog template.</returns>
		public DialogExTemplate Clone() {
			return new DialogExTemplate {
				header = header,
				controls = new List<DialogExTemplateControl>(controls.Select(c => c.Clone())),
				Caption = Caption,
				MenuId = MenuId,
				WindowClassId = WindowClassId,
				TypeFace = TypeFace,
				PointSize = PointSize,
				CharacterSet = CharacterSet,
				Weight = Weight,
				Italic = Italic,
			};
		}
		IDialogBaseTemplate IDialogBaseTemplate.Clone() => Clone();

		IDialogBaseTemplateControl IDialogBaseTemplate.CreateControl() => new DialogExTemplateControl();

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the dialog in the DIALOGEX format.
		/// </summary>
		/// <returns>The string representation of the dialog.</returns>
		public override string ToString() => $"DIALOGEX \"{Caption}\"";

		#endregion
	}
}