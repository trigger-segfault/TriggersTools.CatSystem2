using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Attributes;

namespace TriggersTools.CatSystem2.Scenes.Commands.Sounds {
	public enum SoundType {
		None = 0,
		[CommandName("se")]
		Se,
		[CommandName("hse")]
		Hse,
		[CommandName("bgm")]
		Bgm,
		[CommandName("pcm")]
		Pcm,
	}
}
