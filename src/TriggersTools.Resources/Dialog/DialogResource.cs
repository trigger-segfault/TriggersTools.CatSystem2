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
using System.Text;
using TriggersTools.Resources.Internal;

namespace TriggersTools.Resources.Dialog {
    /// <summary>
    ///  A dialog template resource.
    /// </summary>
    public sealed class DialogResource : Resource {
		#region Fields

		/// <summary>
		///  The dialog template for this resource.<para/>
		///  Either a <see cref="DialogTemplate"/> or <see cref="DialogExTemplate"/>.
		/// </summary>
		private IDialogBaseTemplate template;

		#endregion

		#region Constructors

		//public DialogResource() : base(ResourceTypes.RT_DIALOG) { }
		public DialogResource(ResourceId name, ushort language)
			: base(ResourceTypes.Dialog, name, language)
		{
		}
		public DialogResource(string fileName, ResourceId name, ushort language)
			: base(fileName, ResourceTypes.Dialog, name, language)
		{
		}
		public DialogResource(IntPtr hModule, ResourceId name, ushort language)
            : base(hModule, ResourceTypes.Dialog, name, language)
		{
        }

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the menu template for this resource.<para/>
		///  Either a <see cref="DialogTemplate"/> or <see cref="DialogExTemplate"/>.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		public IDialogBaseTemplate Template {
            get => template;
			set => template = value ?? throw new ArgumentNullException(nameof(Template));
		}

		#endregion

		#region Clone

		/// <summary>
		///  Creates a clone of the resource with an optional different name and or langauge.
		/// </summary>
		/// <param name="name">The optional new reource name. Null if it shouldn't change.</param>
		/// <param name="language">The optional new resource langauge. Null if it shouldn't change.</param>
		/// <returns>The clone of the resource with optional change in name and or language.</returns>
		public new DialogResource Clone(ResourceId? name = null, ushort? language = null) {
			return (DialogResource) base.Clone(name, language);
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

			uint version = reader.ReadUInt32();
			stream.Position -= 4;

			switch (version >> 16) {
                case 0xFFFF:
                    template = new DialogExTemplate();
                    break;
                default:
                    template = new DialogTemplate();
                    break;
            }

			((BinaryReadableWriteable) template).Read(reader);
		}
		/// <summary>
		///  Writes the resource to the stream.
		/// </summary>
		/// <param name="stream">The stream to write the resource to.</param>
		protected override void Write(Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode);
            ((BinaryReadableWriteable) template).Write(writer);
		}
		/// <summary>
		///  Creates a clone of the resource with the specified name and langauge.
		/// </summary>
		/// <param name="name">The new reource name.</param>
		/// <param name="language">The new resource langauge..</param>
		/// <returns>The clone of the resource with the name and language.</returns>
		protected override Resource CreateClone(ResourceId name, ushort language) {
			return new DialogResource(name, language) {
				template = template.Clone(),
			};
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the dialog resource in the DIALOG format.
		/// </summary>
		/// <returns>The string representation of the dialog resource.</returns>
		public override string ToString() => $"{base.ToString()} {Template}";

		#endregion
	}
}