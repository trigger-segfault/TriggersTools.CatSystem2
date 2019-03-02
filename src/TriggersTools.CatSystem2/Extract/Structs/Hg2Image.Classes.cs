using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Structs;

namespace TriggersTools.CatSystem2.Structs {
	internal class Hg2FrameInfo {
		#region Fields

		public HG2IMG Img { get; set; }
		public HG2IMG_EX? ImgEx { get; set; }

		#endregion

		#region Constructors

		public Hg2FrameInfo(HG2IMG img) {
			Img = img;
		}

		#endregion
	}
}
